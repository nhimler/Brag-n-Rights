using System.Security.Cryptography;
using System.Text;
using System.Net.Http.Headers;
using System.Text.Json;
using GymBro_App.Models;
using GymBro_App.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using GymBro_App.Helper;

namespace GymBro_App.Services
{
public class OAuthService : IOAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly GymBroDbContext _context;
    private readonly EncryptionHelper _encryptionHelper;

    public OAuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration, 
        IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, GymBroDbContext context,
        EncryptionHelper encryptionHelper)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _encryptionHelper = encryptionHelper;
    }

        public async Task ExchangeCodeForToken(string identityId, string code)
        {
            var codeVerifier = _httpContextAccessor.HttpContext?.Session.GetString("code_verifier");

            if (string.IsNullOrEmpty(codeVerifier))
            {
                throw new InvalidOperationException("Missing code_verifier.");
            }

            var clientId = _configuration["OAuth:ClientId"];
            var clientSecret = _configuration["OAuth:ClientSecret"];
            var redirectUri = _configuration["OAuth:RedirectUri"];
            var tokenEndpoint = _configuration["OAuth:TokenEndpoint"];

            // Use IHttpClientFactory to get an HttpClient instance
            var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"client_id", clientId},
                    {"grant_type", "authorization_code"},
                    {"redirect_uri", redirectUri},
                    {"code", code},
                    {"code_verifier", codeVerifier}
                })
            };

            // Set the Authorization header using Basic authentication
            var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            // Send the request and get the response
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error exchanging code for token: {responseContent}");
            }

            // Parse the response JSON
            var json = JsonDocument.Parse(responseContent).RootElement;
            var accessToken = json.GetProperty("access_token").GetString();
            var refreshToken = json.GetProperty("refresh_token").GetString();
            var expiresIn = json.GetProperty("expires_in").GetInt32();
            var scope = json.GetProperty("scope").GetString();
            var tokenType = json.GetProperty("token_type").GetString();
            var userId = json.GetProperty("user_id").GetString();

            var expirationTime = DateTime.Now.AddSeconds(expiresIn);

            // Fetch user from the database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var encryptedAccessToken = _encryptionHelper.EncryptToken(accessToken);
            var encryptedRefreshToken = _encryptionHelper.EncryptToken(refreshToken);

            // Check if a token already exists and update or insert
            var existingToken = await _context.Tokens.FirstOrDefaultAsync(t => t.UserId == user.UserId);

            if (existingToken != null)
            {
                existingToken.AccessToken = encryptedAccessToken;
                existingToken.RefreshToken = encryptedRefreshToken;
                existingToken.ExpirationTime = expirationTime;
                existingToken.Scope = scope;
                existingToken.TokenType = tokenType;

                _context.Tokens.Update(existingToken);
            }
            else
            {
                var newToken = new TokenEntity
                {
                    UserId = user.UserId,
                    AccessToken = encryptedAccessToken,
                    RefreshToken = encryptedRefreshToken,
                    ExpirationTime = expirationTime,
                    Scope = scope,
                    TokenType = tokenType
                };

                await _context.Tokens.AddAsync(newToken);
            }

            await _context.SaveChangesAsync();
        }






        private string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Replace("+", "-").Replace("/", "_").Replace("=", "");
            return output;
        }

        public string GenerateCodeVerifier()
        {
            var byteArray = new byte[64];
            RandomNumberGenerator.Fill(byteArray);
            return Base64UrlEncode(byteArray);
        }

        public string GenerateCodeChallenge(string codeVerifier)
        {
            using (var sha256 = SHA256.Create())
            {
                var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
                return Base64UrlEncode(challengeBytes);
            }
        }

        public string GenerateState()
        {
            var byteArray = new byte[32];
            RandomNumberGenerator.Fill(byteArray);
            return Base64UrlEncode(byteArray);
        }

        public string GetAuthorizationUrl()
        {
            // Retrieve configuration values
            var clientId = _configuration["OAuth:ClientId"];
            var scope = _configuration["OAuth:Scope"];
            var redirectUri = _configuration["OAuth:RedirectUri"];
            var authorizationBaseUrl = _configuration["OAuth:AuthorizationBaseUrl"];

            // Generate dynamic components
            var codeVerifier = GenerateCodeVerifier();
            var codeChallenge = GenerateCodeChallenge(codeVerifier);
            var state = GenerateState();

            // Store the codeVerifier and state in session
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                httpContext.Session.SetString("code_verifier", codeVerifier);
                httpContext.Session.SetString("oauth_state", state);
            }

            // Build the URL dynamically
            var authorizationUrl = $"{authorizationBaseUrl}" +
                                $"?response_type=code" +
                                $"&client_id={clientId}" +
                                $"&scope={scope}" +
                                $"&code_challenge={codeChallenge}" +
                                $"&code_challenge_method=S256" +
                                $"&state={state}" +
                                $"&redirect_uri={Uri.EscapeDataString(redirectUri)}";

            return authorizationUrl;
        }
    }
}

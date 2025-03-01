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

    public OAuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration
    , IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, GymBroDbContext context,
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
        var codeVerifier = _httpContextAccessor.HttpContext.Session.GetString("code_verifier"); // Retrieve the code_verifier from session (generated during OAuth flow)

        if (string.IsNullOrEmpty(codeVerifier))
        {
            throw new Exception("Missing code_verifier.");
        }

        var clientId = _configuration["OAuth:ClientId"];
        var clientSecret = _configuration["OAuth:ClientSecret"];
        var redirectUri = _configuration["OAuth:RedirectUri"];

        using (var client = new HttpClient())
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.fitbit.com/oauth2/token");

            // Set up the POST data
            var postData = new Dictionary<string, string>
            {
                {"client_id", clientId},
                {"grant_type", "authorization_code"},
                {"redirect_uri", redirectUri},
                {"code", code},
                {"code_verifier", codeVerifier}
            };

            request.Content = new FormUrlEncodedContent(postData);

            // Set the Authorization header using Basic authentication
            var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);

            // Set the content type for the request
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            // Send the request
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                // Parse the response to get the tokens
                var json = JsonDocument.Parse(responseContent);
                var accessToken = json.RootElement.GetProperty("access_token").GetString();
                var refreshToken = json.RootElement.GetProperty("refresh_token").GetString();
                var expiresIn = json.RootElement.GetProperty("expires_in").GetInt32();
                var scope = json.RootElement.GetProperty("scope").GetString();
                var tokenType = json.RootElement.GetProperty("token_type").GetString();
                var userId = json.RootElement.GetProperty("user_id").GetString();

                // Calculate expiration time from expires_in (this will be current time + expires_in seconds)
                var expirationTime = DateTime.UtcNow.AddSeconds(expiresIn);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityId);

                var encryptedAccessToken = _encryptionHelper.EncryptToken(accessToken);
                var encryptedRefreshToken = _encryptionHelper.EncryptToken(refreshToken);
                

                // Check if a token already exists for this user and update it if necessary
                var existingToken = await _context.Tokens.FirstOrDefaultAsync(t => t.UserId == user.UserId);

                if (existingToken != null)
                {
                    // Update the existing token with new values
                    existingToken.AccessToken = encryptedAccessToken;
                    existingToken.RefreshToken = encryptedRefreshToken;
                    existingToken.ExpirationTime = expirationTime;
                    existingToken.Scope = scope;
                    existingToken.TokenType = tokenType;
                    _context.Tokens.Update(existingToken);
                }
                else
                {
                    // Insert a new token for this user if no token exists
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

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            else
            {
                // If the request was not successful, throw an exception
                throw new Exception($"Error exchanging code for token: {responseContent}");
            }
        }
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
            var clientId = _configuration["OAuth:ClientId"];
            var scope = "activity";
            var codeVerifier = GenerateCodeVerifier();
            var codeChallenge = GenerateCodeChallenge(codeVerifier);

            _httpContextAccessor.HttpContext.Session.SetString("code_verifier", codeVerifier);

            var state = GenerateState();
            _httpContextAccessor.HttpContext.Session.SetString("oauth_state", state);

            var authorizationUrl = $"https://www.fitbit.com/oauth2/authorize" +
                                $"?response_type=code" +
                                $"&client_id={clientId}" +
                                $"&scope={scope}" +
                                $"&code_challenge={codeChallenge}" +
                                $"&code_challenge_method=S256" +
                                $"&state={state}" +
                                $"&redirect_uri=http://localhost:5075/signin-fitbit";

            return authorizationUrl;
        }
    }
}

namespace GymBro_App.Services
{
    public interface IOAuthService
    {
        string GenerateCodeVerifier();
        string GenerateCodeChallenge(string codeVerifier);
        string GenerateState();
        string GetAuthorizationUrl();

        public Task ExchangeCodeForToken(string userId ,string code);
    }

}
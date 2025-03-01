namespace GymBro_App.Services
{
    public interface IOAuthService
    {
        string GenerateCodeVerifier();
        string GenerateCodeChallenge(string codeVerifier);
        string GenerateState();
        string GetAuthorizationUrl();

        public Task<string> GetAccessToken(string identityId);
        public Task<int> GetUserSteps(string accessToken, string date);

        public Task ExchangeCodeForToken(string userId ,string code);
    }

}
namespace Eumel.Dj.Core.Models
{
    public interface ITokenService
    {
        UserToken GetUserToken(string usernameRequest);
        bool UsernameIsAvailable(string usernameRequest, out string usernameRecommendation);
        bool TryFindUser(string token, out string username);
        void DisposeUserToken(string token);
    }
}
namespace Eumel.Dj.Core.Models
{
    public class UserToken
    {
        public UserToken(string username, string usertoken)
        {
            Username = username;
            Usertoken = usertoken;
        }

        public string Username { get; }
        public string Usertoken { get; }
    }
}
using POC.Common;

namespace POC.OAuthServiceWebAPI.Services
{
    public class UserRepository
    {
        private static readonly List<UserInfo> _users = new List<UserInfo>
        {
            new ("admin", "12345"),
            new ("manager", "54321"),
            new ("employee", "11111"),
        };

        public UserInfo? Find(string username, string password)
        {
            return _users.FirstOrDefault(x => x.UserName == username && x.Password == password);
        }
    }
}

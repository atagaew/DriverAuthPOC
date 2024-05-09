using POC.Common;

namespace POC.OAuthServiceWebAPI.Nowy_folder
{
    public class UserRepository
    {
        private static readonly List<UserInfo> _users = new List<UserInfo>
        {
            new ("admin", "12345", "AdminToken"),
            new ("manager", "54321", "ManagerToken"),
            new ("employee", "11111", "EmployeeToken"),
        };

        public UserInfo? Find(string username, string password)
        {
            return _users.FirstOrDefault(x => x.UserName == username && x.Password == password);
        }
    }
}

using System.Security;

namespace SaintSender.Core.Models
{
    public class Account
    {
        private string _username;
        private SecureString _password;
        private bool _rememberUserCredentials;

        public Account()
        {
        }

        public Account(string username, SecureString password, bool rememberUserCredentials)
        {
            _username = username;
            _password = password;
            _rememberUserCredentials = rememberUserCredentials;
        }

        public string Username
        {
            get => _username;
            set => _username = value;
        }

        public SecureString Password
        {
            get => _password;
            set => _password = value;
        }

        public bool RememberUserCredentials
        {
            get => _rememberUserCredentials;
            set => _rememberUserCredentials = value;
        }
    }
}
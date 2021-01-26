using System.ComponentModel;
using System.Security;
using SaintSender.Core.Models;

namespace SaintSender.DesktopUI.ViewModels
{
    public class LoginWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private SecureString _password;
        private string _username;

        private Account _account;


        public LoginWindowViewModel()
        {
            _account = new Account();
        }

        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
            }
        }
    }
}
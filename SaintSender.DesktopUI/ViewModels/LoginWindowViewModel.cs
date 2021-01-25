using System.Windows;
using SaintSender.Core.Models;

namespace SaintSender.DesktopUI.ViewModels
{
    public class LoginWindowViewModel : Window
    {
        private Account _account;

        public LoginWindowViewModel()
        {
            _account = new Account();
        }
    }
}
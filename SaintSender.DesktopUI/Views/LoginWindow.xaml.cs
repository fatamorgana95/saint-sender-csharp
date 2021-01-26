using System;
using System.Net.Mail;
using System.Windows;
using SaintSender.Core.Models;
using SaintSender.DesktopUI.ViewModels;

namespace SaintSender.DesktopUI.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginWindowViewModel _vm;

        public LoginWindow()
        {
            _vm = new LoginWindowViewModel();
            DataContext = _vm;
            InitializeComponent();
        }


        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var username = _vm.Username;
            if (IsEmail(username) && PasswordTxt.Password.Length > 0)
            {
                SaveCredentials(username);
                CloseWindow();
            }
        }

        private void CloseWindow()
        {
            this.Close();
        }

        private void SaveCredentials(string username)
        {
            bool rememberCredentials = false;
            if (SaveChBx.IsChecked.HasValue)
            {
                rememberCredentials = (bool) SaveChBx.IsChecked;
            }

            Account account = new Account();
            account.Setup(username, PasswordTxt.Password, rememberCredentials);
            Account.SaveCredentials(account);
        }

        private bool IsEmail(string input)
        {
            try
            {
                var m = new MailAddress(input);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
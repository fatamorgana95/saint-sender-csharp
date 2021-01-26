using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            var username = UsernameTxt.Text;
            var password = new SecureString();
            if (IsEmail(username) && PasswordTxt.Text.Length > 0)
            {
                SaveCredentials(username, password);
                CloseWindow();
            }
        }

        private void CloseWindow()
        {
            this.Close();
        }

        private void SaveCredentials(string username, SecureString password)
        {
            foreach (var character in PasswordTxt.Text)
            {
                password.AppendChar(character);
            }

            bool rememberCredentials = false;
            if (SaveChBx.IsChecked.HasValue)
            {
                rememberCredentials = (bool) SaveChBx.IsChecked;
            }

            Account account = new Account();
            account.Setup(username, password, rememberCredentials);
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
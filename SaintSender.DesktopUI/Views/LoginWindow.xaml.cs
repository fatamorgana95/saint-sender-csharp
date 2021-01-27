using System;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            Login();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }

        private void Login()
        {
            string username = _vm.Username;
            if (!string.IsNullOrEmpty(username))
            {
                if (IsEmail(username))
                {
                    if (PasswordTxt.Password.Length > 0)
                    {
                        if (_vm.AttemptLogin(_vm.Username, PasswordTxt.Password))
                        {
                            SaveCredentials(username);
                            HideWindow();
                        }
                        else
                        {
                            if (!HaveInternetConnection())
                            {
                                Account backupAccount = Account.LoadBackupAccount();
                                if (backupAccount != null && backupAccount.Username == _vm.Username &&
                                    backupAccount.Password == PasswordTxt.Password)
                                {
                                    SaveCredentials(username);
                                    HideWindow();
                                }
                                else
                                {
                                    DisplayWarning("Given credentials do not meet backup credentials.");
                                }
                            }
                            else
                            {
                                DisplayWarning("Either the username or password is incorrect");
                            }
                        }
                    }
                    else
                    {
                        DisplayWarning("Password field is empty");
                    }
                }
                else
                {
                    DisplayWarning("Wrong email format");
                }
            }
            else
            {
                DisplayWarning("Email field is empty");
            }
        }

        private bool HaveInternetConnection()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }

        private void CompareLoginWithBackup(string username)
        {
        }

        private void DisplayWarning(string warningMessage)
        {
            _vm.WarningMessage = warningMessage;
        }

        private void HideWindow()
        {
            this.Hide();
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

        public void ClearInputFields()
        {
            _vm.Username = string.Empty;
            _vm.Password = string.Empty;
            _vm.WarningMessage = String.Empty;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
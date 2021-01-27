using System;
using System.Collections.Generic;
using SaintSender.DesktopUI.ViewModels;
using SaintSender.DesktopUI.Views;
using System.Windows;
using MimeKit;
using SaintSender.Core.Models;
using SaintSender.Core.Services;

namespace SaintSender.DesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _vm;

        public MainWindow()
        {
            // set DataContext to the ViewModel object
            _vm = new MainWindowViewModel();
            DataContext = _vm;
            LoginProcedure();
            InitializeComponent();
        }

        private void LoginProcedure()
        {
            AuthenticationPhase();
            _vm.LoadMails();
        }

        private void AuthenticationPhase()
        {
            if (Account.SavedCredentialsFound())
            {
                Account account = Account.LoadCredentials();
                if (!account.RememberUserCredentials)
                {
                    AskForLogin();
                }
            }
            else
            {
                AskForLogin();
            }

            _vm.LoadCredentials();
        }

        private void AskForLogin()
        {
            this.Hide();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
            this.Show();
        }

        private void GreetBtn_Click(object sender, RoutedEventArgs e)
        {
            // dispatch user interaction to view model
            _vm.Greet();
        }

        private void NewEmailBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            NewEmailWindow newEmailWindow = new NewEmailWindow();
            newEmailWindow.ShowDialog();
            this.Show();
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Logout();
            AskForLogin();
        }

        private void Logout()
        {
            Account.BackupCredentials();
            Account.DeleteCredentials();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
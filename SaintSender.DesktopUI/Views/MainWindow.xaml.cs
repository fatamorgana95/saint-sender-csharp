using System;
using System.Collections.Generic;
using SaintSender.DesktopUI.ViewModels;
using SaintSender.DesktopUI.Views;
using System.Windows;
using System.Windows.Controls;
using MimeKit;
using SaintSender.Core.Models;
using SaintSender.Core.Services;
using System.Windows.Input;
using System.Data;
using MailKit;
using MailService = SaintSender.Core.Services.MailService;

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
            InitializeComponent();
            LoginProcedure();
        }

        private void LoginProcedure()
        {
            AuthenticationPhase();
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
                else
                {
                    LoadMails();
                }
            }
            else
            {
                AskForLogin();
            }
        }

        private void AskForLogin()
        {
            this.Hide();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
            LoadMails();
            this.Show();
        }

        private void LoadMails()
        {
            _vm.LoadCredentials();
            _vm.LoadMails();
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

        private void DataGridRow_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            DataGrid dataGrid = sender as DataGrid;
            var email = (Email) dataGrid.SelectedItem;
            DetailWindow detailWindow = new DetailWindow(email);
            detailWindow.ShowDialog();
            _vm.SetEmailSeen(email.UId);
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
            Account.BackupCredentials();
            Environment.Exit(0);
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                _vm.LoadMails();
            }
        }
    }
}
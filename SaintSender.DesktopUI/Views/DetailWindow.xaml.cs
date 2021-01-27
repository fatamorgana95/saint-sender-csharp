using System;
using System.Windows;
using SaintSender.Core.Models;
using SaintSender.DesktopUI.ViewModels;

namespace SaintSender.DesktopUI.Views
{
    /// <summary>
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        private DetailWindowViewModel _vm;

        public DetailWindow(Email email)
        {
            _vm = new DetailWindowViewModel(email);
            DataContext = _vm;
            InitializeComponent();
        }

        private void NewEmailBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            NewEmailWindow newEmailWindow = new NewEmailWindow(Sender.Text);
            newEmailWindow.ShowDialog();
            this.Show();
        }

    }
}
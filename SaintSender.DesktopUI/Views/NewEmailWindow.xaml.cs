using System;
using System.Collections.Generic;
using System.Linq;
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
using SaintSender.DesktopUI.ViewModels;

namespace SaintSender.DesktopUI.Views
{
    /// <summary>
    /// Interaction logic for NewEmailWindow.xaml
    /// </summary>
    public partial class NewEmailWindow : Window
    {
        private SendNewEmailWindowViewModel _vm;

        public NewEmailWindow()
        {
            _vm = new SendNewEmailWindowViewModel();
            DataContext = _vm;
            InitializeComponent();
            _vm.LoadCredentials();
        }

        public NewEmailWindow(string sender, string subject)
        {
            _vm = new SendNewEmailWindowViewModel(sender, subject);
            DataContext = _vm;
            InitializeComponent();
            _vm.LoadCredentials();
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            //_vm.ToMail = 
            _vm.SendMail();
            this.Hide();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
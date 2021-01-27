using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
            var senderString = ExtractEmail.ExtractEmails(Sender.Text);
            var subject = String.Format("re: {0}", Subject.Text);
            NewEmailWindow newEmailWindow = new NewEmailWindow(senderString, subject);
            newEmailWindow.ShowDialog();
            this.Show();
        }

    }


    public class ExtractEmail
    {
        public static string ExtractEmails(string textToScrape)
        {
            Regex reg = new Regex(@"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}", RegexOptions.IgnoreCase);
            Match match;

            //List<string> results = new List<string>();
            //for (match = reg.Match(textToScrape); match.Success; match = match.NextMatch())
            //{
            //    if (!(results.Contains(match.Value)))
            //        results.Add(match.Value);
            //}

            match = reg.Match(textToScrape);
            return match.Value;
        }
    }
}
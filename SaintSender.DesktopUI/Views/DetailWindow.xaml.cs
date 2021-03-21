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
            var split = "Sender: ";
            string senderSubString = Sender.Text.Substring(Sender.Text.IndexOf(split) + split.Length);
            var emptyLines = new String('\n', 5);
            var body = String.Format("{0}{1} sent this (date: {2}):\n" +
                                     "{3}", emptyLines, senderSubString, Date.Text, Body.Text);
            NewEmailWindow newEmailWindow = new NewEmailWindow(senderString, subject, body);
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
            match = reg.Match(textToScrape);
            return match.Value;
        }
    }
}
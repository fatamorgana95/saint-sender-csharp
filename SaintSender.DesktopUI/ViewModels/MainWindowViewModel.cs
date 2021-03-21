﻿using System.Collections.Generic;
using SaintSender.Core.Interfaces;
using SaintSender.Core.Services;
using System.ComponentModel;
using System.Linq;
using MailKit;
using MimeKit;
using SaintSender.Core.Models;
using MailService = SaintSender.Core.Services.MailService;

namespace SaintSender.DesktopUI.ViewModels
{
    /// <summary>
    /// ViewModel for Main window. Contains all shown information
    /// and necessary service classes to make view functional.
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _greeting;
        private readonly IGreetService _greetService;
        private List<Email> _emails;
        private Account _account;

        /// <summary>
        /// Whenever a property value changed the subscribed event handler is called.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets value of Greeting.
        /// </summary>
        public string Greeting
        {
            get { return _greeting; }
            set
            {
                _greeting = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Greeting)));
            }
        }

        public List<Email> Emails
        {
            get => _emails;
            set
            {
                _emails = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Emails)));
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public MainWindowViewModel()
        {
            Name = string.Empty;
            _greetService = new GreetService();
        }

        /// <summary>
        /// Call a vendor service and apply its value into <see cref="Greeting"/> property.
        /// </summary>
        public void Greet()
        {
            Greeting = _greetService.Greet(Name);
        }

        public void LoadMails()
        {
            Emails = MailService.GetMails(_account.Username, _account.Password);
        }

        public void SetEmailSeen(UniqueId uId)
        {
            MailService.SetEmailSeen(uId, _account.Username, _account.Password);

            List<Email> copyEmailList = Emails.ToList();
            foreach (Email email in copyEmailList)
            {
                if (email.UId == uId)
                {
                    email.Seen = true;
                    Emails = copyEmailList;
                }
            }
        }

        public void LoadCredentials()
        {
            _account = Account.LoadCredentials();
            Name = _account.Username;
        }
    }
}
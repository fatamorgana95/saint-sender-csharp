using System;
using System.ComponentModel;
using SaintSender.Core.Models;
using SaintSender.Core.Services;

namespace SaintSender.DesktopUI.ViewModels
{
    public class SendNewEmailWindowViewModel : INotifyPropertyChanged
    {
        private Account _account;
        private string _toMail;
        private string _message;
        private string _newMailSubject;

        public SendNewEmailWindowViewModel(string sender, string subject)
        {
            ToMail = sender;
            NewMailSubject = subject;
        }
        
        public SendNewEmailWindowViewModel()
        {
        }


        /// <summary>
        /// Whenever a property value changed the subscribed event handler is called.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the ToMail.
        /// </summary>
        public string ToMail
        {
            get => _toMail;
            set
            {
                _toMail = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ToMail)));
            }
        }

        /// <summary>
        /// Gets or sets the MessageTitle.
        /// </summary>
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
            }
        }

        /// <summary>
        /// Gets or sets the MessageTitle.
        /// </summary>
        public string NewMailSubject
        {
            get => _newMailSubject;
            set
            {
                _newMailSubject = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewMailSubject)));
            }
        }



        public void LoadCredentials()
        {
            _account = Account.LoadCredentials();
        }

        public void SendMail()
        {
            // MailService.SendNewEmail("tom1.wales2@gmail.com", "Almafa1234", "should work", "someTitle", "mateszathmari@gmail.com");
            MailService.SendNewEmail(_account.Username, _account.Password, _message, _newMailSubject, _toMail);
        }
    }
}
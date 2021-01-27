using System;
using System.Collections.Generic;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Net.Smtp;
using MimeKit;
using SaintSender.Core.Models;

namespace SaintSender.Core.Services
{
    public class MailService
    {

        private static List<Email> _emails = new List<Email>();

        public static List<Email> GetMails(string username, string password)
        {
            using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, true);
                client.Authenticate(username, password);
                //The Inbox folder is always available on all IMAP servers...
                IMailFolder inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                
                AddEmailsToList(client);

                client.Disconnect(true);
            }
            _emails.Reverse();
            return _emails;
        }

        private static void AddEmailsToList(ImapClient client)
        {
            var uniqueIdList = client.Inbox.Search(SearchQuery.All);
            foreach (UniqueId id in uniqueIdList)
            {
                var info = client.Inbox.Fetch(new[] {id}, MessageSummaryItems.Flags);
                var seen = info[0].Flags.Value.HasFlag(MessageFlags.Seen);
                var mail = client.Inbox.GetMessage(id);
                //var sender = mail.Sender == null ? null : mail.Sender.ToString();

                Email email = new Email(seen, mail.From.ToString(), mail.Subject, mail.Date.DateTime, mail.TextBody);
                _emails.Add(email);
            }
        }

        public static void SendNewEmail(string username, string password, string text, string subject, string toMail)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(username));
            message.To.Add(new MailboxAddress(toMail));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = text
            };

            using (var client = new SmtpClient())
            {
                client.Connect("imap.gmail.com", 465, true);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(username, password);

                client.Send(message);
                client.Disconnect(true);
            }
        }

        public static bool IsCorrectLoginCredentials(string username, string password)
        {
            try
            {
                using (var client = new ImapClient())
                {
                    client.Connect("imap.gmail.com", 993, true);
                    client.Authenticate(username, password);
                    client.Disconnect(true);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
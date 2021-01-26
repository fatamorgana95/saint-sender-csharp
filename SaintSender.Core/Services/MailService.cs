using System;
using System.Collections.Generic;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
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
                var sender = mail.Sender == null ? null : mail.Sender.ToString();

                Email email = new Email(seen, sender, mail.Subject, mail.Date.DateTime);
                _emails.Add(email);
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
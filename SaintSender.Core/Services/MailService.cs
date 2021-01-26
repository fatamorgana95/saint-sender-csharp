using System;
using System.Collections.Generic;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;

namespace SaintSender.Core.Services
{
    public class MailService
    {
        public static List<MimeMessage> GetMails(string username, string password)
        {
            List<MimeMessage> emails = new List<MimeMessage>();
            using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, true);
                client.Authenticate(username, password);
                //The Inbox folder is always available on all IMAP servers...
                IMailFolder inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                foreach (MimeMessage mail in inbox)
                {
                    emails.Add(mail);
                }

                client.Disconnect(true);
            }

            return emails;
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
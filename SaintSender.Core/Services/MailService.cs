using System.Collections.Generic;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using SaintSender.Core.Models;

namespace SaintSender.Core.Services
{
    public class MailService
    {
        public static List<Email> GetMails(string username, string password)
        {
            List<Email> emails = new List<Email>();
            using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, true);
                client.Authenticate(username, password);
                //The Inbox folder is always available on all IMAP servers...
                IMailFolder inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                foreach (MimeMessage mail in inbox)
                {
                    //emails.Add(mail);
                }

                client.Disconnect(true);
            }

            return emails;
        }
    }
}
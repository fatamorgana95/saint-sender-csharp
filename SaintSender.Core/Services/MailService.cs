using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Reflection;
using System.Xml.Serialization;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Net.Smtp;
using MimeKit;
using SaintSender.Core.Interfaces;
using SaintSender.Core.Models;

namespace SaintSender.Core.Services
{
    public class MailService
    {
        private static List<Email> _emails = new List<Email>();
        private static string _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Remail";

        public static List<Email> GetMails(string username, string password)
        {
            _emails = new List<Email>();
            if (CheckInternet() || System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
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
                    NewBackup(_emails);
                }
            }
            else
            {
                _emails = LoadBackup();
            }

            _emails.Reverse();
            return _emails;
        }

        private static bool CheckInternet()
        {
            using (WebClient wc = new WebClient())
            {
                try
                {
                    var json = wc.DownloadString("http://vanenet.hu/");
                    return json.Contains("Van");
                }
                catch (Exception e)
                {
                    return false;
                }
            }
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

        public static void NewBackup(List<Email> emails, string path = "EmailBackup.xml")
        {
            string filePath = Path.Combine(_path, path);

            if (File.Exists(filePath))
            {
                File.Exists(filePath);
            }

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<Email>));
                xs.Serialize(sw, emails);
            }
        }

        public static List<Email> LoadBackup(string path = "EmailBackup.xml")
        {
            IsolatedStorageFile isoStore =
                IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            if (isoStore.FileExists(path))
            {
                using (IsolatedStorageFileStream isoStream =
                    new IsolatedStorageFileStream(path, FileMode.Open, isoStore))
                {
                    using (StreamReader sw = new StreamReader(isoStream))
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(List<Email>));
                        return (List<Email>) xs.Deserialize(sw);
                    }
                }
            }

            return null;
        }
    }
}
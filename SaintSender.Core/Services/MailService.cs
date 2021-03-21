using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;
using SaintSender.Core.Interfaces;
using SaintSender.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Reflection;
using System.Xml.Serialization;

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
                var info = client.Inbox.Fetch(new[] { id }, MessageSummaryItems.Flags);
                var seen = info[0].Flags.Value.HasFlag(MessageFlags.Seen);
                var mail = client.Inbox.GetMessage(id);

                Email email = new Email(seen, mail.From.ToString(), mail.Subject, mail.Date.DateTime, mail.TextBody,
                    id);
                _emails.Add(email);
            }
        }

        public static void SetEmailSeen(UniqueId uId, string username, string password)
        {
            if (CheckInternet() || System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                using (var client = new ImapClient())
                {
                    client.Connect("imap.gmail.com", 993, true);
                    client.Authenticate(username, password);
                    //The Inbox folder is always available on all IMAP servers...
                    IMailFolder inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadWrite);
                    inbox.AddFlags(uId, MessageFlags.Seen, true);
                    client.Disconnect(true);
                }
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
                File.Delete(filePath);
            }
            List<Email> encremails = EncryptEmails(emails);
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<Email>));
                xs.Serialize(sw, encremails);
            }
        }

        private static List<Email> EncryptEmails(List<Email> unencryptedEmails)
        {
            List<Email> encryptedEmails = new List<Email>();
            foreach (Email unencryptedEmail in unencryptedEmails)
            {
                Email encryptedEmail = new Email(unencryptedEmail.Seen, unencryptedEmail.Sender, unencryptedEmail.Subject, unencryptedEmail.Date, unencryptedEmail.Body, unencryptedEmail.UId);
                encryptedEmail.Sender = EncryptService.Encrypt(unencryptedEmail.Sender);
                encryptedEmail.Subject = EncryptService.Encrypt(unencryptedEmail.Subject);
                encryptedEmail.Body = EncryptService.Encrypt(unencryptedEmail.Sender);
                encryptedEmails.Add(encryptedEmail);
            }
            return encryptedEmails;
        }

        public static List<Email> LoadBackup(string path = "EmailBackup.xml")
        {
            string filePath = Path.Combine(_path, path);

            if (File.Exists(filePath))
            {
                using (StreamReader sw = new StreamReader(filePath))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(List<Email>));
                    List<Email> encryptedEmails = (List<Email>)xs.Deserialize(sw);
                    List<Email> decryptedEmails = DecryptEmails(encryptedEmails);
                    return decryptedEmails;
                }
            }

            return null;
        }

        private static List<Email> DecryptEmails(List<Email> encryptedEmails)
        {
            List<Email> decryptedEmails = new List<Email>();
            foreach (Email encryptedEmail in encryptedEmails)
            {
                Email decryptedEmail = new Email(encryptedEmail.Seen, encryptedEmail.Sender, encryptedEmail.Subject, encryptedEmail.Date, encryptedEmail.Body, encryptedEmail.UId);
                decryptedEmail.Sender = EncryptService.Decrypt(encryptedEmail.Sender);
                decryptedEmail.Subject = EncryptService.Decrypt(encryptedEmail.Subject);
                decryptedEmail.Body = EncryptService.Decrypt(encryptedEmail.Body);
                decryptedEmails.Add(decryptedEmail);
            }
            return decryptedEmails;
        }
    }
}
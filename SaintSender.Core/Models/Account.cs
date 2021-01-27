using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using SaintSender.Core.Services;

namespace SaintSender.Core.Models
{
    public class Account : ISerializable
    {
        private string _username;
        private string _password;
        private bool _rememberUserCredentials;

        public Account()
        {
        }

        public void Setup(string username, string password, bool rememberUserCredentials)
        {
            _username = username;
            _password = password;
            _rememberUserCredentials = rememberUserCredentials;
        }

        public static void SaveCredentials(Account account, string path = "Credentials.xml")
        {
            IsolatedStorageFile isoStore =
                IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            if (isoStore.FileExists(path))
            {
                isoStore.DeleteFile(path);
            }

            using (IsolatedStorageFileStream isoStream =
                new IsolatedStorageFileStream(path, FileMode.CreateNew, isoStore))
            {
                using (StreamWriter sw = new StreamWriter(isoStream))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(Account));
                    Account encryptedAccount = EncryptAccount(account);
                    xs.Serialize(sw, encryptedAccount);
                }
            }
        }

        public static void BackupCredentials(string path = "BackupCredentials.xml")
        {
            Account account = LoadCredentials();
            SaveCredentials(account, path);
        }

        private static Account EncryptAccount(Account account)
        {
            account.Username = EncryptService.Encrypt(account.Username);
            account.Password = EncryptService.Encrypt(account.Password);
            return account;
        }

        public static Account LoadCredentials(string path = "Credentials.xml")
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
                        XmlSerializer xs = new XmlSerializer(typeof(Account));
                        Account account = (Account) xs.Deserialize(sw);
                        return DecryptAccount(account);
                    }
                }
            }
            else
            {
                return LoadBackupAccount();
            }
        }

        public static Account LoadBackupAccount(string path = "BackupCredentials.xml")
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
                        XmlSerializer xs = new XmlSerializer(typeof(Account));
                        Account account = (Account) xs.Deserialize(sw);
                        return DecryptAccount(account);
                    }
                }
            }

            return null;
        }

        private static Account DecryptAccount(Account account)
        {
            account.Username = EncryptService.Decrypt(account.Username);
            account.Password = EncryptService.Decrypt(account.Password);
            return account;
        }

        public static void DeleteCredentials(string path = "Credentials.xml")
        {
            IsolatedStorageFile isoStore =
                IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            isoStore.DeleteFile(path);
        }

        public static bool SavedCredentialsFound(string path = "Credentials.xml")
        {
            IsolatedStorageFile isoStore =
                IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            return isoStore.FileExists(path);
        }

        public string Username
        {
            get => _username;
            set => _username = value;
        }

        public string Password
        {
            get => _password;
            set => _password = value;
        }

        public bool RememberUserCredentials
        {
            get => _rememberUserCredentials;
            set => _rememberUserCredentials = value;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", Username);
            info.AddValue("Password", Password);
            info.AddValue("RememberUserCredentials", RememberUserCredentials);
        }

        public Account(SerializationInfo info, StreamingContext context)
        {
            Username = (string) info.GetValue("Name", typeof(string));
            Password = (string) info.GetValue("Password", typeof(string));
            RememberUserCredentials = (bool) info.GetValue("RememberUserCredentials", typeof(bool));
        }
    }
}
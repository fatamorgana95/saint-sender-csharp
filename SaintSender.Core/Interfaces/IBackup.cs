using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Xml.Serialization;
using MimeKit;
using SaintSender.Core.Models;

namespace SaintSender.Core.Interfaces
{
    public interface IBackup
    {
        void NewBackup(List<MimeMessage> emails, string path = "EmailBackup.xml");
        List<MimeMessage> LoadBackup(string path = "EmailBackup.xml");
    }
}
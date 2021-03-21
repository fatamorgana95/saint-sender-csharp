using System;
using MailKit;

namespace SaintSender.Core.Models
{
    public class Email
    {
        public bool Seen { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public string Body { get; set; }
        public UniqueId UId { get; set; }


        public Email(bool seen, string sender, string subject, DateTime date, string body, UniqueId uId)
        {
            Seen = seen;
            Sender = sender.Replace(@"""", String.Empty);
            Subject = subject;
            Date = date;
            Body = body;
            UId = uId;
        }

        public Email() { }

    }
}

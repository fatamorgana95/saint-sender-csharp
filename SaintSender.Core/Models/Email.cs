using System;
namespace SaintSender.Core.Models
{
    public class Email
    {
        public bool Seen { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        

        public Email(bool seen, string sender, string subject, DateTime date)
        {
            Seen = seen;
            Sender = sender;
            Subject = subject;
            Date = date;
            
        }

    }
}

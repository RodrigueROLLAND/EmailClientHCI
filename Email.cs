using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace EmailClient
{
    class Email
    {
        public Email(string sender, List<string> recipients, List<string> ccrecipients, string subject, string content, List<string> attachments, DateTime date)
        {
            Sender = sender;
            Recipients = recipients;
            CCRecipients = ccrecipients;
            Subject = subject;
            Content = content;
            Attachments = attachments;
            Date = date;
        }

        public string Sender
        {
            get { return sender; }
            set
            {
                if (value.Length == 0)
                {
                    MessageBox.Show("Need a sender");

                }
                else if (value.Contains('@') == false)
                {
                    MessageBox.Show("Incorrect entry for sender email");
                }
                else
                {
                    sender = value;
                }
                //A revoir ça car ouai c'est toi le sender donc il faudrait récupérer l'email
            }
        }
        private string sender;

        public List<string> Recipients 
        {
            get { return recipients; }
            set 
            {
                recipients = value;
                //réaliser un test pour savoir si c'est bien un email et si c'est pas vide
            } 
        }
        private List<string> recipients;

        public string FormattedRecipients => string.Join(", ", Recipients);

        public List<string> CCRecipients
        {
            get { return ccrecipients; }
            set
            {
                ccrecipients = value;
                //test de si il y a quelque chose bah que ce soit bien des adresses mails
                //sinon champs vide
            }
        }
        private List<string> ccrecipients;

        public string Subject
        {
            get { return subject; }
            set
            {
                if(value.Length == 0)
                {
                    MessageBox.Show("Need a subject");
                }
                else
                {
                    subject = value;
                }
            }
        }
        private string subject;

        public string Content
        {
            get { return content; }
            set
            {
                if (value.Length == 0)
                {
                    MessageBox.Show("Need a content");
                }
                else
                {
                    content = value;
                }
            }
        }
        private string content;

        public List<string> Attachments
        {
            get { return attachments; }
            set
            {
                attachments = value;
                //tests que d'une pièce jointe sinon vide
            }
        }
        private List<string> attachments;



        public DateTime Date
        {
            get { return date; }
            set
            {
                //tester si c'est bien une date
                date = value;
            }
        }
        private DateTime date = DateTime.Now;

        public override string ToString()
        {
            return $"{Subject}";
            // return String.Format("{0}, {1}, {2}", Name, Description, Deadline.ToString());
        }


    }
}

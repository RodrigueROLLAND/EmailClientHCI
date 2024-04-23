using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace EmailClient
{

    public class Email : INotifyPropertyChanged
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
            Read = false;
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
                    OnPropertyChanged("Sender");
                }
            }
        }
        private string sender;

        public List<string> Recipients 
        {
            get { return recipients; }
            set 
            {
                recipients = value;
                OnPropertyChanged("Recipients");
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
                OnPropertyChanged("CCRecipients");
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
                    OnPropertyChanged("Subject");
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
                    OnPropertyChanged("Content");
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
                OnPropertyChanged("Attachments");
            }
        }
        private List<string> attachments;



        public DateTime Date
        {
            get { return date; }
            set
            {
                if(value is DateTime)
                {
                    date = value;
                }
                OnPropertyChanged("Date");
            }
        }
        private DateTime date = DateTime.Now;

        public bool Read
        {
            get { return read; }
            set
            {
                read = value;
                OnPropertyChanged("Read");
            }
        }
        private bool read;

        public event PropertyChangedEventHandler? PropertyChanged;

        public override string ToString()
        {
            return $"{Subject}";    //find in internet
        }

        //Copy of LabWork
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }
        }
    }
}

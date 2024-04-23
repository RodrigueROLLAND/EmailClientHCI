using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EmailClient
{
    class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            //define the selected subfolder, this definitions is used for delete method
            SelectSubFolderCommand = new Command(obj =>
            {
                SelectedSubFolder = (Folder)obj;
            });


            EditDraftCommand = new Command(obj =>
            {

                if (SelectedEmail != null && SelectedSubFolder != null && SelectedSubFolder.Name == "Draft")
                {
                    // Edit parameter with pre-set values
                    SelectedEmail.Recipients = new List<string> { "nouveau@mail.com" };
                    SelectedEmail.Content = "New content for message";
                    SelectedEmail.Subject = "New subject";

                    // PropertyChanged for all elements
                    OnPropertyChanged("SelectedEmail");
                    OnPropertyChanged("Subject");
                    OnPropertyChanged("Content");
                    OnPropertyChanged("RecipientstoString");
                    OnPropertyChanged("CCRecipientstoString");
                }
                else
                {
                    MessageBox.Show("No email is selected or the email is not in draft ! ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            });

            AddEmailCommand = new Command(obj =>
            {
                createAddEmail();
            });

            DeleteEmailCommand = new Command(obj =>
            {
                if(SelectedSubFolder != null)
                {
                    SelectedSubFolder.Messages.Remove(SelectedEmail);
                }
                else
                {
                    MessageBox.Show("No email is selected ! ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            });

            createEmail();


        }

        public ICommand AddEmailCommand { get; private set; }

        public ICommand DeleteEmailCommand { get; private set; }

        public ICommand EditDraftCommand { get; private set; }

        // Used for save the selectedEmail
        private Email selectedEmail;
        public Email SelectedEmail
        {
            get { return selectedEmail; }
            set
            {
                if (selectedEmail != value)
                {
                    selectedEmail = value;
                    OnPropertyChanged("SelectedEmail");
                    OnPropertyChanged("RecipientstoString");
                    OnPropertyChanged("CCRecipientstoString");
                }
            }
        }

        // Used for save the selected SubFolder
        private Folder _selectedSubFolder;
        public Folder SelectedSubFolder
        {
            get { return _selectedSubFolder; }
            set
            {
                if (_selectedSubFolder != value)
                {
                    _selectedSubFolder = value;
                    OnPropertyChanged("SelectedSubFolder");
                }
            }
        }

        public ICommand SelectSubFolderCommand { get; private set; }

        public ObservableCollection<Folder> folderList { get; private set; } = new ObservableCollection<Folder>();

        public event PropertyChangedEventHandler? PropertyChanged;

        //Copy of LabWork
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }
        }

        public string CCRecipientstoString
        {
            get
            {
                if(SelectedEmail != null)
                {
                    return string.Join(", ", SelectedEmail.CCRecipients);
                }
                else { return ""; }
            }
        }

        public string RecipientstoString
        {
            get
            {
                if (SelectedEmail != null)
                {
                    return string.Join(", ", SelectedEmail.Recipients);
                }
                else { return ""; }
            }
        }

        void createEmail()
        {
            // Recipients
            List<string> recipients = new List<string>();
            recipients.Add("rodrigue.rolland@student.um.si");
            recipients.Add("bastien@gmail.com");

            // CCRecipients
            List<string> ccRecipients = new List<string>();
            ccRecipients.Add("Bastien");
            ccRecipients.Add("Pierre");

            Email email1 = new Email("millon@gmail.com", recipients, ccRecipients, "Course", "Test test test test", new List<string>(), DateTime.Parse("28/01/2024"));
            Email email2 = new Email("millon@gmail.com", recipients, ccRecipients, "Course1", "Test test test test", new List<string>(), DateTime.Parse("28/01/2024"));
            Email email3 = new Email("delahaies@gmail.com", recipients, ccRecipients, "Course in Trash", "Test test test test", new List<string>(), DateTime.Parse("28/01/2024"));

            List<Folder> listsubfolders1 = new List<Folder>();
            List<Email> listemail = new List<Email> { email1, email2 };
            List<Email> listemailtrash = new List<Email> { email3 };

            Email emaildraft1 = new Email("me@gmail.com", recipients, ccRecipients, "Test 1", "Test test test test", new List<string>(), DateTime.Parse("26/03/2024"));
            Email emaildraft2 = new Email("me@gmail.com", recipients, ccRecipients, "Test 2", "Test test test test", new List<string>(), DateTime.Parse("31/03/2024"));

            List<Email> listemaildraft1 = new List<Email> { emaildraft1, emaildraft2 };

            listsubfolders1.Add(new Folder("InnBox", null, listemail));
            listsubfolders1.Add(new Folder("Trash", null, listemailtrash));
            listsubfolders1.Add(new Folder("Sent", null));
            listsubfolders1.Add(new Folder("Draft", null, listemaildraft1));

            //Create MailBox 1
            folderList.Add(new Folder("MailBox 1", listsubfolders1));

            Email emaildraft3 = new Email("me@gmail.com", recipients, ccRecipients, "Test 1", "Test test test test", new List<string>(), DateTime.Parse("26/03/2024"));
            Email emaildraft4 = new Email("me@gmail.com", recipients, ccRecipients, "Test 2", "Test test test test", new List<string>(), DateTime.Parse("31/03/2024"));

            List<Email> listemaildraft2 = new List<Email> { emaildraft3, emaildraft4 };

            List<Folder> listsubfolders2 = new List<Folder>();
            listsubfolders2.Add(new Folder("InnBox", null, listemail));
            listsubfolders2.Add(new Folder("Trash"));
            listsubfolders2.Add(new Folder("Sent"));
            listsubfolders2.Add(new Folder("Draft", null, listemaildraft2));

            //Create MailBox 2
            folderList.Add(new Folder("MailBox 2", listsubfolders2));
        }

        void createAddEmail()
        {
            //Recipients
            List<string> recipients = new List<string>();
            recipients.Add("me@gmail.com");
            recipients.Add("bastien@gmail.com");

            //CCRecipients
            List<string> ccRecipients = new List<string>();

            //Create Email
            Email emailAdd = new Email("add@gmail.com", recipients, ccRecipients, "EmailAdd", "Test test test test", new List<string>(), DateTime.Parse("28/01/2024"));


            //Define the mailbox how email is create
            Folder mainFolder = folderList.FirstOrDefault(folder => folder.Name == "MailBox 1");


            if (mainFolder != null)
            {
                //Define the subfolder for create email
                Folder inboxFolder = mainFolder.SubFolders.FirstOrDefault(subfolder => subfolder.Name == "Sent");
                if (inboxFolder != null)
                {
                    //Add email to sent folder
                    inboxFolder.Messages.Add(emailAdd);
                }
            }
        }
    }
}

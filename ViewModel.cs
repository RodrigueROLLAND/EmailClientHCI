using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;


namespace EmailClient
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            //////////////////////////////////////////////// Fenetre Settings ////////////////////////////////////////////////
            // Charger les catégories depuis les paramètres de l'application
            LoadCategories();
            AddCategoryCommand = new Command(AddCategory);
            DeleteCategoryCommand = new Command(DeleteCategory);
            EditCategoryCommand = new Command(EditCategory);

            //////////////////////////////////////////////// Fenetre Edit ////////////////////////////////////////////////

            SendEmailCommand = new Command(SendEmail);// Dans le constructeur ViewModel

            ImportXML = new Command(ImportXMLCallback);

            ExportXML = new Command(ExportXMLCallback);




            //////////////////////////////////////////////// Exercice 1, 2, 3 ////////////////////////////////////////////////

            EditElement = new Command(EditElementCallback);

            NewElement = new Command(NewElementWindowCallback);

            SettingsElement = new Command(SettingsElementCallback);

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
                if(SelectedSubFolder != null && SelectedEmail != null)
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

        public ICommand EditElement { get; private set; }

        public ICommand AddEmailCommand { get; private set; }

        public ICommand NewElement { get; private set; }

        public ICommand SettingsElement { get; private set; }

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

        //public string CCRecipientstoString
        //{
        //    get
        //    {
        //        if(SelectedEmail != null)
        //        {
        //            return string.Join(", ", SelectedEmail.CCRecipients);
        //        }
        //        else { return ""; }
        //    }
        //}



        private void SettingsElementCallback(object sender)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        private bool isEditing = false;

        public bool IsEditing
        {
            get { return isEditing; }
            set
            {
                if (isEditing != value)
                {
                    isEditing = value;
                    OnPropertyChanged(nameof(IsEditing));
                }
            }
        }
        private void EditElementCallback(object sender)
        {
            if (selectedEmail != null)
            {
                if (!IsEditing)
                {
                    EditWindow editWindow = new EditWindow(this);
                    editWindow.Closed += (s, e) => { IsEditing = false; }; // Mettre à jour la variable lorsque la fenêtre est fermée
                    editWindow.Topmost = true;
                    editWindow.Show();

                    IsEditing = true; // Définir la variable sur true lors de l'ouverture de la fenêtre
                }
                else
                {
                    MessageBox.Show("Une fenêtre d'édition est déjà ouverte.");
                }
                
               
            }
            else
            {
                MessageBox.Show("Select Email");
            }
        }



        private void NewElementWindowCallback(object sender)
        {
            NewWindow newWindow = new NewWindow();
            // Affichez la fenêtre de composition en tant que fenêtre modale
            newWindow.ShowDialog();
            bool? goodClose = newWindow.IsClicked;

            // Vérifiez si l'utilisateur a cliqué sur "Envoyer" ou "Enregistrer comme brouillon"
            if (goodClose == true)
            {
                // Récupérez l'email depuis la fenêtre de composition
                Email newEmail = newWindow.Email;

                // Vérifiez l'action de l'utilisateur
                if (newWindow.IsSendClicked)  //test si Send a été appuyé
                {
                    if (SelectedSubFolder != null)
                    {
                        SelectedSubFolder.Messages.Add(newEmail);
                    }
                    
                }
                else
                {
                    //Define the mailbox how email is create
                    Folder mainFolder = folderList.FirstOrDefault(folder => folder.Name == "MailBox 1");

                    if (mainFolder != null)
                    {
                        //Define the subfolder for create email
                        Folder inboxFolder = mainFolder.SubFolders.FirstOrDefault(subfolder => subfolder.Name == "Draft");
                        if (inboxFolder != null)
                        {
                            //Add email to sent folder
                            inboxFolder.Messages.Add(newEmail);
                        }
                    }
                }
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

            List<string> attachments = new List<string>();
            attachments.Add("C:\\Chemin\\vers\\fichier1.txt");
            attachments.Add("D:\\Chemin\\vers\\fichier2.txt");
            attachments.Add("E:\\Chemin\\vers\\fichier3.txt");

            Email emaildraft1 = new Email("me@gmail.com", recipients, ccRecipients, "Test 1", "Test test test test", attachments, DateTime.Parse("26/03/2024"));
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

        void createAddEmail()  //Add in Draft
        {
            //Recipients
            List<string> recipients = new List<string>();
            recipients.Add("me@gmail.com");
            recipients.Add("bastien@gmail.com");

            //CCRecipients
            List<string> ccRecipients = new List<string>();

            //Attachments
            List<string> attachments = new List<string>();
            attachments.Add("/D/CR");
            attachments.Add("/C/PD");

            //Create Email
            Email emailAdd = new Email("add@gmail.com", recipients, ccRecipients, "Email Add with Add Button", "Add Button Add Button", attachments, DateTime.Parse("28/01/2024"));


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


        //////////////////////////////////////////////// Settings Windows ////////////////////////////////////////////////
        ///Ce qui permet de stocker les catégories
        private ObservableCollection<string> _categories = new ObservableCollection<string>();
        public ObservableCollection<string> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;

            }
        }

        // Charger les catégories depuis les paramètres de l'application
        private void LoadCategories()
        {
            StringCollection stringCollection = Properties.Settings.Default.Categories;

            foreach (string category in stringCollection)
            {
                Categories.Add(category);
            }
        }

        public ICommand AddCategoryCommand { get; private set; }


        private string _textboxcategory;
        public string TextBoxCategory
        {
            get { return _textboxcategory; }
            set
            {
                if (_textboxcategory != value)
                {
                    _textboxcategory = value;
                    OnPropertyChanged(nameof(TextBoxCategory));
                }
            }
        }

        private void AddCategory(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxCategory))
            {
                ///https://www.youtube.com/watch?v=bRD-iyT9n7Q pour mettre des strings dans une collection c# settings
                StringCollection stringgg = Properties.Settings.Default.Categories;
                stringgg.Add(TextBoxCategory);
                Properties.Settings.Default.Categories = stringgg;
                Properties.Settings.Default.Save();

                // Add the new category to the Categories collection
                Categories.Add(TextBoxCategory);
                // Effacer le champ de texte après l'ajout de la catégorie pour que ce qu'il y a dans text box se supprime
                TextBoxCategory = string.Empty;
            }
        }

        private string _selectedsategory;
        public string SelectedCategory
        {
            get { return _selectedsategory; }
            set
            {
                _selectedsategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        public ICommand DeleteCategoryCommand { get; private set; }
        private void DeleteCategory(object sender)
        {
            if (SelectedCategory != null)
            {
                StringCollection stringgg = Properties.Settings.Default.Categories;
                stringgg.Remove(SelectedCategory.ToString());
                Properties.Settings.Default.Categories = stringgg;
                Properties.Settings.Default.Save();
                Categories.Remove(SelectedCategory.ToString());
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("Select Category");
            }
        }

        public ICommand EditCategoryCommand { get; private set; }

        private void EditCategory(object sender)
        {
            if (SelectedCategory != null && !string.IsNullOrWhiteSpace(TextBoxCategory))
            {
                //int selectedIndex = lstCategories.SelectedIndex;
                //Categories[selectedIndex] = txtCategoryName.Text;
                //txtCategoryName.Clear();
                StringCollection stringgg = Properties.Settings.Default.Categories;
                stringgg.Remove(SelectedCategory.ToString());
                stringgg.Add(TextBoxCategory);
                Properties.Settings.Default.Categories = stringgg;
                Properties.Settings.Default.Save();
                Categories.Remove(SelectedCategory.ToString());
                Categories.Add(TextBoxCategory);
                Properties.Settings.Default.Save();
                // Effacer le champ de texte après l'ajout de la catégorie pour que ce qu'il y a dans text box se supprime
                TextBoxCategory = string.Empty;
            }
        }


        //////////////////////////////////////////////// Edit Windows ////////////////////////////////////////////////

        public ICommand SendEmailCommand { get; private set; }
        private void SendEmail(object sender)
        {
            Email mail = SelectedEmail;

            Folder mainFolder = folderList.FirstOrDefault(folder => folder.Name == "MailBox 1");


            if (mainFolder != null)
            {
                //Define the subfolder for create email
                Folder inboxFolder = mainFolder.SubFolders.FirstOrDefault(subfolder => subfolder.Name == "Sent");
                if (inboxFolder != null)
                {
                    //Add email to sent folder
                    inboxFolder.Messages.Add(mail);
                }
            }
            if (SelectedSubFolder != null && SelectedEmail != null)
            {
                SelectedSubFolder.Messages.Remove(SelectedEmail);
            }
        }





        public ICommand ImportXML { get; private set; }

        public ICommand ExportXML { get; private set; }


        private void ImportXMLCallback(object sender)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichiers XML (*.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                // Traitez le fichier sélectionné ici
                string filePath = openFileDialog.FileName;
                // Code pour importer à partir du fichier XML
            }
        }

        private void ExportXMLCallback(object sender)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Fichiers XML (*.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == true)
            {
                // Traitez le fichier de sauvegarde ici
                string filePath = saveFileDialog.FileName;
                

                using (var sw = new StreamWriter(filePath))
                {
                    XmlSerializer serial = new XmlSerializer(typeof(Task));
                    serial.Serialize(sw, "test");
                }
                // Code pour exporter vers le fichier XML
            }
        }


    }
}

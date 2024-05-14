using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EmailClient
{
    /// <summary>
    /// Logique d'interaction pour NewWindow.xaml
    /// </summary>
    /// 

    public partial class NewWindow : Window, INotifyPropertyChanged
    {
        public Email Email { get; private set; }
        public bool IsSendClicked { get; private set; }
        public bool IsClicked { get; private set; }

        public NewWindow()
        {
            InitializeComponent();
            DataContext = this;

            StringCollection stringCollection = Properties.Settings.Default.Categories;
            Categories = new ObservableCollection<string>();

            foreach (string category in stringCollection)
            {
                Categories.Add(category);
                categoryComboBox.ItemsSource = Categories;
                TDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }
        public ObservableCollection<string> Categories { get; set; }

        private void ButtonDraft_Click(object sender, RoutedEventArgs e)
        {
            CompleteEmail();

            IsSendClicked = false;
            IsClicked = true;

            Close();
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            CompleteEmail();

            IsSendClicked = true;
            IsClicked = true;

            Close();
        }

        private void CompleteEmail()
        {
            //Recipients

            string recip = TRecipients.Text;
            char separator = ',';
            string[] substrings = recip.Split(separator);
            List<string> recipients = new List<string>(substrings);


            //CCRecipients
            string ccrecip = TCCRecipients.Text;
            string[] ccsubstrings = ccrecip.Split(separator);
            List<string> ccRecipients = new List<string>(ccsubstrings);

            DateTime dateTime = DateTime.Today;


            //CCRecipients
            string attach = TAttachment.Text;
            string[] attachstring = attach.Split(separator);
            List<string> Attachment = new List<string>(attachstring);

            //Create Email
            Email = new Email(TSender.Text, recipients, ccRecipients, TSubject.Text, TContent.Text, Attachment, dateTime);

            string selectedCategory = categoryComboBox.SelectedItem as string;

            Email.Category = selectedCategory;
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Attachment_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true; // Permettre la sélection de plusieurs fichiers

            if (openFileDialog.ShowDialog() == true)
            {
                // Récupérer les chemins d'accès des fichiers sélectionnés
                string[] selectedFilePaths = openFileDialog.FileNames;
                List<string> AttachedFilePaths = new List<string>();

                // Traiter les fichiers sélectionnés ici
                foreach (string filename in selectedFilePaths)
                {
                    // Vous pouvez également stocker les chemins d'accès dans une liste
                    AttachedFilePaths.Add(filename);
                }

                string filesText = string.Join(", ", AttachedFilePaths);

                TAttachment.Text = filesText;
            }
        }
    }
}

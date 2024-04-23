using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace EmailClient
{

    public partial class MainWindow : Window
    {
        ObservableCollection<Email> emailList = new ObservableCollection<Email>();
        ObservableCollection<Folder> folderList = new ObservableCollection<Folder>();

        public MainWindow()
        {
            InitializeComponent();

            // Création d'une liste pour les destinataires
            List<string> recipients = new List<string>();
            recipients.Add("me@gmail.com");
            recipients.Add("bastien@gmail.com");

            // Création d'une liste pour les destinataires en copie
            List<string> ccRecipients = new List<string>();
            ccRecipients.Add("Bastien");
            ccRecipients.Add("Pierre");

            // Création de l'e-mail en utilisant les listes pour les destinataires et les destinataires en copie
            emailList.Add(new Email("millon@gmail.com",recipients, ccRecipients, "Course", "Test test test test", new List<string>(), DateTime.Parse("28/01/2024")));
            emailList.Add(new Email("millon@gmail.com", recipients, ccRecipients, "Course1", "Test test test test", new List<string>(), DateTime.Parse("28/01/2024")));


            LVEmails.ItemsSource = emailList;

            List<string> listsubfolders = new List<string>();
            listsubfolders.Add("InnBox");
            listsubfolders.Add("Trash");

            folderList.Add(new Folder("Folder 1", listsubfolders));

            TVFolders.ItemsSource = folderList;

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(LVEmails.SelectedItem != null)
            {
                Email selectedEmail = (Email)LVEmails.SelectedItem;
                MessageBox.Show(selectedEmail.Subject,"Subject",MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }

    }
}
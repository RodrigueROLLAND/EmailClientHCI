using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
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
    /// Logique d'interaction pour EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window, INotifyPropertyChanged
    {
        public EditWindow(ViewModel? vm)

        {
            InitializeComponent();
            if (vm != null)
            {
                this.DataContext = vm;

                //C'est pour ajouter l'événement à PropertyChanged de Vue Modèle, ce qui permet que lorsqu'on change d'Email, cela appelle cette fonction
                vm.PropertyChanged += ViewModel_PropertyChanged;
            }

            LoadCategories();
            CheckIfDraft();


        }

        private ViewModel _viewModel;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<string> Categories { get; set; }


        //Copy of LabWork
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }
        }

        private void LoadCategories()
        {
            StringCollection stringCollection = Properties.Settings.Default.Categories;
            Categories = new ObservableCollection<string>();

            // Ajouter chaque catégorie de StringCollection à ObservableCollection
            foreach (string category in stringCollection)
            {
                Categories.Add(category);
            }
            categoryComboBox.ItemsSource = Categories;
        }
        private void CheckIfDraft()
        {
            if (DataContext is ViewModel viewModel)
            {
                if (viewModel.SelectedEmail != null)
                {
                    bool isDraft = viewModel.SelectedSubFolder != null && viewModel.SelectedSubFolder.Name == "Draft";
                    // Vérifiez si l'email est un brouillon

                    // Désactiver les contrôles d'édition si ce n'est pas un brouillon
                    bool enableEditing = isDraft;

                    // Activer ou désactiver les contrôles en fonction de l'état du brouillon
                    TSender.IsEnabled = enableEditing;
                    TRecipients.IsEnabled = enableEditing;
                    TCCRecipients.IsEnabled = enableEditing;
                    TTSubject.IsEnabled = enableEditing;
                    TTDate.IsEnabled = enableEditing;
                    TTAttachment.IsEnabled = enableEditing;
                    TTContent.IsEnabled = enableEditing;
                    categoryComboBox.IsEnabled = enableEditing;
                    ButtonAttachment.Visibility = enableEditing ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedEmail")
            {
                CheckIfDraft();
            }
        }

        private void Saveasdarft_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonAttachment_Click(object sender, RoutedEventArgs e)
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

                    TTAttachment.Text = filesText;
                    OnPropertyChanged("AttachmenttoString");
            }
            
        }
    }
}

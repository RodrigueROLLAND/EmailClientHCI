using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;


namespace EmailClient
{
    /// <summary>
    /// Logique d'interaction pour SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = this;

            StringCollection stringCollection = Properties.Settings.Default.Categories;

            // Copier les éléments de la StringCollection dans la liste
            foreach (string category in stringCollection)
            {
                Categories.Add(category);
            }
            //c'est normal si il y a deux paramètres dans settings.settings, si il y en a qu'un l app crash
            
        }

        private ObservableCollection<string> _categories = new ObservableCollection<string>();
        public ObservableCollection<string> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                
            }
        }


        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                ///https://www.youtube.com/watch?v=bRD-iyT9n7Q pour mettre des strings dans une collection c# settings
                StringCollection stringgg = Properties.Settings.Default.Categories;
                stringgg.Add(txtCategoryName.Text);
                Properties.Settings.Default.Categories = stringgg;
                Properties.Settings.Default.Save();

                // Add the new category to the Categories collection
                Categories.Add(txtCategoryName.Text);
                // Clear the text input field
                txtCategoryName.Clear();
            }
        }

        private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {


            if (lstCategories.SelectedItem != null)
            {
                StringCollection stringgg = Properties.Settings.Default.Categories;
                stringgg.Remove(lstCategories.SelectedItem.ToString());
                Properties.Settings.Default.Categories = stringgg;
                Properties.Settings.Default.Save();
                Categories.Remove(lstCategories.SelectedItem.ToString());
                Properties.Settings.Default.Save();
            }
        }

        private void EditCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (lstCategories.SelectedItem != null && !string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                int selectedIndex = lstCategories.SelectedIndex;
                Categories[selectedIndex] = txtCategoryName.Text;
                txtCategoryName.Clear();
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EmailClient
{
    public class Folder : INotifyPropertyChanged
    {



        public string Name
        {
            get { return name; }
            set
            {
                if (value.Length == 0)
                {
                    MessageBox.Show("The folder need a name !");
                }
                else
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        private string name;

        public ObservableCollection<Folder> SubFolders
        {
            get { return subfolders; }
            set
            {
                subfolders = value;
                OnPropertyChanged("SubFolders");
            }
        }
        private ObservableCollection<Folder> subfolders;

        public ObservableCollection<Email> Messages
        {
            get { return messages; }
            set
            {
                
                messages = value;
                OnPropertyChanged("Messages");
            }
        }
        private ObservableCollection<Email> messages;

        public event PropertyChangedEventHandler? PropertyChanged;


        public Folder(string name, List<Folder>? subfolders = null, List<Email>? messages = null)
        {
            Name = name;
            SubFolders = new ObservableCollection<Folder>(subfolders ?? new List<Folder>());
            Messages = new ObservableCollection<Email>(messages ?? new List<Email>());
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

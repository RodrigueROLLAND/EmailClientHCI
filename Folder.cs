using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EmailClient
{
    class Folder
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
                }
            }
        }
        private string name;

        public List<string> SubFolders
        {
            get { return subfolders; }
            set
            {
                subfolders = value;
                //pour moi pas de tests car si c'est vide bah c'est bien
                //si c'est pas vide
            }
        }
        private List<string> subfolders;

        public List<Email> Messages
        {
            get { return messages; }
            set
            {
                
                messages = value;
                //pour moi pas de tests car si c'est vide bah c'est bien
                //si c'est pas vide
            }
        }
        private List<Email> messages;

        public Folder(string name, List<string> subfolders, List<Email>? messages = null)
        {
            Name = name;
            SubFolders = subfolders;
            Messages = messages;
        }

    }
}

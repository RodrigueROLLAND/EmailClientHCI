using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EmailClient
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ViewModel();   
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();   //find in internet
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(LVEmails.SelectedItem != null)
            {
                Email selectedEmail = (Email)LVEmails.SelectedItem;   //recover a selected email
                MessageBox.Show(selectedEmail.Subject,"Subject",MessageBoxButton.OK,MessageBoxImage.Information);  //display the subject of selected email
            }
        }

        private void TVFolders_SelectedItem(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
                if (TVFolders.SelectedItem is Folder selectedFolder)
                {
                    LVEmails.ItemsSource = selectedFolder.Messages;        //Update Source for ListView with the select subfolder
                    ((ViewModel)DataContext).SelectSubFolderCommand.Execute(selectedFolder);  //Call a icommand for define the selected subfolder is used for delete an email         
                }
        }
        private void LVEmails_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var gridView = LVEmails.View as GridView;
            if (gridView != null)
            {
                double remainingSpace = LVEmails.ActualWidth - SystemParameters.VerticalScrollBarWidth;
                //SubjectColumn.Width = remainingSpace * 0.3; // 30%
                //RecipientColumn.Width = remainingSpace * 0.7; // 70%
            }
        }
    }
}



////Source
///1 : https://stackoverflow.com/questions/3209129/unable-to-send-an-email-to-multiple-addresses-recipients-using-c-sharp
///Comprehension of WPF : https://youtu.be/t9ivUosw_iI?si=i4mL3JfP5BY097jH 
///WPF TreeView : https://youtu.be/Oe8PXFechTI?si=fFNP6VeZyJx33raT
///Binding MVVM Project : https://youtu.be/Ocq-fBet9mI?si=acVN7ZVbpI8fIkvE 
///Data binding : https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/?view=netdesktop-8.0
///Property Change : https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/how-to-implement-property-change-notification?view=netframeworkdesktop-4.8&viewFallbackFrom=netdesktop-8.0 
///I used AI to help me understand the major principles (for examples : MVVM, ICommand and Command) and questions
///For style of project, I use many website
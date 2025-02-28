using MyApp;
using System.Windows;
using System.Windows.Navigation;

namespace multitool
{
    public partial class DescriptionWindow : Window
    {
        public DescriptionWindow()
        {
            InitializeComponent();
        }

        // Minimize Button
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Close Button
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Εμφάνιση του MainWindow
            if (this.Owner is MainWindow mainWindow)
            {
                mainWindow.Show();
            }

            // Κλείσιμο του τρέχοντος παραθύρου
            this.Close();
        }

        // Hyperlink Navigation
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // Open the link in the default browser
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true
            });
            e.Handled = true; // Prevent further processing of the event
        }
        private void GitHubButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the GitHub profile in the default browser
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://github.com/thomasthanos",
                UseShellExecute = true
            });
        }
    }
}
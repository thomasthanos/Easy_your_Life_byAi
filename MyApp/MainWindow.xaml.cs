using System.Windows;
using System.Windows.Input;

namespace MyApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Add an event handler for the CodeTextBox to check the code when Enter is pressed or focus is lost
            CodeTextBox.KeyDown += CodeTextBox_KeyDown;
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region --> buttons
        private void InstallAppsButton_Click(object sender, RoutedEventArgs e)
        {
            // Κρύψε το MainWindow
            this.Hide();

            // Άνοιγμα του InstallAppsWindow
            InstallAppsWindow installAppsWindow = new InstallAppsWindow();
            installAppsWindow.Closed += (s, args) => this.Show(); // Όταν κλείσει το InstallAppsWindow, εμφάνισε ξανά το MainWindow
            installAppsWindow.Show();
        }
        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void CrackSitesButton_Click(object sender, RoutedEventArgs e)
        {
            // Κρύψε το MainWindow
            this.Hide();

            // Άνοιγμα του CrackSiteWindow
            CrackSiteWindow crackSiteWindow = new CrackSiteWindow();
            crackSiteWindow.Closed += (s, args) => this.Show(); // Όταν κλείσει το CrackSiteWindow, εμφάνισε ξανά το MainWindow
            crackSiteWindow.Show();
        }

        private void CrackAppsButton_Click(object sender, RoutedEventArgs e)
        {
            // Κρύψε το MainWindow
            this.Hide();

            // Άνοιγμα του PublicInstallWindow
            PublicInstallWindow publicInstallWindow = new PublicInstallWindow();
            publicInstallWindow.Closed += (s, args) => this.Show(); // Όταν κλείσει το PublicInstallWindow, εμφάνισε ξανά το MainWindow
            publicInstallWindow.Show();
        }

        private void SysMaintenanceButton_Click(object sender, RoutedEventArgs e)
        {
            // Κρύψε το MainWindow
            this.Hide();

            // Άνοιγμα του multitool window
            multitool multitoolWindow = new multitool();
            multitoolWindow.Closed += (s, args) => this.Show(); // Όταν κλείσει το multitool, εμφάνισε ξανά το MainWindow
            multitoolWindow.Show();
        }

        private void ActivateAutoLoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Κρύψε το MainWindow
            this.Hide();

            // Άνοιγμα του CustomWindow
            CustomWindow customWindow = new CustomWindow();
            customWindow.Closed += (s, args) => this.Show(); // Όταν κλείσει το CustomWindow, εμφάνισε ξανά το MainWindow
            customWindow.Show();
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            // Κρύψε το MainWindow
            this.Hide();

            // Άνοιγμα του info window
            info infoWindow = new info();
            infoWindow.Closed += (s, args) => this.Show(); // Όταν κλείσει το info, εμφάνισε ξανά το MainWindow
            infoWindow.Show();
        }

        // Handle code checking when Enter is pressed in CodeTextBox
        private void CodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string enteredCode = CodeTextBox.Text.Trim().ToLower(); // Get the code from the TextBox and convert to lowercase
                if (enteredCode == "sims")
                {
                    this.Hide();
                    Sims simsWindow = new Sims();
                    simsWindow.Closed += (s, args) => this.Show(); // Show MainWindow when Sims window closes
                    simsWindow.Show();
                    CodeTextBox.Text = ""; // Clear the code after successful entry
                }
                else
                {
                    MessageBox.Show("Invalid code. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        #endregion
    }
}
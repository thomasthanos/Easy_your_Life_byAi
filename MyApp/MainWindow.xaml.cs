using System.Text;
using System.Windows;


namespace MyApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
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
        // Δημιουργία και εμφάνιση του νέου παραθύρου
        InstallAppsWindow installAppsWindow = new InstallAppsWindow();
        installAppsWindow.Show();
    }
    #endregion

}
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
        // Κρύψε το MainWindow
        this.Hide();

        // Άνοιγμα του InstallAppsWindow
        InstallAppsWindow installAppsWindow = new InstallAppsWindow();
        installAppsWindow.Closed += (s, args) => this.Show(); // Όταν κλείσει το InstallAppsWindow, εμφάνισε ξανά το MainWindow
        installAppsWindow.Show();
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
    #endregion

}
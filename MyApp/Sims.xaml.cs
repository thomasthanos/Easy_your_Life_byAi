using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;

namespace MyApp
{
    public partial class Sims : Window
    {
        public Sims()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Επιστροφή στο MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close(); // Κλείνει το τρέχον παράθυρο
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Ελαχιστοποίηση του παραθύρου
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Κλείσιμο της εφαρμογής (shutdown)
            Application.Current.Shutdown();
        }

        private void sims_bat_Click(object sender, RoutedEventArgs e)
        {
            string downloadUrl = "https://raw.githubusercontent.com/thomasthanos/Make_your_life_easier/main/.zip%20files/EA-DLC-Unlocker.zip";
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "EA-DLC-Unlocker-v2.rar");
            string extractPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "EA_UNLOCKER");

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(downloadUrl, downloadPath);
                }

                if (Directory.Exists(extractPath))
                {
                    new CustomMessageBox("The folder already exists.", "Info", IconType.Info).ShowDialog();
                }
                else
                {
                    Directory.CreateDirectory(extractPath);
                    System.IO.Compression.ZipFile.ExtractToDirectory(downloadPath, extractPath);
                }

                string batFilePath = Path.Combine(extractPath, "setup.bat");
                if (File.Exists(batFilePath))
                {
                    Process.Start(batFilePath);
                }

                File.Delete(downloadPath);

                new CustomMessageBox("Operation completed successfully.", "Success", IconType.Success).ShowDialog();
            }
            catch (Exception ex)
            {
                new CustomMessageBox($"An error occurred: {ex.Message}", "Error", IconType.Error, ex.ToString()).ShowDialog();
            }
        }

        private void install_Click(object sender, RoutedEventArgs e)
        {
            string downloadUrl = "https://raw.githubusercontent.com/thomasthanos/Make_your_life_easier/main/.zip%20files/sims_install.zip";
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "sims_install_update.zip");
            string extractPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "sims install update");

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(downloadUrl, downloadPath);
                }

                if (Directory.Exists(extractPath))
                {
                    new CustomMessageBox("The folder already exists.", "Info", IconType.Info).ShowDialog();
                }
                else
                {
                    Directory.CreateDirectory(extractPath);
                    System.IO.Compression.ZipFile.ExtractToDirectory(downloadPath, extractPath);
                }

                string exeFilePath = Path.Combine(extractPath, "sims-4-updater-v1.3.4.exe");
                if (File.Exists(exeFilePath))
                {
                    Process.Start(exeFilePath);
                }

                File.Delete(downloadPath);

                new CustomMessageBox("Operation completed successfully.", "Success", IconType.Success).ShowDialog();
            }
            catch (Exception ex)
            {
                new CustomMessageBox($"An error occurred: {ex.Message}", "Error", IconType.Error, ex.ToString()).ShowDialog();
            }
        }
    }
}
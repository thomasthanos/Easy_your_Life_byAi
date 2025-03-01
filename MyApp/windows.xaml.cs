using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyApp
{
    public partial class CustomWindow : Window
    {
        public CustomWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
        }

        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            string fileUrl = "https://github.com/thomasthanos/Make_your_life_easier/raw/main/.exe%20files/auto%20login.exe";
            string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "auto-login.exe");

            try
            {
                // Κατέβασμα του αρχείου
                await DownloadFileAsync(fileUrl, savePath);

                // Εμφάνιση μηνύματος επιτυχίας
                MessageBox.Show("Το αρχείο κατεβήθηκε επιτυχώς!", "Επιτυχία", MessageBoxButton.OK, MessageBoxImage.Information);

                // Εκτέλεση του αρχείου
                Process.Start(savePath);
            }
            catch (Exception ex)
            {
                // Εμφάνιση μηνύματος σφάλματος
                MessageBox.Show($"Σφάλμα κατά τη λήψη ή εκτέλεση του αρχείου: {ex.Message}", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private async void Button2_Click(object sender, RoutedEventArgs e)
        {
            string fileUrl = "https://raw.githubusercontent.com/thomasthanos/Make_your_life_easier/main/.bat%20files/activate.bat";
            string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "activate.bat");

            try
            {
                // Έλεγχος αν το αρχείο υπάρχει
                if (!File.Exists(savePath))
                {
                    // Κατέβασμα του αρχείου αν δεν υπάρχει
                    await DownloadFileAsync(fileUrl, savePath);
                }

                // Εκτέλεση του αρχείου
                Process process = new Process();
                process.StartInfo.FileName = savePath;
                process.StartInfo.UseShellExecute = true;
                process.Start();

                // Αναμονή για την ολοκλήρωση της εκτέλεσης
                process.WaitForExit();

                // Διαγραφή του αρχείου μετά την εκτέλεση
                File.Delete(savePath);

                // Εμφάνιση μηνύματος επιτυχίας
                MessageBox.Show("Η ενεργοποίηση ολοκληρώθηκε επιτυχώς!", "Επιτυχία", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Εμφάνιση μηνύματος σφάλματος
                MessageBox.Show($"Σφάλμα κατά την εκτέλεση του αρχείου: {ex.Message}", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DownloadFileAsync(string fileUrl, string savePath)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(fileUrl);
                response.EnsureSuccessStatusCode();

                using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                             fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    await contentStream.CopyToAsync(fileStream);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
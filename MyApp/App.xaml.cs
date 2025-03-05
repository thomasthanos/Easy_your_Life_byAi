using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace MyApp
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Έλεγχος αν το winget είναι εγκατεστημένο
            if (!IsWingetInstalled())
            {
                MessageBox.Show("Το Winget δεν είναι εγκατεστημένο. Παρακαλώ εγκαταστήστε το πρώτα.", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
                return;
            }

            // Έλεγχος αν ο χρήστης έχει αποδεχτεί τους όρους χρήσης
            if (!await CheckWingetTermsAcceptedAsync())
            {
                var result = MessageBox.Show("Πρέπει να αποδεχτείτε τους όρους χρήσης του Winget για να συνεχίσετε. Θέλετε να τους αποδεχτείτε τώρα;", "Όροι Χρήσης", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    await AcceptWingetTermsAsync();
                }
                else
                {
                    MessageBox.Show("Η εφαρμογή δεν μπορεί να συνεχίσει χωρίς την αποδοχή των όρων χρήσης.", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
                    Shutdown();
                    return;
                }
            }
        }

        private bool IsWingetInstalled()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "winget",
                        Arguments = "--version",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                process.WaitForExit();

                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> CheckWingetTermsAcceptedAsync()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "winget",
                        Arguments = "list --accept-source-agreements",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = await process.StandardOutput.ReadToEndAsync();
                await process.WaitForExitAsync();

                // Αν η έξοδος περιέχει το μήνυμα για τους όρους, σημαίνει ότι δεν έχουν αποδεχτεί
                if (output.Contains("You must accept the source agreements"))
                {
                    return false;
                }

                return true; // Οι όροι έχουν ήδη αποδεχτεί
            }
            catch
            {
                return false;
            }
        }

        private async Task AcceptWingetTermsAsync()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "winget",
                        Arguments = "source update --accept-source-agreements",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                await process.WaitForExitAsync();

                if (process.ExitCode == 0)
                {
                    MessageBox.Show("Οι όροι χρήσης του Winget έχουν αποδεχτεί επιτυχώς!", "Ολοκλήρωση", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Η αποδοχή των όρων χρήσης του Winget απέτυχε.", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Σφάλμα κατά την αποδοχή των όρων χρήσης: {ex.Message}", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
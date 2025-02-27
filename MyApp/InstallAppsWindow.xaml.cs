using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace MyApp
{
    public partial class InstallAppsWindow : Window
    {
        private List<AppInfo> apps; // Λίστα για αποθήκευση των εφαρμογών

        public InstallAppsWindow()
        {
            InitializeComponent();

            // Έλεγχος για το αν το winget είναι εγκατεστημένο
            if (!IsWingetInstalled())
            {
                // Εάν δεν είναι εγκατεστημένο, εγκαθιστά το winget
                InstallWingetAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        // Εμφάνιση σφάλματος αν η εγκατάσταση αποτύχει
                        Dispatcher.Invoke(() =>
                        {
                            var customMessageBox = new CustomMessageBox("Η εγκατάσταση του Winget απέτυχε. Παρακαλώ εγκαταστήστε το χειροκίνητα.", "Σφάλμα", IconType.Error);
                            customMessageBox.ShowDialog();
                        });
                    }
                    else
                    {
                        // Εάν η εγκατάσταση είναι επιτυχής, κλείνουμε το τρέχον παράθυρο και ανοίγουμε ένα νέο
                        Dispatcher.Invoke(() =>
                        {
                            var newWindow = new InstallAppsWindow();
                            newWindow.Show();
                            this.Close();
                        });
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                // Εάν το winget είναι ήδη εγκατεστημένο, φόρτωσε τις εφαρμογές
                LoadAppsAsync();
            }
        }

        private async void LoadAppsAsync()
        {
            try
            {
                Debug.WriteLine("Starting to load apps...");
                AppsListBox.ItemsSource = new List<AppInfo> { new AppInfo { Name = "Φόρτωση εφαρμογών...", Version = "", PackageName = "" } };

                apps = new List<AppInfo>
                {
                    new AppInfo { Name = "7-Zip", PackageName = "7zip.7zip" },
                    new AppInfo { Name = "Advanced Installer", PackageName = "Caphyon.AdvancedInstaller" },
                    new AppInfo { Name = "Better Discord", PackageName = "BetterDiscord.BetterDiscord" },
                    new AppInfo { Name = "Blender", PackageName = "BlenderFoundation.Blender" },
                    new AppInfo { Name = "Bluestack", PackageName = "BlueStack.BlueStacks" },
                    new AppInfo { Name = "Brave", PackageName = "Brave.Brave" },
                    new AppInfo { Name = "Corsair", PackageName = "Corsair.iCUE.5" },
                    new AppInfo { Name = "CPU-Info", PackageName = "CPUID.CPU-Z" },
                    new AppInfo { Name = "Discord", PackageName = "Discord.Discord" },
                    new AppInfo { Name = "DiscordPTB", PackageName = "Discord.Discord.PTB" },
                    new AppInfo { Name = "Download Manager", PackageName = "SoftDeluxe.FreeDownloadManager" },
                    new AppInfo { Name = "Driver Booster", PackageName = "IObit.DriverBooster" },
                    new AppInfo { Name = "Epic Games", PackageName = "EpicGames.EpicGamesLauncher" },
                    new AppInfo { Name = "GitHub", PackageName = "GitHub.GitHubDesktop" },
                    new AppInfo { Name = "Google Chrome", PackageName = "Google.Chrome" },
                    new AppInfo { Name = "HWiNFO", PackageName = "REALiX.HWiNFO 8.22" },
                    new AppInfo { Name = "IObit Advanced", PackageName = "IObit.AdvancedSystemCare" },
                    new AppInfo { Name = "IObit Uninstaller", PackageName = "IObit.Uninstaller" },
                    new AppInfo { Name = "LibreOffice", PackageName = "TheDocumentFoundation.LibreOffice" },
                    new AppInfo { Name = "Malwarebytes", PackageName = "Malwarebytes.Malwarebytes" },
                    new AppInfo { Name = "Microsoft.NET6", PackageName = "Microsoft.DotNet.DesktopRuntime.6" },
                    new AppInfo { Name = "Microsoft.NET7", PackageName = "Microsoft.DotNet.DesktopRuntime.7" },
                    new AppInfo { Name = "Microsoft.NET8", PackageName = "Microsoft.DotNet.DesktopRuntime.8" },
                    new AppInfo { Name = "Microsoft.NET9", PackageName = "Microsoft.DotNet.DesktopRuntime.9" },
                    new AppInfo { Name = "Msi-AfterBurner", PackageName = "Guru3D.Afterburner" },
                    new AppInfo { Name = "NexusMods", PackageName = "NexusMods.Vortex" },
                    new AppInfo { Name = "NotePad++", PackageName = "Notepad++.Notepad++" },
                    new AppInfo { Name = "Nvidia", PackageName = "Nvidia.GeForceExperience" },
                    new AppInfo { Name = "Nvidia Broadcast", PackageName = "Nvidia.Broadcast" },
                    new AppInfo { Name = "PowerISO", PackageName = "PowerSoftware.PowerISO" },
                    new AppInfo { Name = "PS Remote", PackageName = "PlayStation.PSRemotePlay" },
                    new AppInfo { Name = "Python", PackageName = "Python.Python.3.13" },
                    new AppInfo { Name = "Razer", PackageName = "Razer.Synapse" },
                    new AppInfo { Name = "Riot", PackageName = "RiotGames.Valorant.EU" },
                    new AppInfo { Name = "SaveWizard", PackageName = "SaveWizard.SaveWizard" },
                    new AppInfo { Name = "Spotify", PackageName = "Spotify.Spotify" },
                    new AppInfo { Name = "Steam", PackageName = "Valve.Steam" },
                    new AppInfo { Name = "StreamDeck", PackageName = "Elgato.StreamDeck" },
                    new AppInfo { Name = "Ubisoft", PackageName = "Ubisoft.Connect" },
                    new AppInfo { Name = "VirtualBox", PackageName = "Oracle.VirtualBox" },
                    new AppInfo { Name = "Visual Studio", PackageName = "Microsoft.VisualStudio.2022.Community" },
                    new AppInfo { Name = "VS Code", PackageName = "Microsoft.VisualStudioCode" },
                    new AppInfo { Name = "WeMod", PackageName = "WeMod.WeMod" },
                    new AppInfo { Name = "WinRAR", PackageName = "RARLab.WinRAR" }
                };

                Debug.WriteLine("Retrieving versions...");
                var tasks = apps.Select(async app =>
                {
                    app.Version = await GetPackageVersionAsync(app.PackageName);
                    Debug.WriteLine($"Retrieved version for {app.Name}: {app.Version}");
                }).ToList();

                await Task.WhenAll(tasks);

                apps.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));

                Debug.WriteLine("Updating ListBox...");
                await Dispatcher.InvokeAsync(() =>
                {
                    AppsListBox.ItemsSource = apps;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading apps: {ex.Message}");
                var customMessageBox = new CustomMessageBox($"Σφάλμα κατά τη φόρτωση των εφαρμογών: {ex.Message}", "Σφάλμα");
                customMessageBox.ShowDialog();
            }
        }


        private async Task<string> GetPackageVersionAsync(string packageName)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "winget",
                        // Προσθήκη flags για αυτόματη αποδοχή όρων
                        Arguments = $"list --id {packageName} --accept-package-agreements --accept-source-agreements",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = await process.StandardOutput.ReadToEndAsync();
                await process.WaitForExitAsync();

                // Ανάλυση της εξόδου
                var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    if (line.Contains(packageName, StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 2)
                        {
                            return parts[1]; // Η έκδοση είναι το δεύτερο μέρος
                        }
                    }
                }

                return ""; // Επιστροφή κενής συμβολοσειράς αν δεν βρεθεί έκδοση
            }
            catch
            {
                return ""; // Επιστροφή κενής συμβολοσειράς σε περίπτωση σφάλματος
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsWingetInstalled())
            {
                await InstallWingetAsync();
                return;
            }

            var selectedApps = AppsListBox.SelectedItems.Cast<AppInfo>().ToList();
            if (selectedApps.Count > 0)
            {
                DownloadButton.IsEnabled = false;

                foreach (var app in selectedApps)
                {
                    try
                    {
                        DownloadProgressBar.IsIndeterminate = false;
                        DownloadProgressBar.Value = 0;
                        DownloadProgressBar.Maximum = 100;

                        var timer = new System.Timers.Timer(500);
                        timer.Elapsed += (s, ev) =>
                        {
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (DownloadProgressBar.Value < 95)
                                {
                                    DownloadProgressBar.Value += 5;
                                }
                            }));
                        };
                        timer.Start();

                        string errorOutput = string.Empty;
                        bool installationSuccess = false;

                        await Task.Run(() =>
                        {
                            var process = new Process
                            {
                                StartInfo = new ProcessStartInfo
                                {
                                    FileName = "winget",
                                    Arguments = $"install --id {app.PackageName} --silent --accept-package-agreements --accept-source-agreements",
                                    RedirectStandardOutput = true,
                                    RedirectStandardError = true,
                                    UseShellExecute = false,
                                    CreateNoWindow = true
                                }
                            };

                            process.Start();
                            errorOutput = process.StandardError.ReadToEnd();
                            process.WaitForExit();

                            installationSuccess = process.ExitCode == 0;
                        });

                        timer.Stop();

                        await Dispatcher.InvokeAsync(() =>
                        {
                            if (installationSuccess)
                            {
                                DownloadProgressBar.Value = 100;
                                var customMessageBox = new CustomMessageBox($"Η εφαρμογή {app.Name} εγκαταστάθηκε επιτυχώς!", "Εγκατάσταση Ολοκληρώθηκε", IconType.Success);
                                customMessageBox.ShowDialog();

                                // Κατάργηση της εφαρμογής από τις επιλεγμένες
                                AppsListBox.SelectedItems.Remove(app);
                            }
                            else
                            {
                                var customMessageBox = new CustomMessageBox($"Σφάλμα κατά την εγκατάσταση της εφαρμογής {app.Name}: {errorOutput}", "Σφάλμα", IconType.Error, errorOutput);
                                customMessageBox.ShowDialog();
                            }
                        });

                        await Task.Delay(1000); // Καθυστέρηση 1 δευτερολέπτου πριν την επόμενη εγκατάσταση
                    }
                    catch (Exception ex)
                    {
                        await Dispatcher.InvokeAsync(() =>
                        {
                            var customMessageBox = new CustomMessageBox($"Σφάλμα: {ex.Message}", "Σφάλμα", IconType.Error, ex.StackTrace);
                            customMessageBox.ShowDialog();
                        });
                    }
                }

                DownloadButton.IsEnabled = true;

                // Έλεγχος αν δεν υπάρχει άλλη επιλεγμένη εφαρμογή
                if (AppsListBox.SelectedItems.Count == 0)
                {
                    // Animation για clear του ProgressBar
                    var clearTimer = new System.Windows.Threading.DispatcherTimer
                    {
                        Interval = TimeSpan.FromMilliseconds(50)
                    };

                    clearTimer.Tick += (s, ev) =>
                    {
                        if (DownloadProgressBar.Value > 0)
                        {
                            DownloadProgressBar.Value -= 5; // Μείωση της τιμής σταδιακά
                        }
                        else
                        {
                            clearTimer.Stop();
                            DownloadProgressBar.Value = 0; // Βεβαιώνεται ότι είναι στο 0
                        }
                    };

                    clearTimer.Start();
                }

            }
            else
            {
                var customMessageBox = new CustomMessageBox("Παρακαλώ επιλέξτε τουλάχιστον μία εφαρμογή για εγκατάσταση.", "Δεν Επιλέχθηκε Εφαρμογή", IconType.Info);
                customMessageBox.ShowDialog();
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

        private async Task InstallWingetAsync()
        {
            try
            {
                string url = "https://aka.ms/getwinget"; // Official Microsoft link
                string filePath = Path.Combine(Path.GetTempPath(), "winget.msixbundle");

                // Download the winget package
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        ShowError("Αποτυχία λήψης του Winget. Παρακαλώ δοκιμάστε ξανά.");
                        return;
                    }

                    byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(filePath, fileBytes);
                }

                // Verify that the file exists before attempting to install
                if (!File.Exists(filePath))
                {
                    ShowError("Το αρχείο εγκατάστασης του Winget δεν βρέθηκε.");
                    return;
                }

                // Run PowerShell as Admin to install Winget
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell",
                        Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"Start-Process PowerShell -ArgumentList 'Add-AppxPackage -Path {filePath}' -Verb RunAs\"",
                        Verb = "runas", // Run as administrator
                        UseShellExecute = true
                    }
                };

                process.Start();
                await process.WaitForExitAsync();

                if (process.ExitCode == 0)
                {
                    ShowSuccess("Το Winget εγκαταστάθηκε επιτυχώς!");
                }
                else
                {
                    ShowError("Η εγκατάσταση του Winget απέτυχε. Παρακαλώ εγκαταστήστε το χειροκίνητα.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Σφάλμα κατά την εγκατάσταση του Winget: {ex.Message}");
            }
        }

        private void ShowSuccess(string message)
        {
            var customMessageBox = new CustomMessageBox(message, "Εγκατάσταση Ολοκληρώθηκε");
            customMessageBox.ShowDialog();
        }

        private void ShowError(string message)
        {
            var customMessageBox = new CustomMessageBox(message, "Σφάλμα");
            customMessageBox.ShowDialog();
        }

        private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text;
            if (!string.IsNullOrEmpty(searchText))
            {
                // Φίλτραρε την λίστα των εφαρμογών
                var filteredApps = apps.Where(app => app.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                AppsListBox.ItemsSource = filteredApps;
            }
            else
            {
                // Εμφάνισε όλες τις εφαρμογές αν το κείμενο αναζήτησης είναι κενό
                AppsListBox.ItemsSource = apps;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text;
            if (!string.IsNullOrEmpty(searchText))
            {
                // Φίλτραρε την λίστα των εφαρμογών
                var filteredApps = apps.Where(app => app.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                AppsListBox.ItemsSource = filteredApps;
            }
            else
            {
                // Εμφάνισε όλες τις εφαρμογές αν το κείμενο αναζήτησης είναι κενό
                AppsListBox.ItemsSource = apps;
            }
        }
    }

    public class AppInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string PackageName { get; set; }
    }
}
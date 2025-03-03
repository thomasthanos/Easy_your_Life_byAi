using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace MyApp
{
    public partial class InstallAppsWindow : Window
    {
        private List<AppInfo> apps; // Λίστα για αποθήκευση των εφαρμογών
        private bool isDownloading = false; // Flag για έλεγχο αν υπάρχει λήψη σε εξέλιξη

        public InstallAppsWindow()
        {
            InitializeComponent();
            LoadAppsAsync();  // Ασύγχρονη φόρτωση των εφαρμογών

            // Προσθήκη event handlers για το κλείσιμο και την ελαχιστοποίηση του παραθύρου
            this.Closing += InstallAppsWindow_Closing;
            this.StateChanged += InstallAppsWindow_StateChanged;
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
        }

        private void InstallAppsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isDownloading)
            {
                var result = MessageBox.Show("Η λήψη βρίσκεται σε εξέλιξη. Είστε βέβαιοι ότι θέλετε να κλείσετε την εφαρμογή;", "Λήψη σε Εξέλιξη", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true; // Ακύρωση του κλεισίματος αν ο χρήστης επιλέξει "Όχι"
                }
            }
        }

        private void InstallAppsWindow_StateChanged(object sender, EventArgs e)
        {
            if (isDownloading && this.WindowState == WindowState.Minimized)
            {
                var result = MessageBox.Show("Η λήψη βρίσκεται σε εξέλιξη. Είστε βέβαιοι ότι θέλετε να ελαχιστοποιήσετε την εφαρμογή;", "Λήψη σε Εξέλιξη", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    this.WindowState = WindowState.Normal; // Επαναφορά του παραθύρου σε κανονική κατάσταση
                }
            }
        }
        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private async void LoadAppsAsync()
        {
            try
            {
                // Εμφάνιση προσωρινού μηνύματος ή loading indicator
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

                // Ασύγχρονη ανάκτηση έκδοσης για κάθε πακέτο
                var tasks = apps.Select(async app =>
                {
                    app.Version = await GetPackageVersionAsync(app.PackageName);
                }).ToList();

                await Task.WhenAll(tasks);  // Περιμένουμε όλες τις εργασίες να ολοκληρωθούν

                // Ταξινόμηση της λίστας με αλφαβητική σειρά
                apps.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));

                // Ενημέρωση του ListBox στο UI thread
                await Dispatcher.InvokeAsync(() =>
                {
                    AppsListBox.ItemsSource = apps;
                });
            }
            catch (Exception ex)
            {
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
                        Arguments = $"list --id {packageName}",
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
                        return "Εγκαταστάθηκε"; // Αν βρεθεί το πακέτο, επιστρέφουμε "Εγκαταστάθηκε"
                    }
                }

                return ""; // Αν δεν βρεθεί, επιστρέφουμε κενό
            }
            catch
            {
                return ""; // Σε περίπτωση σφάλματος, επιστρέφουμε κενό
            }
        }


        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (isDownloading)
            {
                var result = MessageBox.Show("Η λήψη βρίσκεται σε εξέλιξη. Είστε βέβαιοι ότι θέλετε να ελαχιστοποιήσετε την εφαρμογή;", "Λήψη σε Εξέλιξη", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    return; // Ακύρωση της ελαχιστοποίησης αν ο χρήστης επιλέξει "Όχι"
                }
            }
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (isDownloading)
            {
                var result = MessageBox.Show("Η λήψη βρίσκεται σε εξέλιξη. Είστε βέβαιοι ότι θέλετε να τερματίσετε την εφαρμογή;",
                    "Λήψη σε Εξέλιξη",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                {
                    return; // Ακύρωση του τερματισμού αν ο χρήστης επιλέξει "Όχι"
                }
            }
            Application.Current.Shutdown(); // Τερματισμός της εφαρμογής
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsWingetInstalled())
            {
                await InstallWingetAsync(); // Εγκατάσταση του winget αν δεν είναι εγκατεστημένο
                return;
            }

            var selectedApp = AppsListBox.SelectedItem as AppInfo;
            if (selectedApp != null)
            {
                try
                {
                    isDownloading = true; // Ορισμός του flag σε true κατά την έναρξη της λήψης
                    DownloadProgressBar.IsIndeterminate = false; // Απενεργοποίηση της απροσδιόριστης κατάστασης
                    DownloadProgressBar.Value = 0; // Αρχικοποίηση της τιμής του ProgressBar
                    DownloadProgressBar.Maximum = 100; // Ορισμός της μέγιστης τιμής
                    DownloadButton.IsEnabled = false;

                    // Δημιουργία Timer για περιοδική ενημέρωση του ProgressBar
                    var timer = new System.Timers.Timer(500); // Ανανεώνεται κάθε 500ms
                    timer.Elapsed += (s, ev) =>
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (DownloadProgressBar.Value < 95)
                            {
                                DownloadProgressBar.Value += 5; // Αύξηση κατά 5% κάθε 500ms
                            }
                        }));
                    };
                    timer.Start();

                    string errorOutput = string.Empty;
                    bool installationSuccess = false;

                    // Εκτέλεση της διεργασίας σε ξεχωριστό νήμα
                    await Task.Run(() =>
                    {
                        var process = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = "winget",
                                Arguments = $"install --id {selectedApp.PackageName} --silent --accept-package-agreements",
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                StandardOutputEncoding = System.Text.Encoding.UTF8
                            }
                        };

                        process.Start();
                        errorOutput = process.StandardError.ReadToEnd(); // Διαβάζουμε την έξοδο του σφάλματος
                        process.WaitForExit();

                        installationSuccess = process.ExitCode == 0; // Ελέγχουμε αν η εγκατάσταση ήταν επιτυχής
                    });

                    timer.Stop();

                    await Dispatcher.InvokeAsync(() =>
                    {
                        if (installationSuccess)
                        {
                            DownloadProgressBar.Value = 100;
                            var customMessageBox = new CustomMessageBox($"Η εφαρμογή {selectedApp.Name} εγκαταστάθηκε επιτυχώς!", "Εγκατάσταση Ολοκληρώθηκε", IconType.Success);
                            customMessageBox.ShowDialog();
                        }
                        else
                        {
                            var customMessageBox = new CustomMessageBox($"Σφάλμα κατά την εγκατάσταση της εφαρμογής {selectedApp.Name}: {errorOutput}", "Σφάλμα", IconType.Error, errorOutput);
                            customMessageBox.ShowDialog();
                        }
                        DownloadButton.IsEnabled = true;
                        isDownloading = false; // Ορισμός του flag σε false μετά το τέλος της λήψης
                    });
                    if (installationSuccess)
                    {
                        DownloadProgressBar.Value = 100;
                        selectedApp.Version = await GetPackageVersionAsync(selectedApp.PackageName); // Ενημέρωση της έκδοσης
                        AppsListBox.Items.Refresh(); // Ανανεώνει τη λίστα στο UI
                    }

                }

                catch (Exception ex)
                {
                    await Dispatcher.InvokeAsync(() =>
                    {
                        var customMessageBox = new CustomMessageBox($"Σφάλμα: {ex.Message}", "Σφάλμα", IconType.Error, ex.StackTrace);
                        customMessageBox.ShowDialog();
                        DownloadButton.IsEnabled = true;
                        isDownloading = false; // Ορισμός του flag σε false σε περίπτωση σφάλματος
                    });
                }
                finally
                {
                    DownloadProgressBar.IsIndeterminate = false;
                }
            }
            else
            {
                var customMessageBox = new CustomMessageBox("Παρακαλώ επιλέξτε μια εφαρμογή για εγκατάσταση.", "Δεν Επιλέχθηκε Εφαρμογή", IconType.Info);
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
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell",
                        Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"Add-AppxPackage -RegisterByFamilyName -MainPackage Microsoft.DesktopAppInstaller_8wekyb3d8bbwe\"",
                        Verb = "runas", // Εκτέλεση ως διαχειριστής
                        UseShellExecute = true
                    }
                };

                process.Start();
                await process.WaitForExitAsync();

                if (process.ExitCode == 0)
                {
                    var customMessageBox = new CustomMessageBox("Το Winget εγκαταστάθηκε επιτυχώς!", "Εγκατάσταση Ολοκληρώθηκε");
                    customMessageBox.ShowDialog();
                }
                else
                {
                    var customMessageBox = new CustomMessageBox("Η εγκατάσταση του Winget απέτυχε. Παρακαλώ εγκαταστήστε το χειροκίνητα.", "Σφάλμα");
                    customMessageBox.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                var customMessageBox = new CustomMessageBox($"Σφάλμα κατά την εγκατάσταση του Winget: {ex.Message}", "Σφάλμα");
                customMessageBox.ShowDialog();
            }
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
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Κλείνει το τρέχον παράθυρο και επιστρέφει στο προηγούμενο
            this.Close();
        }
    }

    public class AppInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string PackageName { get; set; }
    }
}
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace MyApp
{
    public partial class PublicInstallWindow : Window
    {
        private readonly Dictionary<string, string> _installExecutables = new Dictionary<string, string>
        {
            { "Photoshop", "Set-up.exe" },
            { "PremierePro", "Set-up.exe" },
            { "MediaEncoder", "Set-up.exe" },
            { "LightroomClassic", "Set-up.exe" },
            { "Office2024", "OInstall_x64.exe" },
            { "ClipStudio", "clipstudio_crack.exe" },
            { "DaVinci_Resolve", "Install Resolve 19.1.3.exe" }
        };

        private CancellationTokenSource _cancellationTokenSource;
        private bool _isPaused = false;

        public PublicInstallWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void clipstudio_Click(object sender, RoutedEventArgs e)
        {
            string sourcePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "Clip_Studio_Paint", "CLIPStudioPaint.exe");
            string destinationPath = @"C:\Program Files\CELSYS\CLIP STUDIO 1.5\CLIP STUDIO PAINT\CLIPStudioPaint.exe";

            try
            {
                if (File.Exists(destinationPath))
                {
                    File.Delete(destinationPath);
                }

                File.Copy(sourcePath, destinationPath);
                new CustomMessageBox("Η αντικατάσταση του αρχείου ολοκληρώθηκε επιτυχώς!", "Επιτυχία", IconType.Success).ShowDialog();
            }
            catch (Exception ex)
            {
                new CustomMessageBox($"Προέκυψε σφάλμα: {ex.Message}", "Σφάλμα", IconType.Error).ShowDialog();
            }
        }

        private void davinci_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string basePath = @"C:\Program Files\TEAM R2R\DVREMU2 Manager\commands";

                string[] commands = new string[]
                {
                    "DVREMU2 - Install Emulator.cmd",
                    "DVREMU2 - Renew Emulator License.cmd",
                    "DVREMU2 - Test License and Emulator.cmd",
                    "DVREMU2 - Use Shared VCRuntime.cmd"
                };

                for (int i = 0; i < commands.Length; i++)
                {
                    string fullPath = Path.Combine(basePath, commands[i]);
                    ProcessStartInfo processInfo = new ProcessStartInfo
                    {
                        FileName = fullPath,
                        WorkingDirectory = basePath,
                        UseShellExecute = true,
                        CreateNoWindow = false
                    };

                    new CustomMessageBox($"Εκτέλεση εντολής {i + 1} από {commands.Length}: {commands[i]}", "Πληροφορίες", IconType.Info).ShowDialog();

                    Process process = Process.Start(processInfo);
                    process.WaitForExit();
                }

                new CustomMessageBox("Όλες οι εντολές ολοκληρώθηκαν επιτυχώς!", "Επιτυχία", IconType.Success).ShowDialog();
            }
            catch (Exception ex)
            {
                new CustomMessageBox($"Σφάλμα κατά την εκτέλεση: {ex.Message}", "Σφάλμα", IconType.Error).ShowDialog();
            }
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            string fileUrl = "";
            string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            if (sender == PhotoshopButton)
            {
                fileUrl = "https://drive.usercontent.google.com/download?id=12Ee6r0mTziMHi4I5N2J6CsV4Hyqcj9lw&export=download&authuser=0&confirm=t&uuid=d77760ad-9e12-438e-991f-745c5c508928&at=AEz70l7KibMmZL_RFs-3o8UVlaov:1740762499506";
                savePath = Path.Combine(savePath, "Photoshop.zip");
            }
            else if (sender == PremiereProButton)
            {
                fileUrl = "https://drive.usercontent.google.com/download?id=18pHzczi7FE9nsNoAEWoYqC26Ru-kASHQ&export=download&authuser=0&confirm=t&uuid=d77760ad-9e12-438e-991f-745c5c508928&at=AEz70l7KibMmZL_RFs-3o8UVlaov:1740762499506";
                savePath = Path.Combine(savePath, "PremierePro.zip");
            }
            else if (sender == MediaEncoderButton)
            {
                fileUrl = "https://drive.usercontent.google.com/download?id=1E0tFoV_4DT3LpgEQpd-iIRyN3Z_tFsSj&export=download&authuser=0&confirm=t&uuid=d77760ad-9e12-438e-991f-745c5c508928&at=AEz70l7KibMmZL_RFs-3o8UVlaov:1740762499506";
                savePath = Path.Combine(savePath, "MediaEncoder.zip");
            }
            else if (sender == LightroomClassicButton)
            {
                fileUrl = "https://drive.usercontent.google.com/download?id=1iGYI6c0VcvIgLXhvItzl2jfAZqEMRxif&export=download&authuser=0&confirm=t&uuid=d77760ad-9e12-438e-991f-745c5c508928&at=AEz70l7KibMmZL_RFs-3o8UVlaov:1740762499506";
                savePath = Path.Combine(savePath, "LightroomClassic.zip");
            }
            else if (sender == IllustratorButton)
            {
                fileUrl = "https://drive.usercontent.google.com/download?id=1dMhoBS7Cp9W_qzKfM1HqI1WOmAhWsaX2&export=download&authuser=0&confirm=t&uuid=d77760ad-9e12-438e-991f-745c5c508928&at=AEz70l7KibMmZL_RFs-3o8UVlaov:1740762499506";
                savePath = Path.Combine(savePath, "Illustrator.zip");
            }
            else if (sender == Office2024Button)
            {
                fileUrl = "https://drive.usercontent.google.com/download?id=1Ur66w8CIGlKScy21ZoNQUzyidZPsJD39&export=download&authuser=0&confirm=t&uuid=d77760ad-9e12-438e-991f-745c5c508928&at=AEz70l7KibMmZL_RFs-3o8UVlaov:1740762499506";
                savePath = Path.Combine(savePath, "Office2024.zip");
            }
            else if (sender == DaVinci_Resolve)
            {
                fileUrl = "https://drive.usercontent.google.com/download?id=12tzYDLQ6kthDIdAZuAtSpeeoi58uxtNn&export=download&authuser=0&confirm=t&uuid=d77760ad-9e12-438e-991f-745c5c508928&at=AEz70l7KibMmZL_RFs-3o8UVlaov:1740762499506";
                savePath = Path.Combine(savePath, "DaVinci_Resolve.zip");
            }
            else if (sender == ClipStudioButton)
            {
                fileUrl = "https://drive.usercontent.google.com/download?id=1-5uPMgiPAf_pDtxyvD5MDb1cMTT6XFK_&export=download&authuser=0&confirm=t&uuid=d77760ad-9e12-438e-991f-745c5c508928&at=AEz70l7KibMmZL_RFs-3o8UVlaov:1740762499506";
                savePath = Path.Combine(savePath, "ClipStudio.zip");
            }

            await DownloadFileWithProgressAsync(fileUrl, savePath);

            string extractPath = Path.Combine(Path.GetDirectoryName(savePath), Path.GetFileNameWithoutExtension(savePath));
            Directory.CreateDirectory(extractPath);

            string zipPassword = "123";
            bool extractionSuccess = ExtractZipFile(savePath, extractPath, zipPassword);

            if (extractionSuccess)
            {
                File.Delete(savePath);
                new CustomMessageBox("Η εξαγωγή ολοκληρώθηκε και το ZIP αρχείο διαγράφηκε!", "Επιτυχία", IconType.Success).ShowDialog();

                string zipName = Path.GetFileNameWithoutExtension(savePath);
                if (_installExecutables.ContainsKey(zipName))
                {
                    string executableName = _installExecutables[zipName];
                    string executablePath = Path.Combine(extractPath, executableName);

                    if (File.Exists(executablePath))
                    {
                        RunExecutableAsAdmin(executablePath);
                        new CustomMessageBox($"Η εγκατάσταση του {zipName} ξεκίνησε!", "Πληροφορίες", IconType.Info).ShowDialog();
                    }
                    else
                    {
                        new CustomMessageBox($"Το εκτελέσιμο {executableName} δεν βρέθηκε στον φάκελο {extractPath}!", "Σφάλμα", IconType.Error).ShowDialog();
                    }
                }
                else
                {
                    new CustomMessageBox($"Δεν βρέθηκε εκτελέσιμο εγκατάστασης για το {zipName}!", "Σφάλμα", IconType.Error).ShowDialog();
                }
            }
            else
            {
                new CustomMessageBox("Η εξαγωγή απέτυχε! Ελέγξτε αν ο κωδικός είναι σωστός.", "Σφάλμα", IconType.Error).ShowDialog();
            }
        }

        private void RunExecutableAsAdmin(string executablePath)
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = executablePath,
                    UseShellExecute = true,
                    Verb = "runas"
                };

                using (Process process = Process.Start(processStartInfo))
                {
                    process.WaitForExit();
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                new CustomMessageBox($"Αποτυχία εκκίνησης διαδικασίας: {ex.Message}", "Σφάλμα", IconType.Error).ShowDialog();
            }
        }

        private async Task DownloadFileWithProgressAsync(string fileUrl, string savePath)
        {
            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = _cancellationTokenSource.Token;

                using (var httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true }))
                {
                    var response = await httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                    response.EnsureSuccessStatusCode();

                    var totalBytes = response.Content.Headers.ContentLength ?? -1L;
                    var canReportProgress = totalBytes != -1;

                    using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        var totalBytesRead = 0L;
                        var buffer = new byte[8192];
                        var isMoreToRead = true;

                        do
                        {
                            if (_isPaused)
                            {
                                await Task.Delay(500, cancellationToken); // Περιμένει μέχρι να ξαναγίνει resume
                                continue;
                            }

                            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                            if (bytesRead == 0)
                            {
                                isMoreToRead = false;
                            }
                            else
                            {
                                await fileStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);

                                totalBytesRead += bytesRead;

                                if (canReportProgress)
                                {
                                    var progressPercentage = (int)((double)totalBytesRead / totalBytes * 100);
                                    Dispatcher.Invoke(() => DownloadProgressBar.Value = progressPercentage);
                                }
                            }
                        } while (isMoreToRead && !cancellationToken.IsCancellationRequested);
                    }
                }

                if (!cancellationToken.IsCancellationRequested)
                {
                    new CustomMessageBox("Η λήψη ολοκληρώθηκε!", "Επιτυχία", IconType.Success).ShowDialog();
                }
            }
            catch (OperationCanceledException)
            {
                new CustomMessageBox("Η λήψη τέθηκε σε παύση.", "Πληροφορίες", IconType.Info).ShowDialog();
            }
            catch (Exception ex)
            {
                new CustomMessageBox($"Σφάλμα κατά τη λήψη του αρχείου: {ex.Message}", "Σφάλμα", IconType.Error).ShowDialog();
            }
        }

        private bool ExtractZipFile(string zipPath, string extractPath, string password)
        {
            try
            {
                string sevenZipPath = "\"C:\\Program Files (x86)\\Kolokithes A.E\\Make your life easier\\7-Zip\\7z.exe\"";

                if (!File.Exists(zipPath))
                {
                    new CustomMessageBox("Το ZIP αρχείο δεν υπάρχει!", "Σφάλμα", IconType.Error).ShowDialog();
                    return false;
                }

                bool hasPassword = CheckIfZipHasPassword(zipPath, sevenZipPath);

                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = sevenZipPath,
                    Arguments = hasPassword ? $"x -p{password} -o\"{extractPath}\" \"{zipPath}\"" : $"x -o\"{extractPath}\" \"{zipPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(processStartInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        if (error.Contains("Wrong password"))
                        {
                            new CustomMessageBox("Λάθος κωδικός για το ZIP αρχείο!", "Σφάλμα", IconType.Error).ShowDialog();
                            string downloadFolder = Path.GetDirectoryName(zipPath);
                            Process.Start("explorer.exe", downloadFolder);
                            return false;
                        }
                        else if (error.Contains("Unexpected end of archive") || error.Contains("CRC Failed"))
                        {
                            new CustomMessageBox("Το ZIP αρχείο είναι κατεστραμμένο ή ελλιπές. Παρακαλώ κατεβάστε το ξανά.", "Σφάλμα", IconType.Error).ShowDialog();
                        }
                        else
                        {
                            new CustomMessageBox($"Σφάλμα κατά την εξαγωγή του αρχείου: {error}", "Σφάλμα", IconType.Error).ShowDialog();
                        }
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                new CustomMessageBox($"Σφάλμα κατά την εξαγωγή του αρχείου: {ex.Message}", "Σφάλμα", IconType.Error).ShowDialog();
                return false;
            }
        }

        private bool CheckIfZipHasPassword(string zipPath, string sevenZipPath)
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = sevenZipPath,
                    Arguments = $"l -slt \"{zipPath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(processStartInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    return output.Contains("Encrypted = +");
                }
            }
            catch
            {
                return false;
            }
        }

        private void PauseResumeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isPaused)
            {
                _isPaused = false;
                PauseResumeButton.Content = "Pause";
            }
            else
            {
                _isPaused = true;
                PauseResumeButton.Content = "Resume";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e) => this.Close();
        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            string[] foldersToDelete = new string[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "Photoshop"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "PremierePro"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "MediaEncoder"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "LightroomClassic"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "Illustrator"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "Office2024"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "DaVinci_Resolve"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "ClipStudio")
            };

            foreach (var folder in foldersToDelete)
            {
                try
                {
                    if (Directory.Exists(folder))
                    {
                        Directory.Delete(folder, true);
                    }
                }
                catch
                {
                }
            }

            Application.Current.Shutdown();
        }
    }
}
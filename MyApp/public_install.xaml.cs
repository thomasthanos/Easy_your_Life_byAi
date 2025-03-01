using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

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
            { "ClipStudio", "clipstudio_crack.exe" }
        };

        public PublicInstallWindow()
        {
            InitializeComponent();
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            string fileUrl = "";
            string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            // Καθορισμός URL και διαδρομής για κάθε κουμπί
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
            else if (sender == ClipStudioButton)
            {
                fileUrl = "https://cdn.discordapp.com/attachments/1203478665304866887/1338317398692069378/Clip_Studio_Paint.zip?ex=67c30874&is=67c1b6f4&hm=1c6fbdee037fdda529cc45c5355667f22a07bba2f17977eae52c953beef52c71&";
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
                new CustomMessageBox("Extraction completed and ZIP file deleted!", "Success", IconType.Success).ShowDialog();

                string zipName = Path.GetFileNameWithoutExtension(savePath);
                if (_installExecutables.ContainsKey(zipName))
                {
                    string executableName = _installExecutables[zipName];
                    string executablePath = Path.Combine(extractPath, executableName);

                    if (File.Exists(executablePath))
                    {
                        RunExecutableAsAdmin(executablePath);
                        new CustomMessageBox($"Installation of {zipName} has started!", "Info", IconType.Info).ShowDialog();
                    }
                    else
                    {
                        new CustomMessageBox($"Executable {executableName} not found in {extractPath}!", "Error", IconType.Error).ShowDialog();
                    }
                }
                else
                {
                    new CustomMessageBox($"No installation executable found for {zipName}!", "Error", IconType.Error).ShowDialog();
                }
            }
            else
            {
                new CustomMessageBox("Extraction failed! Check if the password is correct.", "Error", IconType.Error).ShowDialog();
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
                new CustomMessageBox($"Failed to start process: {ex.Message}", "Error", IconType.Error).ShowDialog();
            }
        }

        private async Task DownloadFileWithProgressAsync(string fileUrl, string savePath)
        {
            try
            {
                using (var httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true }))
                {
                    var response = await httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead);
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
                            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                            if (bytesRead == 0)
                            {
                                isMoreToRead = false;
                            }
                            else
                            {
                                await fileStream.WriteAsync(buffer, 0, bytesRead);

                                totalBytesRead += bytesRead;

                                if (canReportProgress)
                                {
                                    var progressPercentage = (int)((double)totalBytesRead / totalBytes * 100);
                                    Dispatcher.Invoke(() => DownloadProgressBar.Value = progressPercentage);
                                }
                            }
                        } while (isMoreToRead);
                    }
                }

                new CustomMessageBox("Download completed!", "Success", IconType.Success).ShowDialog();
            }
            catch (Exception ex)
            {
                new CustomMessageBox($"Error downloading file: {ex.Message}", "Error", IconType.Error).ShowDialog();
            }
        }

        private bool ExtractZipFile(string zipPath, string extractPath, string password)
        {
            try
            {
                string sevenZipPath = @"C:\Program Files (x86)\Kolokithes A.E\myapp\7-Zip\7z.exe";

                if (!File.Exists(zipPath))
                {
                    new CustomMessageBox("ZIP file does not exist!", "Error", IconType.Error).ShowDialog();
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
                            new CustomMessageBox("Wrong password for the ZIP file!", "Error", IconType.Error).ShowDialog();
                        }
                        else if (error.Contains("Unexpected end of archive") || error.Contains("CRC Failed"))
                        {
                            new CustomMessageBox("The ZIP file is corrupted or incomplete. Please download it again.", "Error", IconType.Error).ShowDialog();
                        }
                        else
                        {
                            new CustomMessageBox($"Error extracting file: {error}", "Error", IconType.Error).ShowDialog();
                        }
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                new CustomMessageBox($"Error extracting file: {ex.Message}", "Error", IconType.Error).ShowDialog();
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

        // Event handlers για τα κουμπιά
        private void BackButton_Click(object sender, RoutedEventArgs e) => this.Close();
        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;
        private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}
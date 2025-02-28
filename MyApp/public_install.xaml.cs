using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace MyApp
{
    public partial class PublicInstallWindow : Window
    {
        public PublicInstallWindow()
        {
            InitializeComponent();
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            string fileUrl = "";
            string savePath = "E:\\Downloads\\";

            // Determine which button was clicked and set the appropriate file URL and save path
            if (sender == PhotoshopButton)
            {
                fileUrl = "https://drive.usercontent.google.com/download?id=12Ee6r0mTziMHi4I5N2J6CsV4Hyqcj9lw&export=download&authuser=0&confirm=t&uuid=d77760ad-9e12-438e-991f-745c5c508928&at=AEz70l7KibMmZL_RFs-3o8UVlaov:1740762499506";
                savePath += "Photoshop.zip";
            }
            else if (sender == PremiereProButton)
            {
                fileUrl = "https://drive.usercontent.google.com/download?id=18pHzczi7FE9nsNoAEWoYqC26Ru-kASHQ&export=download&authuser=0&confirm=t&uuid=d77760ad-9e12-438e-991f-745c5c508928&at=AEz70l7KibMmZL_RFs-3o8UVlaov:1740762499506";
                savePath += "PremierePro.zip";
            }
            else if (sender == MediaEncoderButton)
            {
                fileUrl = "https://drive.usercontent.google.com/download?id=1E0tFoV_4DT3LpgEQpd-iIRyN3Z_tFsSj&export=download&authuser=0&confirm=t&uuid=d77760ad-9e12-438e-991f-745c5c508928&at=AEz70l7KibMmZL_RFs-3o8UVlaov:1740762499506";
                savePath += "MediaEncoder.zip";
            }
            else if (sender == LightroomClassicButton)
            {
                fileUrl = "https://drive.usercontent.google.com/download?id=1iGYI6c0VcvIgLXhvItzl2jfAZqEMRxif&export=download&authuser=0&confirm=t&uuid=d77760ad-9e12-438e-991f-745c5c508928&at=AEz70l7KibMmZL_RFs-3o8UVlaov:1740762499506";
                savePath += "LightroomClassic.zip";
            }
            else if (sender == IllustratorButton)
            {
                fileUrl = "https://drive.usercontent.google.com/download?id=1dMhoBS7Cp9W_qzKfM1HqI1WOmAhWsaX2&export=download&authuser=0&confirm=t&uuid=d77760ad-9e12-438e-991f-745c5c508928&at=AEz70l7KibMmZL_RFs-3o8UVlaov:1740762499506";
                savePath += "Illustrator.zip";
            }
            else if (sender == Office2024Button)
            {
                fileUrl = "https://drive.usercontent.google.com/download?id=1Ur66w8CIGlKScy21ZoNQUzyidZPsJD39&export=download&authuser=0&confirm=t&uuid=d77760ad-9e12-438e-991f-745c5c508928&at=AEz70l7KibMmZL_RFs-3o8UVlaov:1740762499506";
                savePath += "Office2024.zip";
            }
            else if (sender == ClipStudioButton)
            {
                fileUrl = "https://cdn.discordapp.com/attachments/1203478665304866887/1338317398692069378/Clip_Studio_Paint.zip?ex=67c30874&is=67c1b6f4&hm=1c6fbdee037fdda529cc45c5355667f22a07bba2f17977eae52c953beef52c71&"; // Βάλε το σωστό URL εδώ
                savePath += "ClipStudio.zip";
            }
            await DownloadFileWithProgressAsync(fileUrl, savePath);
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

                MessageBox.Show("Download completed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handlers για τα κουμπιά Minimize, Back, Close
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
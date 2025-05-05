using MyApp;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace multitool
{
    public partial class ClearTempWindow : Window
    {
        private long totalSizeDeleted = 0;
        private int totalFilesDeleted = 0;
        private int totalFoldersDeleted = 0;

        public ClearTempWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            Loaded += async (s, e) => await StartCleaningAsync();
        }

        private void AppendOutput(string text, SolidColorBrush color)
        {
            if (string.IsNullOrEmpty(text)) return;
            text = text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            Dispatcher.InvokeAsync(() =>
            {
                Paragraph paragraph = new Paragraph(new Run(text))
                {
                    Foreground = color,
                    Margin = new Thickness(0)
                };
                OutputRichTextBox.Document.Blocks.Add(paragraph);
                OutputRichTextBox.ScrollToEnd();
            }, DispatcherPriority.Background);
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private async Task StartCleaningAsync()
        {
            AppendOutput("Starting to clear temp folders...", Brushes.White);

            var progress = new Progress<string>(message => AppendOutput(message, Brushes.White));

            await Task.Run(async () =>
            {
                AppendOutput("Stopping Windows Update service...", Brushes.Yellow);
                await StopWindowsUpdateServiceAsync();

                AppendOutput("Cleaning folders...", Brushes.White);

                var tasks = new List<Task<(int, long)>>();
                tasks.Add(ClearFolderWithDetails(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp"), progress));
                tasks.Add(ClearFolderWithDetails(Environment.GetEnvironmentVariable("TEMP"), progress));
                tasks.Add(ClearFolderWithDetails(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch"), progress));
                tasks.Add(ClearFolderWithDetails(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SoftwareDistribution", "Download"), progress));
                tasks.Add(ClearFolderWithDetails(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Windows.old"), progress));
                tasks.Add(ClearFolderWithDetails(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Logs", "CBS"), progress));

                // Περιμένουμε όλες τις εργασίες διαγραφής να ολοκληρωθούν
                var results = await Task.WhenAll(tasks);

                AppendOutput("All folder cleaning tasks completed.", Brushes.Green);

                await ClearRecycleBinAsync(progress);

                AppendOutput("Starting Windows Update service...", Brushes.Yellow);
                await StartWindowsUpdateServiceAsync();

                AppendOutput("All cleaning tasks completed. Displaying results...", Brushes.Green);

                // Ενημέρωση UI μόνο αφού ολοκληρωθούν όλες οι εργασίες
                await Dispatcher.InvokeAsync(() =>
                {
                    OutputRichTextBox.Document.Blocks.Clear();
                    UpdateFinalResult(
                        results[0], // Temp
                        results[1], // %TEMP%
                        results[2], // Prefetch
                        results[3], // SoftwareDistribution
                        results[4], // Windows.old
                        results[5]  // CBS Logs
                    );
                });
            });
        }

        private async Task StopWindowsUpdateServiceAsync()
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "net";
                    process.StartInfo.Arguments = "stop wuauserv";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.Start();

                    await process.WaitForExitAsync();
                }
            }
            catch (Exception ex)
            {
                AppendOutput($"Error stopping Windows Update service: {ex.Message}", Brushes.Red);
            }
        }

        private async Task StartWindowsUpdateServiceAsync()
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "net";
                    process.StartInfo.Arguments = "start wuauserv";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.Start();

                    await process.WaitForExitAsync();
                }
            }
            catch (Exception ex)
            {
                AppendOutput($"Error starting Windows Update service: {ex.Message}", Brushes.Red);
            }
        }

        private async Task ClearRecycleBinAsync(IProgress<string> progress)
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "Clear-RecycleBin -Force",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process())
                {
                    process.StartInfo = processStartInfo;
                    process.Start();

                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();

                    await process.WaitForExitAsync();

                    if (!string.IsNullOrEmpty(output)) progress.Report(output);
                    if (!string.IsNullOrEmpty(error)) progress.Report(error);
                }
            }
            catch (Exception ex)
            {
                progress.Report($"Error clearing Recycle Bin: {ex.Message}");
            }
        }

        private async Task<(int, long)> ClearFolderWithDetails(string folderPath, IProgress<string> progress)
        {
            int deletedFilesInThisFolder = 0;
            long deletedSizeInThisFolder = 0;

            try
            {
                if (Directory.Exists(folderPath))
                {
                    progress.Report($"Cleaning: {folderPath}");
                    DirectoryInfo directory = new DirectoryInfo(folderPath);

                    // Διαγραφή αρχείων
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        try
                        {
                            file.Attributes = FileAttributes.Normal;
                            long fileSize = file.Length;
                            Interlocked.Add(ref totalSizeDeleted, fileSize);
                            Interlocked.Add(ref deletedSizeInThisFolder, fileSize);
                            Interlocked.Increment(ref totalFilesDeleted);
                            Interlocked.Increment(ref deletedFilesInThisFolder);
                            file.Delete();
                        }
                        catch (Exception ex)
                        {
                            progress.Report($"Error deleting file: {file.FullName} - {ex.Message}");
                        }
                        await Task.Yield();
                    }

                    // Διαγραφή φακέλων
                    foreach (DirectoryInfo dir in directory.GetDirectories())
                    {
                        try
                        {
                            dir.Attributes = FileAttributes.Normal;
                            long dirSize = await DeleteDirectoryRecursivelyAndCalculateSizeAsync(dir, progress);
                            Interlocked.Add(ref totalSizeDeleted, dirSize);
                            Interlocked.Add(ref deletedSizeInThisFolder, dirSize);
                            Interlocked.Increment(ref totalFoldersDeleted);
                            progress.Report($"Deleted directory: {dir.FullName} (Size: {FormatSize(dirSize)})");
                        }
                        catch (Exception ex)
                        {
                            progress.Report($"Error deleting directory: {dir.FullName} - {ex.Message}");
                        }
                        await Task.Yield();
                    }
                }
            }
            catch (Exception ex)
            {
                progress.Report($"Error clearing folder: {folderPath} - {ex.Message}");
            }

            return (deletedFilesInThisFolder, deletedSizeInThisFolder);
        }

        private async Task<long> DeleteDirectoryRecursivelyAndCalculateSizeAsync(DirectoryInfo directory, IProgress<string> progress)
        {
            long size = 0;

            // Διαγραφή αρχείων
            foreach (FileInfo file in directory.GetFiles())
            {
                try
                {
                    file.Attributes = FileAttributes.Normal;
                    size += file.Length;
                    file.Delete();
                }
                catch (Exception ex)
                {
                    progress.Report($"Error deleting file: {file.FullName} - {ex.Message}");
                }
                await Task.Yield();
            }

            // Διαγραφή υποφακέλων
            foreach (DirectoryInfo subDir in directory.GetDirectories())
            {
                try
                {
                    subDir.Attributes = FileAttributes.Normal;
                    size += await DeleteDirectoryRecursivelyAndCalculateSizeAsync(subDir, progress);
                }
                catch (Exception ex)
                {
                    progress.Report($"Error deleting subdirectory: {subDir.FullName} - {ex.Message}");
                }
                await Task.Yield();
            }

            try
            {
                directory.Delete();
            }
            catch (Exception ex)
            {
                progress.Report($"Error deleting directory: {directory.FullName} - {ex.Message}");
            }

            return size;
        }

        private string FormatSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        private void UpdateFinalResult((int, long) resultTemp, (int, long) resultEnvTemp, (int, long) resultPrefetch,
            (int, long) resultSoftwareDistribution, (int, long) resultWindowsOld, (int, long) resultCBSLogs)
        {
            FinalResultBorder.Visibility = Visibility.Visible;

            TempResultText.Text = $"C:\\Windows\\Temp - Deleted {resultTemp.Item1} files, Size: {FormatSize(resultTemp.Item2)}";
            EnvTempResultText.Text = $"%TEMP% - Deleted {resultEnvTemp.Item1} files, Size: {FormatSize(resultEnvTemp.Item2)}";
            PrefetchResultText.Text = $"C:\\Windows\\Prefetch - Deleted {resultPrefetch.Item1} files, Size: {FormatSize(resultPrefetch.Item2)}";
            SoftwareDistributionResultText.Text = $"C:\\Windows\\SoftwareDistribution\\Download - 🗑️ {resultSoftwareDistribution.Item1} files, Size: {FormatSize(resultSoftwareDistribution.Item2)}";
            WindowsOldResultText.Text = $"C:\\Windows.old - Deleted {resultWindowsOld.Item1} files, Size: {FormatSize(resultWindowsOld.Item2)}";
            CBSLogsResultText.Text = $"C:\\Windows\\Logs\\CBS - Deleted {resultCBSLogs.Item1} files, Size: {FormatSize(resultCBSLogs.Item2)}";
            TotalFilesText.Text = $"Total files deleted: {totalFilesDeleted}";
            TotalFoldersText.Text = $"Total folders deleted: {totalFoldersDeleted}";
            TotalSizeText.Text = $"Total size deleted: {FormatSize(totalSizeDeleted)}";
        }
    }

    public static class ProcessExtensions
    {
        public static Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<object>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(null);
            if (cancellationToken != default)
                cancellationToken.Register(() => tcs.TrySetCanceled());
            return tcs.Task;
        }
    }
}

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
using System.Collections.Generic;

namespace multitool
{
    public partial class ClearTempWindow : Window
    {
        private long totalSizeDeleted = 0;
        private int totalFilesDeleted = 0;
        private int totalFoldersDeleted = 0;
        // Indicates whether a cleaning process is currently running. While this flag is true
        // the window should not be allowed to close, otherwise background tasks that
        // still try to update the UI can cause the application to freeze.
        private bool isCleaning = false;

        public ClearTempWindow()
        {
            InitializeComponent();
            // Allow the window to be dragged when clicking anywhere on its surface
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            // Kick off the cleaning process once the window has finished loading
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
            // Prevent premature shutdown while cleaning is in progress. Trying to close
            // the window before the background tasks have completed can cause the
            // application to hang because those tasks will continue to post messages
            // to the dispatcher of a closed window. Instead, inform the user and
            // ignore the close request until cleaning finishes.
            if (isCleaning)
            {
                AppendOutput("Cleaning is still in progress, please wait until it completes.", Brushes.Yellow);
                return;
            }

            // Use Environment.Exit to terminate the application immediately without
            // waiting for the dispatcher to complete pending operations. Calling
            // Application.Current.Shutdown() can sometimes introduce a noticeable
            // delay if there are still queued messages to process.
            Environment.Exit(0);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the main window if it exists. In WPF applications
            // the main window can be accessed via Application.Current.MainWindow.
            // If a main window has been hidden (not closed), show it again.
            var mainWindow = Application.Current?.MainWindow;
            if (mainWindow != null && mainWindow != this)
            {
                mainWindow.Show();
            }
            this.Close();
        }

        private async Task StartCleaningAsync()
        {
            // Mark the beginning of a cleaning session. This flag prevents the user from
            // closing the window mid-cleaning, which could otherwise result in a freeze.
            isCleaning = true;
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

                // Πριν εμφανίσουμε τα τελικά αποτελέσματα, βεβαιωνόμαστε ότι έχουν ολοκληρωθεί
                // όλες οι εκκρεμείς ενημερώσεις προόδου που προστέθηκαν στον dispatcher με
                // Background προτεραιότητα. Χωρίς αυτό, μπορεί να εμφανιστούν επιπλέον
                // μηνύματα διαγραφής μετά την εμφάνιση των αποτελεσμάτων.
                await Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Background);

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

            // Ensure the cleaning flag is cleared once the background operation completes.
            // Placing this statement outside of the Task.Run scope guarantees it runs
            // regardless of exceptions that might occur within the cleaning loop.
            isCleaning = false;
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

                    // Wait up to 5 seconds for the service stop command to complete.
                    // Without a timeout the net.exe call may hang if the user lacks
                    // permission, causing the UI to freeze and results to never show.
                    var waitTask = process.WaitForExitAsync();
                    var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));
                    await Task.WhenAny(waitTask, timeoutTask);
                    // If timeout occurred and process is still running, kill it to avoid blocking.
                    if (!process.HasExited)
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch
                        {
                            // Ignore any errors killing the process; we'll proceed regardless.
                        }
                    }
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

                    // Wait up to 5 seconds for the service start command to complete.
                    // If the user does not have the required privileges the command may hang.
                    var waitTask = process.WaitForExitAsync();
                    var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));
                    await Task.WhenAny(waitTask, timeoutTask);
                    if (!process.HasExited)
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch
                        {
                            // Ignore any errors killing the process.
                        }
                    }
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
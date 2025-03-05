using MyApp;
using System.Diagnostics;
using System.IO;
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
            // Ξεκινάμε τον καθαρισμό ασύγχρονα
            Loaded += async (s, e) => await StartCleaningAsync();
        }

        public void AppendOutput(string text, SolidColorBrush color)
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
            }, DispatcherPriority.Background); // Χαμηλότερη προτεραιότητα για το UI
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
            // Κλείνει το multitool και επιστρέφει στο MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }




        private async Task StartCleaningAsync()
        {
            AppendOutput("Starting to clear temp folders...", Brushes.White);

            // Εκτέλεση σε background thread
            await Task.Run(async () =>
            {
                var resultTemp = await ClearFolderWithDetails(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp"));
                var resultEnvTemp = await ClearFolderWithDetails(Environment.GetEnvironmentVariable("TEMP"));
                var resultPrefetch = await ClearFolderWithDetails(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch"));
                var resultSoftwareDistribution = await ClearFolderWithDetails(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SoftwareDistribution", "Download"));
                var resultWindowsOld = await ClearFolderWithDetails(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Windows.old"));
                var resultCBSLogs = await ClearFolderWithDetails(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Logs", "CBS"));

                ClearRecycleBin();

                await Task.Delay(2000);

                // Ενημέρωση UI από το UI thread
                await Dispatcher.InvokeAsync(() =>
                {
                    OutputRichTextBox.Document.Blocks.Clear();
                    UpdateFinalResult(resultTemp, resultEnvTemp, resultPrefetch, resultSoftwareDistribution, resultWindowsOld, resultCBSLogs);
                });
            });
        }

        private void ClearRecycleBin()
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
                    process.WaitForExit();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (!string.IsNullOrEmpty(output)) AppendOutput(output, Brushes.White);
                    if (!string.IsNullOrEmpty(error)) AppendOutput(error, Brushes.Red);
                }
            }
            catch (Exception ex)
            {
                AppendOutput($"Error clearing Recycle Bin: {ex.Message}", Brushes.Red);
            }
        }

        private async Task<Tuple<int, long>> ClearFolderWithDetails(string folderPath)
        {
            int deletedFilesInThisFolder = 0;
            long deletedSizeInThisFolder = 0;

            try
            {
                if (Directory.Exists(folderPath))
                {
                    DirectoryInfo directory = new DirectoryInfo(folderPath);

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
                            AppendOutput($"Deleted file: {file.FullName} (Size: {FormatSize(fileSize)})", Brushes.White);
                        }
                        catch (Exception ex)
                        {
                            AppendOutput($"Error deleting file: {file.FullName} - {ex.Message}", Brushes.Red);
                        }
                    }

                    foreach (DirectoryInfo dir in directory.GetDirectories())
                    {
                        try
                        {
                            dir.Attributes = FileAttributes.Normal;
                            long dirSize = DeleteDirectoryRecursivelyAndCalculateSize(dir);
                            Interlocked.Add(ref totalSizeDeleted, dirSize);
                            Interlocked.Add(ref deletedSizeInThisFolder, dirSize);
                            Interlocked.Increment(ref totalFoldersDeleted);
                            AppendOutput($"Deleted directory: {dir.FullName} (Size: {FormatSize(dirSize)})", Brushes.White);
                        }
                        catch (Exception ex)
                        {
                            AppendOutput($"Error deleting directory: {dir.FullName} - {ex.Message}", Brushes.Red);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppendOutput($"Error clearing folder: {folderPath} - {ex.Message}", Brushes.Red);
            }

            return Tuple.Create(deletedFilesInThisFolder, deletedSizeInThisFolder);
        }

        private long DeleteDirectoryRecursivelyAndCalculateSize(DirectoryInfo directory)
        {
            long size = 0;

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
                    AppendOutput($"Error deleting file: {file.FullName} - {ex.Message}", Brushes.Red);
                }
            }

            foreach (DirectoryInfo subDir in directory.GetDirectories())
            {
                try
                {
                    subDir.Attributes = FileAttributes.Normal;
                    size += DeleteDirectoryRecursivelyAndCalculateSize(subDir);
                }
                catch (Exception ex)
                {
                    AppendOutput($"Error deleting subdirectory: {subDir.FullName} - {ex.Message}", Brushes.Red);
                }
            }

            try
            {
                directory.Delete();
            }
            catch (Exception ex)
            {
                AppendOutput($"Error deleting directory: {directory.FullName} - {ex.Message}", Brushes.Red);
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

        private void UpdateFinalResult(Tuple<int, long> resultTemp, Tuple<int, long> resultEnvTemp, Tuple<int, long> resultPrefetch,
            Tuple<int, long> resultSoftwareDistribution, Tuple<int, long> resultWindowsOld, Tuple<int, long> resultCBSLogs)
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
}
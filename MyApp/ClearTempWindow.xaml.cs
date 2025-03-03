using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace multitool
{
    public partial class ClearTempWindow : Window
    {
        private long totalSizeDeleted = 0; // Συνολικό μέγεθος διαγραμμένων αρχείων και φακέλων
        private int totalFilesDeleted = 0; // Συνολικός αριθμός διαγραμμένων αρχείων
        private int totalFoldersDeleted = 0; // Συνολικός αριθμός διαγραμμένων φακέλων

        public ClearTempWindow()
        {
            InitializeComponent();
            // Ξεκινάμε τον καθαρισμό αυτόματα όταν ανοίγει το παράθυρο
            ClearTempFolders();
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
        }

        // Μέθοδος για προσθήκη κειμένου στο RichTextBox
        public void AppendOutput(string text, SolidColorBrush color)
        {
            if (string.IsNullOrEmpty(text)) return;
            // Αφαίρεση περιττών κενών και κενών γραμμών
            text = text.Trim();
            if (string.IsNullOrEmpty(text)) return;
            // Χρήση του Dispatcher για να ενημερωθεί το UI
            Dispatcher.Invoke(() =>
            {
                Paragraph paragraph = new Paragraph(new Run(text))
                {
                    Foreground = color,
                    Margin = new Thickness(0)
                };
                OutputRichTextBox.Document.Blocks.Add(paragraph);
                OutputRichTextBox.ScrollToEnd();
            });
            // Προσθήκη μηνύματος για έλεγχο
            Debug.WriteLine($"AppendOutput: {text}");
        }
        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        // Κουμπί Minimize
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Κουμπί Close
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void ClearTempFolders()
        {
            AppendOutput("Starting to clear temp folders...", Brushes.White); // Προσθήκη μηνύματος έναρξης

            // Εκτέλεση εντολών για καθαρισμό temp folders
            var resultTemp = await ClearFolderWithDetails(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp")); // C:\Windows\Temp
            var resultEnvTemp = await ClearFolderWithDetails(Environment.GetEnvironmentVariable("TEMP")); // %TEMP%
            var resultPrefetch = await ClearFolderWithDetails(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch")); // C:\Windows\Prefetch
            var resultSoftwareDistribution = await ClearFolderWithDetails(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SoftwareDistribution", "Download")); // C:\Windows\SoftwareDistribution\Download
            var resultWindowsOld = await ClearFolderWithDetails(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Windows.old")); // C:\Windows.old
            var resultCBSLogs = await ClearFolderWithDetails(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Logs", "CBS")); // C:\Windows\Logs\CBS

            // Καθαρισμός του Recycle Bin
            ClearRecycleBin();

            // Περιμένουμε 2 δευτερόλεπτα
            await Task.Delay(2000);

            // Καθαρίζουμε το RichTextBox
            Dispatcher.Invoke(() =>
            {
                OutputRichTextBox.Document.Blocks.Clear();
            });

            // Ενημέρωση των αποτελεσμάτων
            UpdateFinalResult(resultTemp, resultEnvTemp, resultPrefetch, resultSoftwareDistribution, resultWindowsOld, resultCBSLogs);
        }

        // Μέθοδος για καθαρισμό του Recycle Bin
        private void ClearRecycleBin()
        {
            try
            {
                // Εκτέλεση της εντολής για καθαρισμό του Recycle Bin
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

                    if (!string.IsNullOrEmpty(output))
                    {
                        AppendOutput(output, Brushes.White);
                    }
                    if (!string.IsNullOrEmpty(error))
                    {
                        AppendOutput(error, Brushes.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                AppendOutput($"Error clearing Recycle Bin: {ex.Message}", Brushes.Red);
            }
        }



        // Μέθοδος για καθαρισμό φακέλου με λεπτομέρειες
        private async Task<Tuple<int, long>> ClearFolderWithDetails(string folderPath)
        {
            int deletedFilesInThisFolder = 0; // Μετράει ΜΟΝΟ τα αρχεία σε αυτόν τον φάκελο
            long deletedSizeInThisFolder = 0; // Μετράει το μέγεθος των διαγραμμένων αρχείων σε αυτόν τον φάκελο

            try
            {
                if (Directory.Exists(folderPath))
                {
                    DirectoryInfo directory = new DirectoryInfo(folderPath);

                    // Διαγραφή ΑΡΧΕΙΩΝ
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        try
                        {
                            file.Attributes = FileAttributes.Normal;
                            long fileSize = file.Length; // Μέγεθος του αρχείου
                            totalSizeDeleted += fileSize;
                            deletedSizeInThisFolder += fileSize;
                            totalFilesDeleted++; // Αύξηση ΜΟΝΟ εδώ
                            deletedFilesInThisFolder++;
                            file.Delete();
                            AppendOutput($"Deleted file: {file.FullName} (Size: {FormatSize(fileSize)})", Brushes.White); // Προσθήκη μεγέθους στο μήνυμα
                        }
                        catch (Exception ex)
                        {
                            AppendOutput($"Error deleting file: {file.FullName} - {ex.Message}", Brushes.Red);
                        }
                    }

                    // Διαγραφή ΥΠΟΦΑΚΕΛΩΝ
                    foreach (DirectoryInfo dir in directory.GetDirectories())
                    {
                        try
                        {
                            dir.Attributes = FileAttributes.Normal;
                            long dirSize = DeleteDirectoryRecursivelyAndCalculateSize(dir); // Διαγραφή και υπολογισμός μεγέθους
                            totalSizeDeleted += dirSize;
                            deletedSizeInThisFolder += dirSize;
                            totalFoldersDeleted++; // Αύξηση ΜΟΝΟ εδώ
                            AppendOutput($"Deleted directory: {dir.FullName} (Size: {FormatSize(dirSize)})", Brushes.White); // Προσθήκη μεγέθους στο μήνυμα
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

            return Tuple.Create(deletedFilesInThisFolder, deletedSizeInThisFolder); // Επιστροφή αριθμού αρχείων και μεγέθους
        }

        // Αναδρομική διαγραφή και υπολογισμός μεγέθους
        private long DeleteDirectoryRecursivelyAndCalculateSize(DirectoryInfo directory)
        {
            long size = 0;

            // Διαγραφή αρχείων και υπολογισμός μεγέθους
            foreach (FileInfo file in directory.GetFiles())
            {
                try
                {
                    file.Attributes = FileAttributes.Normal;
                    size += file.Length; // Προσθήκη μεγέθους του αρχείου
                    file.Delete();
                }
                catch (Exception ex)
                {
                    AppendOutput($"Error deleting file: {file.FullName} - {ex.Message}", Brushes.Red);
                }
            }

            // Αναδρομική διαγραφή υποφακέλων
            foreach (DirectoryInfo subDir in directory.GetDirectories())
            {
                try
                {
                    subDir.Attributes = FileAttributes.Normal;
                    size += DeleteDirectoryRecursivelyAndCalculateSize(subDir); // Υπολογισμός μεγέθους υποφακέλου
                }
                catch (Exception ex)
                {
                    AppendOutput($"Error deleting subdirectory: {subDir.FullName} - {ex.Message}", Brushes.Red);
                }
            }

            // Διαγραφή του φακέλου
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

        // Βοηθητική μέθοδος για μορφοποίηση του μεγέθους σε αναγνώσιμη μορφή (KB, MB, GB)
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

        // Ενημέρωση των τελικών αποτελεσμάτων
        private void UpdateFinalResult(Tuple<int, long> resultTemp, Tuple<int, long> resultEnvTemp, Tuple<int, long> resultPrefetch, Tuple<int, long> resultSoftwareDistribution, Tuple<int, long> resultWindowsOld, Tuple<int, long> resultCBSLogs)
        {
            Dispatcher.Invoke(() =>
            {
                FinalResultBorder.Visibility = Visibility.Visible;

                // Ενημέρωση των TextBlock με τα αποτελέσματα
                TempResultText.Text = $"C:\\Windows\\Temp - Deleted {resultTemp.Item1} files, Size: {FormatSize(resultTemp.Item2)}";
                EnvTempResultText.Text = $"%TEMP% - Deleted {resultEnvTemp.Item1} files, Size: {FormatSize(resultEnvTemp.Item2)}";
                PrefetchResultText.Text = $"C:\\Windows\\Prefetch - Deleted {resultPrefetch.Item1} files, Size: {FormatSize(resultPrefetch.Item2)}";
                SoftwareDistributionResultText.Text = $"C:\\Windows\\SoftwareDistribution\\Download - 🗑️ {resultSoftwareDistribution.Item1} files, Size: {FormatSize(resultSoftwareDistribution.Item2)}";
                WindowsOldResultText.Text = $"C:\\Windows.old - Deleted {resultWindowsOld.Item1} files, Size: {FormatSize(resultWindowsOld.Item2)}";
                CBSLogsResultText.Text = $"C:\\Windows\\Logs\\CBS - Deleted {resultCBSLogs.Item1} files, Size: {FormatSize(resultCBSLogs.Item2)}";
                TotalFilesText.Text = $"Total files deleted: {totalFilesDeleted}";
                TotalFoldersText.Text = $"Total folders deleted: {totalFoldersDeleted}";
                TotalSizeText.Text = $"Total size deleted: {FormatSize(totalSizeDeleted)}";
            });
        }
    }
}
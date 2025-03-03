using multitool;
using System.Diagnostics;
using System.Windows;

namespace MyApp
{

    public partial class multitool : Window
    {
        public multitool()
        {
            InitializeComponent();
        }
        #region --> mouse drag
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOZORDER = 0x0004;
        #endregion

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
            this.Close();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

        private void WingetUpgradeAllButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Kolokithes A.E\update.exe";
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    this.WindowState = WindowState.Minimized; // Κάντε minimize το MainWindow

                    Process process = new Process();
                    process.StartInfo.FileName = filePath;
                    process.Start();
                    System.Threading.Thread.Sleep(1000);
                    IntPtr handle = process.MainWindowHandle;
                    if (handle == IntPtr.Zero)
                    {
                        System.Threading.Thread.Sleep(500);
                        handle = process.MainWindowHandle;
                    }
                    if (handle != IntPtr.Zero)
                    {
                        var screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                        var screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
                        RECT rect;
                        if (GetWindowRect(handle, out rect))
                        {
                            int windowWidth = rect.Right - rect.Left;
                            int windowHeight = rect.Bottom - rect.Top;
                            int x = (int)((screenWidth - windowWidth) / 2);
                            int y = (int)((screenHeight - windowHeight) / 2);
                            SetWindowPos(handle, IntPtr.Zero, x, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("File not found: " + filePath);
            }
        }

        private void ClearTempButton_Click(object sender, RoutedEventArgs e)
        {
            ClearTempWindow clearTempWindow = new ClearTempWindow();
            clearTempWindow.Show(); // Εμφάνιση του παραθύρου χωρίς ιδιοκτήτη
            this.WindowState = WindowState.Minimized; // Κάντε minimize μόνο το MainWindow
        }

        private void SfcScanButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Kolokithes A.E\healthcare.exe";
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    WindowState = WindowState.Minimized;
                    Process process = new Process();
                    process.StartInfo.FileName = filePath;
                    process.Start();
                    System.Threading.Thread.Sleep(1000);
                    IntPtr handle = process.MainWindowHandle;
                    if (handle == IntPtr.Zero)
                    {
                        System.Threading.Thread.Sleep(500);
                        handle = process.MainWindowHandle;
                    }
                    if (handle != IntPtr.Zero)
                    {
                        var screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                        var screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
                        RECT rect;
                        if (GetWindowRect(handle, out rect))
                        {
                            int windowWidth = rect.Right - rect.Left;
                            int windowHeight = rect.Bottom - rect.Top;
                            int x = (int)((screenWidth - windowWidth) / 2);
                            int y = (int)((screenHeight - windowHeight) / 2);
                            SetWindowPos(handle, IntPtr.Zero, x, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("File not found: " + filePath);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Κλείνει την εφαρμογή
        }
    }
}

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.IO;

namespace MyApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Δηλώνουμε τα handlers μία φορά
            CodeTextBox.KeyDown += CodeTextBox_KeyDown;
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;

            // Εμφάνιση πάνω απ’ όλα για 0,2 δευτ.
            this.Topmost = true;
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.2)
            };
            timer.Tick += (s, e) =>
            {
                this.Topmost = false;
                timer.Stop();
            };
            timer.Start();
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOZORDER = 0x0004;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region --> buttons

        private void UpdateAppsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\\Kolokithes A.E\\update.exe";
            if (File.Exists(filePath))
            {
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = filePath;
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = false;
                    process.Start();

                    // Αναμονή έως 5 δευτ. για να πάρετε handle
                    IntPtr handle = IntPtr.Zero;
                    int timeout = 5000;
                    int elapsed = 0;
                    int delay = 500;
                    while (handle == IntPtr.Zero && elapsed < timeout)
                    {
                        System.Threading.Thread.Sleep(delay);
                        elapsed += delay;
                        handle = process.MainWindowHandle;
                    }

                    if (handle != IntPtr.Zero)
                    {
                        var screenWidth = SystemParameters.PrimaryScreenWidth;
                        var screenHeight = SystemParameters.PrimaryScreenHeight;
                        if (GetWindowRect(handle, out RECT rect))
                        {
                            int windowWidth = rect.Right - rect.Left;
                            int windowHeight = rect.Bottom - rect.Top;
                            int x = (int)((screenWidth - windowWidth) / 2);
                            int y = (int)((screenHeight - windowHeight) / 2);
                            SetWindowPos(handle, IntPtr.Zero, x, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
                        }
                        else
                        {
                            MessageBox.Show("Αποτυχία ανάκτησης διαστάσεων παραθύρου.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Δεν βρέθηκε handle παραθύρου μετά από 5 δευτερόλεπτα.");
                    }

                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Σφάλμα: " + ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Το αρχείο δεν βρέθηκε: " + filePath);
            }
            this.Show();
        }

        // Το όνομα του handler πρέπει να παραμείνει έτσι για να ταιριάζει με το XAML
        private void install_public2_Click(object sender, RoutedEventArgs e)
        {
            install_public_primary nextWindow = new install_public_primary();
            nextWindow.Show();
            this.Close();
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void CrackSitesButton_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() => this.Hide());
            CrackSiteWindow crackSiteWindow = new CrackSiteWindow();
            crackSiteWindow.Closed += (s, args) => this.Dispatcher.Invoke(() => this.Show());
            crackSiteWindow.Show();
        }

        private void CrackAppsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            PublicInstallWindow publicInstallWindow = new PublicInstallWindow();
            publicInstallWindow.Closed += (s, args) => this.Show();
            publicInstallWindow.Show();
        }

        private void SysMaintenanceButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            multitool multitoolWindow = new multitool();
            multitoolWindow.Closed += (s, args) => this.Show();
            multitoolWindow.Show();
        }

        private void ActivateAutoLoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            CustomWindow customWindow = new CustomWindow();
            customWindow.Closed += (s, args) => this.Show();
            customWindow.Show();
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            info infoWindow = new info();
            infoWindow.Closed += (s, args) => this.Show();
            infoWindow.Show();
        }

        // Χειριστής για το Enter στο CodeTextBox
        private void CodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                string enteredCode = CodeTextBox.Text.Trim().ToLower();
                if (enteredCode == "sims")
                {
                    this.Hide();
                    Sims simsWindow = new Sims();
                    simsWindow.Closed += (s, args) => this.Show();
                    simsWindow.Show();
                    CodeTextBox.Text = "";
                }
                else if (enteredCode == "2873")
                {
                    this.Hide();
                    password_manager passwordManagerWindow = new password_manager();
                    passwordManagerWindow.Closed += (s, args) => this.Show();
                    passwordManagerWindow.Show();
                }
                else if (enteredCode == "chris")
                {
                    this.Hide();
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"iwr -useb https://christitus.com/win | iex\"",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    Process process = new Process { StartInfo = psi };
                    process.Start();
                    process.WaitForExit();
                    this.Show();
                }
                else if (enteredCode == "spotify")
                {
                    this.Hide();
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"iwr -useb https://raw.githubusercontent.com/thomasthanos/SpotifyBlocker/a18556e7b12d6cb934bdba944742d9c81aadbe26/spotify.ps1 | iex\"",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    Process process = new Process { StartInfo = psi };
                    process.Start();
                    process.WaitForExit();
                    this.Show();
                }
                else if (enteredCode == "auto")
                {
                    // Διόρθωση: χρήση WPF WindowState αντί FormWindowState
                    this.WindowState = WindowState.Minimized;

                    string autoPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "Kolokithes A.E",
                        "auto_clicker.exe"
                    );

                    if (File.Exists(autoPath))
                    {
                        try
                        {
                            Process process = new Process();
                            process.StartInfo.FileName = autoPath;
                            process.StartInfo.UseShellExecute = false;
                            process.Start();

                            System.Threading.Thread.Sleep(1000);
                            IntPtr handle = IntPtr.Zero;
                            for (int i = 0; i < 10; i++)
                            {
                                handle = process.MainWindowHandle;
                                if (handle != IntPtr.Zero)
                                    break;
                                System.Threading.Thread.Sleep(300);
                            }

                            if (handle != IntPtr.Zero)
                            {
                                var screenWidth = SystemParameters.PrimaryScreenWidth;
                                var screenHeight = SystemParameters.PrimaryScreenHeight;
                                if (GetWindowRect(handle, out RECT rect))
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
                        MessageBox.Show("File not found: " + autoPath);
                    }
                }
                else if (enteredCode == "bios")
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "shutdown",
                        Arguments = "/r /fw /t 0",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    Process.Start(psi);
                }
                else
                {
                    CustomMessageBox warningMessage = new CustomMessageBox(
                        "Μη έγκυρος κωδικός. Παρακαλώ δοκιμάστε ξανά.",
                        "Προειδοποίηση",
                        IconType.Danger
                    );
                    warningMessage.ShowDialog();
                }
            }
        }
        #endregion
    }
}

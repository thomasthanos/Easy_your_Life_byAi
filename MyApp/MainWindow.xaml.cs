using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MyApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Add an event handler for the CodeTextBox to check the code when Enter is pressed or focus is lost
            CodeTextBox.KeyDown += CodeTextBox_KeyDown;
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;

            // Ορίστε το παράθυρο να εμφανίζεται πάνω από άλλα παράθυρα για 0.2 δευτερόλεπτα
            this.Topmost = true;

            // Δημιουργήστε ένα DispatcherTimer για 0.2 δευτερόλεπτα
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.2);
            timer.Tick += (s, e) =>
            {
                this.Topmost = false; // Επαναφέρετε το Topmost σε false μετά από 0.2 δευτερόλεπτα
                timer.Stop(); // Σταματήστε το timer
            };
            timer.Start();
            // Add an event handler for the CodeTextBox to check the code when Enter is pressed or focus is lost
            CodeTextBox.KeyDown += CodeTextBox_KeyDown;
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
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
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Kolokithes A.E\update.exe";

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = filePath;
                    process.StartInfo.UseShellExecute = true; // Για να εμφανιστεί το παράθυρο
                    process.StartInfo.CreateNoWindow = false; // Εξασφαλίζει ότι θα δούμε το terminal
                    process.Start();

                    // Περίμενε μέχρι να πάρεις έγκυρο handle (μέγιστο 5 δευτερόλεπτα)
                    IntPtr handle = IntPtr.Zero;
                    int timeout = 5000; // 5 δευτερόλεπτα
                    int elapsed = 0;
                    int delay = 500; // 0.5 δευτερόλεπτα ανά επανάληψη

                    while (handle == IntPtr.Zero && elapsed < timeout)
                    {
                        System.Threading.Thread.Sleep(delay);
                        elapsed += delay;
                        handle = process.MainWindowHandle;
                    }

                    if (handle != IntPtr.Zero)
                    {
                        // Κεντράρισμα του παραθύρου
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
                        else
                        {
                            MessageBox.Show("Αποτυχία ανάκτησης διαστάσεων παραθύρου.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Δεν βρέθηκε handle παραθύρου μετά από 5 δευτερόλεπτα.");
                    }

                    process.WaitForExit(); // Περιμένει να κλείσει το update.exe
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

        private void ChrisappsButton_Click(object sender, RoutedEventArgs e)
        {
            // Hide the main window
            this.Hide();

            // Create a new process to run PowerShell
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"iwr -useb https://christitus.com/win | iex\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            Process process = new Process
            {
                StartInfo = psi
            };

            // Start the process
            process.Start();

            // Wait for the process to exit
            process.WaitForExit();

            // Show the main window again after the process has finished
            this.Show();
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
            this.Dispatcher.Invoke(() => this.Hide()); // Κρύβει το κύριο παράθυρο

            CrackSiteWindow crackSiteWindow = new CrackSiteWindow();
            crackSiteWindow.Closed += (s, args) => this.Dispatcher.Invoke(() => this.Show()); // Όταν κλείσει το CrackSiteWindow, εμφάνισε ξανά το MainWindow
            crackSiteWindow.Show();
        }


        private void CrackAppsButton_Click(object sender, RoutedEventArgs e)
        {
            // Κρύψε το MainWindow
            this.Hide();

            // Άνοιγμα του PublicInstallWindow
            PublicInstallWindow publicInstallWindow = new PublicInstallWindow();
            publicInstallWindow.Closed += (s, args) => this.Show(); // Όταν κλείσει το PublicInstallWindow, εμφάνισε ξανά το MainWindow
            publicInstallWindow.Show();
        }

        private void SysMaintenanceButton_Click(object sender, RoutedEventArgs e)
        {
            // Κρύψε το MainWindow
            this.Hide();

            // Άνοιγμα του multitool window
            multitool multitoolWindow = new multitool();
            multitoolWindow.Closed += (s, args) => this.Show(); // Όταν κλείσει το multitool, εμφάνισε ξανά το MainWindow
            multitoolWindow.Show();
        }

        private void ActivateAutoLoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Κρύψε το MainWindow
            this.Hide();

            // Άνοιγμα του CustomWindow
            CustomWindow customWindow = new CustomWindow();
            customWindow.Closed += (s, args) => this.Show(); // Όταν κλείσει το CustomWindow, εμφάνισε ξανά το MainWindow
            customWindow.Show();
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            // Κρύψε το MainWindow
            this.Hide();

            // Άνοιγμα του info window
            info infoWindow = new info();
            infoWindow.Closed += (s, args) => this.Show(); // Όταν κλείσει το info, εμφάνισε ξανά το MainWindow
            infoWindow.Show();
        }

        // Handle code checking when Enter is pressed in CodeTextBox
        private void CodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // Stop further processing of the KeyDown event

                string enteredCode = CodeTextBox.Text.Trim().ToLower(); // Get the code from the TextBox and convert to lowercase
                if (enteredCode == "sims")
                {
                    this.Hide(); // Hide the MainWindow
                    Sims simsWindow = new Sims();
                    simsWindow.Closed += (s, args) => this.Show(); // Show MainWindow when Sims window closes
                    simsWindow.Show();
                    CodeTextBox.Text = ""; // Clear the code after successful entry
                }
                else if (enteredCode == "2873")
                {
                    this.Hide(); // Hide the MainWindow
                    private_password privatePasswordWindow = new private_password();
                    privatePasswordWindow.Closed += (s, args) => this.Show(); // Show MainWindow when PrivatePassword window closes
                    privatePasswordWindow.Show();
                }
                else
                {
                    // Use CustomMessageBox for warning message
                    CustomMessageBox warningMessage = new CustomMessageBox(
                        "Μη έγκυρος κωδικός. Παρακαλώ δοκιμάστε ξανά.",
                        "Προειδοποίηση",
                        IconType.Danger  // Ή IconType.Warning αν υπάρχει
                    );
                    warningMessage.ShowDialog(); // Show as modal dialog
                }
            }
        }
        #endregion


    }
}
﻿using multitool;
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
            // Ανοίγει ξανά το multitool πριν κλείσει το ClearTempWindow
            multitool multiToolWindow = new multitool();
            multiToolWindow.Show();

            // Κλείνει το ClearTempWindow
            this.Close();
        }


        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

        private void WingetUpgradeAllButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Kolokithes A.E\patch_my_pc.exe";
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = filePath;
                    process.StartInfo.UseShellExecute = true; // Απαραίτητο για runas
                    process.StartInfo.Verb = "runas";         // Ζητάει admin άδεια
                    process.Start();

                    System.Threading.Thread.Sleep(1500); // λίγο παραπάνω για admin app

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
            this.Dispatcher.Invoke(() => this.Hide()); // Κρύβει το κύριο παράθυρο
            ClearTempWindow clearTempWindow = new ClearTempWindow();
            clearTempWindow.Show(); // Εμφάνιση του παραθύρου χωρίς ιδιοκτήτη
        }

        private void SfcScanButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide(); // Κρύβει την κύρια εφαρμογή
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Kolokithes A.E\healthcare.exe";

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

                    process.WaitForExit(); // Περιμένει να κλείσει το healthcare.exe
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
            this.Show(); // Εμφανίζει ξανά την κύρια εφαρμογή
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Κλείνει την εφαρμογή
        }
    }
}

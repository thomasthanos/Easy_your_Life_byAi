using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;

namespace MyApp
{
    public partial class password_manager : Window
    {
        public password_manager()
        {
            InitializeComponent();
            Loaded += PasswordManager_Loaded;

            // Add event handlers for resize grips
            ResizeGripRight.MouseLeftButtonDown += (s, e) => { ResizeWindow(ResizeDirection.Right); };
            ResizeGripLeft.MouseLeftButtonDown += (s, e) => { ResizeWindow(ResizeDirection.Left); };
            ResizeGripTop.MouseLeftButtonDown += (s, e) => { ResizeWindow(ResizeDirection.Top); };
            ResizeGripBottom.MouseLeftButtonDown += (s, e) => { ResizeWindow(ResizeDirection.Bottom); };
            ResizeGripTopRight.MouseLeftButtonDown += (s, e) => { ResizeWindow(ResizeDirection.TopRight); };
            ResizeGripTopLeft.MouseLeftButtonDown += (s, e) => { ResizeWindow(ResizeDirection.TopLeft); };
            ResizeGripBottomRight.MouseLeftButtonDown += (s, e) => { ResizeWindow(ResizeDirection.BottomRight); };
            ResizeGripBottomLeft.MouseLeftButtonDown += (s, e) => { ResizeWindow(ResizeDirection.BottomLeft); };
        }

        private async void PasswordManager_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Φτιάξε διαδρομή %AppData%\Kolokithes A.E\WebView2UserData
                string userDataFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Kolokithes A.E",
                    "WebView2UserData");

                Directory.CreateDirectory(userDataFolder); // Βεβαιώσου ότι υπάρχει!

                var env = await CoreWebView2Environment.CreateAsync(null, userDataFolder);
                await MyWebView.EnsureCoreWebView2Async(env);
                MyWebView.CoreWebView2.Navigate("https://password-manager-78x.pages.dev");
            }
            catch (Exception ex)
            {
                MessageBox.Show("WebView2 error: " + ex.Message);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void ResizeWindow(ResizeDirection direction)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 0x112, (IntPtr)(61440 + direction), IntPtr.Zero);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private enum ResizeDirection
        {
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8,
        }
    }
}

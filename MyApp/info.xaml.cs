using System;
using System.Windows;
using System.Windows.Input;

namespace MyApp
{
    public partial class info : Window
    {
        public info()
        {
            InitializeComponent();
        }

        // Event handler for the Back button - returns to MainWindow
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Hide this window
            this.Hide();

            // Create and show the MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            // Close this window (optional, depending on your needs)
            this.Close();
        }

        // Event handler for the Minimize button
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; // Minimize the window
        }

        // Event handler for the Close button - shuts down the application
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Shutdown the entire application
        }

        // Event handler for window dragging
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove(); // Allows moving the window when dragged
            }
        }
    }
}
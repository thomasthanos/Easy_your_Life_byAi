﻿using System.Windows;
using System.Windows.Input;

namespace MyApp
{
    public partial class info : Window
    {
        public info()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
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
        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
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
        // Event handler for the GitHub button
        private void GitHubButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the GitHub profile in the default browser
            string githubUrl = "https://github.com/ThomasThanos"; // Replace with your GitHub profile URL
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = githubUrl,
                UseShellExecute = true
            });
        }
    }
}
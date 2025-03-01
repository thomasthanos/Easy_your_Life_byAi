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

        // Event handler για το κουμπί Back
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Εδώ μπορείτε να προσθέσετε τη λειτουργία που θέλετε να εκτελείται όταν πατηθεί το κουμπί Back
            MessageBox.Show("Back button clicked!");
        }

        // Event handler για το κουμπί Minimize
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; // Ελαχιστοποίηση του παραθύρου
        }

        // Event handler για το κουμπί Close
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Κλείσιμο του παραθύρου
        }

        // Event handler για την κίνηση του παραθύρου όταν γίνεται drag
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove(); // Επιτρέπει την κίνηση του παραθύρου όταν γίνεται drag
            }
        }
    }
}
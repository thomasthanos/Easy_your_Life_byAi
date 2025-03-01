using System.Windows;
using System.Windows.Input;

namespace MyApp
{
    public partial class CustomMessageBox : Window
    {
        private string _errorDetails;
        private bool _isErrorDetailsVisible = false;

        public CustomMessageBox(string message, string title, IconType iconType = IconType.None, string errorDetails = null)
        {
            InitializeComponent();
            MessageText.Text = message;
            Title = title;
            _errorDetails = errorDetails;
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            // Αυτόματη ρύθμιση του ύψους του παραθύρου ανάλογα με το μήνυμα
            AdjustWindowHeight();

            // Set the icon based on the iconType
            switch (iconType)
            {
                case IconType.Info:
                    InfoIcon.Visibility = Visibility.Visible;
                    break;
                case IconType.Success:
                    SuccessIcon.Visibility = Visibility.Visible;
                    OkButton.Visibility = Visibility.Visible;
                    break;
                case IconType.Danger:
                    DangerIcon.Visibility = Visibility.Visible;
                    OkButton.Visibility = Visibility.Visible;
                    InfoButton.Visibility = string.IsNullOrEmpty(errorDetails) ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case IconType.Error:
                    ErrorIcon.Visibility = Visibility.Visible;
                    OkButton.Visibility = Visibility.Visible;
                    InfoButton.Visibility = string.IsNullOrEmpty(errorDetails) ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case IconType.Question:
                    InfoIcon.Visibility = Visibility.Visible;
                    YesButton.Visibility = Visibility.Visible;
                    CancelButton.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }
        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void AdjustWindowHeight()
        {
            // Υπολογισμός του απαιτούμενου ύψους για το μήνυμα
            double messageHeight = MessageText.ActualHeight;
            if (messageHeight > 50)  // Αν το μήνυμα είναι μεγάλο, αυξάνουμε το ύψος του παραθύρου
            {
                Height += messageHeight - 50;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isErrorDetailsVisible)
            {
                ErrorDetailsTextBox.Text = _errorDetails ?? "No error details available.";
                ErrorDetailsTextBox.Visibility = Visibility.Visible;
                Height += 60; // Αύξηση του ύψους για τα σφάλματα
                _isErrorDetailsVisible = true;
            }
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

    public enum IconType
    {
        None,
        Info,
        Success,
        Danger,
        Error,
        Question
    }
}
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;


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
            AdjustWindowHeight();

            // Ορίστε το παράθυρο να εμφανίζεται πάνω από άλλα παράθυρα για 0.6 δευτερόλεπτα
            this.Topmost = true;

            // Δημιουργήστε ένα DispatcherTimer για 0.6 δευτερόλεπτα
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.6); // Ορίστε το χρονικό διάστημα σε 0.6 δευτερόλεπτα
            timer.Tick += (s, e) =>
            {
                this.Topmost = false; // Επαναφέρετε το Topmost σε false μετά από 0.6 δευτερόλεπτα
                timer.Stop(); // Σταματήστε το timer
            };
            timer.Start();

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
            double messageHeight = MessageText.ActualHeight;
            if (messageHeight > 50)
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

                // Υπολογισμός του απαιτούμενου ύψους για τα error details
                double requiredHeight = ErrorDetailsTextBox.ActualHeight + 20; // Προσθήκη περιθωρίου
                Height += requiredHeight;

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
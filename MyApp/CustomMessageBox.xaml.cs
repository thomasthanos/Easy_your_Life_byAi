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
            this.Loaded += CustomMessageBox_Loaded;
            this.Topmost = true;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += (s, e) =>
            {
                if (!this.Topmost)
                {
                    this.Topmost = true;
                }
            };
            timer.Start();

            // Ορισμός εικονιδίων και κουμπιών
            switch (iconType)
            {
                case IconType.Info:
                    InfoIcon.Visibility = Visibility.Visible;
                    OkButton.HorizontalAlignment = HorizontalAlignment.Center; // Κέντρο όταν δεν είναι Error
                    break;
                case IconType.Success:
                    SuccessIcon.Visibility = Visibility.Visible;
                    OkButton.Visibility = Visibility.Visible;
                    OkButton.HorizontalAlignment = HorizontalAlignment.Center; // Κέντρο όταν δεν είναι Error
                    break;
                case IconType.Danger:
                    DangerIcon.Visibility = Visibility.Visible;
                    OkButton.Visibility = Visibility.Visible;
                    InfoButton.Visibility = string.IsNullOrEmpty(errorDetails) ? Visibility.Collapsed : Visibility.Visible;
                    OkButton.HorizontalAlignment = HorizontalAlignment.Center; // Κέντρο όταν δεν είναι Error
                    break;
                case IconType.Error:
                    ErrorIcon.Visibility = Visibility.Visible;
                    OkButton.Visibility = Visibility.Visible;
                    InfoButton.Visibility = string.IsNullOrEmpty(errorDetails) ? Visibility.Collapsed : Visibility.Visible;
                    CopyButton.Visibility = Visibility.Visible; // Εμφάνιση CopyButton μόνο για Error
                    OkButton.HorizontalAlignment = HorizontalAlignment.Left; // Αριστερά για Error
                    break;
                case IconType.Question:
                    InfoIcon.Visibility = Visibility.Visible;
                    YesButton.Visibility = Visibility.Visible;
                    CancelButton.Visibility = Visibility.Visible;
                    OkButton.HorizontalAlignment = HorizontalAlignment.Center; // Κέντρο όταν δεν είναι Error
                    break;
                default:
                    OkButton.HorizontalAlignment = HorizontalAlignment.Center; // Κέντρο όταν δεν είναι Error
                    break;
            }
        }

        private void CustomMessageBox_Loaded(object sender, RoutedEventArgs e)
        {
            AdjustWindowHeight();
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
            MessageText.Measure(new Size(MessageText.MaxWidth, double.PositiveInfinity));
            double messageHeight = MessageText.DesiredSize.Height;

            double baseHeight = 40 + 50 + 40; // Title Bar + Buttons + Margins
            double newHeight = baseHeight + messageHeight;

            if (messageHeight <= MessageText.MaxHeight)
            {
                this.Height = newHeight + (CopyButton.Visibility == Visibility.Visible ? 40 : 0);
            }
            else
            {
                this.Height = baseHeight + MessageText.MaxHeight + (CopyButton.Visibility == Visibility.Visible ? 40 : 0);
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
                double requiredHeight = ErrorDetailsTextBox.ActualHeight + 20;
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

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(MessageText.Text);
            new CustomMessageBox("Το κείμενο αντιγράφηκε στο πρόχειρο!", "Επιτυχία", IconType.Success).ShowDialog();
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
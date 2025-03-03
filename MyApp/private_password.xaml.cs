using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyApp
{
    public partial class private_password : Window
    {
        private readonly Dictionary<string, (string Email, string Password)> credentials = new();
        private readonly HashSet<string> services = new()
        {
            "1535_maria", "1535_marios", "1535_thomas", "2take1", "bitdefender", "discord",
            "discord_primary", "efood", "epic_games", "facebook", "github@28", "github@3",
            "google_@3", "google_@28", "google_@41", "google_@78", "google_@89", "google_@090",
            "league_of_legends", "microsoft", "netflix", "picsart", "playstation", "socialclub",
            "socialclubprimary", "spotify", "steam", "twitch", "ubisoft"
        };

        // Dictionary για τα ονόματα εμφάνισης (display names)
        private readonly Dictionary<string, string> displayNames = new()
        {
            { "discord", "discord #2" }, // Αλλαγή του "discord" σε "discord_ptb"
            { "discord_primary", "discord #1" }, // Παραμένει ίδιο
            { "epic_games", "Epic Games" }, // Παραμένει ίδιο
            { "league_of_legends", "Riot Games" }, // Παραμένει ίδιο
            { "socialclubprimary", "rockstar #1" }, // Παραμένει ίδιο
            { "socialclub", "rockstar #2" }, // Παραμένει ίδιο
            // Προσθήκη περισσότερων mappings αν χρειάζεται
        };

        public private_password()
        {
            InitializeComponent();
            LoadCredentialsFromFile("E:\\Github\\Easy_your_Life_byAi\\MyApp\\credentials.txt");
            AddButtons();
        }

        private void LoadCredentialsFromFile(string filePath)
        {
            try
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
                if (!File.Exists(fullPath))
                {
                    CustomMessageBox messageBox = new CustomMessageBox("Δεν βρέθηκε το αρχείο διαπιστευτηρίων!", "Σφάλμα", IconType.Error);
                    messageBox.ShowDialog();
                    return;
                }

                foreach (string line in File.ReadAllLines(fullPath))
                {
                    string[] parts = line.Split(':');
                    if (parts.Length == 3)
                    {
                        credentials[parts[0]] = (parts[1], parts[2]);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox($"Σφάλμα κατά τη φόρτωση: {ex.Message}", "Σφάλμα", IconType.Error);
                messageBox.ShowDialog();
            }
        }

        private void AddButtons()
        {
            foreach (string service in services)
            {
                if (!credentials.TryGetValue(service, out var credential)) continue;

                string displayName = displayNames.GetValueOrDefault(service, service);
                var button = new Button
                {
                    Style = (Style)FindResource("ModernButtonStyle"),
                    Content = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Children =
                        {
                            GetIconImage(displayName), // Χρήση του displayName για το εικονίδιο
                            new StackPanel
                            {
                                Orientation = Orientation.Vertical,
                                VerticalAlignment = VerticalAlignment.Center,
                                Children =
                                {
                                    new TextBlock
                                    {
                                        Text = displayName,
                                        FontSize = 16,
                                        FontWeight = FontWeights.Bold,
                                        Foreground = Brushes.White,
                                        Margin = new Thickness(0, 0, 0, 5)
                                    },
                                    new TextBlock
                                    {
                                        Text = $"Email: {credential.Email}",
                                        FontSize = 12,
                                        Foreground = Brushes.LightGray,
                                        Margin = new Thickness(0, 0, 0, 2)
                                    },
                                    new TextBlock
                                    {
                                        Text = $"Password: {credential.Password}",
                                        FontSize = 12,
                                        Foreground = Brushes.LightGray
                                    }
                                }
                            }
                        }
                    }
                };

                button.Click += (s, e) => CopyCredentials(service);

                if (service.StartsWith("google_")) GooglePanel.Children.Add(button);
                else if (IsGameService(service)) GamesPanel.Children.Add(button);
                else if (IsAppService(service)) AppsPanel.Children.Add(button);
                else OtherPanel.Children.Add(button);
            }
        }

        private Image GetIconImage(string iconName)
        {
            var assembly = Assembly.GetExecutingAssembly(); // Δήλωση της μεταβλητής assembly εδώ, μία φορά

            // Αν το όνομα του εικονιδίου ξεκινά με "google_", χρησιμοποίησε το ίδιο εικονίδιο για όλα
            if (iconName.StartsWith("google_"))
            {
                string googleResourceName = "MyApp.Icons.google.ico"; // Όνομα του εικονιδίου για τα Google services
                using Stream? googleStream = assembly.GetManifestResourceStream(googleResourceName); // Χρησιμοποίησε διαφορετικό όνομα (googleStream)
                if (googleStream != null)
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = googleStream;
                    bitmap.DecodePixelWidth = 24;
                    bitmap.DecodePixelHeight = 24;
                    bitmap.EndInit();
                    return new Image { Source = bitmap, Width = 24, Height = 24, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 15, 0) };
                }
            }

            // Για τα υπόλοιπα services, χρησιμοποίησε το κανονικό εικονίδιο
            string resourceName = $"MyApp.Icons.{iconName}.ico";
            using Stream? stream = assembly.GetManifestResourceStream(resourceName); // Χρησιμοποίησε το όνομα "stream" εδώ
            if (stream != null)
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.DecodePixelWidth = 24;
                bitmap.DecodePixelHeight = 24;
                bitmap.EndInit();
                return new Image { Source = bitmap, Width = 24, Height = 24, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 15, 0) };
            }

            // Αν δεν βρεθεί το εικονίδιο, επέστρεψε το default εικονίδιο
            return GetDefaultIcon();
        }

        private Image GetDefaultIcon()
        {
            string defaultResourceName = "MyApp.Icons.default.ico";
            var assembly = Assembly.GetExecutingAssembly();
            using Stream? stream = assembly.GetManifestResourceStream(defaultResourceName);
            if (stream != null)
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.DecodePixelWidth = 24;
                bitmap.DecodePixelHeight = 24;
                bitmap.EndInit();
                return new Image { Source = bitmap, Width = 24, Height = 24, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 15, 0) };
            }
            return new Image { Width = 24, Height = 24, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 15, 0) };
        }

        private bool IsGameService(string key) =>
            key is "epic_games" or "steam" or "ubisoft" or "playstation" or "socialclub" or "socialclubprimary" or "league_of_legends";

        private bool IsAppService(string key) =>
            key is "1535_maria" or "1535_marios" or "1535_thomas" or "bitdefender" or "discord" or "discord_primary" or
                   "efood" or "facebook" or "github@28" or "github@3" or "microsoft" or "netflix" or "picsart" or "spotify" or "twitch";

        private async void CopyCredentials(string serviceName)
        {
            if (!credentials.TryGetValue(serviceName, out var credential)) return;

            Clipboard.SetText(credential.Email);
            CustomMessageBox messageBox1 = new CustomMessageBox($"Το email για {serviceName} αντιγράφηκε!", "Επιτυχία", IconType.Success);
            messageBox1.ShowDialog();
            await System.Threading.Tasks.Task.Delay(2000);
            Clipboard.SetText(credential.Password);
            CustomMessageBox messageBox2 = new CustomMessageBox($"Ο κωδικός για {serviceName} αντιγράφηκε!", "Επιτυχία", IconType.Success);
            messageBox2.ShowDialog();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void BackButton_Click(object sender, RoutedEventArgs e) => Close();
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left) DragMove();
        }
    }
}
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;

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

        private readonly Dictionary<string, string> displayNames = new()
        {
            { "discord", "discord #2" },
            { "discord_primary", "discord #1" },
            { "epic_games", "Epic Games" },
            { "league_of_legends", "Riot Games" },
            { "socialclubprimary", "rockstar #1" },
            { "socialclub", "rockstar #2" },
            { "github@3", "github #1" },
            { "github@28", "github #2" },
        };

        private bool _arePasswordsHidden = true;

        public private_password()
        {
            InitializeComponent();
            LoadCredentialsFromFile("E:\\Github\\Easy_your_Life_byAi\\MyApp\\credentials.txt");
            AddButtons();

            // Αρχικοποίηση του κουμπιού
            if (_arePasswordsHidden)
            {
                HideUnhideIcon.Source = (ImageSource)FindResource("HideIcon");
                HideUnhideText.Text = "Hide Passwords";
            }
            else
            {
                HideUnhideIcon.Source = (ImageSource)FindResource("SeeIcon");
                HideUnhideText.Text = "Show Passwords";
            }
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
                    GetIconImage(displayName),
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
                            new StackPanel
                            {
                                Orientation = Orientation.Horizontal,
                                Children =
                                {
                                    new TextBlock
                                    {
                                        Text = "Password: ",
                                        FontSize = 12,
                                        Foreground = Brushes.LightGray
                                    },
                                    new TextBlock
                                    {
                                        Text = credential.Password, // Πάντα εμφανίζουμε τον πραγματικό κωδικό
                                        FontSize = 12,
                                        Foreground = Brushes.LightGray,
                                        Effect = _arePasswordsHidden ? new BlurEffect { Radius = 10 } : null // Εφαρμογή Blur μόνο στον κωδικό
                                    }
                                }
                            }
                        }
                    }
                }
                    },
                    Tag = service // Ορισμός του Tag με το όνομα της υπηρεσίας
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
            var assembly = Assembly.GetExecutingAssembly();

            if (iconName.StartsWith("google_"))
            {
                return LoadIconImage("MyApp.Icons.google.ico", assembly);
            }

            if (iconName.StartsWith("github"))
            {
                return LoadIconImage("MyApp.Icons.github.ico", assembly);
            }

            string resourceName = $"MyApp.Icons.{iconName}.ico";
            return LoadIconImage(resourceName, assembly);
        }

        private Image LoadIconImage(string resourceName, Assembly assembly)
        {
            using Stream? stream = assembly.GetManifestResourceStream(resourceName);
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

            // Αντιγραφή email
            Clipboard.SetText(credential.Email);
            CustomMessageBox messageBox1 = new CustomMessageBox($"Το email για {serviceName} αντιγράφηκε!", "Επιτυχία", IconType.Success);
            messageBox1.Topmost = true; // Εμφάνιση πάνω από όλα τα παράθυρα
            bool emailCopied = messageBox1.ShowDialog() ?? false;

            if (!emailCopied) return; // Αν ο χρήστης δεν πατήσει OK, σταματάμε εδώ

            // Αντιγραφή κωδικού
            await System.Threading.Tasks.Task.Delay(2000); // Προσθήκη καθυστέρησης
            Clipboard.SetText(credential.Password);
            CustomMessageBox messageBox2 = new CustomMessageBox($"Ο κωδικός για {serviceName} αντιγράφηκε!", "Επιτυχία", IconType.Success);
            messageBox2.Topmost = true; // Εμφάνιση πάνω από όλα τα παράθυρα
            messageBox2.ShowDialog();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void BackButton_Click(object sender, RoutedEventArgs e) => Close();
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left) DragMove();
        }

        private void HideUnhideButton_Click(object sender, RoutedEventArgs e)
        {
            _arePasswordsHidden = !_arePasswordsHidden; // Αλλαγή κατάστασης

            // Ενημέρωση εικονιδίου και κειμένου
            if (_arePasswordsHidden)
            {
                HideUnhideIcon.Source = (ImageSource)FindResource("HideIcon");
                HideUnhideText.Text = "Hide Passwords";
            }
            else
            {
                HideUnhideIcon.Source = (ImageSource)FindResource("SeeIcon");
                HideUnhideText.Text = "Show Passwords";
            }

            UpdatePasswordVisibility(); // Ενημέρωση της εμφάνισης των κωδικών
        }

        private void UpdatePasswordVisibility()
        {
            foreach (var panel in new[] { AppsPanel, GamesPanel, GooglePanel, OtherPanel })
            {
                foreach (Button button in panel.Children)
                {
                    if (button.Content is StackPanel stackPanel && stackPanel.Children.Count > 1 && stackPanel.Children[1] is StackPanel innerStackPanel)
                    {
                        foreach (var child in innerStackPanel.Children)
                        {
                            if (child is StackPanel passwordPanel && passwordPanel.Children.Count > 1 && passwordPanel.Children[1] is TextBlock passwordTextBlock)
                            {
                                if (button.Tag != null && credentials.ContainsKey(button.Tag.ToString()))
                                {
                                    // Ενημέρωση του κειμένου με τον πραγματικό κωδικό
                                    passwordTextBlock.Text = credentials[button.Tag.ToString()].Password;

                                    // Εφαρμογή ή αφαίρεση του BlurEffect
                                    if (_arePasswordsHidden)
                                    {
                                        passwordTextBlock.Effect = new BlurEffect { Radius = 10 }; // Εφαρμογή Blur
                                    }
                                    else
                                    {
                                        passwordTextBlock.Effect = null; // Αφαίρεση Blur
                                    }
                                }
                                else
                                {
                                    passwordTextBlock.Text = "*******";
                                    passwordTextBlock.Effect = null; // Αφαίρεση Blur αν δεν υπάρχει κωδικός
                                }
                            }
                        }
                    }
                }
            }
        }



    }
}
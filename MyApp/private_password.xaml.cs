using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyApp
{
    public partial class private_password : Window
    {
        private Dictionary<string, (string Email, string Password)> credentials = new Dictionary<string, (string, string)>();
        private Dictionary<string, string> services = new Dictionary<string, string>
        {
            { "1535_maria", "" }, { "1535_marios", "" }, { "1535_thomas", "" },
            { "2take1", "" }, { "bitdefender", "" }, { "discord", "" },
            { "discord_primary", "" }, { "efood", "" }, { "epic_games", "" },
            { "facebook", "" }, { "github@28", "" }, { "github@3", "" },
            { "google_@3", "" }, { "google_@28", "" }, { "google_@41", "" },
            { "google_@78", "" }, { "google_@89", "" }, { "google_@090", "" },
            { "league_of_legends", "" }, { "microsoft", "" }, { "netflix", "" },
            { "picsart", "" }, { "playstation", "" }, { "socialclub", "" },
            { "socialclubprimary", "" }, { "spotify", "" }, { "steam", "" },
            { "twitch", "" }, { "ubisoft", "" }
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
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Δεν βρέθηκε το αρχείο διαπιστευτηρίων!", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                foreach (var line in File.ReadAllLines(filePath))
                {
                    var parts = line.Split(':');
                    if (parts.Length == 3)
                    {
                        credentials[parts[0]] = (parts[1], parts[2]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Σφάλμα κατά τη φόρτωση: {ex.Message}", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButtons()
        {
            foreach (var service in services)
            {
                if (!credentials.ContainsKey(service.Key)) continue;

                var button = new Button
                {
                    Style = (Style)FindResource("ModernButtonStyle"),
                    Content = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Children =
                        {
                            new TextBlock
                            {
                                Text = service.Value,
                                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                                FontSize = 24,
                                VerticalAlignment = VerticalAlignment.Center,
                                Foreground = Brushes.White,
                                Margin = new Thickness(0, 0, 15, 0)
                            },
                            new TextBlock
                            {
                                Text = service.Key,
                                FontSize = 14,
                                VerticalAlignment = VerticalAlignment.Center,
                                Foreground = Brushes.White
                            }
                        }
                    }
                };

                button.Click += (s, e) => CopyCredentials(service.Key);

                // Κατηγοριοποίηση
                if (service.Key.StartsWith("google_")) GooglePanel.Children.Add(button);
                else if (IsGameService(service.Key)) GamesPanel.Children.Add(button);
                else if (IsAppService(service.Key)) AppsPanel.Children.Add(button);
                else OtherPanel.Children.Add(button);
            }
        }

        private bool IsGameService(string key) =>
            key == "epic_games" || key == "steam" || key == "ubisoft" || key == "playstation" ||
            key == "socialclub" || key == "socialclubprimary" || key == "league_of_legends";

        private bool IsAppService(string key) =>
            key == "1535_maria" || key == "1535_marios" || key == "1535_thomas" || key == "bitdefender" ||
            key == "discord" || key == "discord_primary" || key == "efood" || key == "facebook" ||
            key == "github@28" || key == "github@3" || key == "microsoft" || key == "netflix" ||
            key == "picsart" || key == "spotify" || key == "twitch";

        private async void CopyCredentials(string serviceName)
        {
            if (!credentials.ContainsKey(serviceName)) return;

            var (email, password) = credentials[serviceName];

            Clipboard.SetText(email);
            MessageBox.Show($"Το email για {serviceName} αντιγράφηκε!", "Επιτυχία", MessageBoxButton.OK, MessageBoxImage.Information);

            await System.Threading.Tasks.Task.Delay(2000);
            Clipboard.SetText(password);
            MessageBox.Show($"Ο κωδικός για {serviceName} αντιγράφηκε!", "Επιτυχία", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Window Controls
        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void BackButton_Click(object sender, RoutedEventArgs e) => Close();
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left) DragMove();
        }
    }
}
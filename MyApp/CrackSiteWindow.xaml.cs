using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace MyApp
{
    public partial class CrackSiteWindow : Window
    {
        // Λίστα για την αντιστοίχιση κουμπιών με URLs
        private Dictionary<string, string> _buttonUrls = new Dictionary<string, string>
        {
            { "Crack Site#1", "https://filecr.com/en/" },
            { "Crack Site#2", "https://www.downloadpirate.com/" },
            { "Crack Site#3", "https://www.thepiratecity.co/" },
            { "Crack Site#4", "" }, // Προσθέστε το URL όταν είστε έτοιμοι
            { "Crack Games#1", "https://repack-games.com/" },
            { "Crack Games#2", "https://steamunlocked.net/" },
            { "Crack Games#3", "" }, // Προσθέστε το URL όταν είστε έτοιμοι
            { "Crack Games#4", "" }  // Προσθέστε το URL όταν είστε έτοιμοι
        };

        public CrackSiteWindow()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Κλείσιμο του CrackSiteWindow
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Τερματισμός ολόκληρης της εφαρμογής
            Application.Current.Shutdown();
        }

        private void NavigateToLink_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Content is string buttonContent)
            {
                if (_buttonUrls.TryGetValue(buttonContent, out string url) && !string.IsNullOrEmpty(url))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error opening URL: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("URL not set for this button.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
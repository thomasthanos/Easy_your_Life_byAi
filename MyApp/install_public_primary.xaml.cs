using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MyApp
{
    public partial class install_public_primary : Window
    {
        private Dictionary<string, string> downloadLinks;
        private List<string> downloadedFiles = new List<string>();
        private bool isDragging;

        public install_public_primary()
        {
            InitializeComponent();
            InitializeDownloadLinks();
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            // Enable window dragging
            MouseDown += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed && e.OriginalSource is not Button)
                {
                    isDragging = true;
                    DragMove();
                }
            };
            MouseUp += (s, e) => isDragging = false;

            // Handle DPI scaling
            Loaded += (s, e) =>
            {
                var dpi = VisualTreeHelper.GetDpi(this);
                var scale = dpi.DpiScaleX;
                if (scale > 1)
                {
                    Width *= scale;
                    Height *= scale;
                }
            };
        }

        private void InitializeDownloadLinks()
        {
            downloadLinks = new Dictionary<string, string>
            {
                { "discord", "https://www.dropbox.com/scl/fi/skxd8a63snzmciqdhdclt/discord.exe?rlkey=pcfengbhf2wcstvunhh127l39&st=2pojcgit&dl=1" },
                { "discord_ptb", "https://www.dropbox.com/scl/fi/s3mjizraz0xthulzjstb8/discord_ptb.exe?rlkey=asslvcp52zq995bb0xao12s5b&st=euikzqqd&dl=1" },
                { "brave", "https://www.dropbox.com/scl/fi/hrzzxh78i2su3ydoq5xaq/brave.exe?rlkey=b9mbyg4lqylhl8ajm122rbjy7&st=sbz96p8d&dl=1" },
                { "better_discord", "https://www.dropbox.com/scl/fi/1zkmjej8c8qwt0hrpr7x3/better_discord.exe?rlkey=bv0y0nmz4o8fuycyzsoz0emu5&st=w02kl1zh&dl=1" },
                { "steam", "https://www.dropbox.com/scl/fi/gop5c9hu4e2gxe0t4mlwi/steam.exe?rlkey=4k5wmsqs8r7srs4syf9qsixj5&st=tyfftt01&dl=1" },
                { "epic_games", "https://www.dropbox.com/scl/fi/zqvmbsdc9id0exjjoexck/epic_games.msi?rlkey=h6cgvu6u3tnfivd9wy8umpb8y&st=i0jasrxz&dl=1" },
                { "ubisoft", "https://www.dropbox.com/scl/fi/78yfx1jinihuoj0itmcmr/ubisoft.exe?rlkey=dlcxncvl54tf510xpasicncux&st=kefgrf8l&dl=1" },
                { "ps_remote", "https://www.dropbox.com/scl/fi/s02e3g10dfhvqqc7cziq2/ps_remote.exe?rlkey=41hoicqpb5xg1rz9k6gfxarx1&st=xsvn7ilw&dl=1" },
                { "google_chrome", "https://www.dropbox.com/scl/fi/79qur9c5r2e066swc07bw/google_chrome.exe?rlkey=bbfk3k4pclm6gn0bmjd5liws5&st=qx45vfun&dl=1" },
                { "advanced", "https://www.dropbox.com/scl/fi/in6xaitegvbk29x4d9d4z/advanced.exe?rlkey=mu5g6crj7wgfeskr7kd1q7ek9&st=ps5zio1f&dl=1" },
                { "driver_booster", "https://www.dropbox.com/scl/fi/8xxw7dhb9upvsf9zj0743/driver_booster.exe?rlkey=r10z5lhscnwoleyu8tgmvvtvl&st=xe437qc7&dl=1" },
                { "iobit_unistaller", "https://www.dropbox.com/scl/fi/cpxovby07h1yues4rry59/iobit_unistaller.exe?rlkey=0e38dcckuka5xu38iz8hofdx0&st=dfhbifjx&dl=1" },
                { "vs_code", "https://www.dropbox.com/scl/fi/fww9w5usk5lfzm7izv94k/vs_code.exe?rlkey=1cskl4fnwgpv1clb99e503eij&st=wihhqo4c&dl=1" },
                { "visualstudio", "https://www.dropbox.com/scl/fi/7incnyznbmipoevxy0hjv/VisualStudio.exe?rlkey=njd5kbfek303k42rlb71k3evu&st=2pfim3nb&dl=1" },
                { "nvidia", "https://www.dropbox.com/scl/fi/4eqdp9q6iumvoidsesas8/nvidia.exe?rlkey=9ls58z0hyt49y7zjfxls0w23c&st=ryf2palp&dl=1" },
                { "nvidia_broadcast", "https://www.dropbox.com/scl/fi/g0hloddges0exc9p5xi6t/nvidia_broadcast.exe?rlkey=t0ag49xxf7pek7enukgdcjfzz&st=6iof2tgv&dl=1" },
                { "corsair", "https://www.dropbox.com/scl/fi/jb6m97dc4fsri5qk4qj1y/corsair.exe?rlkey=rpjsxf2wqbd0623t9feem2l8c&st=vmodv7i1&dl=1" },
                { "razer", "https://www.dropbox.com/scl/fi/m7za6610wbnj3rza5z6o2/razer.exe?rlkey=o5z3qnmjkfwk7ds5sm1xiyyll&st=vmoi7c54&dl=1" },
                { "streamdeck", "https://www.dropbox.com/scl/fi/iw93389uvw3d5cmh0xiaf/Stream_Deck.msi?rlkey=k5jm2sho7enfpjf6f8aqduvvw&st=sodteoo6&dl=1" },
                { "github", "https://www.dropbox.com/scl/fi/b2et7kqhux25yqaza2oxw/github.exe?rlkey=r7u24xvwbzm8luah0v7egvn2o&st=g4z04lm8&dl=1" },
                { "bitdefender", "https://www.dropbox.com/scl/fi/g5x4uvap1ixs00gulb3dv/bitdefender.exe?rlkey=ewyvjij6ox3qhcto6fbzcj60m&st=y4fqdotg&dl=1" },
                { "bluestack", "https://www.dropbox.com/scl/fi/zv6rvcxogquwv64u0vnvc/bluestack.exe?rlkey=gjrl55lwnrkdnf82fsv4wqmav&st=4oci3z48&dl=1" },
                { "spotify", "https://www.dropbox.com/scl/fi/vmialbggtyilkdsfvytgz/spotify.exe?rlkey=nojf5nixlg5pndj5voz9mw19o&st=sc7cne5p&dl=1" },
                { "winrar", "https://www.dropbox.com/scl/fi/9sf9atwsm7l9z27f5ot6b/winrar.exe?rlkey=0vpe69i23wh9uiybfwwqo28i9&st=i5gv0val&dl=1" },
                { "malwarebytes", "https://www.dropbox.com/scl/fi/293x84z02doqb2glft8xd/malwarebytes.exe?rlkey=4790thgb5hq4fy7kd8xsae7ef&st=kclzpr94&dl=1" },
                { "download_manager", "https://www.dropbox.com/scl/fi/3o0v73dt9e3bwkgcj3nec/download-manager.exe?rlkey=suy2i0ylak9rldqevyzpx0tpc&st=jw6wsyvw&dl=1" },
                { "wemod", "https://www.dropbox.com/scl/fi/hgx58f7qzbblu8ltn27zu/WeMod.exe?rlkey=n3tzdk4v1qmwdecn13kybb3fr&st=s2gn60ta&dl=1" },
                { "poweriso", "https://www.dropbox.com/scl/fi/rw38p7dyyvcnl4fl6hw1o/PowerISO.exe?rlkey=6ymej3ju0hohl3mo0gbxilq18&st=0ur5cghf&dl=1" },
                { "virtualbox", "https://www.dropbox.com/scl/fi/dhk2cw15zfc0vbvjgx341/VirtualBox.exe?rlkey=f2wf71exgbcj5sfbdd96psdcf&st=bwub9s34&dl=1" },
                { "savewizard", "https://www.dropbox.com/scl/fi/cqpa1xy4lrsykx9e2mru0/SaveWizard.msi?rlkey=in8zrig8o16muhnz9fpo72t04&st=481wl3w6&dl=1" },
                { "advancedinstaller", "https://www.dropbox.com/scl/fo/vhrcc4gjaastqkyoiuusw/AOfb0J_v1LLsUX-nEwNbRdY?rlkey=qoqn8odq2jcrillc74e1mmmmy&st=3yca0i6g&dl=1" },
                { "", "" },
            };
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            string downloadsFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string folderPath = System.IO.Path.Combine(downloadsFolder, "auto_install_apps");

            if (Directory.Exists(folderPath))
            {
                try
                {
                    Directory.Delete(folderPath, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting folder: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            Application.Current.Shutdown();
        }

        private void AppButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string appName)
            {
                AddToListBox(appName);
            }
        }

        private void AddToListBox(string appName)
        {
            if (string.IsNullOrEmpty(appName))
            {
                MessageBox.Show("Invalid application name!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!AppListBox.Items.Contains(appName))
            {
                AppListBox.Items.Add(appName);
            }
        }

        private async void DownloadAll_Click(object sender, RoutedEventArgs e)
        {
            if (AppListBox.Items.Count == 0)
            {
                MessageBox.Show("No applications selected for download!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string downloadsFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string customFolder = System.IO.Path.Combine(downloadsFolder, "auto_install_apps");
            Directory.CreateDirectory(customFolder);

            var appsToDownload = new List<string>(AppListBox.Items.Cast<string>());

            foreach (var appName in appsToDownload)
            {
                string filePath = System.IO.Path.Combine(customFolder, $"{appName}.exe");

                try
                {
                    await DownloadFileTaskAsync(appName, filePath);

                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"The installation of {appName} has completed!", $"{appName} - Completed", MessageBoxButton.OK, MessageBoxImage.Information);
                        AppListBox.Items.Remove(appName);
                    });

                    await Task.Delay(5000);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error downloading {appName}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task DownloadFileTaskAsync(string appName, string filePath)
        {
            if (!downloadLinks.ContainsKey(appName))
            {
                throw new ArgumentException($"Unknown app name: {appName}");
            }

            string url = downloadLinks[appName];

            using (var client = new WebClient())
            {
                client.DownloadProgressChanged += (s, e) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        DownloadProgressBar.Maximum = e.TotalBytesToReceive > 0 ? e.TotalBytesToReceive : 100;
                        DownloadProgressBar.Value = e.BytesReceived;
                    });
                };

                client.DownloadFileCompleted += async (s, e) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        AppListBox.Items.Remove(appName);
                        DownloadProgressBar.Value = 0;
                    });

                    await Task.Delay(3000);
                };

                try
                {
                    if (appName.Equals("advancedinstaller", StringComparison.OrdinalIgnoreCase))
                    {
                        filePath = System.IO.Path.ChangeExtension(filePath, ".zip");
                    }

                    await client.DownloadFileTaskAsync(new Uri(url), filePath);

                    if (appName.Equals("advancedinstaller", StringComparison.OrdinalIgnoreCase))
                    {
                        string sevenZipPath = Get7ZipPath();
                        string extractPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filePath), "AdvancedInstaller");
                        Directory.CreateDirectory(extractPath);

                        string arguments = $"x \"{filePath}\" -o\"{extractPath}\" -y";
                        var process = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = sevenZipPath,
                                Arguments = arguments,
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true
                            }
                        };

                        process.Start();
                        await process.WaitForExitAsync();

                        Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show($"The extraction of {appName} has completed!", $"{appName} - Extraction Completed", MessageBoxButton.OK, MessageBoxImage.Information);
                        });

                        File.Delete(filePath);

                        string msiFilePath = System.IO.Path.Combine(extractPath, "advancedinstaller.msi");
                        if (File.Exists(msiFilePath))
                        {
                            var msiProcess = new Process
                            {
                                StartInfo = new ProcessStartInfo
                                {
                                    FileName = msiFilePath,
                                    UseShellExecute = true
                                }
                            };

                            msiProcess.Start();
                            await msiProcess.WaitForExitAsync();

                            Dispatcher.Invoke(() =>
                            {
                                MessageBox.Show($"The installation of {appName} has completed!", $"{appName} - Installation Completed", MessageBoxButton.OK, MessageBoxImage.Information);
                            });
                        }
                        else
                        {
                            MessageBox.Show("The advancedinstaller.msi file was not found after extraction.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        var process = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = filePath,
                                UseShellExecute = true
                            }
                        };

                        process.Start();
                        await process.WaitForExitAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error downloading {appName}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string Get7ZipPath()
        {
            string appDirectory = GetAppInstallationPath();
            string sevenZipPath = System.IO.Path.Combine(appDirectory, "7-Zip", "7z.exe");

            if (!File.Exists(sevenZipPath))
            {
                throw new FileNotFoundException("The 7z.exe program was not found.");
            }

            return sevenZipPath;
        }

        private string GetAppInstallationPath()
        {
            return @"C:\Program Files (x86)\Kolokithes A.E\Make your life easier";
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            var toolTipWindow = new AnimatedToolTip("Click an application icon to add it to the list.\n" +
                                                   "Once you've selected all desired apps,\n" +
                                                   "click 'Download All' to download and install them automatically.");
            toolTipWindow.Show();
        }

        private void ActivateAutoLoginButton_Click(object sender, RoutedEventArgs e) { }
        private void SysMaintenanceButton_Click(object sender, RoutedEventArgs e) { }
        private void CrackSitesButton_Click(object sender, RoutedEventArgs e) { }
        private void CrackAppsButton_Click(object sender, RoutedEventArgs e) { }
        private void install_public2_Click(object sender, RoutedEventArgs e) { }

        public class AnimatedToolTip : Window
        {
            private readonly DispatcherTimer animationTimer;
            private double opacityStep;

            public AnimatedToolTip(string message)
            {
                WindowStyle = WindowStyle.None;
                AllowsTransparency = true;
                Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));
                Foreground = Brushes.White;
                ShowInTaskbar = false;
                Topmost = true;
                Width = 300;
                Height = 100;
                ResizeMode = ResizeMode.NoResize;

                var border = new Border
                {
                    CornerRadius = new CornerRadius(35),
                    BorderBrush = new SolidColorBrush(Color.FromArgb(255, 100, 100, 100)),
                    BorderThickness = new Thickness(5),
                    Background = Background,
                    Child = new TextBlock
                    {
                        Text = message,
                        Foreground = Foreground,
                        FontFamily = new FontFamily("Segoe UI"),
                        FontSize = 10,
                        Margin = new Thickness(10),
                        TextWrapping = TextWrapping.Wrap
                    }
                };

                Content = border;

                opacityStep = 0.1;
                Opacity = 0;
                animationTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(20) };
                animationTimer.Tick += (s, e) =>
                {
                    Opacity += opacityStep;
                    if (Opacity >= 1 || Opacity <= 0)
                    {
                        animationTimer.Stop();
                        if (Opacity <= 0) Close();
                    }
                };

                Loaded += (s, e) =>
                {
                    Window owner = Application.Current.MainWindow;
                    if (owner != null)
                    {
                        Left = owner.Left + (owner.Width - Width) / 2;
                        Top = owner.Top + owner.Height - Height - 40;
                    }
                    animationTimer.Start();
                };

                MouseLeave += (s, e) =>
                {
                    opacityStep = -0.1;
                    animationTimer.Start();
                };
            }
        }

    }
}
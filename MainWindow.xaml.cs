using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Hardcodet.Wpf.TaskbarNotification;

namespace Trackify
{
    public partial class MainWindow : Window
    {
        private readonly SongMonitor monitor;
        private readonly GenreFetcher genreFetcher = new();

        private TaskbarIcon trayIcon;
        private bool popupVisible = false;

        private string lastTitle = "";
        private string lastArtist = "";

        public MainWindow()
        {
            InitializeComponent();

            Loaded += Window_Loaded;

            monitor = new SongMonitor();

            monitor.Start(async (title, artist, imageBytes) =>
            {
                if (title == lastTitle && artist == lastArtist)
                    return;

                lastTitle = title;
                lastArtist = artist;

                Dispatcher.Invoke(() =>
                {
                    SongTitle.Text = title;
                    ArtistName.Text = artist;
                    GenreText.Text = "Loading...";

                    if (imageBytes != null)
                        AlbumArt.Source = LoadImage(imageBytes);

                    ShowPopup();
                });

                string genre = await genreFetcher.GetGenreAsync(artist);

                Dispatcher.Invoke(() =>
                {
                    GenreText.Text = genre;
                });
            });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var workArea = SystemParameters.WorkArea;

            Left = workArea.Right - Width - 20;
            Top = workArea.Top + 20;

            Opacity = 0;

            SetupTrayIcon();
        }

        private void SetupTrayIcon()
        {
            trayIcon = new TaskbarIcon();

            try
            {
                trayIcon.Icon = new System.Drawing.Icon("Assets/icon.ico");
            }
            catch { }

            trayIcon.ToolTipText = "Trackify";

            trayIcon.TrayMouseDoubleClick += (s, e) =>
            {
                Show();
                Activate();
            };
        }

        private async void ShowPopup()
        {
            if (popupVisible)
                return;

            popupVisible = true;

            var workArea = SystemParameters.WorkArea;

            Left = workArea.Right - Width - 20;

            double startTop = workArea.Top - Height;
            double endTop = workArea.Top + 20;

            Top = startTop;

            var slide = new DoubleAnimation(startTop, endTop,
                TimeSpan.FromMilliseconds(250));

            BeginAnimation(TopProperty, slide);

            var fadeIn = new DoubleAnimation(0, 1,
                TimeSpan.FromMilliseconds(200));

            BeginAnimation(OpacityProperty, fadeIn);

            await Task.Delay(4000);

            var fadeOut = new DoubleAnimation(1, 0,
                TimeSpan.FromMilliseconds(250));

            BeginAnimation(OpacityProperty, fadeOut);

            await Task.Delay(300);

            popupVisible = false;
        }

        private BitmapImage LoadImage(byte[] bytes)
        {
            using var ms = new MemoryStream(bytes);

            var img = new BitmapImage();

            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.StreamSource = ms;
            img.EndInit();
            img.Freeze();

            return img;
        }

        private void Root_MouseEnter(object sender, MouseEventArgs e)
        {
            var anim = new DoubleAnimation(1, TimeSpan.FromMilliseconds(150));
            ControlPanel.BeginAnimation(OpacityProperty, anim);
        }

        private void Root_MouseLeave(object sender, MouseEventArgs e)
        {
            var anim = new DoubleAnimation(0, TimeSpan.FromMilliseconds(150));
            ControlPanel.BeginAnimation(OpacityProperty, anim);
        }

        private async void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            var manager = await Windows.Media.Control
                .GlobalSystemMediaTransportControlsSessionManager.RequestAsync();

            var session = manager.GetCurrentSession();
            if (session != null)
                await session.TryTogglePlayPauseAsync();
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var manager = await Windows.Media.Control
                .GlobalSystemMediaTransportControlsSessionManager.RequestAsync();

            var session = manager.GetCurrentSession();
            if (session != null)
                await session.TrySkipNextAsync();
        }

        private async void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            var manager = await Windows.Media.Control
                .GlobalSystemMediaTransportControlsSessionManager.RequestAsync();

            var session = manager.GetCurrentSession();
            if (session != null)
                await session.TrySkipPreviousAsync();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
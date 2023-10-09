using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Timers;
using System.Xml.Schema;
using VideoPlayer.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace VideoPlayer
{
    /// <summary>
    /// Логика взаимодействия для VideoPlayerFrame.xaml
    /// </summary>
    public partial class VideoPlayerFrame : Page
    {
        DispatcherTimer timer;
        private bool volumeButtonState = false;
        private bool playButtonState = false;
        private bool windowSizeState = false;

        public VideoPlayerFrame()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timer_tick);
        }

        private void timer_tick(object sender, EventArgs e)
        {
            slider_seek.Value = mediaVideo.Position.TotalSeconds;
        }

        private void Page_Drop(object sender, DragEventArgs e)
        {
            string filename = (string)((DataObject)e.Data).GetFileDropList()[0];
            mediaVideo.Source = new Uri(filename);
            mediaVideo.LoadedBehavior = MediaState.Manual;
            mediaVideo.UnloadedBehavior = MediaState.Manual;
            mediaVideo.Volume = (double)slider_vol.Value;
            mediaVideo.Play();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mediaVideo.Source = new Uri(Globals.pathVideo);
            mediaVideo.LoadedBehavior = MediaState.Manual;
            mediaVideo.UnloadedBehavior = MediaState.Manual;
            mediaVideo.Volume = (double)slider_vol.Value;
            string path = Globals.pathVideo;
            string dir = System.IO.Path.GetDirectoryName(path);
            string file = System.IO.Path.GetFileName(path);

            Type shellAppType = Type.GetTypeFromProgID("Shell.Application");
            dynamic shell = Activator.CreateInstance(shellAppType);
            dynamic folder = shell.NameSpace(dir);
            dynamic folderItem = folder.ParseName(file);
            string value = folder.GetDetailsOf(folderItem, 27).ToString();

            AllTimeLabel.Text = "/" + value.ToString();

            mediaVideo.Play();
        }

        private void PauseVideo_Click(object sender, RoutedEventArgs e)
        {
            mediaVideo.Pause();
        }
        private void StopVideo_Click(object sender, RoutedEventArgs e)
        {
            mediaVideo.Stop();
        }
        private void PlayVideo_Click(object sender, RoutedEventArgs e)
        {
            if (!playButtonState)
            {
                mediaVideo.Pause();
                playButtonState = true;
            }
            else
            {
                mediaVideo.Play();
                playButtonState = false;
            }
        }
        void TimeToString(double t)
        {
            TimeSpan mmm = TimeSpan.FromSeconds(t);
            string temp = mmm.ToString();
            temp = temp.Substring(0,8);
            TimeLabel.Text = temp;
        }
        private void slider_seek_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaVideo.Position = TimeSpan.FromSeconds(slider_seek.Value);
            TimeToString(slider_seek.Value);
        }
        private void slider_vol_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaVideo.Volume = (double)slider_vol.Value;
        }
        private void mediaVideo_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = mediaVideo.NaturalDuration.TimeSpan;
            slider_seek.Maximum = ts.TotalSeconds;
            timer.Start();
        }
        private void ButtonVolume_Click(object sender, RoutedEventArgs e)
        {
            if (!volumeButtonState)
            {
                slider_vol.Visibility = Visibility.Visible;
                volumeButtonState = true;
            }
            else
            {
                slider_vol.Visibility = Visibility.Hidden;
                volumeButtonState = false;
            }
        }
        private async void mediaVideo_MouseMove(object sender, MouseEventArgs e)
        {
            if (StackPanelTools.Visibility == Visibility.Visible)
            {
                return;
            }
            StackPanelTools.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
            await Task.Delay(3000);
            BackButton.Visibility = Visibility.Hidden;
            StackPanelTools.Visibility = Visibility.Hidden;
        }
        private async void mediaVideo_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (StackPanelTools.Visibility == Visibility.Visible)
            {
                return;
            }
            StackPanelTools.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
            await Task.Delay(3000);
            BackButton.Visibility = Visibility.Hidden;
            StackPanelTools.Visibility = Visibility.Hidden;
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            mediaVideo.Stop();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!windowSizeState)
            {
                MainWindow SetWindow = Window.GetWindow(this) as MainWindow;
                SetWindow.WindowState = WindowState.Maximized;
                SetWindow.TitleBar.Visibility = Visibility.Hidden;
                BackButton.Visibility = Visibility.Hidden;
                windowSizeState = true;
            }
            else
            {
                MainWindow SetWindow = Window.GetWindow(this) as MainWindow;
                SetWindow.TitleBar.Visibility = Visibility.Visible;
                BackButton.Visibility = Visibility.Visible;
                windowSizeState = false;
            }
        }
        private async void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (StackPanelTools.Visibility == Visibility.Visible)
            {
                return;
            }
            StackPanelTools.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
            await Task.Delay(3000);
            BackButton.Visibility = Visibility.Hidden;
            StackPanelTools.Visibility = Visibility.Hidden;
        }
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow SetWindow = Window.GetWindow(this) as MainWindow;
            mediaVideo.Stop();
            BackButton.Visibility = Visibility.Hidden;
            SetWindow.TitleBar.Visibility = Visibility.Visible;
            SetWindow.WindowState = WindowState.Normal;
            SetWindow.VideoPlayerFrame2.Visibility = Visibility.Hidden;
        }
        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            slider_seek.Value -= 15;
        }
        private void Vpered_Click(object sender, RoutedEventArgs e)
        {
            slider_seek.Value += 15;

        }
    }
}

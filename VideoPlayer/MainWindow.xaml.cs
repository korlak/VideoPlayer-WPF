using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using VideoPlayer.Models;
using VideoPlayer.Services;
using File = System.IO.File;

namespace VideoPlayer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private readonly string PATH = $"{Environment.CurrentDirectory}\\DirectoriesVideo.json";
        private List<FoldersModels> _buttonsList = new List<FoldersModels>();
        private BindingList<FilesModels> _filesList = new BindingList<FilesModels>();
        public FileIO _fileIO;

        static String BytesToString(long byteCount)
        {
            string[] suf = { "Byt", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0 " + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _fileIO = new FileIO(PATH);
            try
            {
                this.Resources = new ResourceDictionary() { Source = new Uri("pack://application:,,,/DictionaryStyles.xaml") };

                _buttonsList = _fileIO.LoadData();
                int lenght = _buttonsList.Count;
                for (int i = 0; i < lenght; i++)
                {
                    string name = _buttonsList[i].name;
                    System.Windows.Controls.Button button = new System.Windows.Controls.Button();
                    button.Name = "X" + _buttonsList[i].id.ToString();
                    button.Style = (Style)Resources["FolderButton"];
                    button.Content = name;
                    button.Click += ButtonCreatedByCode_Click;
                    DockFolders.Children.Add(button);
                }
            }
            catch (Exception ex)
            {

                System.Windows.Forms.MessageBox.Show(ex.Message);
                System.Windows.Application app = System.Windows.Application.Current;
                app.Shutdown();
            }

        }
        private void ButtonAddFile_Click(object sender, RoutedEventArgs e)
        {
            this.Resources = new ResourceDictionary() { Source = new Uri("pack://application:,,,/DictionaryStyles.xaml") };
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            DialogResult result = folderBrowser.ShowDialog();
            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                string pathD = folderBrowser.SelectedPath;
                int id;
                try
                {
                    id = _buttonsList.Last().id + 1;
                }
                catch
                {
                    id = 0;
                }

                string name = new DirectoryInfo(folderBrowser.SelectedPath).Name;
                System.Windows.Controls.Button button = new System.Windows.Controls.Button();
                button.Name = "X" + id.ToString();
                button.Style = (Style)Resources["FolderButton"];
                button.Content = name;
                button.Click += ButtonCreatedByCode_Click;
                DockFolders.Children.Add(button);

                FoldersModels az = new FoldersModels(id, pathD, name);

                _buttonsList.Add(az);
                _fileIO.SaveData(_buttonsList);

                txtbox.Text = pathD;

            }
        }
        private void ButtonCreatedByCode_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
            FoldersModels found = _buttonsList.Find(item => "X" + item.id.ToString() == button.Name);
            this.Resources = new ResourceDictionary() { Source = new Uri("pack://application:,,,/DictionaryStyles.xaml") };
            string[] allfiles = Directory.GetFiles(found.path);
            _filesList.Clear();
            for (int i = 0; i < allfiles.Length; i++)
            {
                var size = "";
                string path = allfiles[i];
                string dir = Path.GetDirectoryName(path);
                string file = Path.GetFileName(path);
                Type shellAppType = Type.GetTypeFromProgID("Shell.Application");
                dynamic shell = Activator.CreateInstance(shellAppType);
                dynamic folder = shell.NameSpace(dir);
                dynamic folderItem = folder.ParseName(file);
                size = folder.GetDetailsOf(folderItem, 27);
                string name = Path.GetFileName(allfiles[i]);
                long filelengthtemp = new FileInfo(allfiles[i]).Length;
                string filelength = BytesToString(filelengthtemp);
                string date = File.GetCreationTime(allfiles[i]).ToString();
                var extension = Path.GetExtension(allfiles[i]);
                string path1 = allfiles[i];
                if (size == "")
                    continue;

                FilesModels gg = new FilesModels(name, date, size, filelength, extension, path1);
                _filesList.Add(gg);
                dg.ItemsSource = _filesList;
       
            }
        }

        private void RowDoubleClick(object sender, RoutedEventArgs e)
        {
            FilesModels gg = (FilesModels)dg.SelectedItem;
            Globals.pathVideo = gg.path;

            VideoPlayerFrame2.Content = new VideoPlayerFrame();
            VideoPlayerFrame2.Visibility = Visibility.Visible;
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application app = System.Windows.Application.Current;
          //  Trash.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));

            app.Shutdown();
        }
        private void Turn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Trash_Click(object sender, RoutedEventArgs e)
        {
            UIElementCollection sa = DockFolders.Children;

            for (int i = 0; i < sa.Count; i++)
            {
                if (sa[i].Equals(ButtonAddFile))
                {
                    continue;
                }
                sa.Remove(sa[i]);
                i--;
            }
            File.Delete(PATH);
           // dg.Items.Clear();
        }
    }
}
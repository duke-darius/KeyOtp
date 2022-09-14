using Newtonsoft.Json;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using static System.Net.Mime.MediaTypeNames;
using Path = System.IO.Path;

namespace KeyOtp
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        // at class level
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        public LocalOtpDefinition? SelectedDefinition { get; set; }
        public LocalSettings Settings { get; set; }

        public SettingsWindow()
        {
            InitializeComponent();
            Settings = JsonConvert.DeserializeObject<LocalSettings>(JsonConvert.SerializeObject(MainWindow.Settings))!;

            SettingsListBox.SelectionChanged += SettingsListBox_SelectionChanged;

            Redraw();
        }

        private void SettingsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var x = SettingsListBox.SelectedItem as ListBoxItem;
            SelectedDefinition = x?.Tag as LocalOtpDefinition;
            RemoveButton.IsEnabled = (SettingsListBox.SelectedItem != null);

            RedrawEditPanel();

        }

        public void Redraw()
        {
            SettingsListBox.Items.Clear();
            foreach(var row in Settings.Definitions)
            {
                var item = new ListBoxItem() { Content = row.Name, Tag = row };
                SettingsListBox.Items.Add(item);
            }

            RemoveButton.IsEnabled = (SettingsListBox.SelectedItem != null);

            RedrawEditPanel();
        }

        public void RedrawEditPanel()
        {
            SelectedPanel.Visibility = SelectedDefinition == null ? Visibility.Hidden : Visibility.Visible;
            if (SelectedDefinition != null)
            {
                NameTextBox.Text = SelectedDefinition.Name;
                KeyTextBox.Text = SelectedDefinition.Key;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.Definitions.Add(new("New Totp", "ABCDEF"));
            Redraw();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDefinition == null)
                return;
            Settings.Definitions.Remove(SelectedDefinition);
            Redraw();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            LocalSettings.Save(Settings);
            MainWindow.Settings = LocalSettings.Load();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDefinition == null)
                return;
            SelectedDefinition.Name = NameTextBox.Text;
            SelectedDefinition.Key = KeyTextBox.Text;
            Redraw();
        }

        private void FromClipboardButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Clipboard.ContainsImage())
                {
                    var img = Clipboard.GetImage();
                    var filePath = Path.GetTempFileName() + ".png";
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();

                        encoder.Frames.Add(BitmapFrame.Create(img));
                        encoder.Save(fileStream);
                    }
                    var bytes = File.ReadAllBytes(filePath);
                    var bmp = (Bitmap)Bitmap.FromFile(filePath);
                    var lsrc = new BitmapLuminanceSource(bmp);
                    var bitmap2 = new BinaryBitmap(new HybridBinarizer(lsrc));

                    var result = new MultiFormatReader().decode(bitmap2);
                    //otpauth://totp/snapacc.com:SNAP?secret=SFU2ICHTRBRXA3OVWEOCXXCVIAZ5NGOH&issuer=snapacc.com
                    var uri = new Uri(result.Text);
                    var bits = HttpUtility.ParseQueryString(uri.Query);
                    KeyTextBox.Text = bits["secret"];
                }
            }
            catch
            {

            }
        }
    }
}

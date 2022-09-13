using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KeyOtp;

/// <summary>
/// Interaction logic for OtpWindow.xaml
/// </summary>
public partial class OtpWindow : MetroWindow
{
    public OtpWindow()
    {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        Deactivated += OtpWindow_Deactivated;

        foreach(var def in MainWindow.Settings.Definitions)
        {
            ItemsBox.Items.Add(new OtpSelector(def, this));
        }

        Activated += OtpWindow_Activated;
        ItemsBox.KeyDown += ItemsBox_KeyDown;
        
    }

    private void ItemsBox_KeyDown(object sender, KeyEventArgs e)
    {
        if(ItemsBox.SelectedItem is OtpSelector selector && (e.Key == Key.Space || e.Key == Key.Enter))
        {
            HandleDefSelect(selector.Def);
        }
    }

    private void OtpWindow_Activated(object? sender, EventArgs e)
    {
        ItemsBox.Focus();
    }

    public void HandleDefSelect(LocalOtpDefinition def)
    {
        System.Windows.Clipboard.SetText(def.GetCode());
        Close();
    }

    private void OtpWindow_Deactivated(object? sender, EventArgs e)
    {
        try
        {
            Close();
        }
        catch
        {

        }
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new SettingsWindow();
        window.ShowDialog();
    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KeyOtp
{
    /// <summary>
    /// Interaction logic for OtpSelector.xaml
    /// </summary>
    public partial class OtpSelector : ListBoxItem
    {
        public OtpSelector(LocalOtpDefinition def, OtpWindow otpWindow)
        {
            InitializeComponent();
            Def = def;
            OtpWindow = otpWindow;

            DefName = Def.Name;
            DataContext = this;
            RefreshCode();
            
            NameTextBox.Text = def.Name;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if(currentIsHigh != IsHigh)
            {
                RefreshCode();
            }
            double seconds = DateTime.Now.Second + (DateTime.Now.Millisecond / 1000d);
            if (currentIsHigh)
                seconds -= 30;
            Remaining.Value = seconds;
        }

        public string DefName { get; set; } 
        private void RefreshCode()
        {
            Dispatcher.Invoke(() =>
            {
                Code = Def.GetCode();
                CodeTextBox.Text = Code;
                IsHigh = currentIsHigh;
            }, DispatcherPriority.Render);
        }

        public DispatcherTimer timer { get; set; }
        public bool IsHigh { get; set; } = DateTime.Now.Second >= 30;
        private bool currentIsHigh => DateTime.Now.Second >= 30;
        public string Code { get; set; } = "";
        public LocalOtpDefinition Def { get; }
        public OtpWindow OtpWindow { get; }

    }
}

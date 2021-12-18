using BerthaRemote.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BerthaRemote.UserControls
{
    public sealed partial class PanTiltCameraDistanceUserControl : UserControl
    {
        MainViewModel mainViewModel => App.mainViewModel;
        public PanTiltSensorArray device => App.mainViewModel.Devices.PanTiltCombo;

        public PanTiltCameraDistanceUserControl()
        {
            this.InitializeComponent();
        }

        private void connectCamera_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(cameraIP.Text))
            {
                if (!string.IsNullOrEmpty(cameraIP.Text))
                {
                    CameraWebView.Navigate(new Uri(cameraIP.Text));
                }
            }
        }

        private void tiltUp_Click(object sender, RoutedEventArgs e)
        {
            if (device != null)
            {
                device.AutoPan(Enumerations.ServoMovementSpeed.Slow);
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (device != null)
            {
                device.Stop();
            }
        }
    }
}

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
    public sealed partial class StaticCameraUserControl : UserControl
    {
        public ForwardSensorArray device => App.mainViewModel.Devices.ForwardDistance;

        public StaticCameraUserControl()
        {
            this.InitializeComponent();
        }

        private void connectCamera_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(device.CameraIP))
                {
                    //Uri goTo = new Uri(@"http://" + device.CameraIP);
                    Uri goTo = new Uri(device.CameraIP);
                    //CameraWebView.Source=new Uri(@"http://" + device.CameraIP);
                    //CameraWebView.Navigate(goTo);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}

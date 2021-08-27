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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BerthaRemote.UserControls
{
    public class DeviceTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ManipulatorArmDataTemplate { get; set; }
        public DataTemplate PanTiltBaseDataTemplate { get; set; }
        public DataTemplate PanTiltCameraDataTemplate { get; set; }
        public DataTemplate PanTiltDistanceDataTemplate { get; set; }
        public DataTemplate StaticMountCameraDataTemplate { get; set; }
        public DataTemplate TemperatureSensorDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var vm = item as IDevice;

            if (vm != null)
            {
                switch (vm.TypeOfDevice)
                {
                    case DeviceType.ManipulatorArm : return ManipulatorArmDataTemplate; 
                    case DeviceType.PanTiltBase: return PanTiltBaseDataTemplate; 
                    case DeviceType.PanTiltCamera: return PanTiltCameraDataTemplate; 
                    case DeviceType.PanTiltDistance: return PanTiltDistanceDataTemplate; 
                    case DeviceType.StaticMountCamera: return StaticMountCameraDataTemplate; 
                    case DeviceType.TemperatureSensor: return TemperatureSensorDataTemplate; 
                    default: return StaticMountCameraDataTemplate; 
                }                
            }
            else
            {
                return null;
            }
        }
    }

    public sealed partial class DevicesGlobalUserControl : UserControl
    {
        MainViewModel mainViewModel => App.mainViewModel;

        public DevicesGlobalUserControl()
        {
            this.InitializeComponent();
        }

        private void connectCameraOne_Click(object sender, RoutedEventArgs e)
        {

        }

        private void connectCameraTwo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void connectCameraThree_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

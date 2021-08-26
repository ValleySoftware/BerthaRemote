using BurthaRemote.ViewModels;
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

namespace BurthaRemote.UserControls
{
    public sealed partial class MovementUserControl : UserControl
    {
        MainViewModel mainViewModel => App.mainViewModel;

        public MovementUserControl()
        {
            this.InitializeComponent();
        }

        private void TurnLeft_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TurnRight_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RotateLeft_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RotateRight_Click(object sender, RoutedEventArgs e)
        {

        }

        private void moveForward_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MoveBackwards_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MotorPowerOn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MotorPowerOff_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

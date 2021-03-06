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
    public sealed partial class DevicesGlobalUserControl : UserControl
    {
        MainViewModel mainViewModel => App.mainViewModel;

        public DevicesGlobalUserControl()
        {
            this.InitializeComponent();
        }
    }
}

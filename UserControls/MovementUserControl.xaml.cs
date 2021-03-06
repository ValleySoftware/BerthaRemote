using BerthaRemote.ViewModels;
using Enumerations;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization.NumberFormatting;
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
    public sealed partial class MovementUserControl : UserControl
    {
        MainViewModel mainViewModel => App.mainViewModel;

        public MovementUserControl()
        {
            this.InitializeComponent();
            /*
            SetNumberBoxNumberFormatterQuarters(movementDurationEdit);
            SetNumberBoxNumberFormatterInts(powerEdit);*/
        }

        public int StopModeAsInt(MovementAutoStopMode stopMode)
        {
            return Convert.ToInt32(stopMode);
        }

        public void StopModeToInt(int indexer)
        {
            mainViewModel.Movement.StopMode = (MovementAutoStopMode)indexer;
        }

        private void SetNumberBoxNumberFormatterQuarters(NumberBox numberBoxToFormat)
        {
            IncrementNumberRounder rounder = new IncrementNumberRounder();
            rounder.Increment = 1;//0.25;
            rounder.RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp;

            DecimalFormatter formatter = new DecimalFormatter();
            formatter.IntegerDigits = 1;
            formatter.FractionDigits = 0;
            formatter.NumberRounder = rounder;
            numberBoxToFormat.NumberFormatter = formatter;
        }

        private void SetNumberBoxNumberFormatterInts(NumberBox numberBoxToFormat)
        {
            IncrementNumberRounder rounder = new IncrementNumberRounder();
            rounder.Increment = 1;
            rounder.RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp;

            DecimalFormatter formatter = new DecimalFormatter();
            formatter.IntegerDigits = 1;
            formatter.FractionDigits = 0;
            formatter.NumberRounder = rounder;
            numberBoxToFormat.NumberFormatter = formatter;
        }

        private void TurnLeft_Click(object sender, RoutedEventArgs e)
        {
            mainViewModel.Movement.Move(Enumerations.Direction.TurnLeft, (int)powerEdit.Value);
        }

        private void TurnRight_Click(object sender, RoutedEventArgs e)
        {
            mainViewModel.Movement.Move(Enumerations.Direction.TurnRight, (int)powerEdit.Value);
        }

        private void RotateLeft_Click(object sender, RoutedEventArgs e)
        {
            mainViewModel.Movement.Move(Enumerations.Direction.RotateLeft, (int)powerEdit.Value);
        }

        private void RotateRight_Click(object sender, RoutedEventArgs e)
        {
            mainViewModel.Movement.Move(Enumerations.Direction.RotateRight, (int)powerEdit.Value);
        }

        private void moveForward_Click(object sender, RoutedEventArgs e)
        {
            mainViewModel.Movement.Move(Enumerations.Direction.Forward, (int)powerEdit.Value);
        }

        private void MoveBackwards_Click(object sender, RoutedEventArgs e)
        {
            mainViewModel.Movement.Move(Enumerations.Direction.Backwards, (int)powerEdit.Value);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            mainViewModel.Movement.Stop();
        }

        private void MotorPowerOn_Click(object sender, RoutedEventArgs e)
        {
            mainViewModel.Movement.TogglePowerOn(true);
        }

        private void MotorPowerOff_Click(object sender, RoutedEventArgs e)
        {
            mainViewModel.Movement.TogglePowerOn(false);
        }
    }
}

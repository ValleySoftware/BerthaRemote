using BerthaRemote.Helpers;
using Enumerations;
using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace BerthaRemote.ViewModels
{
    public partial class PanTiltSensorArray : BaseDeviceViewModel, IDevice
    {
        private string _cameraIP;
        private ObservableGattCharacteristics _btPanTiltCharacteristic;
        private ObservableGattCharacteristics _btSweepCharacteristic;
        private ObservableGattCharacteristics _btDistanceCharacteristic;
        private int _currentPan = 50;
        private int _currentTilt = 50;
        private double _distance = -1;
        private readonly int ConversionOffset = 90;

        public override void Load(DeviceListViewModel parentList, DeviceType typeOfDevice, string[] parameters)
        {
            if (!Thinking)
            {
                Thinking = true;

                base.Load(parentList, typeOfDevice, parameters);

                if (parameters != null &&
                    parameters.Count() > 1)
                {
                    CameraIP = (string)parameters[0];
                }

                _btPanTiltCharacteristic = App.mainViewModel.charPanTilt;
                UpdateCurrentPanTiltDisplayFromBLE();
                _btSweepCharacteristic = App.mainViewModel.charPanSweep;

                if ((typeOfDevice == DeviceType.PanTiltDistance) ||
                    (typeOfDevice == DeviceType.PanTiltCameraDistCombo))
                {
                    _btDistanceCharacteristic = App.mainViewModel.charPanTiltDistance;

                    _btDistanceCharacteristic.DispatcherQueue = App.dispatcherQueue;
                    _btDistanceCharacteristic.Characteristic.ValueChanged += Characteristic_ValueChanged;
                    _btDistanceCharacteristic.SetNotifyAsync();
                    _btDistanceCharacteristic.SetIndicateAsync();
                }

                IsReady = true;
                IsActive = true;

                Thinking = false;
            }
        }

        private async void UpdateCurrentPanTiltDisplayFromBLE()
        {
            var payload = await _btPanTiltCharacteristic.ReadValueAsync();
            var sp = payload.Split(BLEConstants.BLEHexDivider);

            if (sp.Count() >= 1)
            {

                int pan = StaticHelpers.BLEStringToIntSafe(sp[0]);
                int tilt = StaticHelpers.BLEStringToIntSafe(sp[1]);

                _currentPan = pan - 90;
                OnPropertyChanged(nameof(CurrentPan));
                _currentTilt = tilt - 90;
                OnPropertyChanged(nameof(CurrentTilt));
            }
        }

        private void Characteristic_ValueChanged(
            GattCharacteristic sender,
            GattValueChangedEventArgs args)
        {
            UpdateDistanceValue();
        }

        public async void UpdateDistanceValue()
        {
            if ((TypeOfDevice == DeviceType.PanTiltDistance) ||
                (TypeOfDevice == DeviceType.PanTiltCameraDistCombo))
            {
                try
                {
                    var result = await _btDistanceCharacteristic.ReadValueAsync();
                    Distance = StaticHelpers.BLEStringToIntSafe(result);
                }
                catch (Exception)
                {

                }
            }
        }

        public string CameraIP
        {
            get => _cameraIP;
            set => SetProperty(ref _cameraIP, value);
        }

        public int Distance
        {
            get => Convert.ToInt32(_distance);
            set => SetProperty(ref _distance, value);
        }

        public int CurrentPan
        {
            get => _currentPan;
            set
            {
                if (SetProperty(ref _currentPan, value))
                {
                    MoveToPosition(value, CurrentTilt);
                }
            }
        }

        public int CurrentTilt
        {
            get => _currentTilt;
            set
            {
                if (SetProperty(ref _currentTilt, value))
                {
                    MoveToPosition(CurrentPan, value);
                }
            }
        }

        private void MoveToPosition(int newPan, int newTilt)
        {
            string payload = 0 + BLEConstants.BLEMessageDivider + (CurrentPan + ConversionOffset) + BLEConstants.BLEMessageDivider + (CurrentTilt + ConversionOffset) + BLEConstants.BLEMessageDivider + (int)ServoMovementSpeed.Flank;

            var msg = App.mainViewModel.AddMessageToQueue(_btPanTiltCharacteristic, payload);
            var result = (msg.TransmissionStatus >= BLEMsgSendingStatus.InstantiatedOnly);
        }

        public void AutoPan(ServoMovementSpeed speed)
        {
                //string payload = ParentList.Items.IndexOf(this).ToString() + "-" + (int)speed;
                //GattCommunicationStatus sendResult = await MainViewModel.SendUtf8Message(_btSweepCharacteristic, payload);     
        }

        public void Stop()
        {
            //string payload = ParentList.Items.IndexOf(this).ToString() + "-" + (int)ServoMovementSpeed.Stop;
            //GattCommunicationStatus sendResult = await MainViewModel.SendUtf8Message(_btSweepCharacteristic, payload);
        }
    }
}

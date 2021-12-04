﻿using Enumerations;
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
        private double _distance = -1;


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
                _btSweepCharacteristic = App.mainViewModel.charPanSweep;
                _btDistanceCharacteristic = App.mainViewModel.charPanTiltDistanceDistance;


                _btDistanceCharacteristic.PropertyChanged += _btDistanceCharacteristic_PropertyChanged;
                _btDistanceCharacteristic.SetNotifyAsync();

                Thinking = false;
            }
        }

        private async void _btDistanceCharacteristic_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                var result = _btDistanceCharacteristic.Value; 
                //await _btDistanceCharacteristic.ReadValueAsync();

                var cleaned = result.Replace("-", "");
                var sp = cleaned.Split(Path.DirectorySeparatorChar);
                Distance = Convert.ToInt32(sp[0]);
            }
            catch (Exception)
            {

            }
        }

        public async void UpdateDistanceValue()
        {
            try
            {
                var result = await _btDistanceCharacteristic.ReadValueAsync();

                var cleaned = result.Replace("-", "");
                var sp = cleaned.Split(Path.DirectorySeparatorChar);
                Distance = Convert.ToInt32(sp[0]);
            }
            catch (Exception ex)
            {

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

        public void MoveToPosition(int newPan, int newTilt)
        {

        }

        public async void AutoPan(ServoMovementSpeed speed)
        {
                //string payload = ParentList.Items.IndexOf(this).ToString() + "-" + (int)speed;
                //GattCommunicationStatus sendResult = await MainViewModel.SendUtf8Message(_btSweepCharacteristic, payload);     
        }

        public async void Stop()
        {
            //string payload = ParentList.Items.IndexOf(this).ToString() + "-" + (int)ServoMovementSpeed.Stop;
            //GattCommunicationStatus sendResult = await MainViewModel.SendUtf8Message(_btSweepCharacteristic, payload);
        }
    }
}
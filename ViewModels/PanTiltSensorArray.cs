﻿using BerthaRemote.Helpers;
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

                if (typeOfDevice == DeviceType.PanTiltDistance)
                {
                    _btDistanceCharacteristic = App.mainViewModel.charPanTiltDistanceDistance;

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

        private void Characteristic_ValueChanged(
            GattCharacteristic sender,
            GattValueChangedEventArgs args)
        {
            UpdateDistanceValue();
        }

        public async void UpdateDistanceValue()
        {
            if (TypeOfDevice == DeviceType.PanTiltDistance)
            {
                try
                {
                    var result = await _btDistanceCharacteristic.ReadValueAsync();


                    string cleaned = string.Empty;

                    if (result.Contains("-"))
                    {
                        cleaned = result.Replace("-", "");
                        var c = StaticHelpers.StringToByteArray(cleaned);
                        Distance = Convert.ToInt32(c);
                    }
                    else
                    {
                        var sp = result.Split(Path.DirectorySeparatorChar);
                        Distance = Convert.ToInt32(sp[0]);
                    }
                }
                catch (Exception ex)
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

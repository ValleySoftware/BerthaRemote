using BerthaRemote.Helpers;
using Enumerations;
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
    public partial class ForwardSensorArray: BaseDeviceViewModel, IDevice
    {
        private string _cameraIP;
        private ObservableGattCharacteristics _btDistanceCharacteristic;
        private double _distance = -1;
        private ObservableGattCharacteristics _btLightsCharacteristic;
        private bool _lightsOn = false;

        public override void Load(DeviceListViewModel parentList, DeviceType typeOfDevice, string[] parameters)
        {
            if (!Thinking)
            {
                Thinking = true;

                base.Load(parentList, typeOfDevice, parameters);

                if (parameters != null &&
                    parameters.Count() > 0)
                {
                    CameraIP = parameters[0].ToString();
                }

                _btDistanceCharacteristic = App.mainViewModel.charForwardDistance;
                _btDistanceCharacteristic.DispatcherQueue = App.dispatcherQueue;
                _btDistanceCharacteristic.Characteristic.ValueChanged += Characteristic_ValueChanged;
                _btDistanceCharacteristic.SetNotifyAsync();
                _btDistanceCharacteristic.SetIndicateAsync();

                _btLightsCharacteristic = App.mainViewModel.charLightsCharacteristic;
                _btLightsCharacteristic.DispatcherQueue = App.dispatcherQueue;

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

        public string CameraIP
        {
            get => _cameraIP;
            set => SetProperty(ref _cameraIP, value);
        }

        public async void UpdateDistanceValue()
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

        public int Distance
        {
            get => Convert.ToInt32(_distance);
            set => SetProperty(ref _distance, value);
        }

        public bool LightsOn
        {
            get => _lightsOn;
            set
            {
                SetProperty(ref _lightsOn, value);
                SendLightBTMessage(value);
            }
        }

        private void SendLightBTMessage(bool valueToSend)
        {
            string payload = 0 + BLEConstants.BLEMessageDivider + valueToSend ;

            var msg = App.mainViewModel.AddMessageToQueue(_btLightsCharacteristic, payload);
            _ = (msg.TransmissionStatus >= BLEMsgSendingStatus.InstantiatedOnly);
        }
    }
}

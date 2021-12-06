using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerthaRemote.ViewModels
{
    public partial class ForwardSensorArray: BaseDeviceViewModel, IDevice
    {
        private string _cameraIP;
        private ObservableGattCharacteristics _btDistanceCharacteristic;
        private double _distance = -1;

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
                IsReady = true;
                IsActive = true;
                Thinking = false;
            }
        }

        private void Characteristic_ValueChanged(
            Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic sender, 
            Windows.Devices.Bluetooth.GenericAttributeProfile.GattValueChangedEventArgs args)
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

                var cleaned = result.Replace("-", "");
                var sp = cleaned.Split(Path.DirectorySeparatorChar);
                Distance = Convert.ToInt32(sp[0]);
            }
            catch (Exception ex)
            {

            }
        }

        public int Distance
        {
            get => Convert.ToInt32(_distance);
            set => SetProperty(ref _distance, value);
        }
    }
}

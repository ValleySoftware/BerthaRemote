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
                _btDistanceCharacteristic.PropertyChanged += _btDistanceCharacteristic_PropertyChanged;
                _btDistanceCharacteristic.SetNotifyAsync();

                Thinking = false;
            }
        }

        public string CameraIP
        {
            get => _cameraIP;
            set => SetProperty(ref _cameraIP, value);
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

        public int Distance
        {
            get => Convert.ToInt32(_distance);
            set => SetProperty(ref _distance, value);
        }
    }
}

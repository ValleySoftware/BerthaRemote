using Enumerations;
using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace BerthaRemote.ViewModels
{
    public partial class PanTiltCameraDevice : BaseDeviceViewModel, IDevice
    {
        private string _cameraIP;
        private ObservableGattCharacteristics _btPanTiltCharacteristic;
        private ObservableGattCharacteristics _btSweepCharacteristic;
        private ObservableGattCharacteristics _btDistanceCharacteristic;
        private double _distance = -1;


        public override void Load(DeviceListViewModel parentList, DeviceType typeOfDevice, string[] parameters)
        {
            base.Load(parentList, typeOfDevice, parameters);

            if (parameters != null &&
                parameters.Count() > 1)
            {
                CameraIP = (string)parameters[0];
            }

            _btPanTiltCharacteristic = App.mainViewModel.CurrentService.Characteristics.FirstOrDefault(s => s.UUID.Equals(Constants.UUIDPanTilt));
            _btSweepCharacteristic = App.mainViewModel.CurrentService.Characteristics.FirstOrDefault(s => s.UUID.Equals(Constants.UUIDPanSweep));
            _btDistanceCharacteristic = App.mainViewModel.CurrentService.Characteristics.FirstOrDefault(s => s.UUID.Equals(Constants.UUIDDistance));
            _btDistanceCharacteristic.PropertyChanged += _btDistanceCharacteristic_PropertyChanged;
            _btDistanceCharacteristic.SetNotifyAsync();
        }

        private async void _btDistanceCharacteristic_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                string result = await _btDistanceCharacteristic.ReadValueAsync();

                Distance = Convert.ToInt32(result);
            }
            catch (Exception distUpdateEx)
            {

            }
        }

        public async void UpdateDistanceValue()
        {
            try
            {
                //string result = await _btDistanceCharacteristic.ReadValueAsync();

                //Distance = Convert.ToInt32(result);
            }
            catch (Exception distUpdateEx)
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
                string payload = ParentList.Items.IndexOf(this).ToString() + "-" + (int)speed;
                GattCommunicationStatus sendResult = await MainViewModel.SendUtf8Message(_btSweepCharacteristic, payload);     
        }

        public async void Stop()
        {
            string payload = ParentList.Items.IndexOf(this).ToString() + "-" + (int)ServoMovementSpeed.Stop;
            GattCommunicationStatus sendResult = await MainViewModel.SendUtf8Message(_btSweepCharacteristic, payload);
        }
    }
}

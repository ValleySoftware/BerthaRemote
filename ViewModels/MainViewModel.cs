using MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.Helpers;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
using Buffer = Windows.Storage.Streams.Buffer;
using Enumerations;

namespace BerthaRemote.ViewModels
{
    public class MainViewModel : ValleyBaseViewModel
    {
        private ObservableBluetoothLEDevice _currentDevice;
        private ObservableGattDeviceService _currentService;
        private ObservableGattCharacteristics _currentCharacteristic;
        public BluetoothLEHelper bluetoothLEHelper = BluetoothLEHelper.Context;
        private MovementViewModel _movement;
        private DeviceListViewModel _devices = new DeviceListViewModel();

        public ObservableGattCharacteristics charMove;
        public ObservableGattCharacteristics charPower;
        public ObservableGattCharacteristics charStop;
        public ObservableGattCharacteristics charPanTilt;
        public ObservableGattCharacteristics charPanSweep;
        public ObservableGattCharacteristics charForwardDistance;
        public ObservableGattCharacteristics charPanTiltDistanceDistance;


        public MainViewModel()
        {
            bluetoothLEHelper.EnumerationCompleted += BluetoothLEHelper_EnumerationCompleted;
        }

        public async void BluetoothLEHelper_EnumerationCompleted(object sender, EventArgs e)
        {
            await App.dispatcherQueue.EnqueueAsync(() =>
            {
                bluetoothLEHelper.StopEnumeration();

                Thinking = false;
            });
        }

        public bool IsNotNull(object toCheck)
        {
            return (toCheck != null);
        }

        public bool CountGreaterThanZero(ObservableCollection<ObservableBluetoothLEDevice> toCheck)
        {
            return (toCheck != null && toCheck.Count() > 0);
        }

        public bool CountGreaterThanZero(ObservableCollection<ObservableGattDeviceService> toCheck)
        {
            return (toCheck != null && toCheck.Count() > 0);
        }

        public ObservableGattCharacteristics CurrentCharacteristic
        {
            get => _currentCharacteristic;
            set => SetProperty(ref _currentCharacteristic, value);
        }

        public ObservableBluetoothLEDevice CurrentDevice
        {
            get => _currentDevice;
            set => SetProperty(ref _currentDevice, value);
        }

        public ObservableGattDeviceService CurrentService
        {
            get => _currentService;
            set
            {
                SetProperty(ref _currentService, value);
                ConnectServices();
            }
        }

        public DeviceListViewModel Devices
        {
            get => _devices;
            set => SetProperty(ref _devices, value);
        }

    public MovementViewModel Movement
        {
            get => _movement;
            set => SetProperty(ref _movement, value);
        }

        public void ListAvailableBluetoothDevices()
        {
                Thinking = true;
                // Start the Enumeration
                bluetoothLEHelper.StopEnumeration();
                bluetoothLEHelper.StartEnumeration();
        }

        public async void ConnectToBTEDevice(ObservableBluetoothLEDevice deviceToConnectTo)
        {            
            try
            {
                if (deviceToConnectTo != null)
                {
                    bluetoothLEHelper.StopEnumeration();
                    Thinking = false;

                    await deviceToConnectTo.ConnectAsync();

                    if (deviceToConnectTo.IsConnected)
                    {
                        CurrentDevice = deviceToConnectTo;
                        /*if (!deviceToConnectTo.IsPaired)
                        {
                            await deviceToConnectTo.DoInAppPairingAsync();
                        }*/
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private void ConnectServices()
        {
            if (CurrentService != null)
            {
                if (Movement == null)
                {
                    Movement = new MovementViewModel();
                }

                try
                {
                    charMove = CurrentService.Characteristics.FirstOrDefault(m => m.UUID.Equals(Constants.UUIDAdvancedMove));
                    charPower = CurrentService.Characteristics.FirstOrDefault(p => p.UUID.Equals(Constants.UUIDPower));
                    charStop = CurrentService.Characteristics.FirstOrDefault(s => s.UUID.Equals(Constants.UUIDStop));
                    charPanTilt = App.mainViewModel.CurrentService.Characteristics.FirstOrDefault(s => s.UUID.Equals(Constants.UUIDPanTilt));
                    charPanSweep = App.mainViewModel.CurrentService.Characteristics.FirstOrDefault(s => s.UUID.Equals(Constants.UUIDPanSweep));
                    charForwardDistance = App.mainViewModel.CurrentService.Characteristics.FirstOrDefault(s => s.UUID.Equals(Constants.UUIDForwardDistance));
                    charPanTiltDistanceDistance = App.mainViewModel.CurrentService.Characteristics.FirstOrDefault(s => s.UUID.Equals(Constants.UUIDPanTiltDistance));


                    Movement.Load(
                        charMove,
                        charPower,
                        charStop
                        );

                    Devices.ForwardDistance = new ForwardSensorArray();
                    string[] scp = new string[1];
                    scp[0] = @"http://10.1.1.23";
                    Devices.ForwardDistance.Load(Devices, DeviceType.StaticMountCamera, scp);

                    Devices.PanTiltDist = new PanTiltSensorArray();
                    string[] ptcp = new string[2];
                    Devices.PanTiltDist.Load(Devices, DeviceType.PanTiltDistance, ptcp);

                    Devices.PanTiltCam = new PanTiltSensorArray();
                    string[] ctcp = new string[2];
                    ctcp[0] = @"http://10.1.1.21";
                    Devices.PanTiltCam.Load(Devices, DeviceType.PanTiltCamera, ptcp);
                }
                catch (Exception)
                {
                    Movement.IsReady = false;
                    
                }
            }
        }

        public static async Task<GattCommunicationStatus> SendUtf8Message(ObservableGattCharacteristics sendTo, string message)
        {
            GattCommunicationStatus result = GattCommunicationStatus.AccessDenied;

            try
            {
                if (!string.IsNullOrEmpty(message) &&
                    sendTo != null)
                {
                    IBuffer writeBuffer = null;
                    Console.WriteLine("Attempting BLE message sending (UTF8) - " + message + " - to " + sendTo.UUID);
                    writeBuffer = CryptographicBuffer.ConvertStringToBinary(
                        message,
                        BinaryStringEncoding.Utf8);
                    result = await sendTo.Characteristic.WriteValueAsync(writeBuffer);
                    Console.WriteLine("BLE message sent");
                }
            }
            catch (Exception)
            {

            }

                return result;
        }
    }
}

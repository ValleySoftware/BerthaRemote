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
using Enumerations;
using BerthaRemote.Helpers;

namespace BerthaRemote.ViewModels
{
    public class BleLogItem : ValleyBaseViewModel
    {
        string _characteristicName;
        string _message;
        DateTimeOffset _whenSent;

        public BleLogItem(string characteristicName, string message, DateTimeOffset whenSent, bool sentOk)
        {
            _characteristicName = characteristicName;
            _message = message;
            _whenSent = whenSent;
        }

        public string CharacteristicName
        {
            get => _characteristicName;
            set => SetProperty(ref _characteristicName, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
        public DateTimeOffset WhenSent
        {
            get => _whenSent;
            set => SetProperty(ref _whenSent, value);
        }

    }

    public class MainViewModel : ValleyBaseViewModel
    {
        private ObservableBluetoothLEDevice _currentDevice;
        private ObservableGattDeviceService _currentService;
        private ObservableGattCharacteristics _currentCharacteristic;
        public BluetoothLEHelper bluetoothLEHelper = BluetoothLEHelper.Context;
        private bool _communicationsReady = false;
        private MovementViewModel _movement;
        private DeviceListViewModel _devices = new DeviceListViewModel();

        public ObservableGattCharacteristics charMove;
        public ObservableGattCharacteristics charPower;
        public ObservableGattCharacteristics charStop;
        public ObservableGattCharacteristics charPanTilt;
        public ObservableGattCharacteristics charPanSweep;
        public ObservableGattCharacteristics charForwardDistance;
        public ObservableGattCharacteristics charPanTiltDistance;
        public ObservableGattCharacteristics charLightsCharacteristic;

        private ObservableCollection<BleLogItem> _bleMessageLog;

        public MainViewModel()
        {
            _bleMessageLog = new ObservableCollection<BleLogItem>();
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

        public ObservableCollection<BleLogItem> BleMsgLog
        {
            get => _bleMessageLog;
            set => SetProperty(ref _bleMessageLog, value);
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

        public bool CommunicationsReady
        {
            get => _communicationsReady;
            set => SetProperty(ref _communicationsReady, value);
        }        

        public ObservableGattDeviceService CurrentService
        {
            get => _currentService;
            set
            {
                if (SetProperty(ref _currentService, value))
                {
                    ConnectServices();
                };
            }
        }

        public DeviceListViewModel Devices
        {
            get => _devices;
            set
            {
                SetProperty(ref _devices, value);

            }
        }

    public MovementViewModel Movement
        {
            get => _movement;
            set => SetProperty(ref _movement, value);
        }

        public void ListAvailableBluetoothDevices()
        {
            App.dispatcherQueue.EnqueueAsync(() =>
            {
                Thinking = true;
                // Start the Enumeration
                bluetoothLEHelper.StopEnumeration();
                bluetoothLEHelper.StartEnumeration();
            });
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



                        foreach (var element in CurrentDevice.Services)
                        {
                            if (element.Name.Equals("41"))
                            {
                                if (!CommunicationsReady)
                                {
                                    await Task.Delay(TimeSpan.FromSeconds(1));
                                }
                                CurrentService = element;
                                break;
                            }
                        }
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
                    charMove = CurrentService.Characteristics.FirstOrDefault(m => m.UUID.Equals(BLEConstants.UUIDAdvancedMove));
                    charPower = CurrentService.Characteristics.FirstOrDefault(p => p.UUID.Equals(BLEConstants.UUIDPower));
                    charStop = CurrentService.Characteristics.FirstOrDefault(s => s.UUID.Equals(BLEConstants.UUIDStop));
                    charPanTilt = CurrentService.Characteristics.FirstOrDefault(s => s.UUID.Equals(BLEConstants.UUIDPanTilt));
                    charPanSweep = CurrentService.Characteristics.FirstOrDefault(s => s.UUID.Equals(BLEConstants.UUIDPanSweep));
                    charForwardDistance = CurrentService.Characteristics.FirstOrDefault(s => s.UUID.Equals(BLEConstants.UUIDForwardDistance));
                    charPanTiltDistance = CurrentService.Characteristics.FirstOrDefault(s => s.UUID.Equals(BLEConstants.UUIDPanTiltDistance));
                    charLightsCharacteristic = CurrentService.Characteristics.FirstOrDefault(s => s.UUID.Equals(BLEConstants.UUIDLights));

                    CommunicationsReady = true;


                    Movement.Load(
                        charMove,
                        charPower,
                        charStop
                        );

                    Devices.ForwardDistance = new ForwardSensorArray();
                    string[] scp = new string[1];
                    scp[0] = @"http://10.1.1.23";
                    Devices.ForwardDistance.Load(Devices, DeviceType.StaticMountCamera, scp);

                    Devices.PanTiltCombo = new PanTiltSensorArray();
                    string[] ptcp = new string[2];
                    ptcp[0] = @"http://10.1.1.21";
                    Devices.PanTiltCombo.Load(Devices, DeviceType.PanTiltCameraDistCombo, ptcp);

                    Devices.SetTimerStatus(true);
                }
                catch (Exception)
                {
                    Movement.IsReady = false;
                    CommunicationsReady = false;
                    
                }
            }
        }

        public async Task<GattCommunicationStatus> SendUtf8Message(ObservableGattCharacteristics sendTo, string message)
        {
            GattCommunicationStatus result = GattCommunicationStatus.AccessDenied;

            try
            {
                if (!string.IsNullOrEmpty(message) &&
                    sendTo != null)
                {
                    message = string.Concat(StaticHelpers.GenerateRandomString(), BLEConstants.BLEMessageDivider, message);
                    IBuffer writeBuffer = null;
                    Console.WriteLine("Attempting BLE message sending (UTF8) - " + message + " - to " + sendTo.UUID);
                    writeBuffer = CryptographicBuffer.ConvertStringToBinary(
                        message,
                        BinaryStringEncoding.Utf8);
                    result = await sendTo.Characteristic.WriteValueAsync(writeBuffer);
                    Console.WriteLine("BLE message sent");
                    BleMsgLog.Insert(0, new BleLogItem(sendTo.UUID, message, DateTimeOffset.Now, true));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("BLE message error" + e.Message);
                BleMsgLog.Insert(0, new BleLogItem(sendTo.Name, message, DateTimeOffset.Now, false));
            }

                return result;
        }
    }
}

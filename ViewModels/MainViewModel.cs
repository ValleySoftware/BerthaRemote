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
using Windows.UI;

namespace BerthaRemote.ViewModels
{
    public class BleLogItem : ValleyBaseViewModel
    {
        ObservableGattCharacteristics _characteristic;
        string _message;
        DateTimeOffset _whenQueued;
        DateTimeOffset? _whenSent;
        BLEMsgSendingStatus _transmissionStatus;

        public BleLogItem(ObservableGattCharacteristics characteristic, string message, DateTimeOffset whenQueued, DateTimeOffset? whenSent)
        {
            _characteristic = characteristic;
            _message = message;
            _whenQueued = whenQueued;
            _whenSent = whenSent;
            _transmissionStatus = BLEMsgSendingStatus.InstantiatedOnly;
        }

        public ObservableGattCharacteristics Characteristic
        {
            get => _characteristic;
            set => SetProperty(ref _characteristic, value);
        }

        public BLEMsgSendingStatus TransmissionStatus
        {
            get => _transmissionStatus;
            set
            {
                SetProperty(ref _transmissionStatus, value);
                OnPropertyChanged(nameof(TransmissionStatusColour));
            }
        }

        public Windows.UI.Xaml.Media.Brush TransmissionStatusColour
        {
            get
            {
                switch (TransmissionStatus)
                {
                    case BLEMsgSendingStatus.Error: return new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Red);
                    case BLEMsgSendingStatus.ReQueued: return new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Orange);
                    case BLEMsgSendingStatus.InstantiatedOnly: return new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Gray);
                    case BLEMsgSendingStatus.Queued: return new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Blue);
                    case BLEMsgSendingStatus.InProgress: return new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Yellow);
                    case BLEMsgSendingStatus.Success: return new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Green);
                    default: return new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Gray);
                }
            }
        }
        

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public DateTimeOffset? WhenSent
        {
            get => _whenSent;
            set => SetProperty(ref _whenSent, value);
        }
        public DateTimeOffset WhenQueued
        {
            get => _whenQueued;
            set => SetProperty(ref _whenQueued, value);
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
        private BleLogItem _currentBleMessage = null;

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

        public BleLogItem CurrentBleMessage
        {
            get => _currentBleMessage;
            set => SetProperty(ref _currentBleMessage, value);
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

        public BleLogItem AddMessageToQueue(ObservableGattCharacteristics sendTo, string message)
        {
            BleLogItem newMsg = null;

            try
            {
                newMsg = new BleLogItem(sendTo, message, DateTimeOffset.Now, null);

                BleMsgLog.Insert(0, newMsg);
                newMsg.TransmissionStatus = BLEMsgSendingStatus.Queued;

                ScanBLELogForStuffToSend();
            }
            catch (Exception e)
            {
                Console.WriteLine("BLE add message to queue error" + e.Message);
            }

            return newMsg;
        }

        private async void ScanBLELogForStuffToSend()
        {
            //Check the log exists
            if (BleMsgLog != null && 
                BleMsgLog.Count > 0)
            {
                //Is there no current message in the queue (Error or first message since boot)
                //Set it and then continue in method.
                if (CurrentBleMessage == null)
                {
                    SetCurrentBleMsgToLatestNotSentMsgInQueue();
                }

                // if the current isn't sent or sending
                if (CurrentBleMessage.TransmissionStatus <= BLEMsgSendingStatus.InProgress)
                {
                    await BleSend(CurrentBleMessage);
                    ScanBLELogForStuffToSend();
                }

            }

        }

        private void SetCurrentBleMsgToLatestNotSentMsgInQueue()
        {
            //Check the log exists
            if (BleMsgLog != null &&
                BleMsgLog.Count > 0)
            {
                if (CurrentBleMessage == null)
                {
                    CurrentBleMessage = BleMsgLog[BleMsgLog.Count - 1];
                    return;
                }

                if (CurrentBleMessage != BleMsgLog[0])
                {
                    //Work from newest backwards, should be a shorter scan for long up times                    
                    
                    int i = 0;
                    var element = BleMsgLog[i];

                    while (element.TransmissionStatus < BLEMsgSendingStatus.InProgress)
                    {
                        try
                        {
                            i++;
                            element = BleMsgLog[i];
                        }
                        catch (Exception)
                        {
                            return;
                        }
                    }
                }
            }
        }

        private async Task<bool> BleSend(BleLogItem message)
        {
            GattCommunicationStatus result = GattCommunicationStatus.AccessDenied;
            message.TransmissionStatus = BLEMsgSendingStatus.InProgress;

            try
            {
                IBuffer writeBuffer = null;
                Console.WriteLine("Attempting BLE message sending (UTF8) - " + message.Message + " - to " + message.Characteristic.UUID);
                writeBuffer = CryptographicBuffer.ConvertStringToBinary(
                    message.Message,
                    BinaryStringEncoding.Utf8);
                result = await message.Characteristic.Characteristic.WriteValueAsync(writeBuffer);

                if (result == GattCommunicationStatus.Success)
                {
                    message.WhenSent = DateTimeOffset.Now;
                    message.TransmissionStatus = BLEMsgSendingStatus.Success;
                    Console.WriteLine("BLE message sent");
                }
                else
                {
                    message.TransmissionStatus = BLEMsgSendingStatus.Error;
                }
            }
            catch (Exception)
            {
                message.TransmissionStatus = BLEMsgSendingStatus.Error;
            }

            return result == GattCommunicationStatus.Success; 
        }
    }
}

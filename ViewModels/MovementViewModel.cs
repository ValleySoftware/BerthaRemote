using BerthaRemote.Helpers;
using Enumerations;
using Microsoft.Toolkit.Uwp.Connectivity;
using MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace BerthaRemote.ViewModels
{
    public class MovementViewModel : ValleyBaseViewModel
    {
        private ObservableGattCharacteristics _advancedMovementCharacteristic;
        private ObservableGattCharacteristics _powerCharacteristic;
        private ObservableGattCharacteristics _stopCharacteristic;
        private double _duration = 250;
        private bool _isReady = false;
        public double MaxDuration { get => 10000; }
        public double MinDuration { get => 250; }
        private MovementAutoStopMode _stopMode = MovementAutoStopMode.Timespan;

        public MovementViewModel()
        {

        }

        public bool IsReady
        {
            get => _isReady;
            set => SetProperty(ref _isReady, value);
        }

        public double Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        public MovementAutoStopMode StopMode
        {
            get => _stopMode;
            set => SetProperty(ref _stopMode, value);
        }

        public async Task<bool> TogglePowerOn(bool toggleTo)
        {
            var result = false;

            try
            {
                string payload = string.Empty;

                if (toggleTo)
                {
                    payload = "1";
                }
                else
                {
                    payload = "0";
                }

                var msg = App.mainViewModel.AddMessageToQueue(_powerCharacteristic, payload);                
                result = (msg.TransmissionStatus >= BLEMsgSendingStatus.InstantiatedOnly);
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        public async Task<bool> Stop()
        {
            var result = false;

            try
            {
                string payload = "1";

                var msg = App.mainViewModel.AddMessageToQueue(_stopCharacteristic, payload);
                result = (msg.TransmissionStatus >= BLEMsgSendingStatus.InstantiatedOnly);

            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        public void Load(
            ObservableGattCharacteristics advancedMovementCharacteristic,
            ObservableGattCharacteristics powerCharacteristic,
            ObservableGattCharacteristics stopCharacteristic)
        {
            IsReady = false;

            if (advancedMovementCharacteristic == null ||
                powerCharacteristic == null ||
                stopCharacteristic == null)
            {
                //throw new NullReferenceException();
                return;
            }
            _advancedMovementCharacteristic = advancedMovementCharacteristic;
            _powerCharacteristic = powerCharacteristic;
            _stopCharacteristic = stopCharacteristic;

            IsReady = true;
        }

        public string Move(Direction directionToMove, int movementPowerPercent)
        {
            var payload = string.Empty;
            bool sent;
            

            if (movementPowerPercent < 0 ||
                movementPowerPercent > 100 ||
                Duration > 10000 ||
                Duration < 1
                )
            {
                throw new ArgumentOutOfRangeException();
            }

            try
            {
                string[] payLoadStringArray = null;

                if (StopMode == MovementAutoStopMode.Timespan)
                {
                    payLoadStringArray = new string[] { Convert.ToString((int)directionToMove), Convert.ToString(movementPowerPercent), Convert.ToString(Duration) };
                }
                else
                {
                    payLoadStringArray = new string[] { Convert.ToString((int)directionToMove), Convert.ToString(movementPowerPercent), "0" };
                }

                payload = string.Join("-", payLoadStringArray);

                var msg = App.mainViewModel.AddMessageToQueue(_advancedMovementCharacteristic, payload);
                sent = (msg.TransmissionStatus >= BLEMsgSendingStatus.InstantiatedOnly);
            }
            catch (Exception)
            {
                sent = false;
            }

            return payload.Insert(0, sent.ToString() + " - ");
        }

    }
}

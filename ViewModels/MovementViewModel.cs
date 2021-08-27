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
        private ObservableGattCharacteristics _movementCharacteristic;
        private ObservableGattCharacteristics _powerCharacteristic;
        private ObservableGattCharacteristics _stopCharacteristic;
        private int _defaultPowerPercent = 75;
        private TimeSpan _defaultDuration = new TimeSpan(0,0,0,0,250);
        private bool _isReady = false;

        public MovementViewModel()
        {

        }

        public bool IsReady
        {
            get => _isReady;
            set => SetProperty(ref _isReady, value);
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

                GattCommunicationStatus sendResult = await MainViewModel.SendUtf8Message(_powerCharacteristic, payload);

                switch (sendResult)
                {
                    case GattCommunicationStatus.AccessDenied: result = false; break;
                    case GattCommunicationStatus.ProtocolError: result = false; break;
                    case GattCommunicationStatus.Success: result = true; break;
                    case GattCommunicationStatus.Unreachable: result = false; break;
                    default: result = false; break;
                }
            }
            catch (Exception ex)
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
                string payload = string.Empty;

                GattCommunicationStatus sendResult = await MainViewModel.SendUtf8Message(_stopCharacteristic, payload);

                switch (sendResult)
                {
                    case GattCommunicationStatus.AccessDenied: result = false; break;
                    case GattCommunicationStatus.ProtocolError: result = false; break;
                    case GattCommunicationStatus.Success: result = true; break;
                    case GattCommunicationStatus.Unreachable: result = false; break;
                    default: result = false; break;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public void Load(
            ObservableGattCharacteristics movementCharacteristic, 
            ObservableGattCharacteristics powerCharacteristic,
            ObservableGattCharacteristics stopCharacteristic)
        {
            IsReady = false;

            if (movementCharacteristic == null ||
                powerCharacteristic == null ||
                stopCharacteristic == null)
            {
                throw new NullReferenceException();
            }

            _powerCharacteristic = powerCharacteristic;
            _movementCharacteristic = movementCharacteristic;
            _stopCharacteristic = stopCharacteristic;

            IsReady = true;
        }

        public async Task<bool> Move(Direction directionToMove)
        {
            var result = false;

            try
            {
                result = await commonMove(directionToMove, _defaultPowerPercent, _defaultDuration);
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public async Task<bool> Move(Direction directionToMove, TimeSpan movementDuration)
        {
            var result = false;

            try
            {
                result = await commonMove(directionToMove, _defaultPowerPercent, movementDuration);
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public async Task<bool> Move(Direction directionToMove, int movementPowerPercent)
        {
            var result = false;

            try
            {
                result = await commonMove(directionToMove, movementPowerPercent, _defaultDuration);
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public async Task<bool> Move(Direction directionToMove, int movementPowerPercent, TimeSpan movementDuration)
        {
            var result = false;

            try
            {
                result = await commonMove(directionToMove, movementPowerPercent, movementDuration);
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        private async Task<bool> commonMove(Direction directionToMove, int movementPowerPercent, TimeSpan movementDuration)
        {
            var result = false;

            if (movementPowerPercent < 0 ||
                movementPowerPercent > 100 ||
                movementDuration.TotalMilliseconds > 999
                )
            {
                throw new ArgumentOutOfRangeException();
            }

            try
            {

                string[] payLoadStringArray = { Convert.ToString(directionToMove), Convert.ToString(movementPowerPercent), Convert.ToString(movementDuration.TotalMilliseconds) } ;
                var payload = string.Join("-", payLoadStringArray);

                GattCommunicationStatus sendResult = await MainViewModel.SendUtf8Message(_movementCharacteristic, payload);

                result = Constants.CommStatusToBool(sendResult);
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

    }
}

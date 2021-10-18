using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace Enumerations
{
    public enum ComponentStatus { Error, Ready, Action, UnInitialised };
    public enum Direction { Forward, Backwards, TurnLeft, TurnRight, RotateLeft, RotateRight }
    public enum ServoMovementSpeed { Slow, Medium, Fast, Flank, Stop };
    //public enum speed { stop, slow, medium, fast };

    public class Constants
    {    
        public const string UUIDStop = @"017e99d6-8a61-11eb-8dcd-0242ac1a5100";
        public const string UUIDPanTilt = @"017e99d6-8a61-11eb-8dcd-0242ac1a5102";
        public const string UUIDPower = @"017e99d6-8a61-11eb-8dcd-0242ac1a5103";
        public const string UUIDAdvancedMove = @"017e99d6-8a61-11eb-8dcd-0242ac1a5104";
        public const string UUIDPanSweep = @"017e99d6-8a61-11eb-8dcd-0242ac1a5105";
        public const string UUIDDistance = @"017e99d6-8a61-11eb-8dcd-0242ac1a5106";

        public static bool CommStatusToBool(GattCommunicationStatus statusToCheck)
        {
            bool result = false;

            switch (statusToCheck)
            {
                case GattCommunicationStatus.AccessDenied: result = false; break;
                case GattCommunicationStatus.ProtocolError: result = false; break;
                case GattCommunicationStatus.Success: result = true; break;
                case GattCommunicationStatus.Unreachable: result = false; break;
                default: result = false; break;
            }

            return result;
        }
    }
}

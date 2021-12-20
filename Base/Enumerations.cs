using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace Enumerations
{
    public enum ComponentStatus { Error, Ready, Action, UnInitialised };
    public enum Direction { Forward, Backwards, TurnLeft, TurnRight, RotateLeft, RotateRight, Stop}
    public enum ServoMovementSpeed { Slow, Medium, Fast, Flank, Stop };
    public enum DeviceType { PanTiltBase, PanTiltCamera, PanTiltDistance, PanTiltCameraDistCombo, ManipulatorArm, TemperatureSensor, StaticMountCamera }

    public class Constants
    {    
        public const string UUIDStop = @"017e99d6-8a61-11eb-8dcd-0242ac1a5100";
        public const string UUIDPanTilt = @"017e99d6-8a61-11eb-8dcd-0242ac1a5102";
        public const string UUIDPower = @"017e99d6-8a61-11eb-8dcd-0242ac1a5103";
        public const string UUIDAdvancedMove = @"017e99d6-8a61-11eb-8dcd-0242ac1a5104";
        public const string UUIDPanSweep = @"017e99d6-8a61-11eb-8dcd-0242ac1a5105";
        public const string UUIDForwardDistance = @"017e99d6-8a61-11eb-8dcd-0242ac1a5106";
        public const string UUIDPanTiltDistance = @"017e99d6-8a61-11eb-8dcd-0242ac1a5107";
        public const string UUIDLights = @"017e99d6-8a61-11eb-8dcd-0242ac1a5108";

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enumerations
{
    public enum ComponentStatus { Error, Ready, Action, UnInitialised };
    public enum Direction { Forward, Backwards, TurnLeft, TurnRight, RotateLeft, RotateRight }
    public enum ServoMovementSpeed { Slow, Medium, Fast, Flank };

    public class Constants
    {    
        public const string UUIDStop = @"017e99d6-8a61-11eb-8dcd-0242ac1a5100";
        public const string UUIDMove = @"017e99d6-8a61-11eb-8dcd-0242ac1a5101";
        public const string UUIDPanTilt = @"017e99d6-8a61-11eb-8dcd-0242ac1a5102";
        public const string UUIDPower = @"017e99d6-8a61-11eb-8dcd-0242ac1a5103";
        public const string UUIDAdvancedMove = @"017e99d6-8a61-11eb-8dcd-0242ac1a5104";
    }
}

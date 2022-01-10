using BerthaRemote.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BerthaRemote.Helpers
{
    public class StaticHelpers
    {
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

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

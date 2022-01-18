using BerthaRemote.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
        public static string GenerateRandomString()
        {
            string first10Chars = Path.GetRandomFileName();
            first10Chars = first10Chars.Replace(".", ""); // Remove period.

            string second10Chars = Path.GetRandomFileName();
            second10Chars = second10Chars.Replace(".", ""); // Remove period.

            return string.Concat(first10Chars.Substring(0, 5), second10Chars.Substring(0, 5));
        }

        private static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static int BLEStringToIntSafe(string bleString)
        {
            string cleaned = string.Empty;

            int result = -1;

            try
            {
                if (bleString.Contains("-"))
                {
                    cleaned = bleString.Replace("-", "");
                    var c = StaticHelpers.StringToByteArray(cleaned);
                    result = Convert.ToInt32(c);
                }
                else
                {
                    var sp = bleString.Split(Path.DirectorySeparatorChar);
                    result = Convert.ToInt32(sp[0]);
                }
            }
            catch (Exception BLEValueConvertError)
            {
                
            }

            return result;
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

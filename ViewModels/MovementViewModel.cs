using Microsoft.Toolkit.Uwp.Connectivity;
using MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurthaRemote.ViewModels
{
    public class MovementViewModel : ValleyBaseViewModel
    {
        private ObservableGattCharacteristics _obsCharacteristic;

        public MovementViewModel()
        {

        }

        public void Load(ObservableGattCharacteristics obsCharacteristic)
        {
            _obsCharacteristic = obsCharacteristic;
        }
    }
}

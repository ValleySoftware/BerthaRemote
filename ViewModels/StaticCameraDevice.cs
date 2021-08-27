using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerthaRemote.ViewModels
{
    public partial class StaticCameraDevice: BaseDeviceViewModel, IDevice
    {
        public override void Load(DeviceListViewModel parentList, DeviceType typeOfDevice, string[] parameters) 
        {
            base.Load(parentList, typeOfDevice, parameters);

        }
    }
}

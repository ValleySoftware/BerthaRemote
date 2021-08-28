using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerthaRemote.ViewModels
{
    public partial class PanTiltCameraDevice : BaseDeviceViewModel, IDevice
    {
        private string _cameraIP;

        public override void Load(DeviceListViewModel parentList, DeviceType typeOfDevice, string[] parameters)
        {
            base.Load(parentList, typeOfDevice, parameters);

            if (parameters != null &&
                parameters.Count() > 0)
            {
                CameraIP = parameters[0];
            }
        }

        public string CameraIP
        {
            get => _cameraIP;
            set => SetProperty(ref _cameraIP, value);
        }
    }
}

using MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerthaRemote.ViewModels
{
    public class DeviceListViewModel: ValleyBaseViewModel
    {
        public PanTiltSensorArray PanTiltCam { get; set; }
        public PanTiltSensorArray PanTiltDist { get; set; }
        public ForwardSensorArray ForwardDistance { get; set; }

        public DeviceListViewModel()
        {

        }

        public void Load()
        {
        }
    }
}

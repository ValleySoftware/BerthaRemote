using Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerthaRemote.ViewModels
{

    public interface IDevice
    {
        DeviceType TypeOfDevice { get; set; }
        DeviceListViewModel ParentList { get; set; }
        string Name { get; set; }
        bool IsReady { get; set; }
        string[] Parameters { get; set; }
        void Load(DeviceListViewModel parentList, DeviceType typeOfDevice, string[] parameters);

    }
}

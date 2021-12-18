using Enumerations;
using MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerthaRemote.ViewModels
{
    public partial class BaseDeviceViewModel : ValleyBaseViewModel, IDevice
    {
        private DeviceType _typeOfDevice;
        private DeviceListViewModel _parentList;
        private string _name;
        private bool _isReady;
        private string[] _parameters;

        public DeviceType TypeOfDevice
        {
            get => _typeOfDevice;
            set => SetProperty(ref _typeOfDevice, value);
        }

        public DeviceListViewModel ParentList
        {
            get => _parentList;
            set => SetProperty(ref _parentList, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public bool IsReady
        {
            get => _isReady;
            set => SetProperty(ref _isReady, value);
        }

        public string[] Parameters
        {
            get => _parameters;
            set => SetProperty(ref _parameters, value);
        }

        public virtual void Load(DeviceListViewModel parentList, DeviceType typeOfDevice, string[] parameters)
        {
            _parentList = parentList;
            _typeOfDevice = typeOfDevice;
            _parameters = parameters;
        }
    }
}

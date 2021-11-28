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
        private ObservableCollection<IDevice> _items = new ObservableCollection<IDevice>();

        public ObservableCollection<IDevice> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public bool AddItem(IDevice deviceToAdd)
        {
            bool result = false;

            try
            {
                Items.Add(deviceToAdd);
            }
            catch (Exception)
            {

            }

            return result;
        }

        public void Load()
        {
            if (_items == null)
            {
                _items = new ObservableCollection<IDevice>();
            }
            else
            {
                _items.Clear();
            }
        }
    }
}

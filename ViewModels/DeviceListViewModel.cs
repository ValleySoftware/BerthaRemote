using Microsoft.Toolkit.Uwp;
using MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BerthaRemote.ViewModels
{
    public class DeviceListViewModel: ValleyBaseViewModel
    {
        public PanTiltSensorArray PanTiltCam { get; set; }
        public PanTiltSensorArray PanTiltDist { get; set; }
        public ForwardSensorArray ForwardDistance { get; set; }

        Timer updateDistances;

        public DeviceListViewModel()
        {
            updateDistances = new Timer();
            updateDistances.Interval = 500;
            updateDistances.Elapsed += UpdateDistances_Elapsed;
            updateDistances.AutoReset = true;
            updateDistances.Enabled = false;

        }

        public void SetTimerStatus(bool newStatus)
        {
            updateDistances.Enabled = newStatus;
        }

        private void UpdateDistances_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.dispatcherQueue.EnqueueAsync(() =>
            {
                if (ForwardDistance != null &&
                    ForwardDistance.IsReady)
                {
                    ForwardDistance.UpdateDistanceValue();
                }
                if (PanTiltDist != null &&
                    PanTiltDist.IsReady)
                {
                    PanTiltDist.UpdateDistanceValue();
                }
            });
        }

        public void Load()
        {
        }
    }
}

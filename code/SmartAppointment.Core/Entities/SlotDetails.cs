using SmartAppointment.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SmartAppointment.Core.Entities
{
    public class SlotDetails : BaseModel
    {
        private TimeSpan _startTime;
        private TimeSpan _endTime;
        private int _numberOfAvailableSlots;
        private bool _isSlotDetailsLoaded;
        private bool _isSlotAvailable;
        private string _slotName;
        private string _numberOfAvailableSlotsDisplay;

        public SlotDetails()
        {
            _isSlotAvailable = false;
        }
        public TimeSpan NextAvailableSlotTime { get; set; }

        public string SlotName
        {
            get => _slotName;
            set => SetProperty(ref _slotName, value);
        }
        public string SlotTimings {
            get
            {
                return !IsSlotAvailable ? "" : "(" + StartTime.ToString(@"hh\:mm") + ":" + EndTime.ToString(@"hh\:mm") + ")";
            }
        }
        public TimeSpan StartTime
        {
            get => _startTime;
            set => SetProperty(ref _startTime, value);
        }

        public TimeSpan EndTime
        {
            get => _endTime;
            set => SetProperty(ref _endTime, value);
        }
        public int NumberOfAvailableSlots
        {
            get => _numberOfAvailableSlots;
            set
            {
                SetProperty(ref _numberOfAvailableSlots, value);
                NumberOfAvailableSlotsDisplay = value.ToString();
            }
        }

        public string NumberOfAvailableSlotsDisplay
        {
            get => _numberOfAvailableSlotsDisplay;
            set
            {
                var modValue = value == "0" ? "No Slots" : value + " Slots";
                SetProperty(ref _numberOfAvailableSlotsDisplay, modValue);
            }
        }

        public bool IsSlotDetailsLoaded
        {
            get => _isSlotDetailsLoaded;
            set => SetProperty(ref _isSlotDetailsLoaded, value);
        }
        public bool IsSlotAvailable
        {
            get => _isSlotAvailable;
            set => SetProperty(ref _isSlotAvailable, value);
        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

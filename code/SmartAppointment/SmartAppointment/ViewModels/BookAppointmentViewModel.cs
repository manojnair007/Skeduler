using Appointment.Core.Entities;
using Appointment.Core.Enums;
using Newtonsoft.Json;
using SmartAppointment.Core.Entities;
using SmartAppointment.Core.Interfaces;
using SmartAppointment.Core.Interfaces.Repositories;
using SmartAppointment.Interfaces.ViewModels;
using SmartAppointment.Models;
using SmartAppointment.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartAppointment.ViewModels
{
    [QueryProperty(nameof(Data), nameof(Data))]
    public class BookAppointmentViewModel : BaseViewModel, IBookAppointmentViewModel
    {
        private string _data;
        private string _serviceProviderNametext;
        private string description;
        private string _location;
        private string _subCatergory;
        private string _chooseDate = "Choose Date";
        private ObservableCollection<DateSelectors> _dateSelectors;
        private ObservableCollection<SlotDetails> _slotDetails;
        private Tenant _selectedTenant;
        private DateSelectors _selectedDate;
        private string _isSlotDetailsLoaded;
        private IAppointmentRepository _appointmentRepository;
        private INavigationService _navigationService;
        private SlotDetails _selectedSlot;
        private string _scheduledDate;
        private string _scheduledTime;
        
        private IAuthenticationService _authenticationService;
        private IDialogService _dialogService;
        private bool _isSlotScheduled = true;
        private string _scheduleMessage = "";

        public BookAppointmentViewModel(IAppointmentRepository appointmentRepository, IAuthenticationService authenticationService, IDialogService dialogService, INavigationService navigationService)
        {
            _appointmentRepository = appointmentRepository;
            _navigationService = navigationService;
            ConfirmBookingCommand = new Command(OnConfirmBookingClicked);
            _authenticationService = authenticationService;
            _dialogService = dialogService;
           // IsSlotScheduled = false;
            ScheduleMessage = "No Slot Selected";
        }

        public ICommand ConfirmBookingCommand { get; set; }
        public ObservableCollection<SlotDetails> Slots
        {
            get => _slotDetails;
            set => SetProperty(ref _slotDetails, value);
        }
        public ObservableCollection<DateSelectors> DateSelectors
        {
            get => _dateSelectors;
            set => SetProperty(ref _dateSelectors, value);
        }
        public string ServiceProviderName
        {
            get => _serviceProviderNametext;
            set => SetProperty(ref _serviceProviderNametext, value);
        }

        public string SubCatergory
        {
            get => _subCatergory;
            set => SetProperty(ref _subCatergory, value);
        }


        public SlotDetails SelectedSlot
        {
            get => _selectedSlot;
            set
            {
                SetProperty(ref _selectedSlot, value);
                OnSelectedSlot(_selectedSlot);
            }

        }
        public DateSelectors SelectedDate
        {
            get => _selectedDate;
            set
            {
                SetProperty(ref _selectedDate, value);
                OnSelectDateTapped(_selectedDate);
            }

        }
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }
        public string ChooseDate
        {
            get => _chooseDate;
            set => SetProperty(ref _chooseDate, value);
        }

        public string ScheduledDate
        {
            get => _scheduledDate; //"dddd, d MMM"
            set => SetProperty(ref _scheduledDate, value);
        }

        public string ScheduledTime
        {
            get => _scheduledTime;
            set => SetProperty(ref _scheduledTime, value);
        }

        public bool IsSlotScheduled
        {
            get => _isSlotScheduled;
            set => SetProperty(ref _isSlotScheduled, value);
        }
        public string ScheduleMessage
        {
            get => _scheduleMessage;
            set => SetProperty(ref _scheduleMessage, value);
        }

        public string Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                LoadSelectedServiceProvider(value);
            }
        }

        private async void OnConfirmBookingClicked(object obj)
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
                {
                    { "SelectedTenant",_selectedTenant},
                    { "SelectedDate", SelectedDate.Date.ToString("d")},
                    {"ScheduledTime",ScheduledTime },
                     {"SelectedSlot",SelectedSlot }
                };
            var serializedString = JsonConvert.SerializeObject(data);
            await _navigationService.PushAsync(nameof(BookingConfirmationPage), serializedString);

        }

        public async void LoadSelectedServiceProvider(string itemId)
        {
            try
            {
                string data = Uri.UnescapeDataString(itemId);
                _selectedTenant = JsonConvert.DeserializeObject<Tenant>(data);
                DateSelectors = new ObservableCollection<DateSelectors>();
                for (int i = 0; i < 30; i++)
                {
                    var date = DateTime.Now.AddDays(i);
                    DateSelectors.Add(new DateSelectors()
                    {
                        Day = date.ToString("ddd"),
                        Date = date.Date
                    });
                }
                SelectedDate = DateSelectors[0];

                ServiceProviderName = _selectedTenant.Category == "Doctor" ? _selectedTenant.OwnerName : _selectedTenant.TenantName;
                Location = _selectedTenant.AddressDetails.Location;
                SubCatergory = _selectedTenant.SubCategory;


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        private void UpdateSlotDetails(DateSelectors date)
        {
            var dayOfWeek = (WeekDays)date.Date.DayOfWeek;
            var availableSlots = _selectedTenant.BusinessTimings.TimeSlots[dayOfWeek];
            Slots = new ObservableCollection<SlotDetails>();
            foreach (string slotName in Enum.GetNames(typeof(TimeSlots)))
            {
                var slot = availableSlots?.FirstOrDefault(x => x.Slot.ToString().Equals(slotName));
                var slotDetails = new SlotDetails();
                slotDetails.SlotName = slotName;
                if (slot != null)
                {
                    slotDetails.IsSlotAvailable = true;
                    slotDetails.StartTime = slot.StartTime;
                    slotDetails.EndTime = slot.EndTime;
                }
                else
                {
                    slotDetails.IsSlotAvailable = false;
                }
                slotDetails.NumberOfAvailableSlots = 0;
                slotDetails.IsSlotDetailsLoaded = false;
                Slots.Add(slotDetails);
            }
        }

        public async Task OnSelectedSlot(SlotDetails selectedSlot)
        {
            if(selectedSlot.NumberOfAvailableSlots > 0)
            {
                IsSlotScheduled = true;
                ScheduledTime = selectedSlot.NextAvailableSlotTime.ToString(@"hh\:mm");
                ScheduledDate = SelectedDate.Date.ToString("dddd, d MMM");
            }
            else
            {
                IsSlotScheduled = false;
                ScheduleMessage = "No Slot Available";
            }
        }


        public async Task OnSelectDateTapped(DateSelectors selectedDate)
        {
            ScheduleMessage = "No Slot Selected";
            IsSlotScheduled = false;
            //var selectedDate = item as DateSelectors;
            UpdateSlotDetails(selectedDate);
            foreach (SlotDetails slot in Slots)
            {
                DateTime selectedDateTime = selectedDate.Date + slot.EndTime;
                if (DateTime.Now < selectedDateTime)
                {
                    if (slot.IsSlotAvailable)
                    {
                        slot.NumberOfAvailableSlots = await GetAvailableSlots(selectedDate, slot);
                    }
                }
                slot.IsSlotDetailsLoaded = true;

            }

        }

        private async Task<int> GetAvailableSlots(DateSelectors selectedDate, SlotDetails selectedSlot)
        {
            List<AppointmentDetails> appointments = await _appointmentRepository.GetAppointmentsAysnc(selectedDate.Date.ToString("d"), selectedSlot.SlotName, _selectedTenant.TenantId);
            AppointmentDetails lastAppointmentDetails = null;
            if (appointments != null && appointments.Count > 0)
            {
                lastAppointmentDetails = appointments.OrderByDescending(x => x.OrginialAttendingTime).ToArray()[0];
                // TODO: Logic to find the free in-between slots
            }
            var remainingTime = lastAppointmentDetails != null ? selectedSlot.EndTime - (lastAppointmentDetails?.OrginialAttendingTime + _selectedTenant.BusinessTimings.AverageServiceTime) : selectedSlot.EndTime - selectedSlot.StartTime;
            selectedSlot.NextAvailableSlotTime = lastAppointmentDetails != null ? lastAppointmentDetails.OrginialAttendingTime + _selectedTenant.BusinessTimings.AverageServiceTime : selectedSlot.StartTime;
            return ((int)(remainingTime / _selectedTenant.BusinessTimings.AverageServiceTime));
        }
    }
}

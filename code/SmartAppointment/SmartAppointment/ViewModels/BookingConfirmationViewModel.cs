using Appointment.Core.Entities;
using Appointment.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class BookingConfirmationViewModel : BaseViewModel, IBookingConfirmationViewModel
    {


        private IAuthenticationService _authenticationService;
        private IDialogService _dialogService;
        private bool _bookingInProgress = false;
        private string _bookingStatusMessage = "";
        private string _data;
        private IAppointmentRepository _appointmentRepository;
        private INavigationService _navigationService;
        private Tenant _selectedTenant;
        public BookingConfirmationViewModel(IAppointmentRepository appointmentRepository, IAuthenticationService authenticationService, IDialogService dialogService, INavigationService navigationService)
        {
              _appointmentRepository = appointmentRepository;
            _navigationService = navigationService;
            NewBookingCommand = new Command(OnNewBookingCommandClicked);
            NewAppointmentBookingCommand = new Command(OnNewAppointmentBookingCommandClicked);
            _authenticationService = authenticationService;
            _dialogService = dialogService;
        }


        public ICommand NewBookingCommand { get; set; }
        public ICommand NewAppointmentBookingCommand { get; set; }
        public bool BookingInProgress
        {
            get => _bookingInProgress;
            set => SetProperty(ref _bookingInProgress, value);
        }
        public string BookingStatusMessage
        {
            get => _bookingStatusMessage;
            set => SetProperty(ref _bookingStatusMessage, value);
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
                ConfirmBooking(Uri.UnescapeDataString(_data));
            }
        }

        private async void OnNewBookingCommandClicked(object obj)
        {
            await _navigationService.PopAysnc();
        }
        private async void OnNewAppointmentBookingCommandClicked(object obj)
        {
            await _navigationService.NavigateTo(nameof(SelectServicePage),"");
        }

        public async void ConfirmBooking(string data)
        {
            BookingInProgress = true;
            BookingStatusMessage = "Checking slot availability...";
            var appointmentDetails = JsonConvert.DeserializeObject<Dictionary<string,object>>(data);
            var selectedDate = appointmentDetails["SelectedDate"] as string;
            var selectedSlot = (appointmentDetails["SelectedSlot"] as JObject).ToObject<SlotDetails>();
            _selectedTenant = (appointmentDetails["SelectedTenant"] as JObject).ToObject<Tenant>();
            var scheduledTime = appointmentDetails["ScheduledTime"] as string;
            selectedSlot.NumberOfAvailableSlots = await GetAvailableSlots(selectedDate, selectedSlot, _selectedTenant);
            await Task.Delay(1000);
            if (selectedSlot.NumberOfAvailableSlots > 0)
            {
                BookingStatusMessage = "Booking Appointment...";
                var appointment = new AppointmentDetails()
                {
                    Date = selectedDate,
                    OrginialAttendingTime = TimeSpan.Parse(scheduledTime),
                    Slot = (TimeSlots)Enum.Parse(typeof(TimeSlots), selectedSlot.SlotName),
                    Status = AppointmentStatus.Confirmed,
                    TenantId = _selectedTenant.TenantId,
                    UserId = _authenticationService.GetClaims().Email,
                    OwnerName = _selectedTenant.OwnerName,
                    RevisedAttendingTime = TimeSpan.Parse(scheduledTime)
                };
                AppointmentDetails response = await _appointmentRepository.InsertItemAsync(appointment);
                if (response!=null && !string.IsNullOrEmpty(response.Id))
                {
                    BookingStatusMessage = "Hurray!! Booking Confirmed";
                    await Task.Delay(2000);
                    await _navigationService.NavigateTo(nameof(SelectServicePage), JsonConvert.SerializeObject(response));
                    BookingInProgress = false;
                }
                else
                {
                    BookingInProgress = false;
                    BookingStatusMessage = "Sorry! Failed to booking your appointment.";
                }

            }
            else
            {
                BookingInProgress = false;
                BookingStatusMessage = "I am afraid, all the slots are taken.";
            }

        }

        private async Task<int> GetAvailableSlots(string selectedDate, SlotDetails selectedSlot, Tenant selectedTenant)
        {
            List<AppointmentDetails> appointments = await _appointmentRepository.GetAppointmentsAysnc(selectedDate, selectedSlot.SlotName, selectedTenant.TenantId);
            AppointmentDetails lastAppointmentDetails = null;
            if (appointments != null && appointments.Count > 0)
            {
                lastAppointmentDetails = appointments.OrderByDescending(x => x.OrginialAttendingTime).ToArray()[0];
                // TODO: Logic to find the free in-between slots
            }
            var remainingTime = lastAppointmentDetails != null ? selectedSlot.EndTime - (lastAppointmentDetails?.OrginialAttendingTime + selectedTenant.BusinessTimings.AverageServiceTime) : selectedSlot.EndTime - selectedSlot.StartTime;
            selectedSlot.NextAvailableSlotTime = lastAppointmentDetails != null ? lastAppointmentDetails.OrginialAttendingTime + selectedTenant.BusinessTimings.AverageServiceTime : selectedSlot.StartTime;
            return ((int)(remainingTime / selectedTenant.BusinessTimings.AverageServiceTime));
        }

    }
}

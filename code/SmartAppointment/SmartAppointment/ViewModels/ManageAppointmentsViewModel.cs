using Appointment.Core.Entities;
using SmartAppointment.Core.Interfaces;
using SmartAppointment.Core.Interfaces.Repositories;
using SmartAppointment.Interfaces.ViewModels;
using SmartAppointment.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartAppointment.ViewModels
{
    public class ManageAppointmentsViewModel : BaseViewModel, IManageAppointmentsViewModel
    {
        private IAuthenticationService _authenticationService;
        private INavigationService _navigationService;
        private IAppointmentRepository _appointmentRepository;
        private string _emailId;
        private string _userFullName;
        private ObservableCollection<AppointmentDetails> _appointments;
        private bool _appointmentsNotFound;

        public ManageAppointmentsViewModel(IAuthenticationService authenticationService, INavigationService navigationService, IAppointmentRepository appointmentRepository)
        {
            _authenticationService = authenticationService;
            _navigationService = navigationService;
            _appointmentRepository = appointmentRepository;

            CancelAppointmentCommand = new Command(OnCancelAppointmentClicked);
           
        }

        public ICommand CancelAppointmentCommand { get; set; }
        public string UserFullName
        {
            get => _userFullName;
            set => SetProperty(ref _userFullName, value);
        }
        public bool AppointmentsNotFound
        {
            get => _appointmentsNotFound;
            set => SetProperty(ref _appointmentsNotFound, value);
        }

        private async void OnCancelAppointmentClicked(object data)
        {
            var appointData = data as AppointmentDetails;
            appointData.Status = Appointment.Core.Enums.AppointmentStatus.Cancelled;
            await _appointmentRepository.UpsertDocumentAsync(appointData);
            await GetUpcomingAppointmentData();
        }
        public ObservableCollection<AppointmentDetails> Appointments
        {
            get => _appointments;
            set => SetProperty(ref _appointments, value);
        }
        private async Task GetUpcomingAppointmentData()
        {
            var appointments = await _appointmentRepository.GetAppointmentsAysnc(_authenticationService.GetClaims().Email);
            if (appointments != null && appointments.Count > 0)
            {
                Appointments = new ObservableCollection<AppointmentDetails>(appointments);
                AppointmentsNotFound = false;
            }
            else
            {
                AppointmentsNotFound = true;
            }

        }

        public void OnAppearing()
        {
            GetUpcomingAppointmentData();
        }
    }
}

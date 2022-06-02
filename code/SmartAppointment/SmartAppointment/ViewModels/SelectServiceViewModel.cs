using Appointment.Core.Entities;
using Newtonsoft.Json;
using SmartAppointment.Core.Entities;
using SmartAppointment.Core.Interfaces;
using SmartAppointment.Core.Interfaces.Repositories;
using SmartAppointment.Interfaces.ViewModels;
using SmartAppointment.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartAppointment.ViewModels
{
    [QueryProperty(nameof(Data), nameof(Data))]
    public class SelectServiceViewModel : BaseViewModel, ISelectServiceViewModel
    {
        private string _message;
        private IAuthenticationService _authenticationService;
        private INavigationService _navigationService;
        private ICategoriesRepository _categoriesRepository;
        private ICacheService _cacheService;
        private IHttpService _httpService;
        private IGeoLocationService _geoLocationService;
        private ObservableCollection<SubCategory> _doctor;
        private ObservableCollection<SubCategory> _beautyParlour;
        private bool _doctorsLoaded = false;
        private bool _beautyParlourLoaded = false;
        private IDialogService _dialogService;
        private IAppointmentRepository _appointmentRepository;
        private bool _upcomingAppointmentLoaded = false;
        private ObservableCollection<AppointmentDetails> _upcomingAppointments;
        private string _data;
        private SubCategory _selectedService;
        private AppointmentDetails _selectedAppointment;
        private bool _appointmentsFound = true;

        public SelectServiceViewModel(IAuthenticationService authenticationService, IGeoLocationService geoLocationService, ICacheService cacheService, INavigationService navigationService, IAppointmentRepository appointmentRepository, IDialogService dialogService, ICategoriesRepository categoriesRepository, IHttpService httpService)
        {
            _authenticationService = authenticationService;
            _navigationService = navigationService;
            _categoriesRepository = categoriesRepository;
            _cacheService = cacheService;
            _httpService = httpService;
            _geoLocationService = geoLocationService;
            Title = "Select Service";
            _dialogService = dialogService;
            _appointmentRepository = appointmentRepository;
            CategorySelectedCommand = new Command(OnCategorySelected);
        }
        public ICommand CategorySelectedCommand { get; set; }

        public bool AppointmentsFound
        {
            get => _appointmentsFound;
            set => SetProperty(ref _appointmentsFound, value);
        }
        public bool UpcomingAppointmentLoaded
        {
            get => _upcomingAppointmentLoaded;
            set => SetProperty(ref _upcomingAppointmentLoaded, value);
        }
        public bool DoctorsLoaded
        {
            get => _doctorsLoaded;
            set => SetProperty(ref _doctorsLoaded, value);
        }
        public bool BeautyParlourLoaded
        {
            get => _beautyParlourLoaded;
            set => SetProperty(ref _beautyParlourLoaded, value);
        }
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
        public ObservableCollection<SubCategory> Doctor
        {
            get => _doctor;
            set => SetProperty(ref _doctor, value);
        }
        public ObservableCollection<SubCategory> BeautyParlour
        {
            get => _beautyParlour;
            set => SetProperty(ref _beautyParlour, value);
        }
        public ObservableCollection<AppointmentDetails> UpcomingAppointments
        {
            get => _upcomingAppointments;
            set => SetProperty(ref _upcomingAppointments, value);
        }
        private async void OnCategorySelected(object obj)
        {
            await OnCategoryTapped(obj as SubCategory);
        }
      
        public AppointmentDetails SelectedAppointment
        {
            get => _selectedAppointment;
            set
            {
                SetProperty(ref _selectedAppointment, value);
                OnAppointmentedTapped(value);
            }
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
                //UpdateData(Uri.UnescapeDataString(_data));
            }
        }
               

        private async Task GetUpcomingAppointmentData()
        {
            var appointments =  await _appointmentRepository.GetAppointmentsAysnc(_authenticationService.GetClaims().Email);
            if (appointments != null && appointments.Count > 0)
            {
                UpcomingAppointments = new ObservableCollection<AppointmentDetails>(appointments);
                UpcomingAppointmentLoaded = true;
                AppointmentsFound = true;
            }
            else
            {
                AppointmentsFound = false;
            }

        }

        private async Task GetData(Dictionary<string, List<SubCategory>> data)
        {
            if (data.ContainsKey("Doctor"))
            {
                Doctor = new ObservableCollection<SubCategory>(data["Doctor"]);
                DoctorsLoaded = true;
            }
            if (data.ContainsKey("Beauty Parlour"))
            {
                BeautyParlour = new ObservableCollection<SubCategory>(data["Beauty Parlour"]);
                BeautyParlourLoaded = true;
            }
        }

        private async Task GetCategoryData()
        {
            var data = await _categoriesRepository.GetServiceProviderCategoryItemsAsync();
            if (data.ContainsKey("Doctor"))
            {
                Doctor = new ObservableCollection<SubCategory>(data["Doctor"]);
                DoctorsLoaded = true;
            }
            if (data.ContainsKey("Beauty Parlour"))
            {
                BeautyParlour = new ObservableCollection<SubCategory>(data["Beauty Parlour"]);
                BeautyParlourLoaded = true;
            }
        }

        public async void OnAppointmentedTapped(AppointmentDetails item)
        {
            
        }

        public async Task OnCategoryTapped(SubCategory item)
        {
            await _navigationService.PushAsync(nameof(SelectTenantPage), JsonConvert.SerializeObject(item));
        }

        public void OnAppearing()
        {
            GetUpcomingAppointmentData();
            GetCategoryData();
        }
    }
}
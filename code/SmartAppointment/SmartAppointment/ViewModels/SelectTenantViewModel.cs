using Appointment.Core.Entities;
using Newtonsoft.Json;
using SmartAppointment.Core.Entities;
using SmartAppointment.Core.Interfaces;
using SmartAppointment.Interfaces.ViewModels;
using SmartAppointment.Models;
using SmartAppointment.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartAppointment.ViewModels
{
    [QueryProperty(nameof(Data), nameof(Data))]
    public class SelectTenantViewModel : BaseViewModel,  ISelectTenantViewModel
    {
        private ITenantsRepository _tenantsRepository;
        private INavigationService _navigationService;
        private IGeoLocationService _geoLocationService;
        private ObservableCollection<Tenant> _serviceProviders;
        private string _location;
        private bool _isLocationNotDetected = true;
        private string _data;

        public ICommand BookAppointmentCommand { get; }
        public Command AddItemCommand { get; }

        private SubCategory _selectedSubCategory;
        private bool _tenantsNotFound;

        public ICommand LoadMoreTenantsCommand { get; set; }
        

        public SelectTenantViewModel(ITenantsRepository tenantsRepository, INavigationService navigationService, IGeoLocationService geoLocationService)
        {
            _tenantsRepository = tenantsRepository;
            _navigationService = navigationService;
            _geoLocationService = geoLocationService;
            Title = "Search & Book";
            ServiceProviders = new ObservableCollection<Tenant>();
            BookAppointmentCommand = new Command(async (item) => await ExecuteBookAppointmentCommand(item));
            LoadMoreTenantsCommand = new Command(async (item) => await ExecuteLoadItemsCommand());
            Location = "Detecting Location...";
        }
        public bool IsLocationNotDetected
        {
            get => _isLocationNotDetected;
            set => SetProperty(ref _isLocationNotDetected, value);
        }

        public bool TenantsNotFound
        {
            get => _tenantsNotFound;
            set => SetProperty(ref _tenantsNotFound, value);
        }
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
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
                UpdateData(Uri.UnescapeDataString(_data));
            }
        }

        private void UpdateData(string data)
        {
            _selectedSubCategory = JsonConvert.DeserializeObject<SubCategory>(data);

        }

        async Task ExecuteBookAppointmentCommand(object item)
        {
            await _navigationService.PushAsync(nameof(BookAppointmentPage), JsonConvert.SerializeObject(item));
        }
        async Task ExecuteLoadItemsCommand()
        {
            if (!IsBusy)
            {
                IsBusy = true;

                try
                {

                    var tenants = await _tenantsRepository.FetchTenantsByPage(_selectedSubCategory.Name);
                    foreach (var tenant in tenants)
                    {
                        ServiceProviders.Add(tenant);
                    }
                    if (ServiceProviders != null && ServiceProviders.Count > 0)
                    {
                        TenantsNotFound = false;
                    }
                    else
                    {
                        TenantsNotFound = true;
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public void OnAppearing()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Placemark placemark =  _geoLocationService.GetSavedPlacemark();
               
                if (placemark != null)
                {
                    IsLocationNotDetected = false;
                    Location = placemark?.SubLocality + ", " + placemark?.SubAdminArea;
                }
            });
        }

        public void OnDisappearing()
        {
            
        }


        public ObservableCollection<Tenant> ServiceProviders
        {
            get => _serviceProviders;
            set => SetProperty(ref _serviceProviders, value);
        }
    }
}
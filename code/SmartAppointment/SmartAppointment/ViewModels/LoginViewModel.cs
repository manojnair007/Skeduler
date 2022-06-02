using Microsoft.Identity.Client;
using SmartAppointment.Core;
using SmartAppointment.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IdentityModel.Tokens.Jwt;
using SmartAppointment.Core.Interfaces;
using SmartAppointment.Interfaces.ViewModels;
using SmartAppointment.Core.Interfaces.Repositories;
using SmartAppointment.Core.Interfaces.Models;
using Acr.UserDialogs;
using System.Threading;
using Xamarin.Essentials;

namespace SmartAppointment.ViewModels
{
    [QueryProperty(nameof(Data), nameof(Data))]
    public class LoginViewModel : BaseViewModel, ILoginViewModel
    {
        private CancellationTokenSource _cancellationTokenSource;
        private ILoginModel _model;
        private INavigationService _navigationService;
        private IAuthenticationService _authenticationService;
        private ISettingsService _settingService;
        private IDialogService _dialogService;
        private IGeoLocationService _geoLocationService;
        private bool _showLoginContent = false;
        private bool _navigatedFromLogout = false;
        private bool _isLoginInProgress = false;
        private string _data;
        private string _loginLabel = "Login";
        private bool _isUserAlreadyLoggedIn = false;
        public LoginViewModel(INavigationService navigationService, IGeoLocationService geoLocationService, IAuthenticationService authenticationService, IDialogService dialogService, ISettingsService settingsService, ILoginModel model)
        {
            _model = model;
            _navigationService = navigationService;
            _authenticationService = authenticationService;
            _settingService = settingsService;
            _dialogService = dialogService;
            _geoLocationService = geoLocationService;
            LoginCommand = new Command(OnLoginClicked);

        }

        public Command LoginCommand { get; }
        public bool ShowLoginContent
        {
            get => _showLoginContent;
            set => SetProperty(ref _showLoginContent, value);
        }
        public string LoginLabel
        {
            get => _loginLabel;
            set => SetProperty(ref _loginLabel, value);
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
                bool.TryParse(_data, out _navigatedFromLogout);
            }
        }
        private async void OnLoginClicked(object obj)
        {
            ShowLoginContent = false;
            _isLoginInProgress = true;
            if (_isUserAlreadyLoggedIn)
            {
                _dialogService.ShowLoadingDialog("Signing in...");
                _isLoginInProgress = false;
                await NavigateOnSuccessfulLogin();
                return;
            }
            _dialogService.ShowLoadingDialog("Please wait...");
            if (await _authenticationService.AcquireTokenInteractive(GlobalConfig.Scopes, App.UIParent))
            {
                _isLoginInProgress = false;
                await NavigateOnSuccessfulLogin();
            }
            else
            {
                ShowLoginContent = true;
                _dialogService.HideLoadingDialog();
            }

        }

        public async Task CheckIfUserLoggedInAlready()
        {
            if (_isLoginInProgress)
            {
                return;
            }
            if (_navigatedFromLogout)
            {
                _navigatedFromLogout = false;
                ShowLoginContent = true;
                _isUserAlreadyLoggedIn = false;
                LoginLabel = "Login";
            }
            else
            {
                _dialogService.ShowLoadingDialog("Please wait...");
                // Look for existing account
                if (await _authenticationService.CheckIfUserLoggedInAlready(GlobalConfig.Scopes))
                {
                    var claim = _authenticationService.GetClaims();
                    _isUserAlreadyLoggedIn = true;
                    LoginLabel = "Continue as " + claim.FirstName + " " + claim.LastName;
                }
                ShowLoginContent = true;
                _dialogService.HideLoadingDialog();
            }
        }

        private async Task NavigateOnSuccessfulLogin()
        {
            await _settingService.SetCosmosDBPrimaryKey();
            var claim = _authenticationService.GetClaims();
            if (claim.IsNewUser)
            {
                await _model.AddUser(claim);
            }
            
            await _navigationService.NavigateTo(nameof(SelectServicePage));
            _dialogService.HideLoadingDialog();
        }

        public async Task OnAppearing()
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    Placemark placemark = await _geoLocationService.GetLastKnownLocationAsync();
                    if (placemark == null)
                    {
                        placemark = await _geoLocationService.GetLocationAsync(_cancellationTokenSource);
                    }
                    
                });
                await CheckIfUserLoggedInAlready();
            }
            catch
            {
                // Do nothing - the user isn't logged in
            }
        }

        public void OnDisappearing()
        {
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource.Cancel();
        }

    }
}

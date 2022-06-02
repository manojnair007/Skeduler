using SmartAppointment.Core.Interfaces;
using SmartAppointment.Interfaces.ViewModels;
using SmartAppointment.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartAppointment.ViewModels
{
    public class ProfileViewModel : BaseViewModel, IProfileViewModel
    {
        private IAuthenticationService _authenticationService;
        private INavigationService _navigationService;
        private string _emailId;
        private string _userFullName;

        public ProfileViewModel(IAuthenticationService authenticationService, INavigationService navigationService)
        {
            _authenticationService = authenticationService;
            _navigationService = navigationService;
            var claims = _authenticationService.GetClaims();
            UserFullName = claims.FirstName + " " + claims.LastName;
            EmailId = claims.Email;
            LogoutCommand = new Command(OnLogoutClicked);
        }

        public ICommand LogoutCommand { get; set; }
        public string UserFullName
        {
            get => _userFullName;
            set => SetProperty(ref _userFullName, value);
        }
        public string EmailId
        {
            get => _emailId;
            set => SetProperty(ref _emailId, value);
        }

        private async void OnLogoutClicked(object obj)
        {
            await _authenticationService.Logout();
            await _navigationService.NavigateTo(nameof(LoginPage), bool.TrueString);
        }
    }
}

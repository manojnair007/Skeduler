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
    class AppShellViewModel : BaseViewModel, IAppShellViewModel
    {
        private IAuthenticationService _authenticationService;
        private INavigationService _navigationService;
        public AppShellViewModel(IAuthenticationService authenticationService, INavigationService navigationService)
        {
            _authenticationService = authenticationService;
            _navigationService = navigationService;
        }

    }
}

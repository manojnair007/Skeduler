using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartAppointment.Core.Configurations;
using SmartAppointment.Core.Interfaces;
using Xamarin.Forms;

namespace SmartAppointment.Configurations
{
    public class NavigationService: INavigationService
    {
        IViewViewModelMapping _viewModelMapping;
        public NavigationService(IViewViewModelMapping viewViewModelMapping)
        {
            _viewModelMapping = viewViewModelMapping;
        }

        public async Task NavigateTo(string viewName, string data = null, bool isAnimated = true)
        {
            await Shell.Current.GoToAsync($"//{viewName}?Data={data}", isAnimated);
        }

        public async Task PushAsync(string viewName, string data = null, bool isAnimated = true)
        {
            await Shell.Current.GoToAsync($"{viewName}?Data={data}", isAnimated);
        }

        public async Task PopAysnc(bool isAnimated = true)
        {
            await Shell.Current.Navigation.PopAsync(isAnimated);
        }
    }
}

using SmartAppointment.Configurations;
using SmartAppointment.Interfaces.ViewModels;
using SmartAppointment.ViewModels;
using SmartAppointment.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartAppointment
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            this.BindingContext = AppContainerService.GetInstance().Resolve<IAppShellViewModel>();
            Routing.RegisterRoute(nameof(SelectTenantPage), typeof(SelectTenantPage));
            Routing.RegisterRoute(nameof(BookAppointmentPage), typeof(BookAppointmentPage));
            Routing.RegisterRoute(nameof(BookingConfirmationPage), typeof(BookingConfirmationPage));
            
        }
    }
}

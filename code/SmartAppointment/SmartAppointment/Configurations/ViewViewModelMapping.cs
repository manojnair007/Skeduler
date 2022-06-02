using SmartAppointment.Core.Configurations;
using SmartAppointment.Core.Interfaces;
using SmartAppointment.ViewModels;
using SmartAppointment.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAppointment.Configurations
{
    public class ViewViewModelMapping : BaseViewViewModelMapping, IViewViewModelMapping
    {
        public override void AddMapping()
        {
            Mapping.Add(nameof(SelectServiceViewModel), nameof(SelectServicePage));
            Mapping.Add(nameof(BookAppointmentViewModel), nameof(SelectTenantPage));
            Mapping.Add(nameof(LoginViewModel), nameof(LoginPage));
        }
    }
}

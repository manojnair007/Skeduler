using Autofac;
using SmartAppointment.Core.Interfaces;
using SmartAppointment.Core.Interfaces.Models;
using SmartAppointment.Core.Services;
using SmartAppointment.Interfaces.ViewModels;
using SmartAppointment.Models;
using SmartAppointment.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAppointment.Configurations
{
    public class AppContainerService : BaseAppContainerService, IAppContainerService
    {
        private static IAppContainerService _instance;
        private AppContainerService():base() { }
        protected override void RegisterServices()
        {
            Builder.RegisterInstance(this).As<IAppContainerService>().SingleInstance();
            Builder.RegisterType<ViewViewModelMapping>().As<IViewViewModelMapping>().SingleInstance();
            Builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            base.RegisterServices();
        }

        public static IAppContainerService GetInstance()
        {
            if(_instance == null)
            {
                _instance = new AppContainerService();
            }
            return _instance;
        }
        protected override void RegisterViewModels()
        {
            Builder.RegisterType<LoginViewModel>().As<ILoginViewModel>();
            Builder.RegisterType<SelectServiceViewModel>().As<ISelectServiceViewModel>().SingleInstance();
            Builder.RegisterType<SelectTenantViewModel>().As<ISelectTenantViewModel>();
            Builder.RegisterType<BookAppointmentViewModel>().As<IBookAppointmentViewModel>();
            Builder.RegisterType<BookingConfirmationViewModel>().As<IBookingConfirmationViewModel>();
            Builder.RegisterType<AppShellViewModel>().As<IAppShellViewModel>().SingleInstance();
            Builder.RegisterType<ProfileViewModel>().As<IProfileViewModel>().SingleInstance();
            Builder.RegisterType<ManageAppointmentsViewModel>().As<IManageAppointmentsViewModel>();
            

            //throw new NotImplementedException();
        }
        protected override void RegisterModels()
        {
            Builder.RegisterType<LoginModel>().As<ILoginModel>().SingleInstance();
            //throw new NotImplementedException();
        }
    }
}

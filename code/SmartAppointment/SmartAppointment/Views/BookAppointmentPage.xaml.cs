using SmartAppointment.Configurations;
using SmartAppointment.Interfaces.ViewModels;
using SmartAppointment.Models;
using SmartAppointment.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartAppointment.Views
{
    public partial class BookAppointmentPage : ContentPage
    {
        public BookAppointmentPage()
        {
            InitializeComponent();
            this.BindingContext = AppContainerService.GetInstance().Resolve<IBookAppointmentViewModel>();
        }

       
    }
}
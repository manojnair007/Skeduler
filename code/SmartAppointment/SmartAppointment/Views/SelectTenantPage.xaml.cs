using SmartAppointment.Configurations;
using SmartAppointment.Interfaces.ViewModels;
using SmartAppointment.Models;
using SmartAppointment.ViewModels;
using SmartAppointment.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartAppointment.Views
{
    public partial class SelectTenantPage : ContentPage
    {
        ISelectTenantViewModel _viewModel;

        public SelectTenantPage()
        {
            InitializeComponent();
            _viewModel = AppContainerService.GetInstance().Resolve<ISelectTenantViewModel>();
            this.BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            _viewModel.OnAppearing();
            base.OnAppearing();

        }
        protected override void OnDisappearing()
        {
            _viewModel.OnDisappearing();
            base.OnDisappearing();
           
        }
        
    }
}
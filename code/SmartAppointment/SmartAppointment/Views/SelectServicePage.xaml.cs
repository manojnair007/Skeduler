using SmartAppointment.Configurations;
using SmartAppointment.Interfaces.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartAppointment.Views
{
    public partial class SelectServicePage : ContentPage
    {
        ISelectServiceViewModel _viewModel;
        public SelectServicePage()
        {
            InitializeComponent();
            _viewModel = AppContainerService.GetInstance().Resolve<ISelectServiceViewModel>();
            this.BindingContext = _viewModel;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            _viewModel.OnAppearing();
            base.OnAppearing();

        }
    }
}
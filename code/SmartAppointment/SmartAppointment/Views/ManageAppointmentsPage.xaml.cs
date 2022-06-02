using SmartAppointment.Configurations;
using SmartAppointment.Interfaces.ViewModels;
using SmartAppointment.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace SmartAppointment.Views
{
    public partial class ManageAppointmentsPage : ContentPage
    {
        IManageAppointmentsViewModel _viewModel;
        public ManageAppointmentsPage()
        {
            InitializeComponent();
            _viewModel = AppContainerService.GetInstance().Resolve<IManageAppointmentsViewModel>();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            _viewModel.OnAppearing();
            base.OnAppearing();

        }
    }
}
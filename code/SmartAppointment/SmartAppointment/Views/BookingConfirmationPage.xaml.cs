using SmartAppointment.Configurations;
using SmartAppointment.Interfaces.ViewModels;
using SmartAppointment.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace SmartAppointment.Views
{
    public partial class BookingConfirmationPage : ContentPage
    {
        public BookingConfirmationPage()
        {
            InitializeComponent();
            BindingContext = AppContainerService.GetInstance().Resolve<IBookingConfirmationViewModel>();
        }
    }
}
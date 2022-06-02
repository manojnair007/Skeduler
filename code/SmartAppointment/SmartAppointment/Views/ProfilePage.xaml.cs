using SmartAppointment.Configurations;
using SmartAppointment.Interfaces.ViewModels;
using SmartAppointment.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace SmartAppointment.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = AppContainerService.GetInstance().Resolve<IProfileViewModel>();
        }
    }
}
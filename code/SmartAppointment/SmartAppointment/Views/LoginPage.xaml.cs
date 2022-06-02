using SmartAppointment.Configurations;
using SmartAppointment.Interfaces.ViewModels;
using SmartAppointment.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartAppointment.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        ILoginViewModel _viewModel;
        public LoginPage()
        {
            InitializeComponent();
            _viewModel = AppContainerService.GetInstance().Resolve<ILoginViewModel>();
            this.BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            await _viewModel.OnAppearing();
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            _viewModel.OnDisappearing();
            base.OnDisappearing();

        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
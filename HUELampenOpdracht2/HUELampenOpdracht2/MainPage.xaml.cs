using HUELampenOpdracht2.HUELampen.ViewModel;

namespace HUELampenOpdracht2
{
    public partial class MainPage : ContentPage
    { 
        public MainPage(HUEappViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel; 
        }
    }

}

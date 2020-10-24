using SmartPole.Model;
using SmartPole.ViewModel;
using System;
using System.Globalization;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartPole.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeralView : ContentPage
    {
        GeralViewModel viewModel { get; set; }
        public GeralView()
        {
            InitializeComponent();
            viewModel = new GeralViewModel();
            this.BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Dispositivos.Count == 0)
                viewModel.ConsultarDispositivo();

            MessagingCenter.Subscribe<Localizacao>(this, "GPS", async (msg) =>
            {
                await DisplayAlert("Informativo", "Ainda não possuimos nenhum SmartPole em funcionamento", "OK");
                // opens the 'task chooser' so the user can pick Maps, Chrome or other mapping app
                string origemLatitude = msg.Latitude.ToString(new CultureInfo("en-US"));
                string origemLongitude = msg.Longitude.ToString(new CultureInfo("en-US"));
                string destinoLatitude = "-23.5677666";
                string destinoLongitude = "-46.6487763";
                await Launcher.OpenAsync(String.Format("https://www.google.com/maps/dir/?api=1&origin={0},{1}&destination={2},{3}", origemLatitude, origemLongitude, destinoLatitude, destinoLongitude));                //await Launcher.OpenAsync(String.Format("https://www.google.com/maps/search/?api=1&query={0},{1}", latitude, longitude));
                //await Launcher.OpenAsync("https://www.google.com/maps/search/?api=1&query=47.5951518,-122.3316393");
                //CrossExternalMaps.Current.NavigateTo("Macoratti", latitude, longitude);
            });

            MessagingCenter.Subscribe<String>(this, "FalhaConsulta", (msg) =>
            {
                DisplayAlert("Erro de conexão", msg, "Ok");
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<Localizacao>(this, "GPS");
            MessagingCenter.Unsubscribe<String>(this, "FalhaConsulta");
        }
    }
}
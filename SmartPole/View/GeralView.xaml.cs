using Android.Views;
using SmartPole.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartPole.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeralView : ContentPage
    {
        public GeralView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<Localizacao>(this, "GPS", async (msg) =>
            {
                await DisplayAlert("Informativo", "Ainda não possuimos nenhum SmartPole em funcionamento", "OK");
                // opens the 'task chooser' so the user can pick Maps, Chrome or other mapping app
                string latitude = msg.Latitude.ToString(new CultureInfo("en-US"));
                string longitude = msg.Longitude.ToString(new CultureInfo("en-US"));
                
                await Launcher.OpenAsync(String.Format("https://www.google.com/maps/search/?api=1&query={0},{1}", latitude, longitude));
                //await Launcher.OpenAsync("https://www.google.com/maps/search/?api=1&query=47.5951518,-122.3316393");
                //CrossExternalMaps.Current.NavigateTo("Macoratti", latitude, longitude);
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<String>(this, "GPS");
        }
    }
}
using SmartPole.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartPole.ViewModel
{
    public class GeralViewModel
    {
        public Command cmdGPS { get; set; }
        public GeralViewModel()
        {
            cmdGPS = new Command(async ()=>
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Localizacao gps = new Localizacao();
                    gps.Altitude = location.Altitude;
                    gps.Latitude = location.Latitude;
                    gps.Longitude = location.Longitude;
                   MessagingCenter.Send<Localizacao>(gps, "GPS");
                }
            });
        }
    }
}

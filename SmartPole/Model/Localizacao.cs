using System;
using System.Collections.Generic;
using System.Text;

namespace SmartPole.Model
{
    public class Localizacao
    {
        private double latitude { get; set; }
        public double Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
            }
        }
        private double longitude { get; set; }
        public double Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
            }
        }
        private double? altitude { get; set; }
        public double? Altitude
        {
            get
            {
                return altitude;
            }
            set
            {
                altitude = value;
            }
        }
    }
}

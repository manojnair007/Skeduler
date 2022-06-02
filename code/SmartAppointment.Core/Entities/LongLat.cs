using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appointment.Core.Entities
{
    public class LongLat
    {
        [JsonProperty(PropertyName = "lng")]
        public decimal Longitude { get; set; }
        [JsonProperty(PropertyName = "lat")]
        public decimal Latitude { get; set; }

        public string LatLng
        {
            get
            {
                return Latitude + "," + Longitude;
            }
        }
    }
}

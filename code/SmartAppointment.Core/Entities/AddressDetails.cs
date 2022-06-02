using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appointment.Core.Entities
{
    public class AddressDetails
    {
        public string BuildingName { get; set; }
        public string Number { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public LongLat GeoCordinates { get; set; }
        public string PinCode { get; set; }
        public string Location { get; set; }

    }
}

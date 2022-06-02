using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SmartAppointment.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appointment.Core.Entities
{
    public class UserInfo
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public string EmailId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public UserType UserType { get; set; }
    }
}

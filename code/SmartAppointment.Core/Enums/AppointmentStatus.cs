using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appointment.Core.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AppointmentStatus
    {
        Confirmed = 0,
        InProgress = 1,
        Completed = 2,
        Cancelled = 3,
        Waitlisted = 4,
        NoShow = 5,
        Unconfirmed = 6
    }
}

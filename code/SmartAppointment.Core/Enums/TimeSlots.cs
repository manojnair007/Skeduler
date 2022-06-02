using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Appointment.Core.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TimeSlots
    {
        [EnumMember(Value = "Morning")]
        Morning = 0,
        [EnumMember(Value = "Afternoon")]
        Afternoon = 1,
        [EnumMember(Value = "Evening")]
        Evening = 2
    }
}

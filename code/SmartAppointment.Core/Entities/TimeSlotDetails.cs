using Appointment.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appointment.Core.Entities
{
    public class TimeSlotDetails
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public TimeSlots Slot { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}

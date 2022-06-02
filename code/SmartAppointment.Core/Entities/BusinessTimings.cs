using Appointment.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appointment.Core.Entities
{
    public class BusinessTimings
    {

        public Dictionary<WeekDays, TimeSlotDetails[]> TimeSlots { get; set; }
        public TimeSpan AverageServiceTime { get; set; }
    }
}

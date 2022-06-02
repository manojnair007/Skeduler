using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Appointment.Core.Enums
{
    public enum WeekDays
    {
        [EnumMember(Value = "Sunday")]
        Sunday = 0,
        [EnumMember(Value = "Monday")]
        Monday = 1,
        [EnumMember(Value = "Tuesday")]
        Tuesday = 2,
        [EnumMember(Value = "Wednesday")]
        Wednesday = 3,
        [EnumMember(Value = "Thursday")]
        Thursday = 4,
        [EnumMember(Value = "Friday")]
        Friday = 5 ,
        [EnumMember(Value = "Saturday")]
        Saturday = 6
        
    }
}

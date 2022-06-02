using System;
using System.Collections.Generic;
using System.Text;

namespace Appointment.Core.Entities
{
    public class DateSelectors
    {
        public string Day { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get => Date.Year == 1 ? "" : "(" + Date.ToString("dd, MMM")+")"; }
    }
}

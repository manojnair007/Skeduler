using Appointment.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appointment.Core.Entities
{
    public class TenantAppointments
    {
        public string TenantId { get; set; }
        public DateTime Date { get; set; }
        public TimeSlots Slot { get; set; }
        public List<string> Appointments { get; set; } //List of Appointment IDs
    }
}

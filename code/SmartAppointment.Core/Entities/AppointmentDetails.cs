using Appointment.Core.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appointment.Core.Entities
{
    public class AppointmentDetails
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Date { get; set; }

        [JsonProperty(PropertyName = "tenantId")]
        public string TenantId { get; set; }
        public string UserId { get; set; }
        public string OwnerName { get; set; }
        public TimeSlots Slot { get; set; }
        public TimeSpan OrginialAttendingTime { get; set; }
        public TimeSpan RevisedAttendingTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public string DateLongFormat { get { return DateTime.Parse(Date).ToString("dddd, d MMMM yyyy"); } }
        public string OrignalAttendingTimeString { get { return DateTime.Today.Add(OrginialAttendingTime).ToString("hh:mm tt"); } }
        public string RevisedAttendingTimeString { get { return DateTime.Today.Add(RevisedAttendingTime).ToString("hh:mm tt"); } }
    }

}

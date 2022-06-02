using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appointment.Core.Entities
{
    public class Tenant
    {
        [JsonProperty(PropertyName = "id")]
        public string TenantId { get; set; }
        public string OwnerName { get; set; }
        public string Category { get; set; }    
        public string SubCategory { get; set; }
        public string TenantName { get; set; }
        public string Description { get; set; }
        public AddressDetails AddressDetails { get; set; }
        public string PhoneNo { get; set; }
        public string PicUrl { get; set; }
        public BusinessTimings BusinessTimings { get; set; }
        [JsonIgnore]
        public int IsBusy { get; set; }
    }

}

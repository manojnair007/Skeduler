using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAppointment.Core.Entities
{
    public class ServiceProviderCategory
    {
        public ServiceProviderCategory()
        {
            SubCategories = new List<SubCategory>();
        }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Category { get; set; }
        public List<SubCategory> SubCategories { get; set; }
    }

    public class SubCategory
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }

    }
}


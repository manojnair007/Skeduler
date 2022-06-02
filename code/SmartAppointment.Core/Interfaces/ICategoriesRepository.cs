using Appointment.Core.Entities;
using SmartAppointment.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Interfaces
{
    public interface ICategoriesRepository
    {
        Task<ServiceProviderCategory> InsertItemAsync(ServiceProviderCategory category);
        Task<Dictionary<string, List<SubCategory>>> GetServiceProviderCategoryItemsAsync();

    }
}

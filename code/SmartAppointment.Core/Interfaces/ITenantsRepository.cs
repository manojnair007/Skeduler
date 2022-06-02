using Appointment.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Interfaces
{
    public interface ITenantsRepository
    {
        Task<Tenant> InsertItemAsync(Tenant tenantInfo);
        Task<List<T>> GetTenantsAsync<T>();
        Task<List<Tenant>> FetchTenantsByPage(string subCategoryName, int maxItemCount = 4);
    }
}

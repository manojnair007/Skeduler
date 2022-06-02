using Appointment.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserInfo> InsertItemAsync(UserInfo userInfo);
    }
}

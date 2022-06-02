using SmartAppointment.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Interfaces.Models
{
    public interface ILoginModel
    {
        Task<bool> AddUser(Claims claims);
    }
}

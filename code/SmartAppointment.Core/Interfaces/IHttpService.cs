using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Interfaces
{
    public interface IHttpService
    {
        Task<string> GetAsync(Uri baseAddress, string api);
    }
}

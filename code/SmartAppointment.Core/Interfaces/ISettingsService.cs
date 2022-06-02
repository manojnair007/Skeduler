using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Interfaces
{
    public interface ISettingsService
    {
        string EndpointUri { get;}
        string PrimaryKey { get; }
        Task SetCosmosDBPrimaryKey();
    }
}

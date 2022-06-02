using SmartAppointment.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Interfaces
{
    public interface ICacheService
    {
        void InsertDataToCache(string key, object data);
        T GetCachedData<T>(string key);
    }
}

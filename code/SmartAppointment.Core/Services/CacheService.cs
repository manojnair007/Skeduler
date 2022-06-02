using MonkeyCache.SQLite;
using SmartAppointment.Core.Entities;
using SmartAppointment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Services
{
    public class CacheService : ICacheService
    {
        public CacheService()
        {
            Barrel.ApplicationId = "SkedulerCache";
        }

        public T GetCachedData<T>(string key)
        {
            if (!Barrel.Current.IsExpired(key))
            {
                return Barrel.Current.Get<T>(key);
            }
            return default(T);
        }

        public void InsertDataToCache(string key, object data)
        {
            Barrel.Current.Add(key, data, expireIn: TimeSpan.FromDays(100));
        }
    }
}

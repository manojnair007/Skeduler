using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartAppointment.Core.Interfaces
{
    public interface IGeoLocationService
    {
        Task<Placemark> GetLastKnownLocationAsync();
        Placemark GetSavedPlacemark();
        Task<Placemark> GetLocationAsync(CancellationTokenSource cancellationToken);
    }
}

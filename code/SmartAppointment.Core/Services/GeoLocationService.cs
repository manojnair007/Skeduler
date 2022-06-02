using SmartAppointment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace SmartAppointment.Core.Services
{
    public class GeoLocationService : IGeoLocationService
    {
        private Location _location;
        private Placemark _geocodeAddress;
        private ICacheService _cacheService;

        public GeoLocationService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<Placemark> GetLastKnownLocationAsync()
        {
            try
            {
                _location = await Geolocation.GetLastKnownLocationAsync();


                if (_location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(_location.Latitude, _location.Longitude);
                    var validAddress = SelectValidPlaceholder(placemarks); 
                    if(validAddress != null)
                    {
                        _geocodeAddress = validAddress;
                        _cacheService.InsertDataToCache("CurrentLocation", _geocodeAddress);
                        return _geocodeAddress;
                    }
                    
                }
                return null;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
            return null;
        }

        private Placemark SelectValidPlaceholder(IEnumerable<Placemark> placemarks)
        {
            Placemark placemark = null;
            var subLocatlity = from a in placemarks
                               where a != null
                               group a by a.SubLocality into g
                               where g.Count() > 1
                               select g;
            var subLocal = subLocatlity.FirstOrDefault(x => x.Key != null);

            if (subLocal != null)
            {
                 placemark = placemarks.FirstOrDefault(x => x.SubLocality.Equals(subLocal.Key) && x.SubAdminArea != null);
            }
          
            return placemark;
        }

        public async Task<Placemark> GetLocationAsync(CancellationTokenSource cancellationToken )
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cancellationToken = new CancellationTokenSource();
                if (_location == null)
                {
                    _location = await Geolocation.GetLocationAsync(request, cancellationToken.Token);
                }
                if (_location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(_location.Latitude,_location.Longitude);

                    var validAddress = SelectValidPlaceholder(placemarks);
                    if(validAddress != null)
                    {
                        _geocodeAddress = validAddress;
                        _cacheService.InsertDataToCache("CurrentLocation", _geocodeAddress);
                    }
                    else
                    {
                        _geocodeAddress = _cacheService.GetCachedData<Placemark>("CurrentLocation");
                    }
                }

                return _geocodeAddress;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            return null;
        }

        public Placemark GetSavedPlacemark()
        {
            return  _cacheService.GetCachedData<Placemark>("CurrentLocation"); 
        }
    }
}

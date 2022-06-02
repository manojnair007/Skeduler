using Appointment.Core.Entities;
using Appointment.Core.Enums;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using SmartAppointment.Core.Interfaces;
using SmartAppointment.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Repositories
{
    public class AppointmentsRepository : IAppointmentRepository
    {
        const string _databaseId = @"AppointmentsDB";
        const string _collectionId = @"Appointments";
        private Uri _collectionLink = UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId);
        private ICosmosDBProvider _cosmosDBProvider;
        private ICacheService _cacheService;
        private DocumentClient _client;

        public AppointmentsRepository(ICosmosDBProvider cosmosDBProvider, ICacheService cacheService)
        {
            _client = cosmosDBProvider.GetDocumentClient();
            _cosmosDBProvider = cosmosDBProvider;
            _cacheService = cacheService;
        }
        public async Task<AppointmentDetails> UpsertDocumentAsync(AppointmentDetails appointmentDetails)
        {
            try
            {
                var result = await _client.UpsertDocumentAsync(_collectionLink, appointmentDetails);
               // appointmentDetails.Id = result.Resource.Id;

                var appointments = _cacheService.GetCachedData<List<AppointmentDetails>>(_collectionId);
                if (appointments != null)
                {
                    var found = appointments.FirstOrDefault(x=>x.Id == appointmentDetails.Id);
                    if (found != null) {
                        appointments.Remove(found); 
                    }
                    
                }
                _cacheService.InsertDataToCache(_collectionId, appointments.Where(y => DateTime.Parse(y.Date) >= DateTime.Now.Date).ToList());
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
            return appointmentDetails;
        }
        public async Task<AppointmentDetails> InsertItemAsync(AppointmentDetails appointmentDetails)
        {
            try
            {
                var result = await _client.CreateDocumentAsync(_collectionLink, appointmentDetails);
                appointmentDetails.Id = result.Resource.Id;
            
            var appointments = _cacheService.GetCachedData<List<AppointmentDetails>>(_collectionId);
            if (appointments != null)
            {
              
                appointments.Add(appointmentDetails);
            }
            _cacheService.InsertDataToCache(_collectionId, appointments.Where(y => DateTime.Parse(y.Date) >= DateTime.Now.Date).ToList());
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
            return appointmentDetails;
        }
        public async Task<List<AppointmentDetails>> GetAppointmentsAysnc(string userId)
        {
            var appointments = _cacheService.GetCachedData<List<AppointmentDetails>>(_collectionId);
            if (appointments == null)
            {
                appointments = new List<AppointmentDetails>();
                try
                {
                    var document = _client.CreateDocumentQuery<AppointmentDetails>(_collectionLink).Where(a => a.UserId == userId && (a.Status == AppointmentStatus.Confirmed || a.Status == AppointmentStatus.Waitlisted))
                        .AsDocumentQuery();

                    while (document.HasMoreResults)
                    {
                        var tenant = await document.ExecuteNextAsync<AppointmentDetails>();
                        appointments.AddRange(tenant);
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(@"ERROR {0}", e.Message);
                }
                _cacheService.InsertDataToCache(_collectionId, appointments);
            }

            return appointments.Where(y => DateTime.Parse(y.Date) >= DateTime.Now.Date).OrderBy(x => DateTime.Parse(x.Date)).ThenBy(y => y.RevisedAttendingTime).ToList(); 
        }

        public async Task<List<AppointmentDetails>> GetAppointmentsAysnc(string date, string timeSlot, string tenantId)
        {
            var appointments = new List<AppointmentDetails>();
            try
            {
                var document = _client.CreateDocumentQuery<AppointmentDetails>(_collectionLink).Where(a=>a.Status == AppointmentStatus.Confirmed && a.TenantId == tenantId && a.Date == date && a.Slot.ToString() == timeSlot)
                    .AsDocumentQuery();

                while (document.HasMoreResults)
                {
                    var tenant = await document.ExecuteNextAsync<AppointmentDetails>();
                    appointments.AddRange(tenant);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
            return appointments;
        }

        
    }
}

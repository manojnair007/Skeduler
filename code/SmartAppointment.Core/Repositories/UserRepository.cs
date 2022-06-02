using Appointment.Core.Entities;
using Microsoft.Azure.Documents.Client;
using SmartAppointment.Core.Interfaces;
using SmartAppointment.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        const string _databaseId = @"AppointmentsDB";
        const string _collectionId = @"Users";
        private Uri _collectionLink = UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId);
        private ICosmosDBProvider _cosmosDBProvider;

        public UserRepository(ICosmosDBProvider cosmosDBProvider)
        {
            _cosmosDBProvider = cosmosDBProvider;
        }

        public async Task<UserInfo> InsertItemAsync(UserInfo userInfo)
        {
            try
            {
                var result = await _cosmosDBProvider.GetDocumentClient().CreateDocumentAsync(_collectionLink, userInfo);
                userInfo.Id = result.Resource.Id;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
            return userInfo;
        }
    }
}

using Appointment.Core.Entities;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using SmartAppointment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Repositories
{
    public class TenantsRepository : ITenantsRepository
    {
        const string _databaseId = @"AppointmentsDB";
        const string _collectionId = @"Tenants";
        private Uri _collectionLink = UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId);
        private DocumentClient _client;
        private List<Tenant> _tenantInfos;
        private string _continuationToken = null;
        public TenantsRepository(ICosmosDBProvider cosmosDBProvider)
        {
            _client = cosmosDBProvider.GetDocumentClient();
            _tenantInfos = new List<Tenant>();
        }

        public async Task<Tenant> InsertItemAsync(Tenant tenantInfo)
        {
            try
            {
                var result = await _client.CreateDocumentAsync(_collectionLink, tenantInfo);
                tenantInfo.TenantId = result.Resource.Id;
                _tenantInfos.Add(tenantInfo);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
            return tenantInfo;
        }

        public async Task<List<T>> GetTenantsAsync<T>()
        {
            var tenants = new List<T>();
            try
            {
                var query = _client.CreateDocumentQuery<T>(_collectionLink, new FeedOptions { MaxItemCount = 3 })
                                  .AsDocumentQuery();
                var tenant = await query.ExecuteNextAsync<T>();
                tenants.AddRange(tenant);
                //while (query.HasMoreResults)
                //{
                //    var tenant = await query.ExecuteNextAsync<T>();
                //    tenants.AddRange(tenant);
                //}
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
            return tenants;
        }

        public async Task<List<Tenant>> FetchTenantsByPage(string subCategoryName, int maxItemCount = 2)
        {
            int currentPageNumber = 1;
            // Continuation token for subsequent queries (NULL for the very first request/page)
            if (_continuationToken == "End")
            {
                return null;
            }
           var currentPage = await QueryDocumentsByPage(subCategoryName,currentPageNumber, maxItemCount, _continuationToken);
            // Ensure the continuation token is kept for the next page query execution
            _continuationToken = (currentPage.Key == null) ? "End" : currentPage.Key;
            return currentPage.Value;
           
        }

        private async Task<KeyValuePair<string, List<Tenant>>> QueryDocumentsByPage(string subCategoryName, int pageNumber, int maxItemCount, string continuationToken)
        {
            var feedOptions = new FeedOptions
            {
                MaxItemCount = maxItemCount,
                EnableCrossPartitionQuery = true,
                RequestContinuation = continuationToken
            };

            IQueryable<Tenant> filter = _client.CreateDocumentQuery<Tenant>(_collectionLink, feedOptions).Where(x=>x.SubCategory == subCategoryName);
            IDocumentQuery<Tenant> query = filter.AsDocumentQuery();

            FeedResponse<Tenant> feedRespose = await query.ExecuteNextAsync<Tenant>();
            List<Tenant> documents = new List<Tenant>();
            documents.AddRange(feedRespose);
            return new KeyValuePair<string, List<Tenant>>(feedRespose.ResponseContinuation, documents);
        }

    }
}

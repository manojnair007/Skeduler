using Appointment.Core.Entities;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using SmartAppointment.Core.Entities;
using SmartAppointment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        const string _databaseId = @"AppointmentsDB";
        const string _collectionId = @"ServiceProviderCategories";
        private Uri _collectionLink = UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId);
        private ICacheService _cacheService;
        private DocumentClient _client;

        public CategoriesRepository(ICosmosDBProvider cosmosDBProvider, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _client = cosmosDBProvider.GetDocumentClient();

        }

        public async Task<ServiceProviderCategory> InsertItemAsync(ServiceProviderCategory category)
        {
            try
            {
                var result = await _client.CreateDocumentAsync(_collectionLink, category);
                category.Id = result.Resource.Id;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
            return category;
        }

        public async Task<Dictionary<string,List<SubCategory>>> GetServiceProviderCategoryItemsAsync()
        {

            var categories = _cacheService.GetCachedData<Dictionary<string, List<SubCategory>>>(_collectionId);
            if (categories != null)
            {
                return categories;
            }

            var categoryList = new List<ServiceProviderCategory>();
            categories = new Dictionary<string, List<SubCategory>>();
            try
            {
                var query = _client.CreateDocumentQuery<ServiceProviderCategory>(_collectionLink)
                                  .AsDocumentQuery();
                while (query.HasMoreResults)
                {
                    categoryList.AddRange(await query.ExecuteNextAsync<ServiceProviderCategory>());
                }

                
                foreach (var category in categoryList)
                {
                    categories.Add(category.Category, category.SubCategories);
                }

            }
            catch (DocumentClientException ex)
            {
                Debug.WriteLine("Error: ", ex.Message);
            }

            _cacheService.InsertDataToCache(_collectionId,categories);
            return categories;
        }

      
    }
}

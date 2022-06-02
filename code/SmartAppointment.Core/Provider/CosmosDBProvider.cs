using Microsoft.Azure.Documents.Client;
using SmartAppointment.Core.Interfaces;
using System;
using System.Configuration;

namespace SmartAppointment.Core.Provider
{
    public class CosmosDBProvider : ICosmosDBProvider
    {
        private ISettingsService _settingsService;

        public DocumentClient _client { get; private set; }
        public CosmosDBProvider(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public DocumentClient GetDocumentClient()
        {
            if(_client == null)
            {
                _client = new DocumentClient(new System.Uri(_settingsService.EndpointUri), _settingsService.PrimaryKey);
            }
            return _client;
        }
    }
}

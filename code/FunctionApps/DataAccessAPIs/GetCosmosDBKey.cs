using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DataAccessAPIs.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SmartAppointment.Core.Interfaces;
using SmartAppointment.Core.Provider;
using SmartAppointment.Core.Repositories;
using SmartAppointment.Core.Services;

namespace DataAccessAPIs
{
    public static class GetCosmosDBKey
    {
        static readonly HttpClient httpClient = new HttpClient();

        [FunctionName("GetCosmosDBKey")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                // AzureServiceTokenProvider will help us to get the Service Managed token.
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                // Authenticate to the Azure Resource Manager to get the Service Managed token.
                string accessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://management.azure.com/");
                var subscriptionId = Environment.GetEnvironmentVariable("SubscriptionId");
                var resourceGroupName = Environment.GetEnvironmentVariable("ResourceGroupName");
                var accountName = Environment.GetEnvironmentVariable("CosmosDBAccountName");
                // Setup the List Keys API to get the Azure Cosmos DB keys.
                string endpoint = $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.DocumentDB/databaseAccounts/{accountName}/listKeys?api-version=2019-12-12";

                // Add the access token to request headers.
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                // Post to the endpoint to get the keys result.
                var result = await httpClient.PostAsync(endpoint, new StringContent(""));

                // Get the result back as a DatabaseAccountListKeysResult.
                DatabaseAccountListKeysResult keys = await result.Content.ReadAsAsync<DatabaseAccountListKeysResult>();
      
                return new OkObjectResult(keys.primaryMasterKey);
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult("Could not fetch the data " + ex);
            }
        }
    }
}


using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAppointment.Core.Interfaces
{
    public interface ICosmosDBProvider
    {
        DocumentClient GetDocumentClient();
    }
}

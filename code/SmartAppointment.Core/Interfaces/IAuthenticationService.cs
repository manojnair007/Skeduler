using Microsoft.Identity.Client;
using SmartAppointment.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Claims GetClaims();
        string GetIdToken();
        Task<bool> AcquireTokenInteractive(string[] scopes, object uiParent);
        Task<bool> CheckIfUserLoggedInAlready(string[] scopes);
        Task Logout();
        string GetAccessToken();
    }
}

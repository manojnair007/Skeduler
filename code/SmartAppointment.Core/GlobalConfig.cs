using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAppointment.Core
{
    public class GlobalConfig
    {
        public static readonly string AppName = "SKEDULER";
        public static readonly string DataAccessFunctioAppEndpointUri = "";
        public static readonly string CosmosDBEndpointUri = "";
        public static readonly string TenantName = "";
        public static readonly string TenantId = "";
        public static readonly string ClientId = "";
        public static readonly string SignInPolicy = "";
        public static readonly string IosKeychainSecurityGroups = "";
        public static readonly string[] Scopes = new string[] { };
        public static readonly string AuthorityBase = $"https://{TenantName}.b2clogin.com/tfp/{TenantId}/";
        public static readonly string AuthoritySignin = $"{AuthorityBase}{SignInPolicy}";

    }
}

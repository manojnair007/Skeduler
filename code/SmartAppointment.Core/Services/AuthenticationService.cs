using Microsoft.Identity.Client;
using SmartAppointment.Core.Entities;
using SmartAppointment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private AuthenticationResult _result;
        public static IPublicClientApplication _authenticationClient { get; private set; }

        public AuthenticationService(string clientId, string iosKeychainSecurityGroups, string authoritySignIn)
        {
            try
            {
                _authenticationClient = PublicClientApplicationBuilder.Create(clientId)
            .WithIosKeychainSecurityGroup(iosKeychainSecurityGroups)
            .WithB2CAuthority(authoritySignIn)
            .WithRedirectUri($"msal{clientId}://auth")
            .Build();
            }
            catch (MsalClientException ex)
            {
                //if (ex.Message != null && ex.Message.Contains("AADB2C90118"))
                //{
                //    result = await OnForgotPassword();
                //    await Navigation.PushAsync(new LogoutPage(result));
                //}
                //else
                if (ex.ErrorCode != "authentication_canceled")
                {
                    //await DisplayAlert("An error has occurred", "Exception message: " + ex.Message, "Dismiss");
                }
            }
        }
        public Claims GetClaims()
        {
            Claims claim = null;
            if (_result.IdToken != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var data = handler.ReadJwtToken(_result.IdToken);
                var claims = data.Claims.ToList();
                var isNewUser = false;
                if (data != null)
                {
                    var newUser = data.Claims.FirstOrDefault(x => x.Type.Equals("newUser"));
                    if (newUser != null)
                    {
                        bool.TryParse(newUser.Value, out isNewUser);
                    }
                    claim = new Claims()
                    {
                        FirstName = data.Claims.FirstOrDefault(x => x.Type.Equals("given_name")).Value,
                        LastName = data.Claims.FirstOrDefault(x => x.Type.Equals("family_name")).Value,
                        IsNewUser = isNewUser,
                        Email = data.Claims.FirstOrDefault(x => x.Type.Equals("emails")).Value
                    };
                }
            }
            return claim;
        }

        public string GetIdToken()
        {
            return _result.IdToken;
        }

        public string GetAccessToken()
        {
            return _result.AccessToken;
        }

        public async Task<bool> CheckIfUserLoggedInAlready(string[] scopes)
        {
            // Look for existing account
            IEnumerable<IAccount> accounts = await _authenticationClient.GetAccountsAsync();
            if (accounts.Count() >= 1)
            {
                _result = await _authenticationClient
                    .AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                    .ExecuteAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> AcquireTokenInteractive(string[] scopes, object uiParent)
        {
            try
            {
                _result = await _authenticationClient.AcquireTokenInteractive(scopes)
                                                    .WithUseEmbeddedWebView(true)
                                                   .WithPrompt(Prompt.ForceLogin)
                                                   .WithParentActivityOrWindow(uiParent)
                                                   .ExecuteAsync();

                return _result != null && !string.IsNullOrEmpty(_result.IdToken);
            }
            catch
            {
                return false;
            }
        }

        public async Task Logout()
        {
            IEnumerable<IAccount> accounts = await _authenticationClient.GetAccountsAsync();
            foreach (var account in accounts)
            {
                await _authenticationClient.RemoveAsync(account);
            }
          
        }
    }
}

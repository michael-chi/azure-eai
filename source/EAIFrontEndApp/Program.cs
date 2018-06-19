using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleApp1
{
    class Program
    {
        static readonly string AUTHORITY = "https://login.microsoftonline.com/72f988bf-86f1-41af-91ab-2d7cd011db47";
        //[OR] static readonly string AUTHORITY = "https://login.microsoftonline.com/microsoft.onmicrosoft.com";
        static readonly string FRONT_APP_KEY = "<KEY>";
        static readonly string FRONT_APP_ID = "xxxxaxxxx-xxxx-xxxx-xxxx-xxxxx";
        static readonly string BACKEND_APP_ID = "xxx-xxx-xxx-xxx-xxx";
        protected static string GetOauthAuthorizationHeader()
        {
            AuthenticationResult result = null;
            var context = new AuthenticationContext(AUTHORITY);
            var thread = new Thread(() =>
            {
                var clientCred = new ClientCredential(FRONT_APP_ID, FRONT_APP_KEY);
                result = Task.Run(() => context.AcquireTokenAsync(BACKEND_APP_ID,
                                   clientCred)).Result;
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Name = "AquireTokenThread";
            thread.Start();
            thread.Join();

            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the JWT token");
            }


            string token = result.AccessToken;
            
            return token;
        }
        static void Main(string[] args)
        {
            var token = GetOauthAuthorizationHeader();
            var url = "https://michieaibackendapi.azurewebsites.net/api/values/1";
            var req = HttpWebRequest.Create(url) as HttpWebRequest;
            req.Headers.Add("Authorization", "Bearer " + token);
            //req.Headers.Add("Ocp-Apim-Subscription-Key", "xxxxxxxxxxxxxxxxxxxx");

            req.Method = "GET";
            
            using (var respStream = req.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(respStream))
                {
                    var text = sr.ReadToEnd();
                    Console.WriteLine(text);

                    Console.ReadKey();
                }
            }
        }
    }
}

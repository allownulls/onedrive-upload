using System;
using System.IO;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace onedrive_upload
{
    static class Program
    {
        public static readonly string appId = "";
        public static readonly string clientSecret = "";
        public static readonly string tenantId = "";
        public static readonly string userId = "";

        public static readonly string defaultScopes = "https://graph.microsoft.com/.default";
        public static readonly string defaultLoginUrl = "https://login.microsoftonline.com";
        
        static async Task Main(string[] args)
        {

            string[] scopes = new string[] { defaultScopes };

            AuthenticationResult result = null;
            IConfidentialClientApplication app;

            try
            {
                app = ConfidentialClientApplicationBuilder.Create(appId)
                        .WithClientSecret(clientSecret)
                        .WithAuthority(new Uri($"{defaultLoginUrl}/{tenantId}"))
                        .Build();

                result = await app.AcquireTokenForClient(scopes)
                    .ExecuteAsync();
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
            }

            await ListFiles(result.AccessToken);
            await UploadFile(result.AccessToken);

         }

        public static async Task ListFiles(string accessToken)
        {
            using (var httpClient = new HttpClient())
            {
                var defaultRequestHeaders = httpClient.DefaultRequestHeaders;
                defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                HttpResponseMessage response = await httpClient.GetAsync($"https://graph.microsoft.com/v1.0/users/{userId}/drive/root:/upload:/children");

                await WriteResponse(response);
            }
        }

        public static async Task UploadFile(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                return;

            using (var httpClient = new HttpClient())
            {
                var headers = httpClient.DefaultRequestHeaders;                
                headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);                

                var filename = "example.html";
                var b = new ByteArrayContent(File.ReadAllBytes(filename));

                HttpResponseMessage response = await httpClient.PutAsync($"https://graph.microsoft.com/v1.0/users/{userId}/drive/root:/upload/{filename}:/content",b);

                await WriteResponse(response);
            }
        }

        public static async Task WriteResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                JObject result = JsonConvert.DeserializeObject(json) as JObject;

                foreach (JProperty child in result.Properties().Where(p => !p.Name.StartsWith("@")))
                {
                    Console.WriteLine($"{child.Name} = {child.Value}");
                }
            }
            else
            {
                Console.WriteLine($"Failed to call the web API: {response.StatusCode}");
                string message = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Content: {message}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

using System.Net.Http;

namespace KvtClient
{
    public class User
    {
        public string SubjectId { get; set; }
       // public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    class Client
    {
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
          
          CallAPIAsyncUsingPassword();
          Console.ReadLine();
        }

        static async void CreateUserAsync(User user)
        {
          //  client.BaseAddress = new Uri("http://localhost:5000");
            HttpResponseMessage response = await client.PutAsJsonAsync(
                "http://localhost:5000/api/RegisterUser/", user);

           response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
               
                 Console.WriteLine(response);
            else
                Console.WriteLine("error");
        }

        static async void CallAPIAsyncUsingPassword()
        {
            var discoveryClient = new DiscoveryClient("http://localhost:5000");
            var disco = await discoveryClient.GetAsync();
            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "winclient", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("jayapradhareddy@gmail.com", "jaya12", "customAPI");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(JArray.Parse(content));
                Console.WriteLine(content);
            }
        }
    }
}

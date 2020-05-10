using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using RestSharp;

namespace Smart_Scale.Clients
{
    public sealed class Client
    {
        private readonly string API_BASE_URL = "https://bmi.p.rapidapi.com/";
        // public readonly string resourceNotFoundString = "Resource not found - 404";
        public readonly string key = "2db3416568mshfc034b3d68dd240p101c90jsn485f9a5295c4";
        public readonly string content_type = "application/json";


        // Singleton
        private Client()
        {
        }
        private static readonly Lazy<Client> lazy = new Lazy<Client>(() => new Client());
        public static Client Instance
        {
            get
            {
                return lazy.Value;
            }
        }


        public string Post2(string Waga, string Wiek, string Plec, string Wzrost)
        {
            var client = new RestClient("https://bmi.p.rapidapi.com/");
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-rapidapi-host", "bmi.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", key);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("accept", "application/json");
            request.AddParameter("application/json", "{\"weight\":{\"value\":\""+ Waga +"\",\"unit\":\"kg\"},\"height\":{\"value\":\""+ Wzrost +"\",\"unit\":\"cm\"},\"sex\":\""+ Plec+"\",\"age\":\""+Wiek+"\",\"waist\":\"\",\"hip\":\"\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }


}

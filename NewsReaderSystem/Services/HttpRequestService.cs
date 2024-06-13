using NewsReaderSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NewsReaderSystem.Services
{
    public class HttpRequestService
    {
        private readonly HttpClient client = new HttpClient();

        public async Task<FaceMousingResult> Request()
        {
            //try
            //{
            //    HttpResponseMessage response = await client.GetAsync("http://127.0.0.1:8000/mouse/report");
            //    response.EnsureSuccessStatusCode();
            //    string responseBody = await response.Content.ReadAsStringAsync();
            //    FaceMousingResult faceMousingResult = JsonConvert.DeserializeObject<FaceMousingResult>(responseBody);
            //    Console.WriteLine(responseBody);
            //    return faceMousingResult;
            //}
            //catch (HttpRequestException e)
            //{
            //    Console.WriteLine("\nException Caught!");
            //    Console.WriteLine("Message: {0} ", e.Message);
            //    return null;
            //}

            // Define the URL and the request data
            var url = "http://127.0.0.1:8000/mouse/report";
            var content = new StringContent("", Encoding.UTF8, "application/json");

            // Set the request headers
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Send the POST request
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string responseBody = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(responseBody);

                //
                FaceMousingResult faceMousingResult = JsonConvert.DeserializeObject<FaceMousingResult>(responseBody);

                return faceMousingResult;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message: {0} ", e.Message);
                return null;
            }

        }
    }
}

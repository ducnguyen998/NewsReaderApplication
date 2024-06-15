using NewsReaderSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NewsReaderSystem.Services
{
    public class HttpRequestService
    {
        private readonly HttpClient client = new HttpClient();

        public async Task<FaceMousingResult> Request()
        {
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

        public async Task ConnectWebSocketAsync()
        {
            using (ClientWebSocket client = new ClientWebSocket())
            {
                Uri serverUri = new Uri("ws://127.0.0.1:8000/mouse/ws");
                try
                {
                    await client.ConnectAsync(serverUri, CancellationToken.None);
                    Console.WriteLine("Connected to WebSocket server.");

                    var receivingTask = Task.Run(async () =>
                    {
                        var buffer = new byte[1024 * 4];
                        while (client.State == WebSocketState.Open)
                        {
                            WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            Console.WriteLine("Received: " + message);

                            try
                            {
                                FaceMousingResult faceMousingResult = JsonConvert.DeserializeObject<FaceMousingResult>(message);

                                if (faceMousingResult != null)
                                {
                                    App.Current.Dispatcher.Invoke(() =>
                                    {
                                        FaceMousing?.Invoke(this, faceMousingResult);
                                    });
                                }
                                
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"[A1] Parse Json Error: {e.Message}");
                            }
                        }
                    });

                    // Keep the connection alive for demonstration purposes
                    await receivingTask;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e.Message}");
                }
                finally
                {
                    await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    Console.WriteLine("WebSocket connection closed.");
                }
            }
        }

        public event EventHandler<FaceMousingResult> FaceMousing;
    }
}

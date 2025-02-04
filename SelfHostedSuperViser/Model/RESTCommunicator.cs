using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SelfHostedSuperViser.Model
{
    public class RESTCommunicator
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        public static async Task<List<APIValue>> APIGet(string apiURL, List<List<string>> names, Dictionary<string, string> headers)
        {
            HttpClient.DefaultRequestHeaders.Clear();
            foreach (var header in headers)
            {
                HttpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            HttpResponseMessage response = await HttpClient.GetAsync(apiURL);
            response.EnsureSuccessStatusCode();

            var jsonObject = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            List<APIValue> APIValues = new List<APIValue>();
            foreach (var nameList in names)
            {
                string value;
                try
                {
                    JsonElement tempObj = jsonObject.RootElement;
                    nameList.ForEach(x => { tempObj = tempObj.GetProperty(x); });
                    value = tempObj.GetRawText();
                }
                catch
                {
                    throw new ArgumentException($"API property {nameList[nameList.Count - 1]} doesn't exist!");
                }

                var APIValue = new APIValue()
                {
                    Name = nameList[nameList.Count - 1],
                    Value = value,
                };

                APIValues.Add(APIValue);
            }

            return APIValues;
        }
        public static async Task<JsonDocument> APIPost(string apiURL, Dictionary<string, string> jsonContent)
        {
            HttpClient.DefaultRequestHeaders.Clear();

            var loginRequest = new LoginRequest
            {
                Type = jsonContent.GetValueOrDefault("type"),
                Identity = jsonContent.GetValueOrDefault("identity"),
                Secret = jsonContent.GetValueOrDefault("secret")
            };
            var json = JsonContent.Create(loginRequest);
            HttpResponseMessage response = await HttpClient.PostAsync(apiURL, json);
            response.EnsureSuccessStatusCode();

            var jsonObject = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            return jsonObject;
        }
    }
}

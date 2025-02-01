using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SelfHostedSuperViser.Model
{
    public class APIGetter
    {
        private static HttpClient HttpClient = new();

        public static async Task<List<APIValue>> APIGet(String apiURL, String[] names)
        {
            HttpResponseMessage response = await HttpClient.GetAsync(apiURL);
            response.EnsureSuccessStatusCode();

            var jsonObject = JsonDocument.Parse(
                await response.Content.ReadAsStringAsync());

            List<APIValue> APIValues = [];
            foreach (var name in names)
            {
                // ?? Throws an exception if return value == null
                var value = jsonObject.RootElement.GetProperty(name).GetString() ?? throw new Exception($"API property {name}, doesn't exist!");
                var APIValue = new APIValue()
                {
                    Name = name,
                    Value = value,
                };

                APIValues.Add(APIValue);
            }

            return APIValues;
        }
    }

    public class APIValue
    {
        public required String Value { get; set; }
        public required String Name { get; set; }
    }
}

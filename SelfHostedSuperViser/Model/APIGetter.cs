using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SelfHostedSuperViser.Model
{
    public class APIGetter
    {

        public static async Task<List<APIValue>> APIGet(String apiURL, String[] names, Header[] headers)
        {
            HttpClient HttpClient = new();
            foreach (var header in headers)
            {
                HttpClient.DefaultRequestHeaders.Add(header.Name, header.Value);
            }
            HttpResponseMessage response = await HttpClient.GetAsync(apiURL);
            response.EnsureSuccessStatusCode();

            var jsonObject = JsonDocument.Parse(
                await response.Content.ReadAsStringAsync());

            List<APIValue> APIValues = [];
            foreach (var name in names)
            {
                string value;
                try
                {
                    value = jsonObject.RootElement.GetProperty(name).GetRawText();
                }
                catch
                {
                    throw new ArgumentException($"API property {name}, doesn't exist!");
                }

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

    public class Header
    {
        public required string Name { get; set; }
        public required string Value { get; set; }
    }
}

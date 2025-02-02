using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SelfHostedSuperViser.Model.APIGetter.APIGetter
{
    public class APIGetter
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        public static async Task<List<APIValue>> APIGet(string apiURL, string[] names, Header[] headers)
        {
            HttpClient.DefaultRequestHeaders.Clear();
            foreach (var header in headers)
            {
                HttpClient.DefaultRequestHeaders.Add(header.Name, header.Value);
            }

            HttpResponseMessage response = await HttpClient.GetAsync(apiURL);
            response.EnsureSuccessStatusCode();

            var jsonObject = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            List<APIValue> APIValues = new List<APIValue>();
            foreach (var name in names)
            {
                string value;
                try
                {
                    value = jsonObject.RootElement.GetProperty(name).GetRawText();
                }
                catch
                {
                    throw new ArgumentException($"API property {name} doesn't exist!");
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
        public required string Value { get; set; }
        public required string Name { get; set; }
    }

    public class Header
    {
        public required string Name { get; set; }
        public required string Value { get; set; }
    }
}

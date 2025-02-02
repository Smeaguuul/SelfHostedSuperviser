using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using SelfHostedSuperViser.Model.APIGetter.APIGetter;

namespace SelfHostedSuperViser.Model.APIGetter
{
    abstract class Service : WebsiteAPIModel
    {
        public async Task<List<APIValue>> CallAPIAsync()
        {
            var text = GetFileText();

            Root

            JsonElement jsonElement = GetJsonElement(text);

            string[] names = GetNames(jsonElement);

            Dictionary<string, string> headers = GetHeaders(jsonElement);

            string url = GetUrl(jsonElement);

            var result = await APIGetter.APIGetter.APIGet(url, names, headers);

            result.Add(new APIValue() { Name = "Website", Value = GetWebsiteName() });
            return result;
        }

        protected abstract JsonElement GetJsonElement(string text);
        protected abstract string GetWebsiteName();
        protected string GetUrl(JsonElement jsonElement)
        {
            return jsonElement.GetProperty("BaseUrl").GetString() + GetEndpoint();
        }

        protected abstract string GetEndpoint();
        protected abstract Dictionary<string, string> GetHeaders(JsonElement jsonElement);
        protected string[] GetNames(JsonElement jsonElement)
        {
            JsonElement namesElement = jsonElement.GetProperty("names");
            if (namesElement.ValueKind == JsonValueKind.Array)
            {
                foreach (string name in namesElement)
                {
                    
                }
            }
            string[] names = jsonElement.;
            for (int i = 0; i < jsonArray.Count(); i++)
            {
                names[i] = jsonArray;
            }
            return names;
        }

        private static string GetFileText()
        {
            // Get the path to the solution directory
            string solutionDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));

            // Combine the solution directory with the file name
            string filePath = Path.Combine(solutionDirectory, "SelfHostedSuperViser", "APICalls.json");
            StreamReader reader = new(filePath);

            return reader.ReadToEnd();
        }
    }
}

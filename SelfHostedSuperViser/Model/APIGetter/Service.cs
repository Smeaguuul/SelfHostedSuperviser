using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using SelfHostedSuperViser.Model.APIGetter.APIGetter;

namespace SelfHostedSuperViser.Model.APIGetter
{
    public abstract class Service : WebsiteAPIModel
    {
        public async Task<List<APIValue>> CallAPIAsync()
        {
            var text = GetFileText();

            JsonElement jsonElement = GetJsonElement(text);

            List<string> names = GetNames(jsonElement);

            Dictionary<string, string> headers = GetHeaders(jsonElement);

            string url = GetUrl(jsonElement);

            var result = await APIGetter.APIGetter.APIGet(url, names, headers);

            result.Add(new APIValue() { Name = "Website", Value = GetWebsiteName() });
            return result;
        }

        protected abstract string GetEndpoint();
        protected abstract Dictionary<string, string> GetHeaders(JsonElement jsonElement);

        protected string GetWebsiteName()
        {
            return jsonObject.GetProperty("BaseUrl").GetString();
        }
        protected JsonElement GetJsonElement(string text)
        {
            return JsonDocument.Parse(text).RootElement.GetProperty(GetWebsiteName());
        }
        protected string GetUrl(JsonElement jsonElement)
        {
            return jsonElement.GetProperty("BaseUrl").GetString() + GetEndpoint();
        }
        protected static List<string> GetNames(JsonElement jsonElement)
        {
            List<string> names = [];

            JsonElement namesElement = jsonElement.GetProperty("names");
            if (namesElement.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement name in namesElement.EnumerateArray())
                {
                    names.Add(name.GetString());
                }
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

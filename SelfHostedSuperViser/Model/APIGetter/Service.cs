using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SelfHostedSuperViser.Model.APIGetter.APIGetter;
using static System.Net.Mime.MediaTypeNames;

namespace SelfHostedSuperViser.Model.APIGetter
{
    public abstract class Service : WebsiteAPIModel
    {
        public async Task<List<APIValue>> CallAPIAsync()
        {

            var jsonElement = GetJsonFile("APICalls.json").GetProperty(GetWebsiteName()); ;
            var secretsElement = GetJsonFile("Secrets.json");

            List<List<string>> names = GetNames(jsonElement);

            Dictionary<string, string> headers = await GetHeaders(secretsElement);

            string url = GetUrl(jsonElement);

            var result = await APIGetter.APIGetter.APIGet(url, names, headers);

            result.Add(new APIValue() { Name = "Website", Value = GetWebsiteName() });
            return result;
        }

        protected abstract string GetEndpoint();
        protected abstract Task<Dictionary<string, string>> GetHeaders(JsonElement jsonElement);
        protected abstract string GetWebsiteName();
        protected string GetUrl(JsonElement jsonElement)
        {
            return jsonElement.GetProperty("BaseUrl").GetString() + GetEndpoint();
        }
        protected static List<List<string>> GetNames(JsonElement jsonElement)
        {
            List<List<string>> names = [];

            // "names" er en array af string arrays.
            JsonElement namesElement = jsonElement.GetProperty("names");
            if (namesElement.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement nameArray in namesElement.EnumerateArray())
                {
                    List<string> apiValues = [];
                    foreach(var name in nameArray.EnumerateArray())
                    {
                        apiValues.Add(name.GetString());
                    }
                    names.Add(apiValues);
                }
            }
            return names;
        }
        protected JsonElement GetJsonFile(string fileName)
        {
            // Get the path to the solution directory
            string solutionDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));

            // Combine the solution directory with the file name
            string filePath = Path.Combine(solutionDirectory, "SelfHostedSuperViser", fileName);
            StreamReader reader = new(filePath);
            var text =  reader.ReadToEnd();

            return JsonDocument.Parse(text).RootElement;
        }
    }
}

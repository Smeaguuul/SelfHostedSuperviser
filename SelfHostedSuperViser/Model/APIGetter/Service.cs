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
    public class Service : WebsiteAPIModel
    {
        public required string WebsiteName { get; set; }
        public async Task<List<APIValue>> CallAPIAsync()
        {
            var jsonElement = GetJsonFile("APICalls.json").GetProperty(WebsiteName); ;
            var secretsElement = GetJsonFile("Secrets.json");

            List<List<string>> names = GetNames(jsonElement);

            Dictionary<string, string> headers = await GetHeaders(secretsElement, jsonElement);

            string url = GetUrl(jsonElement);

            var result = await APIGetter.APIGetter.APIGet(url, names, headers);

            result.Add(new APIValue() { Name = "Website", Value = WebsiteName });
            return result;
        }

        protected static string GetEndpoint(JsonElement apiCallsElement)
        {
            var endpoint = apiCallsElement.GetProperty("Endpoint").GetString();
            return endpoint;
        }
        protected async Task<Dictionary<string, string>> GetHeaders(JsonElement secretsElement, JsonElement apiCallsElement)
        {
            string authType = GetAuthType(apiCallsElement);
            var headers = new Dictionary<string, string>();
            if (authType == "basic") headers = AuthorizationBasic(secretsElement);
            if (authType == "bearer") headers = await AuthorizationBearer(secretsElement, apiCallsElement);
            if (authType == "api") headers = AuthorizationAPI(secretsElement);

            return headers;
        }

        private Dictionary<string, string> AuthorizationAPI(JsonElement secretsElement)
        {
            var apiKey = secretsElement.GetProperty(WebsiteName + "_api_ley").GetString() ?? throw new Exception("Key: 'api_key', not found in Secrets.json!");
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "x-api-key", apiKey }
            };

            return headers;
        }
        private async Task<Dictionary<string, string>> AuthorizationBearer(JsonElement secretsElement, JsonElement apiCallsElement)
        {
            var username = secretsElement.GetProperty(WebsiteName + "_usr").GetString();
            var password = secretsElement.GetProperty(WebsiteName + "_pwd").GetString();

            var jsonContent = new Dictionary<string, string>();
            jsonContent.Add("type", "password");
            jsonContent.Add("identity", username);
            jsonContent.Add("secret", password);
            var url = apiCallsElement.GetProperty("BaseUrl").GetString();

            var jsonToken = await APIGetter.APIGetter.APIPost(url + "/api/tokens", jsonContent);
            var token = jsonToken.RootElement.GetProperty("token");

            Dictionary<string, string> headers = [];
            headers.Add($"Authorization", "Bearer " + token);
            return headers;
        }
        protected Dictionary<string, string> AuthorizationBasic(JsonElement secretsElement)
        {
            var password = secretsElement.GetProperty(WebsiteName + "_pwd").GetString();
            var username = secretsElement.GetProperty(WebsiteName + "_usr").GetString();


            var plainTextBytes = Encoding.UTF8.GetBytes(username + ":" + password);
            var authorization = Convert.ToBase64String(plainTextBytes);
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "Authorization", "Basic " + authorization }
            };

            return headers;
        }
        private static string GetAuthType(JsonElement jsonElement)
        {
            var authType = jsonElement.GetProperty("Authorization").GetString();
            return authType;
        }
        protected string GetUrl(JsonElement jsonElement)
        {
            return jsonElement.GetProperty("BaseUrl").GetString() + GetEndpoint(jsonElement);
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

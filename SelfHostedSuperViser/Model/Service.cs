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
using static System.Net.Mime.MediaTypeNames;

namespace SelfHostedSuperViser.Model
{
    public class Service
    {
        public required string WebsiteName { get; set; }
        private JsonElement _JsonElement { get; set; }
        private JsonElement _SecretsElement { get; set; }
        public async Task<List<APIValue>> CallAPIAsync()
        {
            _JsonElement = GetJsonFile("APICalls.json").GetProperty(WebsiteName); ;
            _SecretsElement = GetJsonFile("Secrets.json");

            List<APIValue> names = GetAPIValues();

            Dictionary<string, string> headers = await GetHeaders();

            string url = GetUrl();

            var result = await RESTCommunicator.APIGet(url, names, headers);

            return result;
        }

        protected string GetEndpoint()
        {
            var endpoint = _JsonElement.GetProperty("Endpoint").GetString();
            return endpoint;
        }
        protected async Task<Dictionary<string, string>> GetHeaders()
        {
            string authType = GetAuthType();
            var headers = new Dictionary<string, string>();
            if (authType == "basic") headers = AuthorizationBasic(_SecretsElement);
            if (authType == "bearer") headers = await AuthorizationBearer(_SecretsElement, _JsonElement);
            if (authType == "api") headers = AuthorizationAPI(_SecretsElement);

            return headers;
        }

        private Dictionary<string, string> AuthorizationAPI(JsonElement secretsElement)
        {
            var apiKey = secretsElement.GetProperty(WebsiteName + "_api_key").GetString() ?? throw new Exception("Key: 'api_key', not found in Secrets.json!");
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

            var jsonToken = await RESTCommunicator.APIPost(url + "/api/tokens", jsonContent);
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
        private string GetAuthType()
        {
            var authType = _JsonElement.GetProperty("Authorization").GetString();
            return authType;
        }
        protected string GetUrl()
        {
            return _JsonElement.GetProperty("BaseUrl").GetString() + GetEndpoint();
        }
        protected List<APIValue> GetAPIValues()
        {
            List<APIValue> apiValues = [];

            // "APIValues" er en array af apivalues, som også indeholder en array af "names" / api værdier som ønskes
            JsonElement apiValueElements = _JsonElement.GetProperty("APIValues");
            if (apiValueElements.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement apiValue in apiValueElements.EnumerateArray())
                {
                    string displayName = apiValue.GetProperty("displayName").GetString() ?? "Name Not Found!";
                    List<string> names = [];
                    foreach (var name in apiValue.GetProperty("names").EnumerateArray())
                    {
                        names.Add(name.GetString());
                    }
                    apiValues.Add(new APIValue() { DisplayName = displayName, Names = names});
                }
            }
            return apiValues;
        }
        protected JsonElement GetJsonFile(string fileName)
        {
            // Get the path to the solution directory
            string solutionDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));

            // Combine the solution directory with the file name
            string filePath = Path.Combine(solutionDirectory, "SelfHostedSuperViser", "Files/" + fileName);
            StreamReader reader = new(filePath);
            var text = reader.ReadToEnd();

            return JsonDocument.Parse(text).RootElement;
        }
    }
}

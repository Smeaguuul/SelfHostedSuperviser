using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SelfHostedSuperViser.Model.APIGetter.APIGetter;

namespace SelfHostedSuperViser.Model.APIGetter
{
    internal class NginxProxyManager : Service
    {
        protected override string GetEndpoint()
        {
            return "/api/reports/hosts";
        }

        protected async override Task<Dictionary<string, string>> GetHeaders(JsonElement jsonElement)
        {
            var username = jsonElement.GetProperty("npm_usr").GetString();
            var password = jsonElement.GetProperty("npm_pwd").GetString();

            var jsonContent = new Dictionary<string, string>();
            jsonContent.Add("type", "password");
            jsonContent.Add("identity", username);
            jsonContent.Add("secret", password);

            var jsonToken = await APIGetter.APIGetter.APIPost("http://192.168.1.132:81/api/tokens", jsonContent);
            var token = jsonToken.RootElement.GetProperty("token");

            Dictionary<string, string> headers = [];
            headers.Add($"Authorization", "Bearer " + token);
            return headers;
        }

        protected override string GetWebsiteName()
        {
            return "NginxProxyManager";
        }
    }
}

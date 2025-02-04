using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SelfHostedSuperViser.Model.APIGetter
{
    internal class Traefik : Service
    {
        protected override string GetEndpoint()
        {
            return "/api/overview";
        }

        protected override Task<Dictionary<string, string>> GetHeaders(JsonElement jsonElement)
        {
            var password = jsonElement.GetProperty("traefik_pwd").GetString();
            var username = jsonElement.GetProperty("traefik_usr").GetString();


            var plainTextBytes = Encoding.UTF8.GetBytes(username + ":" + password);
            var authorization = Convert.ToBase64String(plainTextBytes);
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "Authorization", "Basic " + authorization }
            };

            return new Task<Dictionary<string, string>>(() => headers);
        }

        protected override string GetWebsiteName()
        {
            return "Traefik";
        }
    }
}

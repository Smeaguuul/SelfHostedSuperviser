using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SelfHostedSuperViser.Model.APIGetter.APIGetter;
using System.Text.Json.Nodes;
using System.Configuration;

namespace SelfHostedSuperViser.Model.APIGetter.AdguardHome
{
    public class AdguardHome : Service
    {
        protected override string GetEndpoint()
        {
            return "/control/stats";
        }

        protected async override Task<Dictionary<string, string>> GetHeaders(JsonElement jsonElement)
        {
            var password = jsonElement.GetProperty("adguardhome_pwd").GetString();
            var username = jsonElement.GetProperty("adguardhome_usr").GetString();
            

            var plainTextBytes = Encoding.UTF8.GetBytes(username + ":" + password);
            var authorization = Convert.ToBase64String(plainTextBytes);
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "Authorization", "Basic " + authorization }
            };

            return headers;
        }

        protected override string GetWebsiteName()
        {
            return "AdguardHome";
        }
    }
}

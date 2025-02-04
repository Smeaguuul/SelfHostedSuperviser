using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SelfHostedSuperViser.Model.APIGetter
{
    class Immich : Service
    {
        protected override string GetEndpoint()
        {
            return "/api/server/statistics";
        }

        protected async override Task<Dictionary<string, string>> GetHeaders(JsonElement jsonElement)
        {
            var apiKey = jsonElement.GetProperty("immich_api_ley").GetString() ?? throw new Exception("Key: 'immich_api_key', not found in Secrets.json!");
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "x-api-key", apiKey }
            };

            return headers;
        }

        protected override string GetWebsiteName()
        {
            return "Immich";
        }
    }
}

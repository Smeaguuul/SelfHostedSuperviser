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

namespace SelfHostedSuperViser.Model.APIGetter.AdguardHome
{
    public class AdguardHome : WebsiteAPIModel
    {
        public List<APIValue> CallAPI()
        {
            // Get the path to the solution directory
            string solutionDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));

            // Combine the solution directory with the file name
            string filePath = Path.Combine(solutionDirectory, "SelfHostedSuperViser", "APICalls.json");
            StreamReader reader = new(filePath);

            string text = reader.ReadToEnd();

            var jsonObject = JsonDocument.Parse(text).RootElement.GetProperty("AdguardHome");

            var password = jsonObject.GetProperty("Password").GetString();
            var username = jsonObject.GetProperty("UserName").GetString();
            var url = jsonObject.GetProperty("BaseUrl").GetString() + "/control/stats";

            var names = new string[]
            {
                "num_dns_queries",
                "num_blocked_filtering",
                "avg_processing_time",
            };
            var plainTextBytes = Encoding.UTF8.GetBytes(username + ":" + password);
            var authorization = Convert.ToBase64String(plainTextBytes);
            var headers = new Header[]
            {
            new()
            {
                Name = "Authorization",
                Value = "Basic " + authorization,
            }
            };
            var result = APIGetter.APIGetter.APIGet(url, names, headers).GetAwaiter().GetResult();

            return result;
        }
    }
}

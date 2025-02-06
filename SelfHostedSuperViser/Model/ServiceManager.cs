using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SelfHostedSuperViser.Model
{
    public class ServiceManager
    {
        private static ObservableCollection<Service> _Services = new ObservableCollection<Service>();

        public static ObservableCollection<Service> GetServices()
        {
            return _Services;
        }

        public static void AddService(string serviceName)
        {
            _Services.Add(new Service() { WebsiteName = serviceName });
        }

        public static void AddServiceJson(string websiteName, string? baseUrl, string? endpoint, string? authorization, string? protocol, string? password, string? user, string? aPIKey, List<APIValue>? aPIValues)
        {



            string newJson = GetAPIJson(websiteName, baseUrl, endpoint, authorization, aPIValues);
            string secretsJson = GetSecretJson(websiteName, authorization, password, user, aPIKey);

            // Finds the paths to both files
            string solutionDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));
            string apiPath = Path.Combine(solutionDirectory, "SelfHostedSuperViser", "Files/" + "APICalls.json");
            string secretsPath = Path.Combine(solutionDirectory, "SelfHostedSuperViser", "Files/" + "Secrets.json");

            File.AppendAllText(apiPath, "," + Environment.NewLine + newJson);
            File.AppendAllText(secretsPath, "," + Environment.NewLine + secretsJson);
        }

        private static string GetSecretJson(string? websiteName, string? authorization, string? password, string? user, string? aPIKey)
        {            
            string solutionDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));

            string filePath = Path.Combine(solutionDirectory, "SelfHostedSuperViser", "Files/" + "APICalls.json");
            StreamReader reader = new(filePath);
            var text = reader.ReadToEnd();

            var doc = JsonDocument.Parse(text).RootElement;

            var options = new JsonWriterOptions { Indented = true };
            var stream = new MemoryStream();
            var writer = new Utf8JsonWriter(stream, options);

            // Adds the existing json.
            writer.WriteStartObject();
            foreach (var property in doc.EnumerateObject())
            {
                property.WriteTo(writer);
            }

            // Adds the new object
            if (authorization.Equals("api")) writer.WriteString(websiteName + "_api_key", aPIKey);
            if (authorization.Equals("basic") || authorization.Equals("bearer"))
            {
                writer.WriteString(websiteName + "_usr", user);
                writer.WriteString(websiteName + "_pwd", password);
            };
            if (authorization != "bearer")
            {
                writer.WriteString(websiteName + "_token", "");
                writer.WriteString(websiteName + "_token_date", "");
            }
            writer.WriteEndObject();
            writer.Flush();

            return Encoding.UTF8.GetString(stream.ToArray());
        }

        private static string GetAPIJson(string websiteName, string? baseUrl, string? endpoint, string? authorization, List<APIValue>? aPIValues)
        {
            string solutionDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));

            string filePath = Path.Combine(solutionDirectory, "SelfHostedSuperViser", "Files/" + "Secrets.json");
            StreamReader reader = new(filePath);
            var text = reader.ReadToEnd();

            var doc = JsonDocument.Parse(text).RootElement;

            var options = new JsonWriterOptions { Indented = true };
            var stream = new MemoryStream();
            var writer = new Utf8JsonWriter(stream, options);

            // Adds the existing json.
            writer.WriteStartObject();
            foreach (var property in doc.EnumerateObject())
            {
                property.WriteTo(writer);
            }

            writer.WriteStartObject(websiteName);
            writer.WriteString("BaseUrl", baseUrl);
            writer.WriteString("Endpoint", endpoint);
            writer.WriteString("Authorization", authorization);
            writer.WriteStartArray("APIValues");
            aPIValues.ForEach(apiValue =>
            {
                writer.WriteStartObject();
                writer.WriteStartArray("Photos");
                apiValue.Names.ForEach(name => writer.WriteStringValue(name));
                writer.WriteString("displayName", apiValue.DisplayName);
                writer.WriteEndObject();
            });
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.WriteEndObject();
            writer.Flush();

            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}

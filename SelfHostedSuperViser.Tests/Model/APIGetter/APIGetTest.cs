using System.Xml.Linq;
using SelfHostedSuperViser.Model.APIGetter.APIGetter;

namespace SelfHostedSuperViser.Tests.Model;

[TestClass]
public class APIGetTest
{
    [TestMethod]
    public async Task APIGet01Async()
    {
        //Arrange
        var Url = "http://192.168.1.1:3000/control/stats";
        var names = new List<List<string>>()
        {
            new List<string>() { "num_dns_queries" },
            new List<string>() { "num_blocked_filtering" },
            new List<string>() { "avg_processing_time" },
        };
        var headers = new Dictionary<string, string>
        {
            { "Name", "Authorization" },
            { "Value", "Basic Y29vbXNoYWNrOlNtaWtOb3RTaW1w" }
        };
     

        // Act
        var APIValues = await APIGetter.APIGet(Url, names, headers);

        // Assert
        Assert.AreEqual<int>(APIValues.Count, names.Count);
        Assert.IsTrue(int.Parse(APIValues[0].Value) > 100000);
    }

    [TestMethod]
    public async Task APIGet02Async()
    {
        //Arrange
        var Url = "http://192.168.1.1:3000/control/stats";
        var names = new List<List<string>>()
        {
            new List<string>() { "num_dns_queries" },
            new List<string>() { "num_blocked_filtering" },
            new List<string>() { "avg_processing_time" },
            new List<string>() { "Non_existent_Property" },
        };
        var headers = new Dictionary<string, string>
        {
            { "Name", "Authorization" },
            { "Value", "Basic Y29vbXNoYWNrOlNtaWtOb3RTaW1w" }
        };

        // Act
        //await APIGetter.APIGet(Url, names, headers);
        // Assert
        var Exception = await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
        {
            await APIGetter.APIGet(Url, names, headers);
        });
        Assert.AreEqual(Exception.Message, "API property Non_existent_Property, doesn't exist!");
    }
}

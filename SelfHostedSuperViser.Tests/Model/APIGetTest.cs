using System.Xml.Linq;
using SelfHostedSuperViser.Model;

namespace SelfHostedSuperViser.Tests.Model;

[TestClass]
public class APIGetTest
{
    [TestMethod]
    public async Task APIGet01Async()
    {
        //Arrange
        var Url = "http://192.168.1.1:3000/control/stats";
        var names = new string[]
        {
            "num_dns_queries",
            "num_blocked_filtering",
            "avg_processing_time"
        };
        var headers = new Header[]
        {
            new()
            {
                Name = "Authorization",
                Value = "Basic Y29vbXNoYWNrOlNtaWtOb3RTaW1w",
            }
        };

        // Act
        var APIValues = await APIGetter.APIGet(Url, names, headers);

        // Assert
        Assert.AreEqual<int>(APIValues.Count, names.Length);
        Assert.IsTrue(int.Parse(APIValues[0].Value) > 100000);
    }

    [TestMethod]
    public async Task APIGet02Async()
    {
        //Arrange
        var Url = "http://192.168.1.1:3000/control/stats";
        var names = new string[]
        {
            "num_dns_queries",
            "num_blocked_filtering",
            "avg_processing_time",
            "Non_existent_Property",
        };
        var headers = new Header[]
        {
            new()
            {
                Name = "Authorization",
                Value = "Basic Y29vbXNoYWNrOlNtaWtOb3RTaW1w",
            }
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

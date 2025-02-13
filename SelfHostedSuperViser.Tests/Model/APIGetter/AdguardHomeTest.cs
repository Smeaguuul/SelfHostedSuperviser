using SelfHostedSuperViser.Model;

namespace SelfHostedSuperViser.Tests;

[TestClass]
public class AdguardHomeTest
{
    [TestMethod]
    public async Task TestMethod1()
    {
        // Arrange

        //Act
        var result = await new Service() { WebsiteName = "AdguardHome"}.CallAPIAsync();

        //Assert
        Assert.IsNotNull(result);
        //Check that all values are present
        Assert.AreEqual(result[0].Name, "num_dns_queries");
        Assert.AreEqual(result[1].Name, "num_blocked_filtering");
        Assert.AreEqual(result[2].Name, "avg_processing_time");

        //Check that a value has been returned
        Assert.IsNotNull(result[0].Value);
        Assert.IsTrue(result[0].Value.Length != 0);
    }
}

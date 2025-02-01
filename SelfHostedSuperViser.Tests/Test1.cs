namespace SelfHostedSuperViser.Tests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var expected = 5;
            var actual = 2 + 3;

            // Act

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

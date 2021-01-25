using NUnit.Framework;
using SaintSender.Core.Services;

namespace SaintSender.Core.Tests
{
    [TestFixture]
    public class GreetServiceTests
    {
        [Test]
        public void Greet_NameAdded_ReturnGreetings()
        {
            // Arrange
            var service = new GreetService();
            // Act
            var greeting = service.Greet(".NET Padawan");
            // Assert
            Assert.AreEqual("Welcome .NET Padawan, my friend!", greeting);
        }
    }
}

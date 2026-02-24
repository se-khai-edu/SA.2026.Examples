namespace Ping.Core.Tests
{
    /// <summary>
    /// Unit tests for the PingService logic.
    /// </summary>
    public class PingServiceTests
    {
        private readonly PingService pinger;

        public PingServiceTests()
        {
            // Initialize the service before each test run
            pinger = new PingService();
        }

        /// <summary>
        /// Tests that valid and popular hosts return an 'Online' status.
        /// [Theory] allows passing multiple data sets to the same test logic.
        /// </summary>
        /// <param name="validHost">The hostname or IP to be tested.</param>
        [Theory]
        [InlineData("one.one.one.one")] // Cloudflare DNS
        [InlineData("127.0.0.1")]       // Loopback address
        [InlineData("localhost")]       // Local host name
        [InlineData("8.8.8.8")]         // Google Public DNS
        [InlineData("google.com.ua")]   // Common .ua domain
        public void PingValidOnline(string? validHost)
        {
            // Arrange
            // validHost = ...;

            // Act
            var result = pinger.ExecutePing(validHost!);

            // Assert
            Assert.Equal(HostStatus.Online, result.Status);
            Assert.Equal(validHost, result.Address);
            Assert.True(result.RoundtripTime >= 0, "Roundtrip time should be non-negative");
        }

        /// <summary>
        /// Validates the behavior when a host is valid but does not respond.
        /// </summary>
        [Fact]
        public void PingOfflineHost()
        {
            // Arrange
            // Using a specific IP address likely to be offline in a local network context
            // (smartphone, for example:  WiFi - On/Off)
            string offlineHost = "192.168.31.120"; 

            // Act
            var result = pinger.ExecutePing(offlineHost);

            // Assert
            // Most environments return Error or Offline for non-resolvable hosts
            Assert.True(result.Status == HostStatus.Offline);
        }

        /// <summary>
        /// Validates error handling for null, empty, or syntactically invalid addresses.
        /// Ensures the system doesn't crash but returns an 'Error' status instead.
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        [InlineData("invalid.address.that.does.not.exist")]
        public void PingInvalidAddress(string? address)
        {
            // Arrange
            // address = ...;

            // Act
            var result = pinger.ExecutePing(address!);

            // Assert
            Assert.Equal(HostStatus.Error, result.Status);
        }
    }
}

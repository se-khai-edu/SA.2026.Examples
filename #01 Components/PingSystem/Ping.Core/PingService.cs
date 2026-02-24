using System.Net.NetworkInformation;

namespace Ping.Core
{
    /// <summary>
    /// Implementation of IPingService using the standard System.Net.NetworkInformation.Ping class.
    /// </summary>
    public class PingService : IPingService
    {
        public PingResult ExecutePing(string address, int timeout = 1000)
        {
            // Validation: Ensure the address is not empty
            if (string.IsNullOrWhiteSpace(address))
                return new PingResult(address, HostStatus.Error, 0);

            // Disposable pattern: Ensure the underlying system resources are released
            using (var pinger = new System.Net.NetworkInformation.Ping())
            {
                try
                {
                    // Sending a synchronous ICMP echo request
                    var reply = pinger.Send(address, timeout);
                    // Mapping System.Net.NetworkInformation.IPStatus to our custom HostStatus
                    return reply.Status == IPStatus.Success
                        ? new PingResult(address, HostStatus.Online, reply.RoundtripTime)
                        : new PingResult(address, HostStatus.Offline, 0);
                }
                catch (PingException)
                {
                    // Handle cases like "No such host is known" or network adapter issues
                    return new PingResult(address, HostStatus.Error, 0);
                }
                catch (Exception)
                {
                    // Catch-all for any other unexpected runtime issues
                    return new PingResult(address, HostStatus.Error, 0);
                }
            }
        }
    }
}

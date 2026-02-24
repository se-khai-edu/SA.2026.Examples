namespace Ping.Core
{
    /// <summary>
    /// Defines a contract for a service that performs network ping operations.
    /// </summary>
    public interface IPingService
    {
        /// <summary>
        /// Executes a ping request to the specified address.
        /// </summary>
        /// <param name="address">The destination host name or IP address.</param>
        /// <param name="timeout">The maximum time in milliseconds to wait for a reply.</param>
        /// <returns>A PingResult object containing the status and latency.</returns>
        PingResult ExecutePing(string address, int timeout = 1000);
    }
}

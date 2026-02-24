namespace Ping.Core
{
    /// <summary>
    /// Represents the immutable result of a single ping operation.
    /// </summary>
    /// <param name="Address">The IP address or DNS name of the host.</param>
    /// <param name="Status">The calculated status of the host.</param>
    /// <param name="RoundtripTime">Time taken for the round trip in milliseconds.</param>
    public record PingResult(
            string Address,
            HostStatus Status,
            long RoundtripTime
        );
}
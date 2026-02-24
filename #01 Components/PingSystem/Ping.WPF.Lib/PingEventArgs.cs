using Ping.Core;

namespace Ping.WPF.Lib
{
    /// <summary>
    /// Provides detailed information for the PingChangeState event.
    /// Used to notify the consumer when a host's status changes.
    /// </summary>
    /// <param name="prev">The status before the change.</param>
    /// <param name="replay">The current ping result containing the new status.</param>
    public class PingEventArgs(HostStatus prev, PingResult replay) : EventArgs
    {
        /// <summary> Gets the precise time when the status update occurred. </summary>
        public DateTime Timestamp { get; } = DateTime.Now;
        /// <summary> Gets the current ping result (contains Address, Status, and Latency). </summary>
        public PingResult Replay { get; } = replay;
        /// <summary> Gets the previous status for delta comparison. </summary>
        public HostStatus OldStatus { get; } = prev;
        /// <summary>
        /// Provides a human-readable summary of the status change.
        /// Useful for logging or debugging.
        /// </summary>
        public override string ToString() =>
            $"{Timestamp:HH:mm:ss.ff} : {Replay.Address} ({OldStatus} → {Replay.Status})";
    }
}


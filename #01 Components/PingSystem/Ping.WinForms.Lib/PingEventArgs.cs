using Ping.Core;

namespace Ping.WinForms.Lib
{
    /// <summary>
    /// Provides data for the PingChangeState event, 
    /// allowing subscribers to track host availability changes.
    /// </summary>
    public class PingEventArgs(HostStatus prev, PingResult replay) : EventArgs
    {
        /// <summary> Gets the exact time when the status change was detected. </summary>
        public DateTime Timestamp { get; } = DateTime.Now;
        /// <summary> Gets the latest ping operation result (latency and current status). </summary>
        public PingResult Replay { get; } = replay;
        /// <summary> Gets the status of the host before this update. </summary>
        public HostStatus OldStatus { get; } = prev;
        /// <summary>
        /// Returns a formatted string representation of the event for easy logging.
        /// </summary>
        public override string ToString() =>
            $"{Timestamp:HH:mm:ss.ff} : {Replay.Address} ({OldStatus} → {Replay.Status})";
    }
}

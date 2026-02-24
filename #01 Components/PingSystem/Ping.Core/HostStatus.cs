namespace Ping.Core
{
    /// <summary>
    /// Defines the possible states of a monitored host.
    /// </summary>
    public enum HostStatus
    {
        /// <summary> Default state before any action is taken (Dark Gray). </summary>
        Null,
        /// <summary> The monitoring is stopped or the state is undefined (Gray). </summary>
        Unknown,
        /// <summary> The host responded successfully (Green). </summary>
        Online,
        /// <summary> The host is reachable but failed to respond to the ping (Yellow). </summary>
        Offline,
        /// <summary> A network error occurred or the host is completely unreachable (Red). </summary>
        Error
    }
}
using Ping.Core;
using System.ComponentModel;

namespace Ping.WinForms.Lib
{
/// <summary>
    /// A custom WinForms control that monitors host availability using ICMP Ping.
    /// Displays results in a ListView and provides real-time status updates.
    /// </summary>
    [DefaultEvent("PingChangeState")]
    [ToolboxItem(true), ToolboxBitmap(typeof(PingControl), "PingControl.ico")]
    public partial class PingControl : UserControl
    {
        IPingService? pingService = new PingService();
        // Using BindingList to automatically detect when hosts are added or removed
        private BindingList<string> hostnames = [];

        // Cache to track previous statuses and detect changes
        private Dictionary<string, HostStatus> hostStatuses = new();

        /// <summary>
        /// Occurs when a host's reachability status changes (e.g., from Online to Offline).
        /// </summary>
        public event EventHandler<PingEventArgs>? PingChangeState;

        /// <summary>
        /// Gets the collection of host addresses to monitor.
        /// Changes to this list automatically update the UI.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BindingList<string> HostNames
        {
            get => hostnames;
        }

        public PingControl()
        {
            InitializeComponent();
            // Subscribe to list changes to keep the ListView synchronized
            hostnames.ListChanged += HostListChanged;
        }

        /// <summary>
        /// Handles synchronization between the HostNames data collection and the ListView UI.
        /// </summary>
        private void HostListChanged(object? sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    lvHosts.Items.Clear();
                    hostStatuses.Clear();
                    return;
                case ListChangedType.ItemAdded:
                    var item = new ListViewItem(new[] { hostnames[e.NewIndex], "n/a" })
                    { StateImageIndex = (int)HostStatus.Null };
                    hostStatuses.Add(hostnames[e.NewIndex], HostStatus.Null);
                    lvHosts.Items.Insert(e.NewIndex, item);
                    return;
                case ListChangedType.ItemDeleted:
                    lvHosts.Items.RemoveAt(e.OldIndex);
                    hostStatuses.Remove(hostnames[e.OldIndex]);
                    return;
            }
        }

        /// <summary>
        /// Dynamically resizes the 'Host' column to fill the available space.
        /// </summary>
        private void PingControl_ClientSizeChanged(object sender, EventArgs e)
        {
            chHost.Width = Width - (chRountrip.Width + 4);
        }

        private void OnRunCheckedChanged(object sender, EventArgs e)
        {
            if (cbRun.Checked)
                timer.Start();
        }

        /// <summary>
        /// Main monitoring loop. Executes pings and updates UI elements.
        /// </summary>
        private void OnTimerTick(object sender, EventArgs e)
        {
            if (!cbRun.Checked)
                timer.Stop();

            if (pingService is null)
                return;

            for (int i = 0; i < hostnames.Count; i++)
            {
                // Execute ping if running, otherwise reset to Unknown
                var status = (cbRun.Checked) ? 
                        pingService.ExecutePing(hostnames[i], 1000) : 
                        new PingResult(hostnames[i], HostStatus.Unknown, 0) ;

                var item = lvHosts.Items[i];
                // Update latency text (Column 2)
                item.SubItems[1].Text = $"{status.RoundtripTime}";
                // Update status icon index (Maps directly to HostStatus enum values)
                item.StateImageIndex = (int)status.Status;

                // Event Triggering: Check if status has changed since last tick
                if (hostStatuses[hostnames[i]] != status.Status)
                {
                    PingChangeState?.Invoke(this,
                        new PingEventArgs(hostStatuses[hostnames[i]], status));
                    // Update cache with the new status
                    hostStatuses[hostnames[i]] = status.Status;
                }
            }
        }

    }
}


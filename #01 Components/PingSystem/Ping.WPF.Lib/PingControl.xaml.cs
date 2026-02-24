using Ping.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Ping.WPF.Lib
{
    /// <summary>
    /// Logic for the WPF PingControl. 
    /// Manages the background pinging cycle and synchronization with the UI.
    /// </summary>
    [DefaultEvent("PingChangeState")]
    [ToolboxItem(true), ToolboxBitmap(typeof(PingControl), "PingControl.ico")]
    public partial class PingControl : UserControl
    {
        private readonly IPingService pingService = new PingService();
        private readonly DispatcherTimer timer;

        /// <summary> Internal collection of ViewModels for the ListView. </summary>
        public ObservableCollection<HostItemViewModel> HostViewModels { get; } = new();

        /// <summary>
        /// DependencyProperty for HostNames, allowing it to be used with WPF Data Binding.
        /// </summary>
        public static readonly DependencyProperty HostNamesProperty =
            DependencyProperty.Register(nameof(HostNames), typeof(ObservableCollection<string>), typeof(PingControl),
                new PropertyMetadata(null, OnHostNamesChanged));

        /// <summary> The collection of string hostnames to monitor. </summary>
        public ObservableCollection<string> HostNames
        {
            get => (ObservableCollection<string>)GetValue(HostNamesProperty);
            set => SetValue(HostNamesProperty, value);
        }

        /// <summary> Occurs when a host changes its availability status. </summary>
        public event EventHandler<PingEventArgs>? PingChangeState;

        public PingControl()
        {
            InitializeComponent();

            // Setup the background timer (runs on the UI thread for easy updates)
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000) };
            timer.Tick += OnTimerTick;

            // Handle start/stop logic for the monitoring process
            cbRun.Checked += (s, e) => timer.Start();
            cbRun.Unchecked += (s, e) =>
            {
                // When stopped, reset all hosts to 'Unknown' status
                foreach (var vm in HostViewModels)
                {
                    var result = new PingResult(vm.Address, HostStatus.Unknown, -1);
                    UpdateListView(vm, result);
                }
                timer.Stop();
            };
        }

        /// <summary> Updates the ViewModel and triggers external events if the status changed. </summary>
        private void UpdateListView(HostItemViewModel vm, PingResult result)
        {
            if (vm.Status != result.Status)
                PingChangeState?.Invoke(this, new PingEventArgs(vm.Status, result));
            vm.Status = result.Status;
            vm.RoundtripTime = result.RoundtripTime;
        }

        /// <summary> Callback for when the HostNames collection is replaced or initially set. </summary>
        private static void OnHostNamesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PingControl)d;
            control.SyncViewModels(); 
        }

        /// <summary> Synchronizes the string collection with the internal ViewModels. </summary>
        private void SyncViewModels()
        {
            HostViewModels.Clear();
            if (HostNames != null)
            {
                foreach (var name in HostNames)
                    HostViewModels.Add(new HostItemViewModel { Address = name });
            }
        }

        /// <summary> Periodic execution of the ping operation for each host. </summary>
        private void OnTimerTick(object? sender, EventArgs e)
        {
            foreach (var vm in HostViewModels)
            {
                var result = pingService.ExecutePing(vm.Address, 1000);
                UpdateListView(vm, result);
            }
        }
    }
}
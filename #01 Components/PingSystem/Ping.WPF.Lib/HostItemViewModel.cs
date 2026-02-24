using Ping.Core;
using System.ComponentModel;

namespace Ping.WPF.Lib
{
    /// <summary>
    /// Represents the View Model for a single row in the hosts list.
    /// Implements INotifyPropertyChanged to enable real-time 
    /// UI updates via Data Binding.
    /// </summary>
    public class HostItemViewModel : INotifyPropertyChanged
    {
        private HostStatus _status = HostStatus.Null;
        private long _roundtripTime;
        /// <summary> The DNS name or IP address of the host. </summary>
        public string Address { get; set; } = string.Empty;
        /// <summary> Current availability status. Updates the UI via PropertyChanged. </summary>
        public HostStatus Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }
        /// <summary> Network latency in milliseconds. Updates the UI via PropertyChanged. </summary>
        public long RoundtripTime
        {
            get => _roundtripTime;
            set { _roundtripTime = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        /// <summary>
        /// Helper method to raise the PropertyChanged event.
        /// [CallerMemberName] automatically provides the property name.
        /// </summary>
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null!)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

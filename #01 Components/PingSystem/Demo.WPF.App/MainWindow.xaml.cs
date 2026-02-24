using Ping.WPF.Lib;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo.WPF.App
{
    /// <summary>
    /// Interaction logic for the Demo application's Main Window.
    /// Demonstrates how to host and interact with the PingControl.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// A collection of hostnames to monitor. 
        /// Using ObservableCollection ensures the UI stays 
        /// in sync if items are added/removed.
        /// </summary>
        public ObservableCollection<string> Hosts { get; set; } = new();

        public MainWindow()
        {
            this.DataContext = this;

            Hosts.Add("127.0.0.1");
            Hosts.Add("localhost");
            Hosts.Add("google.com.ua");
            Hosts.Add("one.one.one.one");
            Hosts.Add("192.168.31.120");
            //Hosts.Add("Dummy");

            InitializeComponent();
        }

        /// <summary>
        /// Event handler that responds to status changes reported by the PingControl.
        /// Demonstrates how to consume custom events and display them in a Log.
        /// </summary>
        /// <param name="sender">The PingControl instance.</param>
        /// <param name="e">Event arguments containing status details and timestamp.</param>
        private void OnPingChangeState(object sender, PingEventArgs e)
        {
            lbLog.Items.Add(e.ToString());
        }
    }
}
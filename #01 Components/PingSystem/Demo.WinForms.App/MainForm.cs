using Ping.WinForms.Lib;

namespace Demo.WinForms.App
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void OnPingChangeState(object sender, PingEventArgs e)
        {
            tbLog.AppendText(e.ToString() + Environment.NewLine);
        }
    }
}

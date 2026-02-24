namespace Demo.WinForms.App
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pingControl1 = new Ping.WinForms.Lib.PingControl();
            tbLog = new TextBox();
            SuspendLayout();
            // 
            // pingControl1
            // 
            pingControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pingControl1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pingControl1.HostNames.Add("127.0.0.1");
            pingControl1.HostNames.Add("localhost");
            pingControl1.HostNames.Add("google.com.ua");
            pingControl1.HostNames.Add("one.one.one.one");
            pingControl1.HostNames.Add("192.168.31.120");
            pingControl1.Location = new Point(12, 12);
            pingControl1.Name = "pingControl1";
            pingControl1.Size = new Size(186, 212);
            pingControl1.TabIndex = 0;
            pingControl1.PingChangeState += OnPingChangeState;
            // 
            // tbLog
            // 
            tbLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tbLog.Location = new Point(204, 12);
            tbLog.Multiline = true;
            tbLog.Name = "tbLog";
            tbLog.Size = new Size(311, 212);
            tbLog.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(524, 232);
            Controls.Add(tbLog);
            Controls.Add(pingControl1);
            Name = "MainForm";
            Text = "Main Form";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Ping.WinForms.Lib.PingControl pingControl1;
        private TextBox tbLog;
    }
}

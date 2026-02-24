namespace Ping.WinForms.Lib
{
    partial class PingControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PingControl));
            lvHosts = new ListView();
            chHost = new ColumnHeader();
            chRountrip = new ColumnHeader();
            leds = new ImageList(components);
            cbRun = new CheckBox();
            timer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // lvHosts
            // 
            lvHosts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvHosts.Columns.AddRange(new ColumnHeader[] { chHost, chRountrip });
            lvHosts.FullRowSelect = true;
            lvHosts.GridLines = true;
            lvHosts.HideSelection = true;
            lvHosts.LabelWrap = false;
            lvHosts.Location = new Point(0, 0);
            lvHosts.MultiSelect = false;
            lvHosts.Name = "lvHosts";
            lvHosts.ShowGroups = false;
            lvHosts.Size = new Size(240, 127);
            lvHosts.StateImageList = leds;
            lvHosts.TabIndex = 0;
            lvHosts.UseCompatibleStateImageBehavior = false;
            lvHosts.View = View.Details;
            // 
            // chHost
            // 
            chHost.Text = "Host";
            chHost.Width = 196;
            // 
            // chRountrip
            // 
            chRountrip.Text = "ms";
            chRountrip.TextAlign = HorizontalAlignment.Right;
            chRountrip.Width = 40;
            // 
            // leds
            // 
            leds.ColorDepth = ColorDepth.Depth32Bit;
            leds.ImageStream = (ImageListStreamer)resources.GetObject("leds.ImageStream");
            leds.TransparentColor = Color.Transparent;
            leds.Images.SetKeyName(0, "DarkGray 16x16.png");
            leds.Images.SetKeyName(1, "Gray 16x16.png");
            leds.Images.SetKeyName(2, "Green 16x16.png");
            leds.Images.SetKeyName(3, "Yellow 16x16.png");
            leds.Images.SetKeyName(4, "Red 16x16.png");
            // 
            // cbRun
            // 
            cbRun.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            cbRun.AutoSize = true;
            cbRun.Location = new Point(3, 133);
            cbRun.Name = "cbRun";
            cbRun.Size = new Size(81, 19);
            cbRun.TabIndex = 1;
            cbRun.Text = "Ping hosts";
            cbRun.UseVisualStyleBackColor = true;
            cbRun.CheckedChanged += OnRunCheckedChanged;
            // 
            // timer
            // 
            timer.Interval = 1000;
            timer.Tick += OnTimerTick;
            // 
            // PingControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(cbRun);
            Controls.Add(lvHosts);
            Name = "PingControl";
            Size = new Size(240, 158);
            ClientSizeChanged += PingControl_ClientSizeChanged;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView lvHosts;
        private ImageList leds;
        private ColumnHeader chHost;
        private ColumnHeader chRountrip;
        private CheckBox cbRun;
        private System.Windows.Forms.Timer timer;
    }
}


namespace DataFlow
{
    partial class FormMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GroupShow = new System.Windows.Forms.Label();
            this.RS232_Connect_btn = new System.Windows.Forms.Button();
            this.RepositoryView = new System.Windows.Forms.ListView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.PLC_Disconnect_btn = new System.Windows.Forms.Button();
            this.RS232_Disconnect_btn = new System.Windows.Forms.Button();
            this.PLC_Connect_btn = new System.Windows.Forms.Button();
            this.EthernetGroup = new System.Windows.Forms.GroupBox();
            this.Port_Setting = new System.Windows.Forms.TextBox();
            this.Port_Lbl = new System.Windows.Forms.Label();
            this.IP_Setting = new System.Windows.Forms.TextBox();
            this.IP_Lbl = new System.Windows.Forms.Label();
            this.RS232Group = new System.Windows.Forms.GroupBox();
            this.COM_Setting = new System.Windows.Forms.TextBox();
            this.COM_Lbl = new System.Windows.Forms.Label();
            this.DataGroup = new System.Windows.Forms.GroupBox();
            this.PLCStatusGroup = new System.Windows.Forms.GroupBox();
            this.PLC_Status_Log = new System.Windows.Forms.ListBox();
            this.RS232StatusGroup = new System.Windows.Forms.GroupBox();
            this.RS232_Status_Log = new System.Windows.Forms.ListBox();
            this.EthernetGroup.SuspendLayout();
            this.RS232Group.SuspendLayout();
            this.DataGroup.SuspendLayout();
            this.PLCStatusGroup.SuspendLayout();
            this.RS232StatusGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupShow
            // 
            this.GroupShow.AutoSize = true;
            this.GroupShow.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.GroupShow.Location = new System.Drawing.Point(11, 8);
            this.GroupShow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.GroupShow.Name = "GroupShow";
            this.GroupShow.Size = new System.Drawing.Size(0, 27);
            this.GroupShow.TabIndex = 5;
            // 
            // RS232_Connect_btn
            // 
            this.RS232_Connect_btn.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.RS232_Connect_btn.Location = new System.Drawing.Point(27, 144);
            this.RS232_Connect_btn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RS232_Connect_btn.Name = "RS232_Connect_btn";
            this.RS232_Connect_btn.Size = new System.Drawing.Size(103, 55);
            this.RS232_Connect_btn.TabIndex = 6;
            this.RS232_Connect_btn.Text = "Connect";
            this.RS232_Connect_btn.UseVisualStyleBackColor = true;
            this.RS232_Connect_btn.Click += new System.EventHandler(this.button1_Click);
            // 
            // RepositoryView
            // 
            this.RepositoryView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RepositoryView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RepositoryView.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.RepositoryView.FullRowSelect = true;
            this.RepositoryView.GridLines = true;
            this.RepositoryView.HideSelection = false;
            this.RepositoryView.Location = new System.Drawing.Point(4, 28);
            this.RepositoryView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RepositoryView.MultiSelect = false;
            this.RepositoryView.Name = "RepositoryView";
            this.RepositoryView.Size = new System.Drawing.Size(788, 1114);
            this.RepositoryView.TabIndex = 23;
            this.RepositoryView.UseCompatibleStateImageBehavior = false;
            this.RepositoryView.View = System.Windows.Forms.View.Details;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox1.Location = new System.Drawing.Point(1263, 1538);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(237, 31);
            this.textBox1.TabIndex = 24;
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox2.Location = new System.Drawing.Point(1263, 1608);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(237, 31);
            this.textBox2.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(480, 735);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 20);
            this.label1.TabIndex = 26;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(467, 765);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 20);
            this.label2.TabIndex = 27;
            this.label2.Text = "Check";
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button4.Location = new System.Drawing.Point(1533, 1541);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(227, 111);
            this.button4.TabIndex = 28;
            this.button4.Text = "Send";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // PLC_Disconnect_btn
            // 
            this.PLC_Disconnect_btn.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.PLC_Disconnect_btn.Location = new System.Drawing.Point(179, 144);
            this.PLC_Disconnect_btn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PLC_Disconnect_btn.Name = "PLC_Disconnect_btn";
            this.PLC_Disconnect_btn.Size = new System.Drawing.Size(103, 56);
            this.PLC_Disconnect_btn.TabIndex = 29;
            this.PLC_Disconnect_btn.Text = "Disconnect";
            this.PLC_Disconnect_btn.UseVisualStyleBackColor = true;
            this.PLC_Disconnect_btn.Click += new System.EventHandler(this.button5_Click);
            // 
            // RS232_Disconnect_btn
            // 
            this.RS232_Disconnect_btn.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.RS232_Disconnect_btn.Location = new System.Drawing.Point(137, 144);
            this.RS232_Disconnect_btn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RS232_Disconnect_btn.Name = "RS232_Disconnect_btn";
            this.RS232_Disconnect_btn.Size = new System.Drawing.Size(103, 55);
            this.RS232_Disconnect_btn.TabIndex = 31;
            this.RS232_Disconnect_btn.Text = "Disconnect";
            this.RS232_Disconnect_btn.UseVisualStyleBackColor = true;
            this.RS232_Disconnect_btn.Click += new System.EventHandler(this.button6_Click);
            // 
            // PLC_Connect_btn
            // 
            this.PLC_Connect_btn.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.PLC_Connect_btn.Location = new System.Drawing.Point(57, 142);
            this.PLC_Connect_btn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PLC_Connect_btn.Name = "PLC_Connect_btn";
            this.PLC_Connect_btn.Size = new System.Drawing.Size(103, 56);
            this.PLC_Connect_btn.TabIndex = 30;
            this.PLC_Connect_btn.Text = "Connect";
            this.PLC_Connect_btn.UseVisualStyleBackColor = true;
            this.PLC_Connect_btn.Click += new System.EventHandler(this.button7_Click);
            // 
            // EthernetGroup
            // 
            this.EthernetGroup.Controls.Add(this.Port_Setting);
            this.EthernetGroup.Controls.Add(this.Port_Lbl);
            this.EthernetGroup.Controls.Add(this.IP_Setting);
            this.EthernetGroup.Controls.Add(this.IP_Lbl);
            this.EthernetGroup.Controls.Add(this.PLC_Connect_btn);
            this.EthernetGroup.Controls.Add(this.PLC_Disconnect_btn);
            this.EthernetGroup.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.EthernetGroup.Location = new System.Drawing.Point(36, 22);
            this.EthernetGroup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EthernetGroup.Name = "EthernetGroup";
            this.EthernetGroup.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EthernetGroup.Size = new System.Drawing.Size(405, 232);
            this.EthernetGroup.TabIndex = 32;
            this.EthernetGroup.TabStop = false;
            this.EthernetGroup.Text = "PLC";
            // 
            // Port_Setting
            // 
            this.Port_Setting.Location = new System.Drawing.Point(57, 91);
            this.Port_Setting.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Port_Setting.Name = "Port_Setting";
            this.Port_Setting.Size = new System.Drawing.Size(223, 31);
            this.Port_Setting.TabIndex = 36;
            this.Port_Setting.Text = "1000";
            // 
            // Port_Lbl
            // 
            this.Port_Lbl.AutoSize = true;
            this.Port_Lbl.Location = new System.Drawing.Point(5, 105);
            this.Port_Lbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Port_Lbl.Name = "Port_Lbl";
            this.Port_Lbl.Size = new System.Drawing.Size(39, 20);
            this.Port_Lbl.TabIndex = 35;
            this.Port_Lbl.Text = "Port";
            // 
            // IP_Setting
            // 
            this.IP_Setting.Location = new System.Drawing.Point(57, 49);
            this.IP_Setting.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.IP_Setting.Name = "IP_Setting";
            this.IP_Setting.Size = new System.Drawing.Size(223, 31);
            this.IP_Setting.TabIndex = 34;
            this.IP_Setting.Text = "192.168.11.245";
            // 
            // IP_Lbl
            // 
            this.IP_Lbl.AutoSize = true;
            this.IP_Lbl.Location = new System.Drawing.Point(21, 54);
            this.IP_Lbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.IP_Lbl.Name = "IP_Lbl";
            this.IP_Lbl.Size = new System.Drawing.Size(25, 20);
            this.IP_Lbl.TabIndex = 34;
            this.IP_Lbl.Text = "IP";
            // 
            // RS232Group
            // 
            this.RS232Group.Controls.Add(this.COM_Setting);
            this.RS232Group.Controls.Add(this.COM_Lbl);
            this.RS232Group.Controls.Add(this.RS232_Connect_btn);
            this.RS232Group.Controls.Add(this.RS232_Disconnect_btn);
            this.RS232Group.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.RS232Group.Location = new System.Drawing.Point(501, 22);
            this.RS232Group.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RS232Group.Name = "RS232Group";
            this.RS232Group.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RS232Group.Size = new System.Drawing.Size(331, 232);
            this.RS232Group.TabIndex = 33;
            this.RS232Group.TabStop = false;
            this.RS232Group.Text = "RS232";
            // 
            // COM_Setting
            // 
            this.COM_Setting.Location = new System.Drawing.Point(63, 91);
            this.COM_Setting.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.COM_Setting.Name = "COM_Setting";
            this.COM_Setting.Size = new System.Drawing.Size(176, 31);
            this.COM_Setting.TabIndex = 36;
            this.COM_Setting.Text = "3";
            // 
            // COM_Lbl
            // 
            this.COM_Lbl.AutoSize = true;
            this.COM_Lbl.Location = new System.Drawing.Point(8, 95);
            this.COM_Lbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.COM_Lbl.Name = "COM_Lbl";
            this.COM_Lbl.Size = new System.Drawing.Size(53, 20);
            this.COM_Lbl.TabIndex = 35;
            this.COM_Lbl.Text = "COM";
            // 
            // DataGroup
            // 
            this.DataGroup.Controls.Add(this.RepositoryView);
            this.DataGroup.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.DataGroup.Location = new System.Drawing.Point(36, 262);
            this.DataGroup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DataGroup.Name = "DataGroup";
            this.DataGroup.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DataGroup.Size = new System.Drawing.Size(796, 1146);
            this.DataGroup.TabIndex = 34;
            this.DataGroup.TabStop = false;
            this.DataGroup.Text = "Data";
            // 
            // PLCStatusGroup
            // 
            this.PLCStatusGroup.Controls.Add(this.PLC_Status_Log);
            this.PLCStatusGroup.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.PLCStatusGroup.Location = new System.Drawing.Point(2112, 16);
            this.PLCStatusGroup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PLCStatusGroup.Name = "PLCStatusGroup";
            this.PLCStatusGroup.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PLCStatusGroup.Size = new System.Drawing.Size(1159, 706);
            this.PLCStatusGroup.TabIndex = 37;
            this.PLCStatusGroup.TabStop = false;
            this.PLCStatusGroup.Text = "PLC_Status";
            // 
            // PLC_Status_Log
            // 
            this.PLC_Status_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLC_Status_Log.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.PLC_Status_Log.FormattingEnabled = true;
            this.PLC_Status_Log.HorizontalScrollbar = true;
            this.PLC_Status_Log.ItemHeight = 20;
            this.PLC_Status_Log.Location = new System.Drawing.Point(4, 28);
            this.PLC_Status_Log.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PLC_Status_Log.Name = "PLC_Status_Log";
            this.PLC_Status_Log.ScrollAlwaysVisible = true;
            this.PLC_Status_Log.Size = new System.Drawing.Size(1151, 674);
            this.PLC_Status_Log.TabIndex = 20;
            // 
            // RS232StatusGroup
            // 
            this.RS232StatusGroup.Controls.Add(this.RS232_Status_Log);
            this.RS232StatusGroup.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.RS232StatusGroup.Location = new System.Drawing.Point(2088, 738);
            this.RS232StatusGroup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RS232StatusGroup.Name = "RS232StatusGroup";
            this.RS232StatusGroup.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RS232StatusGroup.Size = new System.Drawing.Size(1161, 756);
            this.RS232StatusGroup.TabIndex = 38;
            this.RS232StatusGroup.TabStop = false;
            this.RS232StatusGroup.Text = "RS232_Status";
            // 
            // RS232_Status_Log
            // 
            this.RS232_Status_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RS232_Status_Log.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.RS232_Status_Log.FormattingEnabled = true;
            this.RS232_Status_Log.HorizontalScrollbar = true;
            this.RS232_Status_Log.ItemHeight = 20;
            this.RS232_Status_Log.Location = new System.Drawing.Point(4, 28);
            this.RS232_Status_Log.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RS232_Status_Log.Name = "RS232_Status_Log";
            this.RS232_Status_Log.ScrollAlwaysVisible = true;
            this.RS232_Status_Log.Size = new System.Drawing.Size(1153, 724);
            this.RS232_Status_Log.TabIndex = 21;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1775, 987);
            this.Controls.Add(this.RS232StatusGroup);
            this.Controls.Add(this.PLCStatusGroup);
            this.Controls.Add(this.DataGroup);
            this.Controls.Add(this.RS232Group);
            this.Controls.Add(this.EthernetGroup);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.GroupShow);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormMain";
            this.Text = "DataFlow";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.EthernetGroup.ResumeLayout(false);
            this.EthernetGroup.PerformLayout();
            this.RS232Group.ResumeLayout(false);
            this.RS232Group.PerformLayout();
            this.DataGroup.ResumeLayout(false);
            this.PLCStatusGroup.ResumeLayout(false);
            this.RS232StatusGroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label GroupShow;
        private System.Windows.Forms.Button RS232_Connect_btn;
        private System.Windows.Forms.ListView RepositoryView;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button PLC_Disconnect_btn;
        private System.Windows.Forms.Button RS232_Disconnect_btn;
        private System.Windows.Forms.Button PLC_Connect_btn;
        private System.Windows.Forms.GroupBox EthernetGroup;
        private System.Windows.Forms.Label IP_Lbl;
        private System.Windows.Forms.GroupBox RS232Group;
        private System.Windows.Forms.TextBox IP_Setting;
        private System.Windows.Forms.Label Port_Lbl;
        private System.Windows.Forms.TextBox Port_Setting;
        private System.Windows.Forms.TextBox COM_Setting;
        private System.Windows.Forms.Label COM_Lbl;
        private System.Windows.Forms.GroupBox DataGroup;
        private System.Windows.Forms.GroupBox PLCStatusGroup;
        private System.Windows.Forms.ListBox PLC_Status_Log;
        private System.Windows.Forms.GroupBox RS232StatusGroup;
        private System.Windows.Forms.ListBox RS232_Status_Log;
    }
}
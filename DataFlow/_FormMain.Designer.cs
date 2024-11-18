
namespace DataFlow
{
    partial class _FormMain
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
            this.EthernetGroup = new System.Windows.Forms.GroupBox();
            this.Ethernet_Disconnect_btn = new System.Windows.Forms.Button();
            this.Ethernet_Connect_btn = new System.Windows.Forms.Button();
            this.Port_Setting = new System.Windows.Forms.TextBox();
            this.Port_Lbl = new System.Windows.Forms.Label();
            this.IP_Setting = new System.Windows.Forms.TextBox();
            this.IP_Lbl = new System.Windows.Forms.Label();
            this.RS232Group = new System.Windows.Forms.GroupBox();
            this.RS232_Disconnect_btn = new System.Windows.Forms.Button();
            this.RS232_Connect_btn = new System.Windows.Forms.Button();
            this.COM_Setting = new System.Windows.Forms.TextBox();
            this.COM_Lbl = new System.Windows.Forms.Label();
            this.DataGroup = new System.Windows.Forms.GroupBox();
            this.RepositoryView = new System.Windows.Forms.ListView();
            this.LogGroup = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.EventLog = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ErrorLog = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.PLC_Status_Log = new System.Windows.Forms.ListBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.RS232_Status_Log = new System.Windows.Forms.ListBox();
            this.ID_Lbl = new System.Windows.Forms.Label();
            this.ID_Test = new System.Windows.Forms.TextBox();
            this.SLOT_Test = new System.Windows.Forms.TextBox();
            this.Check_Lbl = new System.Windows.Forms.Label();
            this.SendFirst_Test = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SendSecond_Test = new System.Windows.Forms.Button();
            this.SendRequest_btn = new System.Windows.Forms.Button();
            this.RetryCount_text = new System.Windows.Forms.TextBox();
            this.ResponseWaitingTime_text = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.EthernetGroup.SuspendLayout();
            this.RS232Group.SuspendLayout();
            this.DataGroup.SuspendLayout();
            this.LogGroup.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // EthernetGroup
            // 
            this.EthernetGroup.Controls.Add(this.Ethernet_Disconnect_btn);
            this.EthernetGroup.Controls.Add(this.Ethernet_Connect_btn);
            this.EthernetGroup.Controls.Add(this.Port_Setting);
            this.EthernetGroup.Controls.Add(this.Port_Lbl);
            this.EthernetGroup.Controls.Add(this.IP_Setting);
            this.EthernetGroup.Controls.Add(this.IP_Lbl);
            this.EthernetGroup.Location = new System.Drawing.Point(12, 12);
            this.EthernetGroup.Name = "EthernetGroup";
            this.EthernetGroup.Size = new System.Drawing.Size(249, 185);
            this.EthernetGroup.TabIndex = 0;
            this.EthernetGroup.TabStop = false;
            this.EthernetGroup.Text = "PLC(Buffer)";
            // 
            // Ethernet_Disconnect_btn
            // 
            this.Ethernet_Disconnect_btn.Font = new System.Drawing.Font("新細明體", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Ethernet_Disconnect_btn.Location = new System.Drawing.Point(129, 123);
            this.Ethernet_Disconnect_btn.Name = "Ethernet_Disconnect_btn";
            this.Ethernet_Disconnect_btn.Size = new System.Drawing.Size(105, 46);
            this.Ethernet_Disconnect_btn.TabIndex = 6;
            this.Ethernet_Disconnect_btn.Text = "DisConnect";
            this.Ethernet_Disconnect_btn.UseVisualStyleBackColor = true;
            this.Ethernet_Disconnect_btn.Click += new System.EventHandler(this.Ethernet_Disconnect_btn_Click);
            // 
            // Ethernet_Connect_btn
            // 
            this.Ethernet_Connect_btn.Font = new System.Drawing.Font("新細明體", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Ethernet_Connect_btn.Location = new System.Drawing.Point(16, 123);
            this.Ethernet_Connect_btn.Name = "Ethernet_Connect_btn";
            this.Ethernet_Connect_btn.Size = new System.Drawing.Size(107, 46);
            this.Ethernet_Connect_btn.TabIndex = 5;
            this.Ethernet_Connect_btn.Text = "Connect";
            this.Ethernet_Connect_btn.UseVisualStyleBackColor = true;
            this.Ethernet_Connect_btn.Click += new System.EventHandler(this.Ethernet_Connect_btn_Click);
            // 
            // Port_Setting
            // 
            this.Port_Setting.Location = new System.Drawing.Point(64, 77);
            this.Port_Setting.Name = "Port_Setting";
            this.Port_Setting.Size = new System.Drawing.Size(170, 27);
            this.Port_Setting.TabIndex = 4;
            // 
            // Port_Lbl
            // 
            this.Port_Lbl.AutoSize = true;
            this.Port_Lbl.Location = new System.Drawing.Point(12, 79);
            this.Port_Lbl.Name = "Port_Lbl";
            this.Port_Lbl.Size = new System.Drawing.Size(33, 16);
            this.Port_Lbl.TabIndex = 3;
            this.Port_Lbl.Text = "Port";
            // 
            // IP_Setting
            // 
            this.IP_Setting.Location = new System.Drawing.Point(66, 36);
            this.IP_Setting.Name = "IP_Setting";
            this.IP_Setting.Size = new System.Drawing.Size(168, 27);
            this.IP_Setting.TabIndex = 2;
            // 
            // IP_Lbl
            // 
            this.IP_Lbl.AutoSize = true;
            this.IP_Lbl.Location = new System.Drawing.Point(26, 36);
            this.IP_Lbl.Name = "IP_Lbl";
            this.IP_Lbl.Size = new System.Drawing.Size(21, 16);
            this.IP_Lbl.TabIndex = 1;
            this.IP_Lbl.Text = "IP";
            // 
            // RS232Group
            // 
            this.RS232Group.Controls.Add(this.RS232_Disconnect_btn);
            this.RS232Group.Controls.Add(this.RS232_Connect_btn);
            this.RS232Group.Controls.Add(this.COM_Setting);
            this.RS232Group.Controls.Add(this.COM_Lbl);
            this.RS232Group.Location = new System.Drawing.Point(285, 11);
            this.RS232Group.Name = "RS232Group";
            this.RS232Group.Size = new System.Drawing.Size(237, 186);
            this.RS232Group.TabIndex = 1;
            this.RS232Group.TabStop = false;
            this.RS232Group.Text = "RS232(AOI)";
            // 
            // RS232_Disconnect_btn
            // 
            this.RS232_Disconnect_btn.Font = new System.Drawing.Font("新細明體", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.RS232_Disconnect_btn.Location = new System.Drawing.Point(123, 124);
            this.RS232_Disconnect_btn.Name = "RS232_Disconnect_btn";
            this.RS232_Disconnect_btn.Size = new System.Drawing.Size(105, 46);
            this.RS232_Disconnect_btn.TabIndex = 7;
            this.RS232_Disconnect_btn.Text = "DisConnect";
            this.RS232_Disconnect_btn.UseVisualStyleBackColor = true;
            this.RS232_Disconnect_btn.Click += new System.EventHandler(this.RS232_Disconnect_btn_Click);
            // 
            // RS232_Connect_btn
            // 
            this.RS232_Connect_btn.Font = new System.Drawing.Font("新細明體", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.RS232_Connect_btn.Location = new System.Drawing.Point(10, 124);
            this.RS232_Connect_btn.Name = "RS232_Connect_btn";
            this.RS232_Connect_btn.Size = new System.Drawing.Size(107, 46);
            this.RS232_Connect_btn.TabIndex = 7;
            this.RS232_Connect_btn.Text = "Connect";
            this.RS232_Connect_btn.UseVisualStyleBackColor = true;
            this.RS232_Connect_btn.Click += new System.EventHandler(this.RS232_Connect_btn_Click);
            // 
            // COM_Setting
            // 
            this.COM_Setting.Location = new System.Drawing.Point(65, 77);
            this.COM_Setting.Name = "COM_Setting";
            this.COM_Setting.Size = new System.Drawing.Size(163, 27);
            this.COM_Setting.TabIndex = 7;
            // 
            // COM_Lbl
            // 
            this.COM_Lbl.AutoSize = true;
            this.COM_Lbl.Location = new System.Drawing.Point(6, 80);
            this.COM_Lbl.Name = "COM_Lbl";
            this.COM_Lbl.Size = new System.Drawing.Size(42, 16);
            this.COM_Lbl.TabIndex = 7;
            this.COM_Lbl.Text = "COM";
            // 
            // DataGroup
            // 
            this.DataGroup.Controls.Add(this.RepositoryView);
            this.DataGroup.Location = new System.Drawing.Point(12, 214);
            this.DataGroup.Name = "DataGroup";
            this.DataGroup.Size = new System.Drawing.Size(510, 573);
            this.DataGroup.TabIndex = 8;
            this.DataGroup.TabStop = false;
            this.DataGroup.Text = "Data";
            // 
            // RepositoryView
            // 
            this.RepositoryView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RepositoryView.GridLines = true;
            this.RepositoryView.HideSelection = false;
            this.RepositoryView.Location = new System.Drawing.Point(3, 23);
            this.RepositoryView.MultiSelect = false;
            this.RepositoryView.Name = "RepositoryView";
            this.RepositoryView.Size = new System.Drawing.Size(504, 547);
            this.RepositoryView.TabIndex = 9;
            this.RepositoryView.UseCompatibleStateImageBehavior = false;
            this.RepositoryView.View = System.Windows.Forms.View.Details;
            // 
            // LogGroup
            // 
            this.LogGroup.Controls.Add(this.tabControl1);
            this.LogGroup.Location = new System.Drawing.Point(548, 11);
            this.LogGroup.Name = "LogGroup";
            this.LogGroup.Size = new System.Drawing.Size(791, 773);
            this.LogGroup.TabIndex = 9;
            this.LogGroup.TabStop = false;
            this.LogGroup.Text = "Log";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 23);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(785, 747);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.EventLog);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(777, 717);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Event";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // EventLog
            // 
            this.EventLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventLog.FormattingEnabled = true;
            this.EventLog.HorizontalScrollbar = true;
            this.EventLog.ItemHeight = 16;
            this.EventLog.Location = new System.Drawing.Point(3, 3);
            this.EventLog.Name = "EventLog";
            this.EventLog.Size = new System.Drawing.Size(771, 711);
            this.EventLog.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ErrorLog);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(777, 722);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Error";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ErrorLog
            // 
            this.ErrorLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorLog.FormattingEnabled = true;
            this.ErrorLog.HorizontalScrollbar = true;
            this.ErrorLog.ItemHeight = 16;
            this.ErrorLog.Location = new System.Drawing.Point(3, 3);
            this.ErrorLog.Name = "ErrorLog";
            this.ErrorLog.Size = new System.Drawing.Size(771, 716);
            this.ErrorLog.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.PLC_Status_Log);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(777, 722);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "PLC_Status";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // PLC_Status_Log
            // 
            this.PLC_Status_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLC_Status_Log.FormattingEnabled = true;
            this.PLC_Status_Log.HorizontalScrollbar = true;
            this.PLC_Status_Log.ItemHeight = 16;
            this.PLC_Status_Log.Location = new System.Drawing.Point(0, 0);
            this.PLC_Status_Log.Name = "PLC_Status_Log";
            this.PLC_Status_Log.Size = new System.Drawing.Size(777, 722);
            this.PLC_Status_Log.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.RS232_Status_Log);
            this.tabPage4.Location = new System.Drawing.Point(4, 26);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(777, 722);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "RS232_Status";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // RS232_Status_Log
            // 
            this.RS232_Status_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RS232_Status_Log.FormattingEnabled = true;
            this.RS232_Status_Log.HorizontalScrollbar = true;
            this.RS232_Status_Log.ItemHeight = 16;
            this.RS232_Status_Log.Location = new System.Drawing.Point(0, 0);
            this.RS232_Status_Log.Name = "RS232_Status_Log";
            this.RS232_Status_Log.Size = new System.Drawing.Size(777, 722);
            this.RS232_Status_Log.TabIndex = 0;
            // 
            // ID_Lbl
            // 
            this.ID_Lbl.AutoSize = true;
            this.ID_Lbl.Location = new System.Drawing.Point(2, 32);
            this.ID_Lbl.Name = "ID_Lbl";
            this.ID_Lbl.Size = new System.Drawing.Size(24, 16);
            this.ID_Lbl.TabIndex = 10;
            this.ID_Lbl.Text = "ID";
            // 
            // ID_Test
            // 
            this.ID_Test.Location = new System.Drawing.Point(6, 55);
            this.ID_Test.Name = "ID_Test";
            this.ID_Test.Size = new System.Drawing.Size(163, 27);
            this.ID_Test.TabIndex = 8;
            // 
            // SLOT_Test
            // 
            this.SLOT_Test.Location = new System.Drawing.Point(6, 135);
            this.SLOT_Test.Name = "SLOT_Test";
            this.SLOT_Test.Size = new System.Drawing.Size(163, 27);
            this.SLOT_Test.TabIndex = 11;
            // 
            // Check_Lbl
            // 
            this.Check_Lbl.AutoSize = true;
            this.Check_Lbl.Location = new System.Drawing.Point(2, 112);
            this.Check_Lbl.Name = "Check_Lbl";
            this.Check_Lbl.Size = new System.Drawing.Size(45, 16);
            this.Check_Lbl.TabIndex = 12;
            this.Check_Lbl.Text = "SLOT";
            // 
            // SendFirst_Test
            // 
            this.SendFirst_Test.Font = new System.Drawing.Font("新細明體", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.SendFirst_Test.Location = new System.Drawing.Point(36, 186);
            this.SendFirst_Test.Name = "SendFirst_Test";
            this.SendFirst_Test.Size = new System.Drawing.Size(107, 46);
            this.SendFirst_Test.TabIndex = 8;
            this.SendFirst_Test.Text = "SendFirst";
            this.SendFirst_Test.UseVisualStyleBackColor = true;
            this.SendFirst_Test.Click += new System.EventHandler(this.SendFirst_Test_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 295);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(163, 27);
            this.textBox1.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 266);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 16);
            this.label1.TabIndex = 19;
            this.label1.Text = "ID";
            // 
            // SendSecond_Test
            // 
            this.SendSecond_Test.Font = new System.Drawing.Font("新細明體", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.SendSecond_Test.Location = new System.Drawing.Point(36, 337);
            this.SendSecond_Test.Name = "SendSecond_Test";
            this.SendSecond_Test.Size = new System.Drawing.Size(107, 46);
            this.SendSecond_Test.TabIndex = 20;
            this.SendSecond_Test.Text = "SendSencond";
            this.SendSecond_Test.UseVisualStyleBackColor = true;
            this.SendSecond_Test.Click += new System.EventHandler(this.SendSecond_Test_Click);
            // 
            // SendRequest_btn
            // 
            this.SendRequest_btn.Font = new System.Drawing.Font("新細明體", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.SendRequest_btn.Location = new System.Drawing.Point(36, 425);
            this.SendRequest_btn.Name = "SendRequest_btn";
            this.SendRequest_btn.Size = new System.Drawing.Size(115, 46);
            this.SendRequest_btn.TabIndex = 21;
            this.SendRequest_btn.Text = "SendFirstRequest";
            this.SendRequest_btn.UseVisualStyleBackColor = true;
            this.SendRequest_btn.Click += new System.EventHandler(this.SendRequest_btn_Click);
            // 
            // RetryCount_text
            // 
            this.RetryCount_text.Location = new System.Drawing.Point(1347, 656);
            this.RetryCount_text.Name = "RetryCount_text";
            this.RetryCount_text.Size = new System.Drawing.Size(163, 27);
            this.RetryCount_text.TabIndex = 22;
            this.RetryCount_text.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RetryCount_text_KeyPress);
            // 
            // ResponseWaitingTime_text
            // 
            this.ResponseWaitingTime_text.Location = new System.Drawing.Point(1344, 745);
            this.ResponseWaitingTime_text.Name = "ResponseWaitingTime_text";
            this.ResponseWaitingTime_text.Size = new System.Drawing.Size(163, 27);
            this.ResponseWaitingTime_text.TabIndex = 23;
            this.ResponseWaitingTime_text.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ResponseWaitingTime_text_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1345, 637);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 24;
            this.label2.Text = "RetryCount";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1345, 717);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 16);
            this.label3.TabIndex = 25;
            this.label3.Text = "ResponseWaitingTime";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("新細明體", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.Location = new System.Drawing.Point(36, 538);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 46);
            this.button1.TabIndex = 26;
            this.button1.Text = "SendSerialRequest";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(6, 505);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(163, 27);
            this.textBox2.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(1513, 753);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 19);
            this.label4.TabIndex = 1;
            this.label4.Text = "s";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SendSecond_Test);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.SendFirst_Test);
            this.groupBox1.Controls.Add(this.SLOT_Test);
            this.groupBox1.Controls.Add(this.Check_Lbl);
            this.groupBox1.Controls.Add(this.ID_Test);
            this.groupBox1.Controls.Add(this.ID_Lbl);
            this.groupBox1.Controls.Add(this.SendRequest_btn);
            this.groupBox1.Location = new System.Drawing.Point(1348, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(181, 599);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "test function";
            this.groupBox1.Visible = false;
            // 
            // _FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1530, 879);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ResponseWaitingTime_text);
            this.Controls.Add(this.RetryCount_text);
            this.Controls.Add(this.LogGroup);
            this.Controls.Add(this.DataGroup);
            this.Controls.Add(this.RS232Group);
            this.Controls.Add(this.EthernetGroup);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "_FormMain";
            this.Text = "DataFlow";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this._FormMain_FormClosing);
            this.EthernetGroup.ResumeLayout(false);
            this.EthernetGroup.PerformLayout();
            this.RS232Group.ResumeLayout(false);
            this.RS232Group.PerformLayout();
            this.DataGroup.ResumeLayout(false);
            this.LogGroup.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox EthernetGroup;
        private System.Windows.Forms.Button Ethernet_Disconnect_btn;
        private System.Windows.Forms.Button Ethernet_Connect_btn;
        private System.Windows.Forms.TextBox Port_Setting;
        private System.Windows.Forms.Label Port_Lbl;
        private System.Windows.Forms.TextBox IP_Setting;
        private System.Windows.Forms.Label IP_Lbl;
        private System.Windows.Forms.GroupBox RS232Group;
        private System.Windows.Forms.Button RS232_Disconnect_btn;
        private System.Windows.Forms.Button RS232_Connect_btn;
        private System.Windows.Forms.TextBox COM_Setting;
        private System.Windows.Forms.Label COM_Lbl;
        private System.Windows.Forms.GroupBox DataGroup;
        private System.Windows.Forms.ListView RepositoryView;
        private System.Windows.Forms.GroupBox LogGroup;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox EventLog;
        private System.Windows.Forms.ListBox ErrorLog;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListBox PLC_Status_Log;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ListBox RS232_Status_Log;
        private System.Windows.Forms.Label ID_Lbl;
        private System.Windows.Forms.TextBox ID_Test;
        private System.Windows.Forms.TextBox SLOT_Test;
        private System.Windows.Forms.Label Check_Lbl;
        private System.Windows.Forms.Button SendFirst_Test;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SendSecond_Test;
        private System.Windows.Forms.Button SendRequest_btn;
        private System.Windows.Forms.TextBox RetryCount_text;
        private System.Windows.Forms.TextBox ResponseWaitingTime_text;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

namespace DataFlow
{
    partial class HermesForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.Handheld_Scanner_btn = new System.Windows.Forms.Button();
            this.Alarm_Log_btn = new System.Windows.Forms.Button();
            this.Error_Log_btn = new System.Windows.Forms.Button();
            this.Event_Log_btn = new System.Windows.Forms.Button();
            this.State_btn = new System.Windows.Forms.Button();
            this.ParameterSetting_btn = new System.Windows.Forms.Button();
            this.FilePath_btn = new System.Windows.Forms.Button();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.AlarmLog = new System.Windows.Forms.ListBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.ErrorLog = new System.Windows.Forms.ListBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.EventLog = new System.Windows.Forms.ListBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.Scanning_Place_Text = new System.Windows.Forms.TextBox();
            this.Scanning_Place_Label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Scanning_Code_Text = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.UpstreamState = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DownstreamState = new System.Windows.Forms.ListBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.WriteFilePath_Lbl = new System.Windows.Forms.Label();
            this.ReadFilePath_LBl = new System.Windows.Forms.Label();
            this.WriteFilePathName = new System.Windows.Forms.TextBox();
            this.ReadFilePathName = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.FilePath_btn);
            this.panel1.Controls.Add(this.Handheld_Scanner_btn);
            this.panel1.Controls.Add(this.Alarm_Log_btn);
            this.panel1.Controls.Add(this.Error_Log_btn);
            this.panel1.Controls.Add(this.Event_Log_btn);
            this.panel1.Controls.Add(this.State_btn);
            this.panel1.Controls.Add(this.ParameterSetting_btn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(126, 526);
            this.panel1.TabIndex = 0;
            // 
            // Handheld_Scanner_btn
            // 
            this.Handheld_Scanner_btn.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Handheld_Scanner_btn.Location = new System.Drawing.Point(12, 143);
            this.Handheld_Scanner_btn.Name = "Handheld_Scanner_btn";
            this.Handheld_Scanner_btn.Size = new System.Drawing.Size(93, 42);
            this.Handheld_Scanner_btn.TabIndex = 5;
            this.Handheld_Scanner_btn.Text = "Handheld Scanner";
            this.Handheld_Scanner_btn.UseVisualStyleBackColor = true;
            this.Handheld_Scanner_btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // Alarm_Log_btn
            // 
            this.Alarm_Log_btn.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Alarm_Log_btn.Location = new System.Drawing.Point(12, 387);
            this.Alarm_Log_btn.Name = "Alarm_Log_btn";
            this.Alarm_Log_btn.Size = new System.Drawing.Size(93, 31);
            this.Alarm_Log_btn.TabIndex = 4;
            this.Alarm_Log_btn.Text = "Alarm_Log";
            this.Alarm_Log_btn.UseVisualStyleBackColor = true;
            this.Alarm_Log_btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // Error_Log_btn
            // 
            this.Error_Log_btn.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Error_Log_btn.Location = new System.Drawing.Point(12, 327);
            this.Error_Log_btn.Name = "Error_Log_btn";
            this.Error_Log_btn.Size = new System.Drawing.Size(93, 31);
            this.Error_Log_btn.TabIndex = 3;
            this.Error_Log_btn.Text = "Error_Log";
            this.Error_Log_btn.UseVisualStyleBackColor = true;
            this.Error_Log_btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // Event_Log_btn
            // 
            this.Event_Log_btn.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Event_Log_btn.Location = new System.Drawing.Point(12, 270);
            this.Event_Log_btn.Name = "Event_Log_btn";
            this.Event_Log_btn.Size = new System.Drawing.Size(93, 31);
            this.Event_Log_btn.TabIndex = 2;
            this.Event_Log_btn.Text = "Event_Log";
            this.Event_Log_btn.UseVisualStyleBackColor = true;
            this.Event_Log_btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // State_btn
            // 
            this.State_btn.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.State_btn.Location = new System.Drawing.Point(12, 91);
            this.State_btn.Name = "State_btn";
            this.State_btn.Size = new System.Drawing.Size(93, 31);
            this.State_btn.TabIndex = 1;
            this.State_btn.Text = "State";
            this.State_btn.UseVisualStyleBackColor = true;
            this.State_btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // ParameterSetting_btn
            // 
            this.ParameterSetting_btn.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ParameterSetting_btn.Location = new System.Drawing.Point(12, 32);
            this.ParameterSetting_btn.Name = "ParameterSetting_btn";
            this.ParameterSetting_btn.Size = new System.Drawing.Size(93, 31);
            this.ParameterSetting_btn.TabIndex = 0;
            this.ParameterSetting_btn.Text = "Parameter";
            this.ParameterSetting_btn.UseVisualStyleBackColor = true;
            this.ParameterSetting_btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // FilePath_btn
            // 
            this.FilePath_btn.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FilePath_btn.Location = new System.Drawing.Point(12, 212);
            this.FilePath_btn.Name = "FilePath_btn";
            this.FilePath_btn.Size = new System.Drawing.Size(93, 31);
            this.FilePath_btn.TabIndex = 6;
            this.FilePath_btn.Text = "FilePath";
            this.FilePath_btn.UseVisualStyleBackColor = true;
            this.FilePath_btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.AlarmLog);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(758, 500);
            this.tabPage7.TabIndex = 4;
            this.tabPage7.Text = "tabPage7";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // AlarmLog
            // 
            this.AlarmLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AlarmLog.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.AlarmLog.FormattingEnabled = true;
            this.AlarmLog.ItemHeight = 16;
            this.AlarmLog.Location = new System.Drawing.Point(0, 0);
            this.AlarmLog.Name = "AlarmLog";
            this.AlarmLog.Size = new System.Drawing.Size(758, 500);
            this.AlarmLog.TabIndex = 0;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.ErrorLog);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(758, 500);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "tabPage6";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // ErrorLog
            // 
            this.ErrorLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorLog.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ErrorLog.FormattingEnabled = true;
            this.ErrorLog.ItemHeight = 16;
            this.ErrorLog.Location = new System.Drawing.Point(0, 0);
            this.ErrorLog.Name = "ErrorLog";
            this.ErrorLog.Size = new System.Drawing.Size(758, 500);
            this.ErrorLog.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.EventLog);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(758, 500);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "tabPage5";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // EventLog
            // 
            this.EventLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventLog.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.EventLog.FormattingEnabled = true;
            this.EventLog.ItemHeight = 16;
            this.EventLog.Location = new System.Drawing.Point(0, 0);
            this.EventLog.Name = "EventLog";
            this.EventLog.Size = new System.Drawing.Size(758, 500);
            this.EventLog.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.ReadFilePathName);
            this.tabPage4.Controls.Add(this.WriteFilePathName);
            this.tabPage4.Controls.Add(this.ReadFilePath_LBl);
            this.tabPage4.Controls.Add(this.WriteFilePath_Lbl);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(758, 500);
            this.tabPage4.TabIndex = 6;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.Scanning_Code_Text);
            this.tabPage3.Controls.Add(this.Scanning_Place_Text);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.Scanning_Place_Label);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(758, 500);
            this.tabPage3.TabIndex = 5;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // Scanning_Place_Text
            // 
            this.Scanning_Place_Text.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Scanning_Place_Text.Location = new System.Drawing.Point(141, 60);
            this.Scanning_Place_Text.Name = "Scanning_Place_Text";
            this.Scanning_Place_Text.Size = new System.Drawing.Size(186, 27);
            this.Scanning_Place_Text.TabIndex = 0;
            // 
            // Scanning_Place_Label
            // 
            this.Scanning_Place_Label.AutoSize = true;
            this.Scanning_Place_Label.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Scanning_Place_Label.Location = new System.Drawing.Point(32, 63);
            this.Scanning_Place_Label.Name = "Scanning_Place_Label";
            this.Scanning_Place_Label.Size = new System.Drawing.Size(103, 16);
            this.Scanning_Place_Label.TabIndex = 1;
            this.Scanning_Place_Label.Text = "Scanning Place";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(32, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Scanning Code";
            // 
            // Scanning_Code_Text
            // 
            this.Scanning_Code_Text.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Scanning_Code_Text.Location = new System.Drawing.Point(141, 108);
            this.Scanning_Code_Text.Name = "Scanning_Code_Text";
            this.Scanning_Code_Text.Size = new System.Drawing.Size(186, 27);
            this.Scanning_Code_Text.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(758, 500);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.UpstreamState);
            this.groupBox1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(30, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(335, 482);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Upstream";
            // 
            // UpstreamState
            // 
            this.UpstreamState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UpstreamState.FormattingEnabled = true;
            this.UpstreamState.ItemHeight = 16;
            this.UpstreamState.Location = new System.Drawing.Point(3, 23);
            this.UpstreamState.Name = "UpstreamState";
            this.UpstreamState.Size = new System.Drawing.Size(329, 456);
            this.UpstreamState.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DownstreamState);
            this.groupBox2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox2.Location = new System.Drawing.Point(371, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(356, 482);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Downstream";
            // 
            // DownstreamState
            // 
            this.DownstreamState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DownstreamState.FormattingEnabled = true;
            this.DownstreamState.ItemHeight = 16;
            this.DownstreamState.Location = new System.Drawing.Point(3, 23);
            this.DownstreamState.Name = "DownstreamState";
            this.DownstreamState.Size = new System.Drawing.Size(350, 456);
            this.DownstreamState.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkedListBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(758, 500);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.ColumnWidth = 200;
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "SelectAll",
            "InterfaceId",
            "ProductTypeId",
            "TopBarcode",
            "BottomBarcode",
            "Length",
            "Width",
            "Thickness",
            "ConveyorSpeed",
            "TopClearanceHeight",
            "BottomClearanceHeight",
            "Weight",
            "WorkOrderId",
            "BatchId",
            "ForecastId",
            "TimeUntilAvailable",
            "BoardIdCreatedBy"});
            this.checkedListBox1.Location = new System.Drawing.Point(3, 3);
            this.checkedListBox1.MultiColumn = true;
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(752, 494);
            this.checkedListBox1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(126, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(766, 526);
            this.tabControl1.TabIndex = 1;
            // 
            // WriteFilePath_Lbl
            // 
            this.WriteFilePath_Lbl.AutoSize = true;
            this.WriteFilePath_Lbl.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.WriteFilePath_Lbl.Location = new System.Drawing.Point(28, 69);
            this.WriteFilePath_Lbl.Name = "WriteFilePath_Lbl";
            this.WriteFilePath_Lbl.Size = new System.Drawing.Size(92, 16);
            this.WriteFilePath_Lbl.TabIndex = 0;
            this.WriteFilePath_Lbl.Text = "WriteFilePath";
            // 
            // ReadFilePath_LBl
            // 
            this.ReadFilePath_LBl.AutoSize = true;
            this.ReadFilePath_LBl.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ReadFilePath_LBl.Location = new System.Drawing.Point(28, 121);
            this.ReadFilePath_LBl.Name = "ReadFilePath_LBl";
            this.ReadFilePath_LBl.Size = new System.Drawing.Size(90, 16);
            this.ReadFilePath_LBl.TabIndex = 1;
            this.ReadFilePath_LBl.Text = "ReadFilePath";
            // 
            // WriteFilePathName
            // 
            this.WriteFilePathName.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.WriteFilePathName.Location = new System.Drawing.Point(138, 58);
            this.WriteFilePathName.Name = "WriteFilePathName";
            this.WriteFilePathName.Size = new System.Drawing.Size(419, 27);
            this.WriteFilePathName.TabIndex = 2;
            // 
            // ReadFilePathName
            // 
            this.ReadFilePathName.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ReadFilePathName.Location = new System.Drawing.Point(138, 110);
            this.ReadFilePathName.Name = "ReadFilePathName";
            this.ReadFilePathName.Size = new System.Drawing.Size(419, 27);
            this.ReadFilePathName.TabIndex = 3;
            // 
            // HermesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 526);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HermesForm";
            this.Text = "HermesForm";
            this.panel1.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Alarm_Log_btn;
        private System.Windows.Forms.Button Error_Log_btn;
        private System.Windows.Forms.Button Event_Log_btn;
        private System.Windows.Forms.Button State_btn;
        private System.Windows.Forms.Button ParameterSetting_btn;
        private System.Windows.Forms.Button Handheld_Scanner_btn;
        private System.Windows.Forms.Button FilePath_btn;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.ListBox AlarmLog;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.ListBox ErrorLog;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.ListBox EventLog;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox Scanning_Code_Text;
        private System.Windows.Forms.TextBox Scanning_Place_Text;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Scanning_Place_Label;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox DownstreamState;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox UpstreamState;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TextBox ReadFilePathName;
        private System.Windows.Forms.TextBox WriteFilePathName;
        private System.Windows.Forms.Label ReadFilePath_LBl;
        private System.Windows.Forms.Label WriteFilePath_Lbl;
    }
}
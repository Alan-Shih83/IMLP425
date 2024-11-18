
namespace DataFlow
{
    partial class Form3
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
            this.RS232Log_btn = new System.Windows.Forms.Button();
            this.PLCLog_btn = new System.Windows.Forms.Button();
            this.ErrorLog_btn = new System.Windows.Forms.Button();
            this.EventLog_btn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.EventLog = new System.Windows.Forms.ListBox();
            this.ErrorLog = new System.Windows.Forms.ListBox();
            this.PLCLog = new System.Windows.Forms.ListBox();
            this.RS232Log = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.RS232Log_btn);
            this.panel1.Controls.Add(this.PLCLog_btn);
            this.panel1.Controls.Add(this.ErrorLog_btn);
            this.panel1.Controls.Add(this.EventLog_btn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 711);
            this.panel1.TabIndex = 0;
            // 
            // RS232Log_btn
            // 
            this.RS232Log_btn.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.RS232Log_btn.Location = new System.Drawing.Point(34, 334);
            this.RS232Log_btn.Name = "RS232Log_btn";
            this.RS232Log_btn.Size = new System.Drawing.Size(132, 54);
            this.RS232Log_btn.TabIndex = 4;
            this.RS232Log_btn.Text = "Rs232 Log";
            this.RS232Log_btn.UseVisualStyleBackColor = true;
            // 
            // PLCLog_btn
            // 
            this.PLCLog_btn.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.PLCLog_btn.Location = new System.Drawing.Point(34, 246);
            this.PLCLog_btn.Name = "PLCLog_btn";
            this.PLCLog_btn.Size = new System.Drawing.Size(132, 54);
            this.PLCLog_btn.TabIndex = 3;
            this.PLCLog_btn.Text = "PLC Log";
            this.PLCLog_btn.UseVisualStyleBackColor = true;
            // 
            // ErrorLog_btn
            // 
            this.ErrorLog_btn.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ErrorLog_btn.Location = new System.Drawing.Point(34, 148);
            this.ErrorLog_btn.Name = "ErrorLog_btn";
            this.ErrorLog_btn.Size = new System.Drawing.Size(132, 54);
            this.ErrorLog_btn.TabIndex = 2;
            this.ErrorLog_btn.Text = "Error Log";
            this.ErrorLog_btn.UseVisualStyleBackColor = true;
            // 
            // EventLog_btn
            // 
            this.EventLog_btn.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.EventLog_btn.Location = new System.Drawing.Point(34, 54);
            this.EventLog_btn.Name = "EventLog_btn";
            this.EventLog_btn.Size = new System.Drawing.Size(132, 54);
            this.EventLog_btn.TabIndex = 1;
            this.EventLog_btn.Text = "Event Log";
            this.EventLog_btn.UseVisualStyleBackColor = true;
            this.EventLog_btn.Click += new System.EventHandler(this.EventLog_btn_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(200, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1064, 711);
            this.panel2.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1062, 709);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.EventLog);
            this.tabPage1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1054, 671);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ErrorLog);
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1054, 671);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.PLCLog);
            this.tabPage3.Location = new System.Drawing.Point(4, 34);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1054, 671);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.RS232Log);
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1054, 671);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // EventLog
            // 
            this.EventLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventLog.FormattingEnabled = true;
            this.EventLog.ItemHeight = 24;
            this.EventLog.Location = new System.Drawing.Point(3, 3);
            this.EventLog.Name = "EventLog";
            this.EventLog.Size = new System.Drawing.Size(1048, 665);
            this.EventLog.TabIndex = 0;
            // 
            // ErrorLog
            // 
            this.ErrorLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorLog.FormattingEnabled = true;
            this.ErrorLog.ItemHeight = 24;
            this.ErrorLog.Location = new System.Drawing.Point(3, 3);
            this.ErrorLog.Name = "ErrorLog";
            this.ErrorLog.Size = new System.Drawing.Size(1048, 665);
            this.ErrorLog.TabIndex = 0;
            // 
            // PLCLog
            // 
            this.PLCLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PLCLog.FormattingEnabled = true;
            this.PLCLog.ItemHeight = 24;
            this.PLCLog.Location = new System.Drawing.Point(0, 0);
            this.PLCLog.Name = "PLCLog";
            this.PLCLog.Size = new System.Drawing.Size(1054, 671);
            this.PLCLog.TabIndex = 0;
            // 
            // RS232Log
            // 
            this.RS232Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RS232Log.FormattingEnabled = true;
            this.RS232Log.ItemHeight = 24;
            this.RS232Log.Location = new System.Drawing.Point(0, 0);
            this.RS232Log.Name = "RS232Log";
            this.RS232Log.Size = new System.Drawing.Size(1054, 671);
            this.RS232Log.TabIndex = 0;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 711);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form3";
            this.Text = "Form3";
            this.Resize += new System.EventHandler(this.Form3_Resize);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button RS232Log_btn;
        private System.Windows.Forms.Button PLCLog_btn;
        private System.Windows.Forms.Button ErrorLog_btn;
        private System.Windows.Forms.Button EventLog_btn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ListBox EventLog;
        private System.Windows.Forms.ListBox ErrorLog;
        private System.Windows.Forms.ListBox PLCLog;
        private System.Windows.Forms.ListBox RS232Log;
    }
}
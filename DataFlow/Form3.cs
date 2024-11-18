using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataFlow
{
    public partial class Form3 : Form
    {
        FormInfo formInfo = default;
        public Form3()
        {
            InitializeComponent();
            formInfo = new FormInfo(this);
            FormOperate.TabControlIndexHide(tabControl1);
            LogHandlerManager.Instance.GetLogHandler(LogType.Event).attach(this.EventLog);
            LogHandlerManager.Instance.GetLogHandler(LogType.Error).attach(this.ErrorLog);
            LogHandlerManager.Instance.GetLogHandler(LogType.PLC_Status).attach(this.PLCLog);
            LogHandlerManager.Instance.GetLogHandler(LogType.Serial_Status).attach(this.RS232Log);
        }

        private void Form3_Resize(object sender, EventArgs e)
        {
            if (!Equals(formInfo, default))
                formInfo.ControlResize(this);
        }

        private void EventLog_btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "EventLog_btn":
                    {
                        this.tabControl1.SelectedIndex = 0;
                        break;
                    }
                case "ErrorLog_btn":
                    {
                        this.tabControl1.SelectedIndex = 1;
                        break;
                    }
                case "PLCLog_btn":
                    {
                        this.tabControl1.SelectedIndex = 2;
                        break;
                    }
                case "RS232Log_btn":
                    {
                        this.tabControl1.SelectedIndex = 3;
                        break;
                    }
                default:
                    {
                        this.tabControl1.SelectedIndex = 0;
                        break;
                    }
            }
        }
    }
}

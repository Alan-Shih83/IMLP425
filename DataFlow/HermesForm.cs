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
    public partial class HermesForm : Form
    {
        public HermesForm()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "ParameterSetting_btn":
                    {
                        this.tabControl1.SelectedIndex = 0;
                        //formInfo?.ControlResize(this);
                        break;
                    }
                case "State_btn":
                    {
                        this.tabControl1.SelectedIndex = 1;
                        //formInfo?.ControlResize(this);
                        break;
                    }
                case "Handheld_Scanner_btn":
                    {
                        this.tabControl1.SelectedIndex = 2;
                        //formInfo?.ControlResize(this);
                        break;
                    }
                case "FilePath_btn":
                    {
                        this.tabControl1.SelectedIndex = 3;
                        //formInfo?.ControlResize(this);
                        break;
                    }
                case "Event_Log_btn":
                    {
                        this.tabControl1.SelectedIndex = 4;
                        //formInfo?.ControlResize(this);
                        break;
                    }
                case "Error_Log_btn":
                    {
                        this.tabControl1.SelectedIndex = 5;
                        //formInfo?.ControlResize(this);
                        break;
                    }
                case "Alarm_Log_btn":
                    {
                        this.tabControl1.SelectedIndex = 6;
                        //formInfo?.ControlResize(this);
                        break;
                    }
                default:
                    {
                        this.tabControl1.SelectedIndex = 0;
                        //formInfo?.ControlResize(this);
                        break;
                    }
            }
        }
    }
}

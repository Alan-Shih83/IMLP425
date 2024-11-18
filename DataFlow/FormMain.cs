using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataFlow
{
    public partial class FormMain : Form
    {
        Client client = new Client();

        LogicControlProcessContainer container = new LogicControlProcessContainer();

        Serial Serial = new Serial();

        SerialPortFilter SerialPortFilter = new SerialPortFilter();

        ListViewSorter sorter = default;

        FormInfo formInfo = default;
        public FormMain()
        {
            InitializeComponent();
            //LogHandlerManager.Instance.GetLogHandler(LogType.Event).attach(EventLog);
           // LogHandlerManager.Instance.GetLogHandler(LogType.Error).attach(ErrorLog);
           // LogHandlerManager.Instance.GetLogHandler(LogType.PLC_Status).attach(PLC_Status_Log);
           // LogHandlerManager.Instance.GetLogHandler(LogType.Serial_Status).attach(RS232_Status_Log);
            ListViewInit();
            formInfo = new FormInfo(this);
            
        }

        private void ListViewInit()
        {
            RepositoryMessage repository = new RepositoryMessage();
            var properties = repository.GetType().GetProperties();
            foreach (var property in properties)
            {
                ColumnHeader header = new ColumnHeader() { Text = property.Name, Width = 80, TextAlign = HorizontalAlignment.Left };
                if (property.Name == "ID")
                    header.Width = 160;
                RepositoryView.Columns.Add(header);
                if (property.Name == "Level")
                    sorter = new ListViewSorter(header.Index);
            }
            RepositoryView.ListViewItemSorter = sorter;
            RepositoryView.Sort();
        }

        public void AddData(RepositoryMessage repository)
        {
            //RepositoryMessage repository = new RepositoryMessage() { ID = "DD", check = 2, Level = 17 };
            string[] para = repository.Format();
            if (!Equals(para, default))
            {
                RepositoryView.BeginInvoke(new Action(() => {
                    ListViewItem ListViewItem = new ListViewItem(para);
                    ListViewItem.EnsureVisible();
                    RepositoryView.Items.Add(ListViewItem);
                }));
            }
        }

        public void DelData(RepositoryMessage repository)
        {
            RepositoryView.BeginInvoke(new Action(() => {
                for (int j = 0; j < RepositoryView.Items.Count; j++)
                {
                    if (RepositoryView.Items[j].SubItems[0].Text.Equals(repository.ID))
                    {
                        RepositoryView.Items.Remove(RepositoryView.Items[j]);
                        j--;
                    }
                }
            }));
        }

        public void DelDataALL()
        {
            RepositoryView.BeginInvoke(new Action(() => {
                RepositoryView.Clear();
                ListViewInit();
            }));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //client.Start();
            //upstream.Restart();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SecondJudgmentMessage second = new SecondJudgmentMessage(textBox1.Text, Convert.ToInt32(textBox2.Text));
            container.AddMessage(second);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            client.DisConnect();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Serial.Start();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Serial.Disconnect();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (!Equals(formInfo, default))
                formInfo.ControlResize(this);
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (!Equals(formInfo, default))
                formInfo.ControlResize(this);
        }
    }

}

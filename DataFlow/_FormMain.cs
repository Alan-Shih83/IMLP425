using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataFlow
{
    public interface IFormUpdate
    {
        void UpdateUI(Btn_Msg msg);
        void UpdateUI(ListViewMsg msg);
    }

    public partial class _FormMain : Form, IFormUpdate
    {
        SynchronizationContext context = default(SynchronizationContext);
        ListViewSorter sorter = default(ListViewSorter);
        FormParameter parameter = new FormParameter();
        Client Client = new Client();
        Serial Serial = new Serial();
        public _FormMain()
        {
            InitializeComponent();
            context = SynchronizationContext.Current;
            ListViewInit();
            LogHandlerManager.Instance.GetLogHandler(LogType.Event).attach(EventLog);
            LogHandlerManager.Instance.GetLogHandler(LogType.Error).attach(ErrorLog);
            LogHandlerManager.Instance.GetLogHandler(LogType.None).attach(ErrorLog);
            LogHandlerManager.Instance.GetLogHandler(LogType.PLC_Status).attach(PLC_Status_Log);
            LogHandlerManager.Instance.GetLogHandler(LogType.Serial_Status).attach(RS232_Status_Log);
            FormManager.Instance.attach(this);
            IO_Init();
            ProcessInit();
            LoadStorage();
        }
        private void LoadStorage()
        {
            JsonFormat format = new JsonFormat();
            string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Storage.txt");
            string data = FileManager.Instance.GetFileOperator()?.Read(path);
            if (!string.IsNullOrEmpty(data))
            {
                string[] Separators = new string[] { "}\r\n" };
                string[] infos = data.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
                foreach (var _info in infos)
                {
                    RepositoryMessage info = format.Deserialize<RepositoryMessage>(_info + "}");
                    Repository.Instance.Update(info);
                }
            }
        }
        private void SaveStorage()
        {
            JsonFormat format = new JsonFormat();
            string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Storage.txt");
            StringBuilder builder = new StringBuilder();
            foreach (var record in Repository.Instance.GetRepositories())
            {
                string jsonstr = format.Serialize(record) + "\r\n";
                builder.Append(jsonstr);
            }
            FileManager.Instance.GetFileOperator()?.Write(path, builder.ToString(), System.IO.FileMode.Create);        
        }
        private void IO_Init()
        {
            string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Setting.ini");
            foreach (var attr in parameter.GetAttributes())
            {
                Dictionary<string, string> pairs = Ini.GetSectionDic(path, attr.Section);
                var Enumerator = pairs.GetEnumerator();
                while (Enumerator.MoveNext())
                    parameter.GetType().GetProperty(Enumerator.Current.Key).SetValue(parameter, Enumerator.Current.Value);
            }

            IP_Setting.Text = parameter.IP;
            Port_Setting.Text = parameter.Port;
            COM_Setting.Text = parameter.COM;
            RetryCount_text.Text = parameter.RetryCount;
            ResponseWaitingTime_text.Text = parameter.RetryTime;

            if (parameter.setTestMode  == "1") 
            {
                groupBox1.Visible = true;
            }
        }
        private void ProcessInit()
        {
            IProcessContainer LogicControlContainer = new LogicControlProcessContainer();
            (LogicControlContainer.GetFilter() as LogicControlFilter).Register(Client);
            ProcessManager.Instance.attach(LogicControlContainer);

            IProcessContainer SerialPortContainer = new SerialPortProcessContainer();
            (SerialPortContainer.GetFilter() as SerialPortFilter).Register(Serial);
            ProcessManager.Instance.attach(SerialPortContainer);
           
            Serial.Start("COM" + parameter.COM, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            Client.Start(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(parameter.IP), Convert.ToInt32(parameter.Port)));
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
                {
                    header.Text = "Slot";
                    sorter = new ListViewSorter(header.Index);
                }
            }
            RepositoryView.ListViewItemSorter = sorter;
            RepositoryView.Sort();
        }
        public void UpdateUI(ListViewMsg args)
        {
            context.Post(_ =>
            {
                try
                {
                    List<Control> controls = this.Controls.Find(args.Name, true).ToList();
                    foreach (var control in controls)
                    {
                        if (args.GetType() == typeof(ListViewInsertMsg))
                            FormOperate.ListViewItenInsert(control as ListView, args.GetFormat() as string[]);
                        else if (args.GetType() == typeof(ListViewDeleteMsg))
                            FormOperate.ListViewItemDelete(control as ListView, 1, args.GetFormat() as string);
                        else if (args.GetType() == typeof(ListViewClearMsg))
                        {
                            FormOperate.ListViewClearAll(control as ListView);
                            ListViewInit();
                        }  
                        else if(args.GetType() == typeof(ListViewBackColorMsg))
                            FormOperate.ListViewBackColor(control as ListView, 1, args.GetFormat() as string, args.GetColor());
                        else if(args.GetType() == typeof(ListViewBackColorClearMsg))
                            FormOperate.ListViewBackColorClear(control as ListView, args.GetColor());
                    }
                }
                catch (Exception ex)
                {
                    LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + " " + ex.Message);
                }
            }, null);
        }
        public void UpdateUI(Btn_Msg args)
        {
            context.Post(_ =>
            {
                try
                {
                    List<Control> controls = this.Controls.Find(args.Name, true).ToList();
                    foreach (var control in controls)
                    {
                        control.Enabled   = args.isEnable;
                        control.BackColor = args.backColor;
                    }
                }
               catch(Exception ex)
                {
                    LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + " " + ex.Message);
                }
            }, null);
        }

        private void Ethernet_Connect_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(IP_Setting.Text) && !string.IsNullOrEmpty(Port_Setting.Text))
                {
                    this.parameter.IP = IP_Setting.Text;
                    this.parameter.Port = Port_Setting.Text;
                    Client.Start(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(parameter.IP), Convert.ToInt32(parameter.Port)));
                }
                else
                    ClsMsgBox.MyWarning("IP 及 Port 不能為空字串");
            }
            catch(Exception ex)
            {
                LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(DateTime.Now + " " + this.ToString() + " " + ex.Message);
            }
            
        }

        private void Ethernet_Disconnect_btn_Click(object sender, EventArgs e)
        {
            Client.DisConnect();
        }

        private async void _FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _FormMain form = (sender as _FormMain);
            if (!(bool)form.Tag)
            {
                if (ClsMsgBox.MyConfirmation("確定關閉程式?"))
                {
                    e.Cancel = true;
                    try
                    {
                        Client.DisConnect();
                        Serial.Disconnect();

                        parameter.IP = IP_Setting.Text;
                        parameter.Port = Port_Setting.Text;
                        parameter.COM = COM_Setting.Text;
                        string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Setting.ini");
                        //FormParameter parameter = new FormParameter(IP_Setting.Text, Port_Setting.Text, COM_Setting.Text);
                        var properties = parameter.GetType().GetProperties();
                        foreach (var property in properties)
                        {
                            if (!Equals(property.GetValue(parameter), null))
                            {
                                SectionAttribute section = property.GetCustomAttributes(typeof(SectionAttribute), false).FirstOrDefault(sec => !string.IsNullOrEmpty((sec as SectionAttribute).Section)) as SectionAttribute;
                                if (!Equals(section, null))
                                    Ini.WritePrivateProfileString(section.Section, property.Name, property.GetValue(parameter).ToString(), path);
                            }
                        }
                        await FileManager.Instance.Cancel();
                        SaveStorage();
                        form.Tag = true;
                        form.Close();
                    }
                    catch (Exception ex)
                    {
                        LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + " " + ex.Message);
                        e.Cancel = true;
                    }
                }
                else
                    e.Cancel = true;
            }
            else
                e.Cancel = false;
            //if (!ClsMsgBox.MyConfirmation("確定關閉程式?"))
            //    e.Cancel = true;
            //else
            //{
            //    try
            //    {
            //        parameter.IP   = IP_Setting.Text;
            //        parameter.Port = Port_Setting.Text;
            //        parameter.COM  = COM_Setting.Text;
            //        //FormParameter parameter = new FormParameter(IP_Setting.Text, Port_Setting.Text, COM_Setting.Text);
            //        var properties = parameter.GetType().GetProperties();
            //        foreach (var property in properties)
            //        {
            //            if (!Equals(property.GetValue(parameter), null))
            //            {
            //                SectionAttribute section = property.GetCustomAttributes(typeof(SectionAttribute), false).FirstOrDefault(sec => !string.IsNullOrEmpty((sec as SectionAttribute).Section)) as SectionAttribute;
            //                if (!Equals(section, null))
            //                    Ini.WritePrivateProfileString(section.Section, property.Name, property.GetValue(parameter).ToString(), System.IO.Directory.GetCurrentDirectory() + "\\" + "Setting.ini");
            //            }
            //        }
            //    }
            //    catch(Exception ex)
            //    {
            //        LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + " " + ex.Message);
            //        e.Cancel = true;
            //    }
            //}
        }

        private void RS232_Connect_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(COM_Setting.Text))
                {
                    parameter.COM = COM_Setting.Text;
                    Serial.Start("COM" + parameter.COM, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
                }
                else
                    ClsMsgBox.MyWarning("COM 不能為空字串");
            }
            catch(Exception ex)
            {
                LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(DateTime.Now + " " + this.ToString() + " " + ex.Message);
            }
            
        }

        private void RS232_Disconnect_btn_Click(object sender, EventArgs e)
        {
            Serial.Disconnect();
        }

        private void SendSecond_Test_Click(object sender, EventArgs e)//////////////////////////////////////////////////////
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                string second = textBox1.Text;
                if (second.Contains("OK"))
                {
                    int index = second.IndexOf("OK");
                    if (index != -1)
                    {
                        second = second.Remove(index, "OK".Length);
                        ProcessManager.Instance.Query<LogicControlProcessContainer>()?.AddMessage(new SecondJudgmentMessage(second, 1));
                    }
                }
                else if (second.Contains("NG"))
                {
                    int index = second.IndexOf("NG");
                    if (index != -1)
                    {
                        second = second.Remove(index, "NG".Length);
                        ProcessManager.Instance.Query<LogicControlProcessContainer>()?.AddMessage(new SecondJudgmentMessage(second, 2));
                    }
                }

                //List<byte> arr = Encoding.ASCII.GetBytes(textBox1.Text).ToList();
                //arr.Insert(0, 2);
                //arr.Insert(arr.Count, 3);
                //(ProcessManager.Instance.Query<SerialPortProcessContainer>()?.GetFilter() as SerialPortFilter)?.update(arr.ToArray());
            }
        }

        private void SendFirst_Test_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ID_Test.Text) && !string.IsNullOrEmpty(SLOT_Test.Text))
            {
                try
                {
                    int level = Convert.ToInt32(SLOT_Test.Text);
                    FirstJudgmentMessageTest first_test = new FirstJudgmentMessageTest(ID_Test.Text, level);
                    ProcessManager.Instance.Query<LogicControlProcessContainer>()?.AddMessage(first_test);
                }
                catch (Exception) { }
                
                //List<byte> arr = Encoding.ASCII.GetBytes(ID_Test.Text).ToList();
                //arr.Insert(0, 2);
                //arr.Insert(arr.Count, 3);
                //(ProcessManager.Instance.Query<SerialPortProcessContainer>()?.GetFilter() as SerialPortFilter)?.update(arr.ToArray());
            }
        }

        private void SendRequest_btn_Click(object sender, EventArgs e)
        {
            ProcessManager.Instance.Query<SerialPortProcessContainer>()?.AddMessage(new FirstJudgmentMessage(default, -1));
        }

        private void ResponseWaitingTime_text_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x0D && !string.IsNullOrEmpty(ResponseWaitingTime_text.Text))
            {
                try
                {
                    double ResponseWaitingTime = Convert.ToDouble(ResponseWaitingTime_text.Text);
                    if(ResponseWaitingTime > 0)
                    {
                        (ProcessManager.Instance.Query<SerialPortProcessContainer>()?.GetVisitor() as SerialPortStatusVisitor)?.SetResponseWaitingTime(ResponseWaitingTime);
                        parameter.RetryTime = ResponseWaitingTime_text.Text;
                    }
                    else
                        ClsMsgBox.MyWarning("ResponseWaitingTime 設定錯誤(數值必須為大於0的數值). ");
                }
                catch (Exception)
                {
                    ClsMsgBox.MyWarning("ResponseWaitingTime 設定錯誤(數值必須為大於0的數值). ");
                }
            }
        }

        private void RetryCount_text_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x0D && !string.IsNullOrEmpty(RetryCount_text.Text))
            {
                try
                {
                    int RetryCount = Convert.ToInt32(RetryCount_text.Text);
                    if(RetryCount >= 0)
                    {
                        (ProcessManager.Instance.Query<SerialPortProcessContainer>()?.GetVisitor() as SerialPortStatusVisitor)?.SetRetryCount(RetryCount);
                        parameter.RetryCount = RetryCount_text.Text;
                    }
                    else
                        ClsMsgBox.MyWarning("RetryCount 設定錯誤(數值必須為非負整數). ");
                }
                catch (Exception)
                {
                    ClsMsgBox.MyWarning("RetryCount 設定錯誤(數值必須為非負整數). ");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(textBox2.Text))
            {
                (ProcessManager.Instance.Query<SerialPortProcessContainer>()?.GetFilter() as SerialPortFilter)?.GetSendPipleLine()?.Push(new ProcessPipe(new SerialData(textBox2.Text)));
            }
            //ProcessManager.Instance.Query<SerialPortProcessContainer>()?.AddMessage(new FirstJudgmentMessage(default, -1));
        }

       
    }


    public class ListViewSorter : IComparer
    {
        int Order = default;
        public ListViewSorter(int Order)
        {
            this.Order = Order;
        }
        public void SetSortColumn(int Order)
        {
            this.Order = Order;
        }
        public int Compare(object x, object y)
        {
            string Text1 = ((ListViewItem)x).SubItems[Order].Text;
            string Text2 = ((ListViewItem)y).SubItems[Order].Text;
            if (Text1.Length == Text2.Length)
                return String.CompareOrdinal(Text1, Text2);
            else
                return Text1.Length - Text2.Length;
        }
    }
}

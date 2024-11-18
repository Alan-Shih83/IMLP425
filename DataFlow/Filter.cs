using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataFlow
{
    public interface IFliter { }

    public class SerialPortFilter : Observer<byte[]>, IFliter
    {
        volatile int sharedStorage = 0;
        SemaphoreManage ControlSemaphore = new SemaphoreManage(1, 1);
        TemporaryStorage<string> TemporaryStorage = new TemporaryStorage<string>(new SerialPortDecoding());
        BasicPipleLine<ProcessPipe> BasicReceivePipleLine = new BasicPipleLine<ProcessPipe>();
        BasicPipleLine<ProcessPipe> BasicSendPipleLine = new BasicPipleLine<ProcessPipe>();
        public SerialPortFilter()
        {
            _ = BasicReceivePipleLine.Flow();
            _ = BasicSendPipleLine.Flow(PipleFlow);
        }
        public BasicPipleLine<ProcessPipe> GetReceivePipleLine()
        {
            return BasicReceivePipleLine;
        }
        public BasicPipleLine<ProcessPipe> GetSendPipleLine()
        {
            return BasicSendPipleLine;
        }
        public override void update(byte[] update)
        {
            foreach (var data in TemporaryStorage.Decode(update))
            {
                if(!string.IsNullOrEmpty(data))
                    BasicReceivePipleLine.Push(new ProcessPipe(new SerialData(data)).SetDirection(Direction.SerialPort));
            }
        }
        public override void error(byte[] error)           
        {
            ControlSemaphore.WaitOne();
            Interlocked.Exchange(ref sharedStorage, 1);
            TemporaryStorage.Clear();
            BasicReceivePipleLine.Clear();
            BasicSendPipleLine.Clear();
            BasicReceivePipleLine.Push(new ProcessPipe(new DisConnect()).SetNotifyType(NotifyType.Error).SetDirection(Direction.SerialPort));
            FormManager.Instance.Query<_FormMain>()?.UpdateUI(new Btn_Msg("RS232_Connect_btn"));
            FormManager.Instance.Query<_FormMain>()?.UpdateUI(new Btn_Msg("RS232_Disconnect_btn", Color.Lime));
            ControlSemaphore.Release();
        }
        public override void Connect(byte[] Connect)
        {
            ControlSemaphore.WaitOne();
            Interlocked.Exchange(ref sharedStorage, 0);
            BasicReceivePipleLine.Push(new ProcessPipe(new Connect()).SetNotifyType(NotifyType.Connect).SetDirection(Direction.SerialPort));
            FormManager.Instance.Query<_FormMain>()?.UpdateUI(new Btn_Msg("RS232_Connect_btn", Color.Lime));
            FormManager.Instance.Query<_FormMain>()?.UpdateUI(new Btn_Msg("RS232_Disconnect_btn"));
            ControlSemaphore.Release();
        }
        private void Response(byte[] message)
        {
            Serial serial = this.GetSubject() as Serial;
            string str = Extensions.GetASCIIToHexStr(message) + " (" + Encoding.ASCII.GetString(message) + ")";
            LogHandlerManager.Instance.GetLogHandler(LogType.Serial_Status).refresh(DateTime.Now + " RS232 Send: " + str);
            LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " RS232 Send: " + str);
            serial?.Send(message);
        }
        private async Task PipleFlow(ProcessPipe pipe)
        {
            await Task.Yield();
            int _sharedStorage = Interlocked.CompareExchange(ref sharedStorage, 0, 0);
            if (_sharedStorage == 0)
            {
                if (pipe?.GetData() is Watchdog)
                {
                    //LogHandlerManager.Instance.GetLogHandler(LogType.PLC_Status).refresh(DateTime.Now + " " + this.ToString() + " Watchdog. ");
                    BasicReceivePipleLine.Push(pipe);
                }
                else
                {
                    SerialData sd = pipe?.GetData() as SerialData;
                    if (!Equals(sd, default(SerialData)))
                    {
                        List<byte> msg = Encoding.ASCII.GetBytes(sd.message).ToList();
                        msg.Insert(0, Extensions.isExistinEnum<Command, byte>("STX"));
                        msg.Insert(msg.Count, Extensions.isExistinEnum<Command, byte>("ETX"));
                        Response(msg.ToArray());
                    }
                }
            }
        }
    }

    public class LogicControlFilter : Observer<ConnectPipe>, IFliter
    {
        TemporaryStorage<byte[]> TemporaryStorage = new TemporaryStorage<byte[]>(new LogicControlDecoding());
        ProcessPipeControl ProcessPipeControl = new ProcessPipeControl();
        SemaphoreManage Semaphore = new SemaphoreManage(1, 1);
        SemaphoreManage ControlSemaphore = new SemaphoreManage(1, 1);
        BasicPipleLine<ProcessPipe> BasicReceivePipleLine = new BasicPipleLine<ProcessPipe>();
        BasicPipleLine<ProcessPipe> BasicSendPipleLine = new BasicPipleLine<ProcessPipe>();
        volatile int sharedStorage = 0;
        public LogicControlFilter()
        {
            _ = BasicReceivePipleLine.Flow();
            _ = BasicSendPipleLine.Flow(PipleFlow);
        }
        public BasicPipleLine<ProcessPipe> GetReceivePipleLine()
        {
            return BasicReceivePipleLine;
        }
        public BasicPipleLine<ProcessPipe> GetSendPipleLine()
        {
            return BasicSendPipleLine;
        }
        public override void update(ConnectPipe update)
        {
            foreach (var data in TemporaryStorage.Decode(update.GetData() as byte[]))
            {
                ProcessPipe pipe = ProcessPipeControl.Assemble(data);
                if (!Equals(pipe, default) && ProcessPipeControl.isEmpty())
                {
                    BasicReceivePipleLine.Push(pipe);
                    Semaphore.Release();
                }
                else
                {
                    byte[] message = ProcessPipeControl.Encapsulate();
                    if (!Equals(message, default))
                        Response(new ConnectPipe(message));
                }
            }
        }
        public override void error(ConnectPipe error)
        {
            ControlSemaphore.WaitOne();
            Interlocked.Exchange(ref sharedStorage, 1);
            //LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " Filter Receive Disconnect. ");
            BasicReceivePipleLine.Clear();
            BasicSendPipleLine.Clear();
            ProcessPipeControl.Clear();
            TemporaryStorage.Clear();
            Semaphore.Release();
            BasicReceivePipleLine.Push(new ProcessPipe(new DisConnect()).SetNotifyType(NotifyType.Error));
            //LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(this.ToString() + " Filter Clear.");
            FormManager.Instance.Query<_FormMain>()?.UpdateUI(new Btn_Msg("Ethernet_Disconnect_btn", Color.Lime));
            FormManager.Instance.Query<_FormMain>()?.UpdateUI(new Btn_Msg("Ethernet_Connect_btn"));
            ControlSemaphore.Release();
        }
        public override void Connect(ConnectPipe Connect)
        {
            ControlSemaphore.WaitOne();
            Interlocked.Exchange(ref sharedStorage, 0);
            //LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " Filter Receive Connect. ");
            FormManager.Instance.Query<_FormMain>()?.UpdateUI(new Btn_Msg("Ethernet_Connect_btn", Color.Lime));
            FormManager.Instance.Query<_FormMain>()?.UpdateUI(new Btn_Msg("Ethernet_Disconnect_btn"));
            BasicReceivePipleLine.Push(new ProcessPipe(new Connect()).SetNotifyType(NotifyType.Connect));
            ControlSemaphore.Release();
        }
        private void Response(ConnectPipe pipe)
        {           
            Client client = this.GetSubject() as Client;
            client?.GetSendPipleLine().Push(pipe);
        }
        private async Task PipleFlow(ProcessPipe pipe)
        {
            await Task.Yield();
            Semaphore.WaitOne();
            int _sharedStorage = Interlocked.CompareExchange(ref sharedStorage, 0, 0);
            if(_sharedStorage == 0)
            {
                ProcessPipeControl.SetProcessPipe(pipe);
                byte[] message = ProcessPipeControl.Encapsulate();
                if (!Equals(message, default))
                    Response(new ConnectPipe(message));
                else
                    Semaphore.Release();
            }
            else
                Semaphore.Release();
        }
    }
}

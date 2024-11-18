using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Management;

namespace DataFlow
{
    public class Serial : Subject<byte[]>
    {
        private SerialPort SerialPort;
        private volatile int sharedStorage = 0;
        private volatile int startStorage = 0;
        private List<Task> tasks = new List<Task>();
        private List<CancellationTokenSource> sources = new List<CancellationTokenSource>();
       
        public void Start(string PortName, int BaudRate = 9600, Parity Parity = Parity.None, int DataBits = 8, StopBits StopBits = StopBits.One)
        {
            if (0 == Interlocked.Exchange(ref startStorage, 1))
            {
                this.SerialPort = new SerialPort(PortName, BaudRate, Parity, DataBits, StopBits);
                sources.Clear();
                tasks.Add(Task.Run(async() => { await ConnectDetecion(); }));
                //_= notifyObservers(default(byte[]), NotifyType.Connect);///////////////////////////////////////////////////////////////////////////////////////
                //tasks.Add(Task.Run(async() => { await Receive(source); }));
            }
            else
                LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " SerialPort(" + this.SerialPort.PortName + ") has already Started.");
        }
       
        private async Task ConnectDetecion()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            sources.Add(source);
            try
            {
                while (!source.IsCancellationRequested)
                {
                    await Task.Yield();
                    string port = default(string);
                    using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
                    {
                        string portname = this.SerialPort.PortName;
                        var ports = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString());
                        port = ports.FirstOrDefault(p => p.Contains(portname));
                    }

                    int _sharedStorage = Interlocked.CompareExchange(ref sharedStorage, 0, 0);
                    if(_sharedStorage == 2)
                        _= Disconnect(false).ConfigureAwait(false);
                    else if (string.IsNullOrEmpty(port) && _sharedStorage == 1)
                        _ = Disconnect(true).ConfigureAwait(false);
                    else if (!string.IsNullOrEmpty(port) && _sharedStorage == 0)
                    {
                        Open();
                        Interlocked.Exchange(ref sharedStorage, 1);
                        LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " SerialPort(" + this.SerialPort.PortName + ") Connected.");
                        await notifyObservers(default(byte[]), NotifyType.Connect);
                        tasks.Add(Task.Run(async () => { await Receive(); }));
                    }
                    else if(string.IsNullOrEmpty(port) && _sharedStorage == 0)
                    {
                        LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " SerialPort(" + this.SerialPort.PortName + ") Connect Searching.");
                        Thread.Sleep(1000);
                    } 
                    else
                        Thread.Sleep(1000);
                }
            }
            catch(Exception ex)
            {
                LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(DateTime.Now + " " + this.ToString() + " " + ex.Message);
            }
            finally
            {
                source.Dispose();
                LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " ConnectDetecion Released.");
            }
        }
        private async Task Receive()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            sources.Add(source);
            try
            {
                byte[] buffer = new byte[1];
                while (!source.IsCancellationRequested)
                {
                    if (this.SerialPort.BytesToRead > 0)
                    {
                        int length = this.SerialPort.Read(buffer, 0, 1);
                       // LogHandlerManager.Instance.GetLogHandler(LogType.Serial_Status).refresh(DateTime.Now + " buffer Receive: " + Extensions.GetASCIIToHexStr(buffer));
                        if(length > 0)
                            await notifyObservers(buffer, NotifyType.Update);
                        Thread.Sleep(20);
                    }
                    else
                        Thread.Sleep(50);
                }
            }
            catch(Exception ex)
            {
                LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(DateTime.Now + " " + this.ToString() + " " + ex.Message);
            }
            finally
            {
                source.Dispose();
                LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " SerialPort(" + this.SerialPort.PortName + ") Receive Released.");
            }
            
        }
        private void Open()
        {
            if (!Equals(SerialPort, default(SerialPort)) && !SerialPort.IsOpen)
            {
                this.SerialPort.Open();
            }
        }
        public void Send(byte[] message)
        {
            if (!Equals(SerialPort, default(SerialPort)) && SerialPort.IsOpen && !Equals(message, default(byte[])))
                this.SerialPort.Write(message, 0, message.Length);
        }
        public void Disconnect()
        {
            Interlocked.Exchange(ref sharedStorage, 2);
        }
        private async Task Disconnect(bool reconnect = true)
        {
            string name = this.SerialPort.PortName;
            foreach (var source in sources)
                source?.Cancel();
            await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false);
            await notifyObservers(default(byte[]), NotifyType.Error);
            Interlocked.Exchange(ref startStorage,  0);
            Interlocked.Exchange(ref sharedStorage, 0);
            if (!Equals(SerialPort, default(SerialPort)) && SerialPort.IsOpen)
                this.SerialPort.Close();
            LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " SerialPort(" + this.SerialPort.PortName + ") Disconnected.");
            if(reconnect)
               Start(name);
        }
    }
}

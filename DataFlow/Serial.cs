using System;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace WindowsFormsApp1
{
    public interface ISerialPort_Operation
    {
        void SerialPort_Close(SerialObject serial);

        void SerialPort_Open(SerialObject serial);

        string[] SerialPort_Read(SerialObject serial);

        void SerialPort_ReadCancel(SerialObject serial);

        void SerialPort_Release(SerialObject serial);

        void SerialPort_Write(SerialObject serial, string data);
    }

    public class IG028_Comport : SerialObject
    {
        private Task readTask;

        public IG028_Comport(string portName, int baudRate, System.IO.Ports.Parity parity, int dataBits, System.IO.Ports.StopBits stopBits) : base(portName, baudRate, parity, dataBits, stopBits)
        {
            source = new CancellationTokenSource();
            //readTask = Task.Factory.StartNew(DataReceive);
            _MessageHandler += MessageDataHandle;
        }

        public delegate string[] MessageHandleDelegate(string message);

        public event MessageHandleDelegate _MessageHandler;

        public override void read()
        {
            readTask = Task.Run(DataReceive);
        }

        protected override void DataReceive()
        {
            Byte[] buffer = new Byte[1];
            string message = "";
            try
            {
                while (!source.IsCancellationRequested)
                {
                    if (serialport.BytesToRead > 0)
                    {
                        Int32 length = serialport.Read(buffer, 0, 1);
                        string buf = Encoding.ASCII.GetString(buffer);
                        message += buf;
                        if (buf == "\n")
                        {
                            IAsyncResult result = _MessageHandler.BeginInvoke(message, new AsyncCallback(MessageDataSet), null);
                            message = "";
                        }
                    }
                    Thread.Sleep(20);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                source.Dispose();
                _MessageHandler -= MessageDataHandle;
            }
        }

        protected override string[] MessageDataHandle(string message)
        {
            string[] data_Array = message.Split(new string[] { "\r\n", "," }, StringSplitOptions.RemoveEmptyEntries);
            if (data_Array.Length > 1 && data_Array[0] == "M0")
            {
                return data_Array.Where((index) => index != "M0").ToArray();
            }
            else
            {
                return new string[0];
            }
        }

        protected override void MessageDataSet(IAsyncResult message)
        {
            AsyncResult result = (AsyncResult)message;
            MessageHandleDelegate caller = (MessageHandleDelegate)result.AsyncDelegate;
            string[] returnValue = caller.EndInvoke(message);

            if (returnValue.Length > 1)
                Data = returnValue;
        }
    }

    public abstract class SerialObject
    {
        public SerialPort serialport;

        public CancellationTokenSource source;

        protected readonly object Lock = new object();

        private string[] data = null;

        public SerialObject(string portName, int baudRate, System.IO.Ports.Parity parity, int dataBits, System.IO.Ports.StopBits stopBits)
        {
            try
            {
                serialport = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            }
            catch (System.IO.IOException ex)
            {
                System.Windows.Forms.MessageBox.Show(Convert.ToString(ex));
            }
        }

        public string[] Data
        {
            get { lock (Lock) { return data; } }
            set { lock (Lock) { data = value; } }
        }

        public abstract void read();

        protected abstract void DataReceive();

        protected abstract string[] MessageDataHandle(string message);

        protected abstract void MessageDataSet(IAsyncResult message);
    }

    public class SerialOperation : ISerialPort_Operation
    {
        private static SerialOperation _SerialOperation = null;

        private SerialOperation()
        {
        }

        public static SerialOperation getInstance()
        {
            if (_SerialOperation == null)
            {
                _SerialOperation = new SerialOperation();
            }
            return _SerialOperation;
        }

        public void SerialPort_Close(SerialObject serial)
        {
            if (serial.serialport != null && serial.serialport.IsOpen)
            {
                SerialPort_ReadCancel(serial);
                //serial.serialport.Close();
                SerialPort_Release(serial);
            }
        }

        public void SerialPort_Open(SerialObject serial)
        {
            if (serial.serialport != null && !serial.serialport.IsOpen)
            {
                serial.serialport.Open();
                serial.read();
            }
        }

        public string[] SerialPort_Read(SerialObject serial)
        {
            if (serial != null)
                return serial.Data;
            else
                return null;
        }

        public void SerialPort_ReadCancel(SerialObject serial)
        {
            try
            {
                if (serial.source != null && !serial.source.IsCancellationRequested)
                    serial.source.Cancel();
            }
            catch
            {
            }
        }

        public void SerialPort_Release(SerialObject serial)
        {
            try
            {
                SerialPort_ReadCancel(serial);
                object stream = typeof(SerialPort).GetField("internalSerialStream", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(serial.serialport);
                SafeFileHandle handle_Com1 = (SafeFileHandle)stream.GetType().GetField("_handle", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(stream);
                handle_Com1.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                //serial.serialport.Dispose();
                serial.serialport = null;
            }
        }

        public void SerialPort_Write(SerialObject serial, string data)
        {
            try
            {
                if (serial.serialport != null && serial.serialport.IsOpen)
                {
                    serial.serialport.Write(data + "\r\n");
                }
            }
            catch (System.IO.IOException)
            {
                SerialPort_Close(serial);
                SerialPort_Release(serial);
            }
        }
    }
}
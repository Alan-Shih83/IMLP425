using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataFlow
{
    class SocketAsyncEventArgsPool
    {
        Stack<SocketAsyncEventArgs> m_pool;

        public SocketAsyncEventArgsPool(int capacity)
        {
            m_pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null) { throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null"); }
            lock (m_pool)
            {
                m_pool.Push(item);
            }
        }
        public SocketAsyncEventArgs Pop()
        {
            lock (m_pool)
            {
                return m_pool.Pop();
            }
        }
        public int Count
        {
            get { return m_pool.Count; }
        }

        public void Release()
        {
            m_pool.Clear();
        }
    }

    class AsyncUserToken
    {
        Socket socket = default(Socket);
        public AsyncUserToken() : this(null) { }
        public AsyncUserToken(Socket socket)
        {
            this.socket = socket;
        }
        public Socket Socket
        {
            get { return socket; }
            set { socket = value; }
        }
    }

    class BufferManager
    {
        private int m_numBytes;
        private int m_currentIndex;
        private int m_bufferSize;
        private byte[] m_buffer;

        private Stack<int> m_freeIndexPool;

        public BufferManager(int totalBytes, int bufferSize)
        {
            m_numBytes = totalBytes;
            m_currentIndex = 0;
            m_bufferSize = bufferSize;
            m_freeIndexPool = new Stack<int>();
        }

        public void InitBuffer()
        {
            m_buffer = new byte[m_numBytes];
        }

        public bool SetBuffer(SocketAsyncEventArgs args)
        {

            if (m_freeIndexPool.Count > 0)
            {
                args.SetBuffer(m_buffer, m_freeIndexPool.Pop(), m_bufferSize);
            }
            else
            {
                if ((m_numBytes - m_bufferSize) < m_currentIndex)
                {
                    return false;
                }
                args.SetBuffer(m_buffer, m_currentIndex, m_bufferSize);
                m_currentIndex += m_bufferSize;
            }
            return true;
        }

        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            m_freeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }

        public void ReleaseBuffer()
        {
            m_freeIndexPool.Clear();
        }
    }

    public class Client : Subject<ConnectPipe>
    {
        private Socket Socket;
        private IPEndPoint EndPoint;
        private const int ReceiveSize = 1024;//2048
        private BufferManager BufferManager = new BufferManager(ReceiveSize * 2, ReceiveSize);//4
        private volatile int sharedStorage = 0;
        private volatile int StartStorage = 0;
        private BasicPipleLine<ConnectPipe> BasicReceivePipleLine = new BasicPipleLine<ConnectPipe>();
        private BasicPipleLine<ConnectPipe> BasicSendPipleLine    = new BasicPipleLine<ConnectPipe>();
        public Client(/*IPEndPoint EndPoint*/)
        {
            //this.EndPoint = EndPoint;
            this.BufferManager.InitBuffer();
            _ = BasicSendPipleLine.Flow(SendPipleFlow);
            _ = BasicReceivePipleLine.Flow(ReceivePipleFlow);
        }

        ~Client()
        {
            BufferManager.ReleaseBuffer();
        }

        private async Task ReceivePipleFlow(ConnectPipe Pipe) { await notifyObservers(Pipe, Pipe.GetNotifyType()); }

        private async Task SendPipleFlow(ConnectPipe Pipe) { await Task.Yield(); Send(Pipe.GetData() as byte[]); }

        public BasicPipleLine<ConnectPipe> GetSendPipleLine()
        {
            return BasicSendPipleLine;
        }

        public void Start(IPEndPoint EndPoint)
        {
            if (0 == Interlocked.Exchange(ref StartStorage, 1))
            {
                this.EndPoint = EndPoint;
                LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " " + this.EndPoint.ToString() + " Start." );
                _Start();
            } 
            else
                LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " has already Started.");
        }

        private void _Start()
        {
            int _sharedStorage = Interlocked.CompareExchange(ref sharedStorage, 0, 0);
            if (_sharedStorage == 0)
            {
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                SocketAsyncEventArgs ConnectEventArg = new SocketAsyncEventArgs() { RemoteEndPoint = EndPoint };
                ConnectEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                Socket.IOControl(IOControlCode.KeepAliveValues, GetKeepAliveSetting(1, 500, 100), null);
                Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                StartConnect(ConnectEventArg);
            }
            else if(_sharedStorage != 2)
                Task.Run(() =>
                {
                    Thread.Sleep(50);
                    LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " " +_sharedStorage.ToString() );
                    _Start();
                });
        }

        private void StartConnect(SocketAsyncEventArgs ConnectEventArg)
        {
            if (ConnectEventArg == null)
                _Start();
            else
            {
                bool willRaiseEvent = Socket.ConnectAsync(ConnectEventArg);
                if (!willRaiseEvent)
                    ProcessConnect(ConnectEventArg);
            }
        }

        private void ProcessConnect(SocketAsyncEventArgs e)
        {
            try
            {
                int _sharedStorage = Interlocked.CompareExchange(ref sharedStorage, 0, 0);
                if (e.SocketError == SocketError.Success && !Equals(e.ConnectSocket, default(Socket)) && e.ConnectSocket.Connected)
                {
                    BasicReceivePipleLine.Push(new ConnectPipe(default).SetNotifyType(NotifyType.Connect));
                    SocketAsyncEventArgs ReadAsyncEventArgs = new SocketAsyncEventArgs();
                    ReadAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                    ReadAsyncEventArgs.UserToken = new AsyncUserToken(e.ConnectSocket);
                    BufferManager.SetBuffer(ReadAsyncEventArgs);

                    LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " " + e.RemoteEndPoint.ToString() + " Connect. ");
                    bool willRaiseEvent = e.ConnectSocket.ReceiveAsync(ReadAsyncEventArgs);
                    if (!willRaiseEvent)
                        ProcessReceive(ReadAsyncEventArgs);
                }
                else if(_sharedStorage == 2)
                {
                    e.Dispose();
                    DisConnect(false);
                } 
                else
                    Task.Run(() => { Thread.Sleep(50); LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " " + e.RemoteEndPoint.ToString() + " ReConnect. ");  StartConnect(e); });
            }
            catch (Exception ex)
            {
                LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(DateTime.Now + " " + this.ToString() + " [ProcessConnect] " + ex.ToString());
            }
        }

        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                case SocketAsyncOperation.Connect:
                    ProcessConnect(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            try
            {
                if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
                {
                    AsyncUserToken token = (AsyncUserToken)e.UserToken;
                    BasicReceivePipleLine.Push(new ConnectPipe(e.Buffer.Skip(e.Offset).Take(e.BytesTransferred).ToArray()));
                    //string result = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
                    //Console.WriteLine(result);
                   // Connect.Request(new ConnectPipe(token.Socket, e.Buffer.Skip(e.Offset).Take(e.BytesTransferred).ToArray()));
                    //ResponseCaller?.BeginInvoke(this, e.Buffer.Skip(e.Offset).Take(e.BytesTransferred).ToArray(), null, null);
                    BufferManager.FreeBuffer(e);
                    BufferManager.SetBuffer(e);

                    bool willRaiseEvent = token.Socket.ReceiveAsync(e);
                    if (!willRaiseEvent)
                        ProcessReceive(e);
                }
                else if(e.SocketError == SocketError.OperationAborted)
                {
                    BufferManager.FreeBuffer(e);
                    Interlocked.Exchange(ref sharedStorage, 0);
                }
                else
                {
                    BufferManager.FreeBuffer(e);
                    DisConnect(true);
                }
            }
            catch (Exception ex)
            {
                LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(DateTime.Now + " " + this.ToString() + " [ProcessReceive] " + ex.ToString());
            }
        }

        public void Send(byte[] message)
        {
            if (Socket == default(Socket)) return;
            else if (Socket.Connected)
            {
                SocketAsyncEventArgs WriteAsyncEventArgs = new SocketAsyncEventArgs();
                WriteAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                WriteAsyncEventArgs.SetBuffer(message, 0, message.Length);

                bool willRaiseEvent = Socket.SendAsync(WriteAsyncEventArgs);
                if (!willRaiseEvent)
                {
                    ProcessSend(WriteAsyncEventArgs);
                }
            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            e.Dispose();
        }

        private byte[] GetKeepAliveSetting(int onOff, int keepAliveTime, int keepAliveInterval)
        {
            byte[] buffer = new byte[12];
            BitConverter.GetBytes(onOff).CopyTo(buffer, 0);
            BitConverter.GetBytes(keepAliveTime).CopyTo(buffer, 4);
            BitConverter.GetBytes(keepAliveInterval).CopyTo(buffer, 8);
            return buffer;
        }

        public void DisConnect()
        {
            if(!Equals(Socket, default(Socket)))
            {
                if (Socket.Connected)
                {
                    Interlocked.CompareExchange(ref sharedStorage, 1, 0);
                    DisConnect(false);
                }
                else
                    Interlocked.CompareExchange(ref sharedStorage, 2, 0);
            }     
        }

        private void DisConnect(bool reconnect = true)
        {
            if (Socket != default(Socket))
            {
                try
                {
                    if (Socket.Connected)
                        Socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception ex)
                {
                    LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(DateTime.Now + " " + this.ToString() + " [DisConnect] " + ex.ToString());
                }
                finally
                {
                    Socket.Close();
                    Socket = default(Socket);
                    BasicReceivePipleLine.Clear();
                    BasicSendPipleLine.Clear();
                    BasicReceivePipleLine.Push(new ConnectPipe(default).SetNotifyType(NotifyType.Error));
                    LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " DisConnect. ");

                    if (reconnect)
                        _Start();
                    else
                    {
                        Interlocked.Exchange(ref StartStorage, 0);
                        Interlocked.Exchange(ref sharedStorage, 0);
                        LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " DisReconnect . ");
                    }    
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NXT_Hermes
{

    public interface ISocket
    {
        void Start();
        void Send(Socket socket, byte[] sendBuffer);
        void ProcessReceive(SocketAsyncEventArgs e);
        void ProcessSend(SocketAsyncEventArgs e);
        void DisConnect(bool reconnect = false);
        void DisConnect(Socket socket);
        
    }
    public class SocketAsyncEventArgsPool
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

    public class BufferManager
    {
        private int    m_numBytes;
        private int    m_currentIndex;
        private int    m_bufferSize;
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

    public class Server : ISocket
    {
        //private const int opsToPreAlloc = 2;
        private const int ReceiveSize   = 1024;

        private int TotalConnections = 0;
          
        private BufferManager BufferManager;
        private IPEndPoint    EndPoint;

        private Socket Socket = default(Socket);           
        private SocketAsyncEventArgsPool EventArgsPool;

        private SemaphoreManage NumberAcceptedClients;
        //private volatile int sharedStorage = 0;
        private readonly object Lock = new object();
        private bool close = false;
       

        public Server(IPEndPoint EndPoint, int Connections)
        {
            this.TotalConnections      = Connections;
            this.EndPoint              = EndPoint;
            this.BufferManager         = new BufferManager(ReceiveSize * Connections * 2, ReceiveSize);
            this.EventArgsPool         = new SocketAsyncEventArgsPool(Connections);
            this.NumberAcceptedClients = new SemaphoreManage(Connections, Connections);
            
            Init(Connections);
        }

        public void Init(int Connections)
        {
            BufferManager.InitBuffer();
         
            for (int conn = 0; conn < Connections; ++conn)
            {
                SocketAsyncEventArgs ReadAsyncEventArgs = new SocketAsyncEventArgs();
                ReadAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                ReadAsyncEventArgs.UserToken = new AsyncUserToken();

                BufferManager.SetBuffer(ReadAsyncEventArgs);
                EventArgsPool.Push(ReadAsyncEventArgs);
            }
        }

        public void Start()
        {
            Socket = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Socket.Bind(EndPoint);
            Socket.Listen(TotalConnections);
            Socket.IOControl(IOControlCode.KeepAliveValues, GetKeepAliveSetting(1, 500, 100), null);
            Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            StartAccept(null);
        }

        public void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            }
            else
                acceptEventArg.AcceptSocket = null;

            NumberAcceptedClients.WaitOne();
            if (!close)
            {
                bool willRaiseEvent = Socket.AcceptAsync(acceptEventArg);
                if (!willRaiseEvent)
                {
                    ProcessAccept(acceptEventArg);
                }
            }
        }
        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            //=================================================================================
            if (!close)
            {
                SocketAsyncEventArgs readEventArgs = EventArgsPool.Pop();
                ((AsyncUserToken)readEventArgs.UserToken).Socket = e.AcceptSocket;
                bool willRaiseEvent = e.AcceptSocket.ReceiveAsync(readEventArgs);
                if (!willRaiseEvent)
                {
                    ProcessReceive(readEventArgs);
                }

                
                StartAccept(e);
            }
        }

        void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                case SocketAsyncOperation.Accept:
                    ProcessAccept(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

        public void ProcessReceive(SocketAsyncEventArgs e)
        {
            try
            {
                AsyncUserToken token = (AsyncUserToken)e.UserToken;
                if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
                {
                    

                    BufferManager.FreeBuffer(e);
                    BufferManager.SetBuffer(e);

                    bool willRaiseEvent = token.Socket.ReceiveAsync(e);
                    if (!willRaiseEvent)
                    {
                        ProcessReceive(e);
                    }
                }
                else
                {
                    CloseClientSocket(e);
                    NumberAcceptedClients.Release();
                    BufferManager.FreeBuffer(e);
                    BufferManager.SetBuffer(e);
                    EventArgsPool.Push(e);
                }    
            }
            catch (Exception ex) 
            {
                LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + " [ProcessReceive] " + ex.ToString());
            }
            
        }

        public void Send(Socket socket, byte[] message)
        {
            if(socket != default(Socket) && socket.Connected)
            {
                SocketAsyncEventArgs WriteAsyncEventArgs = new SocketAsyncEventArgs();
                WriteAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                WriteAsyncEventArgs.UserToken = new AsyncUserToken(socket);
                WriteAsyncEventArgs.SetBuffer(message, 0, message.Length);

                bool willRaiseEvent = socket.SendAsync(WriteAsyncEventArgs);
                if (!willRaiseEvent)
                {
                    ProcessSend(WriteAsyncEventArgs);
                }
            }
        }

        public void ProcessSend(SocketAsyncEventArgs e)
        {
            e.Dispose();
            //if (e.SocketError != SocketError.Success)
            //{
            //    CloseClientSocket(e);
            //}
        }

        private byte[] GetKeepAliveSetting(int onOff, int keepAliveTime, int keepAliveInterval)
        {
            byte[] buffer = new byte[12];
            BitConverter.GetBytes(onOff).CopyTo(buffer, 0);
            BitConverter.GetBytes(keepAliveTime).CopyTo(buffer, 4);
            BitConverter.GetBytes(keepAliveInterval).CopyTo(buffer, 8);
            return buffer;
        }

        public void DisConnect(bool reconnect = false)
        {

            //=======================================================
            //bool lockTaken = false;
            //try
            //{
            //    Monitor.TryEnter(Lock, ref lockTaken);
            //    if (lockTaken)
            //    {
            //        close = true;
            //        Connect.detach();
            //        CloseClientSocket();
            //        CloseServer(reconnect);
            //        if (!reconnect)
            //        {
            //            BufferManager.ReleaseBuffer();
            //            EventArgsPool.Release();
            //        }
            //    }
            //    else
            //    {
            //        Task.Run(() => { DisConnect(reconnect); });
            //    }
            //}
            //finally
            //{
            //    if (lockTaken)
            //    {
            //        Monitor.Exit(Lock);
            //    }
            //}
            //=======================================================
            close = true;
            
            CloseClientSocket();
            CloseServer(reconnect);
            if (!reconnect)
            {
                BufferManager.ReleaseBuffer();
                EventArgsPool.Release();
            }
        }

        public void DisConnect(Socket socket)
        {
            if(!Equals(socket, default))
            {
                //if (0 == Interlocked.Exchange(ref sharedStorage, 1))
                //{
                    CloseClientSocket(new SocketAsyncEventArgs() { UserToken = new AsyncUserToken(socket) });
                    //Interlocked.Exchange(ref sharedStorage, 0);
                //}
                    
            }
        }

        private async void CloseClientSocket()
        {
            //IEnumerator<Socket> Enumerator = Connect.GetConnects().GetEnumerator();
            //var task = Task.Run(() => {

            //    while (Enumerator.MoveNext())
            //    {
            //        try
            //        {
            //            CloseClientSocket(new SocketAsyncEventArgs() { UserToken = new AsyncUserToken(Enumerator.Current) });
            //            Thread.Sleep(20);
            //        }
            //        catch (Exception ex) 
            //        {
            //            LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + " [CloseClientSocket()] " + ex.ToString());
            //        }
            //    }
            //});
            
            //try
            //{
            //    await task;
            //}
            //catch(Exception ex) { LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + " " + ex.ToString()); }
            //finally { task.Dispose(); }
        }

        private void CloseServer(bool reconnect = false)
        {
            try
            {
                if(!Equals(Socket, default) && Socket.Connected)
                    Socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + " [CloseServer] " + ex.ToString());
            }
            finally
            {
                Socket.Close();
                Socket = default(Socket);
                if (reconnect)
                {
                    close = false;
                    Start();
                }
            }
        }

        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            Monitor.Enter(Lock);
            try
            {
                AsyncUserToken token = e.UserToken as AsyncUserToken;
                try
                {
                    if (!Equals(token.Socket, default) && token.Socket.Connected)
                        token.Socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception ex)
                {
                    LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + " [CloseClientSocket(SocketAsyncEventArgs e)] " + ex.ToString());
                }
                finally
                {
                    
                    token.Socket.Close();
                }
            }
            finally
            {
                Monitor.Exit(Lock);
            }
            //================================================================================================
            //AsyncUserToken token = e.UserToken as AsyncUserToken;
            //try
            //{
            //    token.Socket.Shutdown(SocketShutdown.Both);
            //}
            //catch (Exception ex)
            //{
            //    LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + " [CloseClientSocket(SocketAsyncEventArgs e)] " + ex.ToString());
            //}
            //finally
            //{
            //    Connect.Detach(token.Socket);
            //    token.Socket.Close();
            //}

        }

    }
}

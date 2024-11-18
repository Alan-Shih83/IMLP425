using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataFlow
{
    public class FileMsg
    {
        public LogType Type { get; set; }
        public string message { get; set; }
        public string path { get; set; } = default(string);

        public FileMsg(LogType Type, string message) : this(Type, message, default) { }

        public FileMsg(LogType Type, string message, string path)
        {
            this.Type = Type;
            this.message = message;
            this.path = path;
        }
    }

    public class FileManager
    {
        Producer_Consumer<FileMsg> messages = new Producer_Consumer<FileMsg>();
        FileOperator @operator = new FileOperator();
        //Dictionary<LogType, string> paths = new Dictionary<LogType, string>();
        CancellationTokenSource source = default(CancellationTokenSource);
        List<Task> tasks = new List<Task>();
        const int size = 10;
        readonly object Lock = new object();
        volatile int sharedStorage = 0;
        bool isCancel = false;

        private static readonly Lazy<FileManager> singleton = new Lazy<FileManager>(() => new FileManager(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        private FileManager() { tasks.Add(Task.Run(async() => { await FileWrite(); })); }
        public static FileManager Instance { get { return singleton.Value; } }
    

        //public void AttachPath(LogType _type,string _path)
        //{
        //    lock (Lock)
        //    {
        //        try
        //        {
        //            string path;
        //            if (paths.TryGetValue(_type, out path))
        //                paths.Remove(_type);
        //            paths.Add(_type, _path);
        //        }
        //        catch(Exception ex)
        //        {
        //            LogHandlerManager.Instance.GetLogHandler(LogType.None).refresh(DateTime.Now + " " + ex.Message);
        //        }
        //    }
        //}

        //public void DetachPath(LogType _type, string _path)
        //{
        //    lock (Lock)
        //    {
        //        try
        //        {
        //            string path;
        //            if (paths.TryGetValue(_type, out path))
        //                paths.Remove(_type);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogHandlerManager.Instance.GetLogHandler(LogType.None).refresh(DateTime.Now + " " + ex.Message);
        //        }
        //    }
        //}
        

        private string QueryPath(LogType _type)
        {
            if (Directory.Exists(Directory.GetCurrentDirectory() + "\\" + _type.ToString()) == false)
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + _type.ToString());
            }

            if (Directory.Exists(Directory.GetCurrentDirectory() + "\\" + _type.ToString() + "\\" + DateTime.Now.Month.ToString() + "月") == false)
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + _type.ToString() + "\\" + DateTime.Now.Month.ToString() + "月");
            }

            return Directory.GetCurrentDirectory() + "\\" + _type.ToString() + "\\" + DateTime.Now.Month.ToString() + "月" + "\\" + DateTime.Now.ToLongDateString() + ".txt";
            //lock (Lock)
            //{
            //    try
            //    {
            //        string path;
            //        if (paths.TryGetValue(_type, out path))
            //            return path;
            //        else
            //            return default(string);
            //    }
            //    catch (Exception ex)
            //    {
            //        LogHandlerManager.Instance.GetLogHandler(LogType.None).refresh(DateTime.Now + " " + ex.Message);
            //        return default(string);
            //    }
            //}
        }

        public void Push(FileMsg msg)
        {
            if(!Equals(msg, default(FileMsg)))
               messages.Produce(msg);
        }

        public FileOperator GetFileOperator() { return this.@operator; }

        public async Task Cancel()
        {
            messages.Complete();
            if (!Equals(this.source, default(CancellationTokenSource)))
                this.source.Cancel();
            await Task.WhenAll(tasks.ToArray());
            messages.Clear();
        }   

        private async Task FileWrite()
        {
            if (0 == Interlocked.Exchange(ref sharedStorage, 1))
            {
                this.source = new CancellationTokenSource();
                var token = this.source.Token;

                LogType type = LogType.Event;
                StringBuilder builder = new StringBuilder(size);
                try
                {
                    while (!token.IsCancellationRequested || messages.Count() > 0)
                    {
                        FileMsg msg = await messages.ConsumeCheckAsync(CancellationToken.None).ConfigureAwait(false);
                        if (Equals(msg, default) || string.IsNullOrEmpty(msg.message))
                             Thread.Sleep(50);
                        else
                        {
                            if (type == msg.Type)
                            {
                                builder.Append(msg.message + "\r\n");
                                if (builder.Length > size)
                                {
                                    @operator.Write(QueryPath(type), builder.ToString());
                                    builder.Clear();
                                }

                            }
                            else
                            {
                                @operator.Write(QueryPath(type), builder.ToString());
                                builder.Clear();
                                builder.Append(msg.message + "\r\n");
                                type = msg.Type;
                            }
                            Thread.Sleep(20);
                        }
                    }

                    if (token.IsCancellationRequested)
                        isCancel = true;

                    if (builder.Length > 0)
                        @operator.Write(QueryPath(type), builder.ToString());  
                }
                catch (OperationCanceledException) { isCancel = true; }
                catch (Exception ex) { LogHandlerManager.Instance.GetLogHandler(LogType.None).refresh(this.ToString() + " " + ex.ToString()); }
                finally
                {
                    this.source.Dispose();
                    Interlocked.Exchange(ref sharedStorage, 0);
                    if (!isCancel)
                        _ = FileWrite();
                }
            }
        }
    }
}

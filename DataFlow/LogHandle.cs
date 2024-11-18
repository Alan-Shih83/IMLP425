using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataFlow
{
    public enum LogType
    {
        Event,
        Alarm,
        Error,
        PLC_Status,
        Serial_Status,
        Storage,
        None
    }

    public class LogHandler
    {
        HashSet<ListBox> ListBoxSet = new HashSet<ListBox>();

        LogType LogType { get; set; } = LogType.None;

        readonly object Lock = new object();

        public LogHandler(LogType LogType)
        {
            this.LogType = LogType;
        }
        public LogType GetLogType()
        {
            return LogType;
        }
        public void attach(ListBox ListBox)
        {
            attach(new List<ListBox>() { ListBox });
        }
        public void attach(IEnumerable<ListBox> ListBoxs)
        {
            lock (Lock)
            {
                var enumerator = ListBoxs.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    ListBox current = enumerator.Current;
                    if (!ListBoxSet.Contains(current))
                        ListBoxSet.Add(current);
                }
            }
        }
        public  void detach(ListBox ListBox)
        {
            detach(new List<ListBox>() { ListBox });
        }
        public void detach(IEnumerable<ListBox> ListBoxs)
        {
            lock (Lock)
            {
                var enumerator = ListBoxs.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    ListBox current = enumerator.Current;
                    if (ListBoxSet.Contains(current))
                        ListBoxSet.Remove(current);
                }
            }
        }
        public void refresh(params object[] item)
        {
            HashSet<ListBox>.Enumerator enumerator = GetSet().GetEnumerator();
            while (enumerator.MoveNext())
            {
                try
                {
                    ListBox current = enumerator.Current;
                    current?.Invoke(new Action(() => { FormOperate.ListBoxItemInsert(current, item); }));
                }
                catch (Exception) { }               
            }

            try
            {
                if (!Equals(item, default) && item.Length > 0 && this.LogType != LogType.None && this.LogType != LogType.PLC_Status)
                {
                    string str = item.FirstOrDefault(it => it.GetType() == typeof(string)) as string;
                    FileManager.Instance.Push(new FileMsg(this.LogType, str));
                }
            }
            catch (Exception) { }
     
        }

        public void clear(ListBox ListBox)
        {
            clear(new List<ListBox>() { ListBox });
        }
        public void clear(IEnumerable<ListBox> ListBoxs = default)
        {
            HashSet<ListBox>.Enumerator enumerator = GetSet(ListBoxs).GetEnumerator();
            while (enumerator.MoveNext())
            {
                try
                {
                    ListBox current = enumerator.Current;
                    current?.Invoke(new Action(() => { current.Items.Clear(); }));
                }
                catch (Exception) { }
            }
        }
        private HashSet<ListBox> GetSet(IEnumerable<ListBox> ListBoxs = default)
        {
            lock (Lock)
            {
                if (!Equals(ListBoxs, default) && ListBoxs.Count() > 0)
                    return ListBoxSet.Intersect(ListBoxs).ToHashSet<ListBox>();
                else
                    return new HashSet<ListBox>(ListBoxSet);
            }
        }

        ~LogHandler()
        {
            ListBoxSet.Clear();
        }
    }

    public class LogHandlerManager
    {
        static readonly Lazy<LogHandlerManager> singleton = new Lazy<LogHandlerManager>(() => new LogHandlerManager(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        LogHandlerManager() { }
        public static LogHandlerManager Instance { get { return singleton.Value; } }

        HashSet<LogHandler> logHandlers = new HashSet<LogHandler>();

        readonly object Lock = new object();

        public LogHandler GetLogHandler(LogType LogType)
        {
            lock(Lock)
            {
                var handlers = logHandlers.FirstOrDefault(handler => handler.GetLogType() == LogType);
                if (Equals(handlers, default))
                {
                    LogHandler logHandler = new LogHandler(LogType);
                    logHandlers.Add(logHandler);
                    return logHandler;
                }
                else
                    return handlers;
            }
        }

        ~LogHandlerManager()
        {
            logHandlers.Clear();
        }
    }



}

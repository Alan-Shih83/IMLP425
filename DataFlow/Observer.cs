using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Threading.Tasks;

namespace DataFlow
{
    public enum NotifyType
    {
        Update,
        Error,
        Connect,
        Publish,
        Finish,
        ConnectReset
    }

    public interface ISubject<T>
    {
        void attach(IObserver<T> observer);
        void detach(IObserver<T> observer);
        void detach();
        Task notifyObservers(T notify, NotifyType notifyType = NotifyType.Publish, Predicate<IObserver<T>> predicate = default);
    }

    public interface IObserver<T>
    {
        void Register  (ISubject<T> subject);
        void DeRegister(ISubject<T> subject);
        void update   (T notify);
        void error    (T error);
        void Connect  (T connect);
       // void Publish  (T publish);
    }

    public abstract class Subject<T> : ISubject<T>
    {
        readonly object Lock = new object();
        protected HashSet<IObserver<T>> ObserverSet = new HashSet<IObserver<T>>();

        public virtual void attach(IObserver<T> observer)
        {
            lock (Lock)
            {
                if (!Equals(observer, default) && !ObserverSet.Contains(observer))
                     ObserverSet.Add(observer);
            }
            //LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + " [Subject] " + ObserverSet.Count());
        }
        public virtual void detach(IObserver<T> observer)
        {
            lock (Lock)
            {
                if (ObserverSet.Contains(observer))
                    ObserverSet.Remove(observer);
            }
        }
        public virtual void detach()
        {
            HashSet<IObserver<T>> observers;
            lock (Lock)
            {
                if (ObserverSet.Count > 0)
                    observers = new HashSet<IObserver<T>>(ObserverSet);
                else
                    return;
            }

            HashSet<IObserver<T>>.Enumerator enumerator = observers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                detach(enumerator.Current);
            }
        }

        public virtual async Task notifyObservers(T notify, NotifyType notifyType = NotifyType.Update, Predicate<IObserver<T>> predicate = default)
        {
            HashSet<IObserver<T>> observers;
            lock (Lock)
            {
                if (ObserverSet.Count > 0)
                {
                    if (Equals(predicate, default))
                        observers = new HashSet<IObserver<T>>(ObserverSet);
                    else
                        observers = ObserverSet.Where(observer => predicate(observer)).ToHashSet<IObserver<T>>();
                }
                else
                    return;
            }

            HashSet<IObserver<T>>.Enumerator enumerator = observers.GetEnumerator();
            List<Task> tasks = new List<Task>();
            while (enumerator.MoveNext())
            {
                IObserver<T> current = enumerator.Current;
                tasks.Add(Task.Run(() =>
                {
                    if (notifyType == NotifyType.Connect)
                        current.Connect(notify);
                    else if (notifyType == NotifyType.Update)
                        current.update(notify);
                    else if(notifyType == NotifyType.Error)
                        current.error(notify);
                }));
            }

            try
            {
               await Task.WhenAll(tasks.ToArray());
            }
            catch(Exception ex) { LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + " " + ex.ToString()); }
            finally
            {
                foreach (var item in tasks)
                    item.Dispose();
            }
        }

        ~Subject() { ObserverSet.Clear(); }

    }

    public abstract class Observer<T> : IObserver<T>
    {
        HashSet<ISubject<T>> RegisterSet = new HashSet<ISubject<T>>();

        readonly object Lock = new object();
        public void Register(ISubject<T> subject)
        {
            lock(Lock)
            {
                if (!Equals(subject, default) && !RegisterSet.Contains(subject))
                {
                    subject.attach(this);
                    RegisterSet.Add(subject);
                }
            }
        }

        public virtual void DeRegister(ISubject<T> subject)
        {
            lock (Lock)
            { 
                if (RegisterSet.Contains(subject))
                {
                    subject.detach(this);
                    RegisterSet.Remove(subject);
                }
            }
        }

        public virtual void DeRegisterAll()
        {
            HashSet<ISubject<T>> subjects;
            lock (Lock)
            {
                if (RegisterSet.Count > 0)
                    subjects = new HashSet<ISubject<T>>(RegisterSet);
                else
                    return;
            }

            IEnumerator<ISubject<T>> enumerator = subjects.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DeRegister(enumerator.Current);
            }
        }

        public virtual ISubject<T> GetSubject(Predicate<T> predicate = default)
        {
            if (Equals(predicate, default))
                return RegisterSet.FirstOrDefault() as ISubject<T>;
            //return RegisterSet.FirstOrDefault(subject => subject.GetType().Equals(typeof(T))) as ISubject<T>;
            else
                return RegisterSet.FirstOrDefault(subject => predicate((T)subject)) as ISubject<T>;
        }

        public abstract void update   (T notify);
        public abstract void error    (T error);
        public abstract void Connect  (T connect);
        //public abstract void Publish  (T publish);
    }

}

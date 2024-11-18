using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataFlow
{
    public interface IStorage<T> : ICreator
    {
        T GetRecord<U>() where U : T;
        T GetRecord(T source);
        IEnumerable<T> GetRecord(bool isRemove = true);
        T Contain(T _T);
        bool SetRecord(T _T);
        void Clear();
        bool Remove(T _T);
        int Count();
    }

    public class StorageCreater<T> : Creater<Storage<T>> { }

    public class Storage<T> : IStorage<T>
    {
        HashSet<T> Record;

        ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        public Storage() { Record = new HashSet<T>(); }
        public Storage(int size) { Record = new HashSet<T>(size); }
        public Storage(IEqualityComparer<T> comparer) { Record = new HashSet<T>(comparer); }
        public Storage(int size, IEqualityComparer<T> comparer) { Record = new HashSet<T>(size, comparer); }

        public bool Reuse(params object[] param)
        {
            IEqualityComparer<T> comparer = param.FirstOrDefault(_para => _para is IEqualityComparer<T>) as IEqualityComparer<T>;
            if (!Equals(comparer, default))
                Record = new HashSet<T>(comparer);
            return true;
        }

        public void Release()
        {
            Record.Clear();
        }

        public virtual T GetRecord<U>() where U : T
        {
            cacheLock.EnterWriteLock();
            try
            {
                T record = Record.FirstOrDefault(item => EqualityComparer<T>.Equals(item.GetType(), typeof(U)));
                if (!Equals(record, default))
                {
                    Record.Remove(record);
                    return record;
                }
                else
                    return default;
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        public virtual T GetRecord(T _T)
        {
            cacheLock.EnterWriteLock();
            try
            {
                if (!Equals(_T, default))
                {
                    T record = Record.FirstOrDefault(item => EqualityComparer<T>.Equals(item.GetHashCode(), _T.GetHashCode()));
                    if (!Equals(record, default))
                    {
                        Record.Remove(record);
                        return record;
                    }
                    else
                        return default;
                }
                else
                    return default;
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        public virtual IEnumerable<T> GetRecord(bool isRemove = true)
        {
            cacheLock.EnterWriteLock();
            try
            {
                IEnumerable<T> records = Record.AsEnumerable<T>().ToList();
                if (isRemove)
                    Record.Clear();
                return records;
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        public virtual IEnumerable<T> GetRecord(Predicate<T> predicate)
        {
            bool holder = false;
            if (!cacheLock.IsWriteLockHeld)
            {
                cacheLock.EnterWriteLock();
                holder = true;
            }

            try
            {
                IEnumerable<T> records = Record.Where(record => predicate(record)).ToList();
                Record = Record.Except(records).ToHashSet<T>();
                return records;
            }
            finally
            {
                if (holder)
                    cacheLock.ExitWriteLock();
            }
        }
        public virtual IEnumerable<T> QueryItem(Predicate<T> predicate)
        {
            cacheLock.EnterWriteLock();
            try
            {
                IEnumerable<T> records = Record.Where(record => predicate(record)).ToList();
                return records;
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }
        public virtual T QueryItem<U>() where U : T
        {
            cacheLock.EnterWriteLock();
            try
            {
                return Record.FirstOrDefault(item => EqualityComparer<T>.Equals(item.GetType(), typeof(U)));
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }
        public virtual int QueryCount(Predicate<T> predicate)
        {
            cacheLock.EnterWriteLock();
            try
            {
                return Record.Where(record => predicate(record)).Count();
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }
        public virtual bool Remove(T _T)
        {
            bool holder = false;
            if (!cacheLock.IsWriteLockHeld)
            {
                cacheLock.EnterWriteLock();
                holder = true;
            }

            try
            {
                if (Record.Contains(_T))
                {
                    Record.Remove(_T);
                    return true;
                }
                else
                    return false;
            }
            finally
            {
                if (holder)
                    cacheLock.ExitWriteLock();
            }
        }

        public virtual bool Remove(Predicate<T> predicate)
        {
            bool holder = false;
            if (!cacheLock.IsWriteLockHeld)
            {
                cacheLock.EnterWriteLock();
                holder = true;
            }

            try
            {
                IEnumerable<T> records = GetRecord(predicate);
                if (records.Count() > 0)
                {
                    foreach (var record in records)
                        Remove(record);
                    return true;
                }
                else
                    return false;
            }
            finally
            {
                if (holder)
                    cacheLock.ExitWriteLock();
            }
        }
        public virtual T Contain(Predicate<T> predicate)
        {
            cacheLock.EnterReadLock();
            try
            {
                return Record.FirstOrDefault(item => predicate(item));
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }
        public virtual T Contain(T source)
        {
            cacheLock.EnterReadLock();
            try
            {
                if (!Equals(source, default))
                    return Record.FirstOrDefault(item => item.GetHashCode() == source.GetHashCode());
                else
                    return default;
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

        public virtual bool SetRecord(T record)
        {
            cacheLock.EnterWriteLock();
            try
            {
                if (!Record.Contains(record))
                {
                    Record.Add(record);
                    return true;
                }
                else
                    return false;
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }
        public virtual void Clear()
        {
            cacheLock.EnterWriteLock();
            try
            {
                Record.Clear();
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }
        public virtual int Count()
        {
            cacheLock.EnterReadLock();
            try
            {
                return Record.Count;
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

        ~Storage()
        {
            Record.Clear();
        }
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataFlow
{
    public class QueueOperation<T>
    {
        public ConcurrentQueue<T> _Queue = new ConcurrentQueue<T>();

        public T GetQueueItem()
        {
            T Item = default(T);
            if (!_Queue.IsEmpty)
            {
                if (_Queue.TryDequeue(out Item)) { }
            }
            return Item;
        }

        public List<T> GetQueueList(Predicate<T> predicate)
        {
            List<T> QueueList = new List<T>();
            ConcurrentQueue<T> __Queue = new ConcurrentQueue<T>();

            while (!_Queue.IsEmpty)
            {
                if (predicate(GetPeekQueueItem()))
                    QueueList.Add(GetQueueItem());
                else
                    __Queue.Enqueue(GetQueueItem());
            }
            _Queue = new ConcurrentQueue<T>(__Queue);
            return QueueList;
        }

        private IEnumerable<T> IEnumeratorQueue()
        {
            if (!_Queue.IsEmpty)
            {
                IEnumerator _QueueEnumerator = _Queue.GetEnumerator();
                while (_QueueEnumerator.MoveNext())
                {
                    yield return ((T)_QueueEnumerator.Current);
                }
            }
        }

        public List<T> GetQueueCloneList()
        {
            List<T> QueueList = new List<T>();
            foreach (var item in IEnumeratorQueue())
            {
                QueueList.Add(item);
            }
            return QueueList;
        }

        public T GetPeekQueueItem()
        {
            T Item = default(T);
            if (!_Queue.IsEmpty)
            {
                if (_Queue.TryPeek(out Item)) { }
            }
            return Item;
        }

        public void SetQueueItem(T item)
        {
            if(!Equals(item, default))
                this._Queue.Enqueue(item);
        }

        public void SetQueueItem(IEnumerable<T> Item)
        {
            IEnumerator _Enumerator = Item.GetEnumerator();
            while (_Enumerator.MoveNext())
            {
                T current = (T)_Enumerator.Current;
                if (!Equals(current, default))
                    _Queue.Enqueue(current);
            }
        }

        public void SetQueueItemIntoHead(T item)
        {
            if (!Equals(item, default))
            {
                ConcurrentQueue<T> __Queue = new ConcurrentQueue<T>();
                __Queue.Enqueue(item);
                foreach (var IEnumeratorQueueItem in IEnumeratorQueue())
                {
                    __Queue.Enqueue(IEnumeratorQueueItem);
                }
                _Queue = new ConcurrentQueue<T>(__Queue);
            }
        }

        public int Count()
        {
            return _Queue.Count;
        }

        public void Clear()
        {
            while (!_Queue.IsEmpty)
            {
                GetQueueItem();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataFlow
{
    public enum Priority
    {
        First = 1,
        Second,
        Third,
        Fourth
    }

    public class Producer_Consumer<T>
    {
        private Priority Priority = Priority.First;

        private BufferBlock<T> Buffer;
        public Producer_Consumer(Priority Priority = Priority.First)
        {
            this.Priority = Priority;
            this.Buffer = new BufferBlock<T>();
        }

        public void SetPriority(Priority Priority)
        {
            this.Priority = Priority;
        }

        public Priority GetPriority()
        {
            return this.Priority;
        }

        public int Count()
        {
            return Buffer.Count;
        }
        
        public void Complete()
        {
            Buffer.Complete();
        }

        public void Produce(T Target)
        {
            if(!Equals(Target, default))
               Buffer.Post<T>(Target);
        }

        public void Produce(IEnumerable<T> Target)
        {
            IEnumerator<T> _Enumerator = Target.GetEnumerator();
            while (_Enumerator.MoveNext())
            {
                T current = (T)_Enumerator.Current;
                if (!Equals(current, default))
                    Buffer.Post<T>(current);
            }
        }

        public async Task<IList<T>> ConsumeCopyAllAsync(CancellationToken token)
        {
            IList<T> item = await ConsumeAllAsync(token);
            var Copy = Task.Run(() =>
            {
                foreach (var _item in item)
                    Buffer.Post<T>(_item);
            });

            try
            {
                await Copy;
            }
            catch { }
            return item;
        }

        public async Task<IList<T>> ConsumeAllAsync(CancellationToken token)
        {
            IList<T> item = new List<T>();
            while (await Buffer.OutputAvailableAsync(token) && Buffer.TryReceiveAll(out item))
            {
                break;
            }
            return item;
        }

        public async Task<T> ConditionConsumeAsync(Predicate<T> predicate, CancellationToken token)
        {
            T item = default(T);
            while (await Buffer.OutputAvailableAsync(token) && Buffer.TryReceive(predicate, out item))
            {
                break;
            }
            return item;
        }

        public async Task<T> ConsumeAsync(CancellationToken token)
        {
            T item = default(T);
            while (await Buffer.OutputAvailableAsync(token) && Buffer.TryReceive(out item))
            {
                break;
            }
            return item;
        }

        public async Task<T> ConsumeCheckAsync(CancellationToken token)
        {
            T item = default(T);
            if (Buffer.Count > 0)
            {
                item = await ConsumeAsync(token);
            }
            return item;
        }

        public void Clear()
        {
            IList<T> item = default;
            Buffer.TryReceiveAll(out item);
        }

    }
}

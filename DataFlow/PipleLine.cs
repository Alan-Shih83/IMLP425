using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataFlow
{
   
    public interface IPipleLine<T> where T : IPipe
    {
        IPipleLine<T> GetNext();
        PipleLine<T> SetWaitingTime(int time);
        PipleLine<T> SetNext(IPipleLine<T> PipleLine);
        void Push(T _T, Priority priority = Priority.First);
        Task Flow(Func<T, Task> FlowBody = default);
        void Cancel();
    }

    public abstract class PipleLine<T> : IPipleLine<T> where T : IPipe
    {
        int WaitingTime = 10;
        protected bool isCancel = false;
        volatile int sharedStorage = 0;
        CancellationTokenSource Source = default;
        IPipleLine<T> PipleLineConnector = default;
        protected List<Producer_Consumer<T>> Piplelines = new List<Producer_Consumer<T>>();

        public abstract bool Reuse(params object[] param);
        public virtual void Release() { WaitingTime = 10; }

        public PipleLine(Priority priority = Priority.First)
        {
            CreateQueue(priority);
        }
        public PipleLine<T> SetWaitingTime(int time)
        {
            WaitingTime = time > 10 ? time : 10;
            return this;
        }
        public IPipleLine<T> GetNext()
        {
            return this.PipleLineConnector;
        }
        public PipleLine<T> SetNext(IPipleLine<T> PipleLine)
        {
            this.PipleLineConnector = PipleLine;
            return this;
        }

        public virtual void Push(T _T, Priority priority = Priority.First)
        {
            if(!Equals(_T, default))
            {
                //Producer_Consumer<T> Pipleline = Piplelines.FirstOrDefault(Piple => Piple.GetPriority() == priority);
                //Pipleline?.Produce(_T);
                Piplelines.First().Produce(_T);
            }
        }

        protected void CreateQueue(Priority Priority)
        {
            int value = (int)Priority;
            if (value < Piplelines.Count())
            {
                Piplelines = Piplelines.Where(Pipleline => (int)Pipleline.GetPriority() <= value).ToList();
            }
            else if (value > Piplelines.Count())
            {
                for (int index = Piplelines.Count(); index < value; ++index)
                    Piplelines.Add(new Producer_Consumer<T>((Priority)(index + 1)));
            }
        }

        public virtual async Task Flow(Func<T, Task> FlowBody = default)
        {
            if (0 == Interlocked.Exchange(ref sharedStorage, 1))
            {
                this.Source = new CancellationTokenSource();
                var token = this.Source.Token;

                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        T _T = await Piplelines.First().ConsumeAsync(token).ConfigureAwait(false);
                        if (!token.IsCancellationRequested)
                        {
                            if (!Equals(FlowBody, default))
                                await FlowBody(_T);
                            else if (!Equals(_T, default))
                                GetNext()?.Push(_T, _T.GetPriority());
                            //GetNext()?.Push(_T);
                            Thread.Sleep(WaitingTime);
                        }
                    }

                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException) { isCancel = true; }
                catch (Exception ex) { LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + " " + ex.ToString()); }
                finally
                {
                    this.Source.Dispose();
                    Interlocked.Exchange(ref sharedStorage, 0);
                    if (!isCancel)
                        _ = Flow(FlowBody);
                }

                //var FlowTask = Task.Run(async () =>
                //{
                //    try
                //    {
                //        while (!token.IsCancellationRequested)
                //        {
                //            T _T = await Piplelines.First().ConsumeAsync(token).ConfigureAwait(false);
                //            if (!token.IsCancellationRequested)
                //            {
                //                if (!Equals(FlowBody, default))
                //                    await FlowBody(_T);
                //                else if (!Equals(_T, default))
                //                    GetNext()?.Push(_T, _T.GetPriority());
                //                //GetNext()?.Push(_T);
                //                Thread.Sleep(WaitingTime);
                //            }
                //        }

                //        if (token.IsCancellationRequested)
                //            token.ThrowIfCancellationRequested();
                //    }
                //    catch (OperationCanceledException) { isCancel = true; }
                //}, token);

                //try
                //{
                //    await FlowTask;
                //}
                //catch (Exception ex)
                //{
                //    LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + " " + ex.ToString());
                //}
                //finally
                //{
                //    FlowTask.Dispose();
                //    this.Source.Dispose();
                //    Interlocked.Exchange(ref sharedStorage, 0);
                //    if (!isCancel)
                //        _ = Flow(FlowBody);
                //}
            }
        }

        public void Cancel()
        {
            try
            {
                if (!Equals(this.Source, default))
                    this.Source.Cancel();
            }
            catch (Exception) { }
        }

        public void Clear()
        {
            foreach (var Pipleline in Piplelines)
                Pipleline.Clear();
        }
    }

    public class BasicPipleLine<T> : PipleLine<T> where T : IPipe
    {
        public BasicPipleLine() : base() { }
        public override bool Reuse(params object[] param)
        {
            IPipleLine<T> PipleLine = param.FirstOrDefault(_para => _para is IPipleLine<T>) as IPipleLine<T>;
            SetNext(PipleLine);
            isCancel = false;
            return true;
        }

        public override void Release()
        {
            Cancel();
            Clear();
            SetNext(default);
            base.Release();
        }
    }

    public class PriorityQueue<T> : PipleLine<T> where T : IPipe
    {
        List<T> Polling = new List<T>();
        public PriorityQueue() : base(Priority.Fourth) { }

        public PriorityQueue(IEnumerable<T> Polling) : base(Priority.Fourth)
        {
            if( Polling?.Count() > 0)
            {
                this.Polling.AddRange(Polling);
                foreach (var poll in Polling)
                    Piplelines.ElementAt((int)Piplelines.Count() - 1).Produce(poll);
            }            
        }

        public PriorityQueue(IEnumerable<T> Polling, Priority Priority) : base(Priority)
        {
            if (Polling?.Count() > 0)
            {
                this.Polling.AddRange(Polling);
                foreach (var poll in Polling)
                    Piplelines.ElementAt((int)Priority - 1).Produce(poll);
            }
        }

        public override bool Reuse(params object[] param)
        {
            IEnumerable<T> Polling = param.FirstOrDefault(_para => _para is IEnumerable<T>) as List<T>;
            IPipleLine<T> PipleLine = param.FirstOrDefault(_para => _para is IPipleLine<T>) as IPipleLine<T>;
            var Priority = param.FirstOrDefault(_para => _para is Priority);
            SetNext(PipleLine);
            if (!Equals(Priority, default))
            {
                CreateQueue((Priority)Priority);
                this.Polling.AddRange(Polling);
                return true;
            }
            else
                return false;
        }

        public override void Release()
        {
            Cancel();
            SetNext(default);
            base.Release();
        }

        //public override void Push(T _T, Priority priority = Priority.First)
        //{
        //    if (!Equals(_T, default))
        //    {
        //        Producer_Consumer<T> Pipleline = Piplelines.FirstOrDefault(Piple => Piple.GetPriority() == priority);
        //        Pipleline?.Produce(_T);
        //        //Piplelines.First().Produce(_T);
        //    }
        //}

        public override async Task Flow(Func<T, Task> FlowBody = default)
        {
            await base.Flow(Refresh);
        }

        private async Task Refresh(T _t)
        {

            if(_t.GetPriority() == Priority.First)
                GetNext()?.Push(_t);
            else
            {
                Producer_Consumer<T> Pipleline = Piplelines.FirstOrDefault(Piple => Piple.GetPriority() == _t.GetPriority());
                Pipleline?.Produce(_t);
            }


            //Producer_Consumer<T> Pipleline = Piplelines.FirstOrDefault(Piple => Piple.GetPriority() == _t.GetPriority());
            //Pipleline?.Produce(_t);

            IEnumerable<Producer_Consumer<T>> lines = Piplelines.OrderByDescending(line => line.GetPriority());
            T __T = default;
            foreach (var line in lines)
            {
                line.Produce(__T);
                __T = await line.ConsumeCheckAsync(CancellationToken.None).ConfigureAwait(false);
            }

            if(!Equals(default, __T))
                GetNext()?.Push(__T);
            else
            {
                Producer_Consumer<T> _Pipleline = Piplelines.FirstOrDefault(Piple => Piple.Count() > 0);
                if (!Equals(_Pipleline, default))
                {
                    T _T = await _Pipleline.ConsumeCheckAsync(CancellationToken.None).ConfigureAwait(false);
                    GetNext()?.Push(_T);
                }
            }
        }
    }

}

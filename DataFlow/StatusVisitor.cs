using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataFlow
{
    public interface IVisitor
    {
        IEnumerable<ProcessPipe> Visit(ProcessPipe pipe);
    }

    public class SerialPortStatusVisitor : IVisitor
    {
        private int ResponseWaitingTime { get; set; } = 5000;
        private int RetryCount { get; set; } = 2;
        private volatile int OccupyTranferSignal = 0;
        QueueOperation<FirstJudgmentMessage> first_messages = new QueueOperation<FirstJudgmentMessage>();
        Dictionary<FirstJudgmentMessage, Task<bool>> task_dic = new Dictionary<FirstJudgmentMessage, Task<bool>>();
        Dictionary<FirstJudgmentMessage, CancellationTokenSource> cancel_dic = new Dictionary<FirstJudgmentMessage, CancellationTokenSource>();
        Dictionary<FirstJudgmentMessage, TaskCompletionSource<string>> tcs_dic = new Dictionary<FirstJudgmentMessage, TaskCompletionSource<string>>();
        public void SetResponseWaitingTime(double time)
        {
            ResponseWaitingTime = Convert.ToInt32(time * 1000);
            LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " ResponseWaitingTime Setting To " + time.ToString() + " sec.");
        }
        public void SetRetryCount(int count)
        {
            RetryCount = count;
            LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " RetryCount Setting To " + RetryCount.ToString() + " times.");
        }
        public void AddMessage(FirstJudgmentMessage first)
        {
            first_messages.SetQueueItem(first);
            if(first.Level > 0)
               LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " Level Request: " + first.Level);
            else
               LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " Test Request: ");
        }
        public void AddMessage(SecondJudgmentMessage second) { }

        public void AddMessage(FirstJudgmentMessageTest test) { }
        //public void Clear()
        //{
        //    if (first_messages.Count() > 0)
        //    {
        //        List<FirstJudgmentMessage> messages = first_messages.GetQueueCloneList();
        //        foreach (var message in messages)
        //        {
        //            if (cancel_dic.TryGetValue(message, out CancellationTokenSource source))
        //            {
        //                source.Cancel();
        //                source.Dispose();
        //            }
        //        }
        //        task_dic.Clear();
        //        cancel_dic.Clear();
        //        tcs_dic.Clear();
        //        first_messages.Clear();
        //    }
        //    Interlocked.Exchange(ref OccupyTranferSignal, 0);
        //    LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " SerialPortStatusVisitor Clear.");
        //}
        public IEnumerable<ProcessPipe> Visit(ProcessPipe pipe)
        {
            if (Equals(pipe, default(ProcessPipe)))
                yield return default(ProcessPipe);
            else
            {
                MethodInfo generic = Extensions.GetMethodInfo<SerialPortStatusVisitor>(nameof(SerialPortStatusVisitor._Visit), new Type[] { pipe.GetData().GetType(), typeof(MessageType) });
                var result = (IEnumerable<ProcessPipe>)generic.Invoke(this, new object[] { pipe.GetData(), pipe.GetMessageType() });
                foreach (var item in result)
                    yield return item;
            }
        }
        private IEnumerable<ProcessPipe> _Visit(Connect _Connect, MessageType type)
        {
            yield return new ProcessPipe(new Watchdog()).SetDirection(Direction.SerialPort);
        }
        private IEnumerable<ProcessPipe> _Visit(DisConnect _DisConnect, MessageType type)
        {
            if (first_messages.Count() > 0)
            {
                List<FirstJudgmentMessage> messages = first_messages.GetQueueCloneList();
                yield return new ProcessPipe(messages.FirstOrDefault(msg => msg.Level > 0).DeepClone()).SetDirection(Direction.LogicController);
                foreach (var message in messages)
                {
                    if (cancel_dic.TryGetValue(message, out CancellationTokenSource source))
                    {
                        source.Cancel();
                        source.Dispose();
                    }
                }
                task_dic.Clear();
                cancel_dic.Clear();
                tcs_dic.Clear();
                first_messages.Clear();
            }
            Interlocked.Exchange(ref OccupyTranferSignal, 0);
            //LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " SerialPortStatusVisitor Clear.");

            yield return new ProcessPipe(_DisConnect);
        }
        private IEnumerable<ProcessPipe> _Visit(Watchdog _Watchdog, MessageType type)
        {
            int Occupy = Interlocked.CompareExchange(ref OccupyTranferSignal, 1, 1);
            if (first_messages.Count() > 0 && Interlocked.Exchange(ref OccupyTranferSignal, 1) == 0)
            {
                if (!Equals(first_messages.GetPeekQueueItem(), default(FirstJudgmentMessage)))
                {
                    CancellationTokenSource source = new CancellationTokenSource();
                    TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
                    cancel_dic.Add(first_messages.GetPeekQueueItem(), source);
                    tcs_dic.Add(first_messages.GetPeekQueueItem(), tcs);
                    Func<TaskCompletionSource<string>, Task<bool>> func = async (_tcs) => { return await IsTimeout(_tcs, source.Token); };
                    task_dic.Add(first_messages.GetPeekQueueItem(), func(tcs));
                    yield return new ProcessPipe(new SerialData("1")).SetDirection(Direction.SerialPort);
                }
            }
            else if(Occupy == 1)
            {
                if (!Equals(first_messages.GetPeekQueueItem(), default(FirstJudgmentMessage)))
                {
                    if (task_dic.TryGetValue(first_messages.GetPeekQueueItem(), out Task<bool> result))
                    {
                        if (result.IsCompleted)
                        {
                            if (cancel_dic.TryGetValue(first_messages.GetPeekQueueItem(), out CancellationTokenSource source)) source.Dispose();
                            if (task_dic.TryGetValue(first_messages.GetPeekQueueItem(), out Task<bool> _result)) _result.Dispose();
                            cancel_dic.Remove(first_messages.GetPeekQueueItem());
                            task_dic.Remove(first_messages.GetPeekQueueItem());
                            tcs_dic.Remove(first_messages.GetPeekQueueItem());

                            if(result.Result && first_messages.GetPeekQueueItem().Request < RetryCount)
                            {
                                first_messages.GetPeekQueueItem().Request++;
                                CancellationTokenSource _source = new CancellationTokenSource();
                                TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
                                cancel_dic.Add(first_messages.GetPeekQueueItem(), _source);
                                tcs_dic.Add(first_messages.GetPeekQueueItem(), tcs);
                                Func<TaskCompletionSource<string>, Task<bool>> func = async (_tcs) => { return await IsTimeout(_tcs, _source.Token); };
                                task_dic.Add(first_messages.GetPeekQueueItem(), func(tcs));
                                yield return new ProcessPipe(new SerialData("1")).SetDirection(Direction.SerialPort);
                            }
                            else
                            {
                                FirstJudgmentMessage _message = first_messages.GetQueueItem();
                                yield return new ProcessPipe(_message.DeepClone()).SetDirection(Direction.LogicController);
                                yield return new ProcessPipe(new SerialData("REC")).SetDirection(Direction.SerialPort);
                                if(result.Result)
                                   LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " Level" + _message?.Level + " Received PCBID TimeOut.");
                                Interlocked.Exchange(ref OccupyTranferSignal, 0);
                            }
                            //if (!result.Result)
                            //{
                            //    FirstJudgmentMessage _message = first_messages.GetQueueItem();
                            //    yield return new ProcessPipe(_message.DeepClone()).SetDirection(Direction.LogicController);
                            //    yield return new ProcessPipe(new SerialData("REC")).SetDirection(Direction.SerialPort);
                            //    Interlocked.Exchange(ref OccupyTranferSignal, 0);
                            //}
                            //else
                            //{
                            //    if(first_messages.GetPeekQueueItem().Request < ResendTime)
                            //    {
                            //        first_messages.GetPeekQueueItem().Request++;
                            //        yield return new ProcessPipe(new SerialData("1")).SetDirection(Direction.SerialPort);
                            //        CancellationTokenSource _source = new CancellationTokenSource();
                            //        TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
                            //        cancel_dic.Add(first_messages.GetPeekQueueItem(), _source);
                            //        tcs_dic.Add(first_messages.GetPeekQueueItem(), tcs);
                            //        Func<TaskCompletionSource<string>, Task<bool>> func = async (_tcs) => { return await IsTimeout(_tcs, _source.Token); };
                            //        task_dic.Add(first_messages.GetPeekQueueItem(), func(tcs));
                            //    }
                            //    else
                            //    {
                            //        FirstJudgmentMessage msg = first_messages.GetQueueItem();
                            //        yield return new ProcessPipe(new SerialData("REC")).SetDirection(Direction.SerialPort);
                            //        LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " Level: " + msg?.Level);
                            //        Interlocked.Exchange(ref OccupyTranferSignal, 0);
                            //    }   
                            //}
                        }
                    }
                }      
            }
            yield return new ProcessPipe(_Watchdog).SetDirection(Direction.SerialPort);
        }
        private IEnumerable<ProcessPipe> _Visit(SerialData _SerialData, MessageType type)
        {
            if(!Equals(_SerialData, default(SerialData)))
            {
                if (_SerialData.message.Contains("OK"))
                {
                    int index = _SerialData.message.IndexOf("OK");
                    if(index != -1)
                    {
                        _SerialData.message = _SerialData.message.Remove(index, "OK".Length);
                        yield return new ProcessPipe(new SecondJudgmentMessage(_SerialData.message, 1)).SetDirection(Direction.LogicController);
                        yield return new ProcessPipe(new SerialData("ACK")).SetDirection(Direction.SerialPort);
                    }
                }
                else if (_SerialData.message.Contains("NG"))
                {
                    int index = _SerialData.message.IndexOf("NG");
                    if (index != -1)
                    {
                        _SerialData.message = _SerialData.message.Remove(index, "NG".Length);
                        yield return new ProcessPipe(new SecondJudgmentMessage(_SerialData.message, 2)).SetDirection(Direction.LogicController);
                        yield return new ProcessPipe(new SerialData("ACK")).SetDirection(Direction.SerialPort);
                    }
                }
                else
                {
                    if (!Equals(first_messages.GetPeekQueueItem(), default(FirstJudgmentMessage)))
                    {
                        if(tcs_dic.TryGetValue(first_messages.GetPeekQueueItem(), out TaskCompletionSource<string> tcs))
                        {
                            tcs.TrySetResult(_SerialData.message);
                            first_messages.GetPeekQueueItem().ID = _SerialData.message;
                        }
                    }    
                }
            }  
            else
                yield return default;
        }

        private async Task<bool> IsTimeout(TaskCompletionSource<string> tcs, CancellationToken token, int millisec = 5000)
        {
            try
            {
                using (CancellationTokenSource source = CancellationTokenSource.CreateLinkedTokenSource(token))
                {
                    Task timeoutTask = Task.Delay(millisec, source.Token);
                    await Task.WhenAny(timeoutTask, tcs.Task);

                    TaskStatus status = timeoutTask.Status;
                    if (timeoutTask.Status == TaskStatus.RanToCompletion)
                    {
                        LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " AOI Response PCBID TimeOut.");
                        return true;
                    }
                    else if (timeoutTask.Status == TaskStatus.Canceled)
                    {
                        LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " TimeOut Canaeled.");
                        return false;
                    }
                    else
                    {
                        source.Cancel();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(DateTime.Now + " " + this.ToString() + " " + ex.Message);
                return true;
            }
        }
    }

    public class LogicControlStatusVisitor : IVisitor
    {
        private volatile int FirstRequestSend = 0;
        private volatile int FirstTranferSignal = 0;
        private volatile int StopTranferSignal = 0;
        private volatile int OccupyTranferSignal = 0;
        QueueOperation<SecondJudgmentMessage> second_messages = new QueueOperation<SecondJudgmentMessage>();
        QueueOperation<FirstJudgmentMessage>  first_messages = new QueueOperation<FirstJudgmentMessage>();
        QueueOperation<FirstJudgmentMessageTest> first_test_messages = new QueueOperation<FirstJudgmentMessageTest>();
        public void Clear()
        {
            Interlocked.Exchange(ref FirstTranferSignal,  0);
            Interlocked.Exchange(ref StopTranferSignal,   0);
            Interlocked.Exchange(ref OccupyTranferSignal, 0);
            //LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(this.ToString() + " StatusVisitor Clear.");
        }
        public void AddMessage(FirstJudgmentMessageTest test)
        {
            if (test.Level > 0 && test.Level < 18)
            {
                first_test_messages.SetQueueItem(test);
                //Interlocked.Decrement(ref FirstRequestSend);
                LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " firstTest_messages: " + first_test_messages.Count() + " ID: " + test.ID + " Level: " + test.Level.ToString());
            }
        }
        public void AddMessage(FirstJudgmentMessage first)
        {
            if(first.Level > 0 && first.Level < 18)
            {
                first_messages.SetQueueItem(first);
                //Interlocked.Decrement(ref FirstRequestSend);
                LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " first_messages: " + first_messages.Count() + " ID: " + first.ID);
            }
        }
        public void AddMessage(SecondJudgmentMessage second)
        {
            second_messages.SetQueueItem(second);
            LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " second_messages: " + second_messages.Count());
        }
        public IEnumerable<ProcessPipe> Visit(ProcessPipe pipe)
        {
            if (Equals(pipe, default(ProcessPipe)))
                yield return default(ProcessPipe);
            else
            {
                MethodInfo generic = Extensions.GetMethodInfo<LogicControlStatusVisitor>(nameof(LogicControlStatusVisitor._Visit), new Type[] { pipe.GetData().GetType(), typeof(MessageType) });
                var result = (IEnumerable<ProcessPipe>)generic.Invoke(this, new object[] { pipe.GetData(), pipe.GetMessageType() });
                foreach (var item in result)
                    yield return item;
            }
        }
        private IEnumerable<ProcessPipe> _Visit(Connect _Connect, MessageType type)
        {
            int _sharedStorage = Interlocked.CompareExchange(ref FirstRequestSend, 0, 0);
            if (_sharedStorage == 0)
               yield return new ProcessPipe(new HandShakeSignl() { WriteSignalToLogicControl  = 0 });
            yield return new ProcessPipe(new Watchdog());
        }
        private IEnumerable<ProcessPipe> _Visit(DisConnect _DisConnect, MessageType type)
        {
            FormManager.Instance.Query<_FormMain>()?.UpdateUI(new ListViewBackColorClearMsg("RepositoryView"));
            yield return new ProcessPipe(_DisConnect);
        }
        private IEnumerable<ProcessPipe> _Visit(Watchdog _Watchdog, MessageType type)
        {
            yield return new ProcessPipe(_Watchdog);
            int _StopStorage   = Interlocked.CompareExchange(ref StopTranferSignal, 0, 0);
            int _OccupyStorage = Interlocked.CompareExchange(ref OccupyTranferSignal, 0, 0);

            if(first_test_messages.Count() > 0 && !Equals(first_test_messages.GetPeekQueueItem(), default(FirstJudgmentMessageTest)))
            {
                yield return new ProcessPipe(first_test_messages.GetQueueItem().DeepClone()).SetDirection(Direction.LogicController);
            }

            if(first_messages.Count() > 0 && Interlocked.Exchange(ref FirstTranferSignal, 1) == 0)
            {
                yield return new ProcessPipe(first_messages.GetPeekQueueItem().DeepClone()).SetDirection(Direction.LogicController);
            }

            if (_StopStorage == 0 && _OccupyStorage == 0 && second_messages.Count() > 0)
            {
                Interlocked.Exchange(ref OccupyTranferSignal, 1);
                var repository = Repository.Instance.SEARCH(second_messages.GetPeekQueueItem()._ID);
                if (!Equals(repository, default(RepositoryMessage)))
                {
                    second_messages.GetPeekQueueItem().Level = repository.Level;
                    yield return new ProcessPipe(second_messages.GetPeekQueueItem().DeepClone()).SetMessageType(MessageType.Read);
                }
                else
                {
                    SecondJudgmentMessage second= second_messages.GetQueueItem();
                    Interlocked.Exchange(ref OccupyTranferSignal, 0);
                    if(!Equals(second, default))
                        LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(this.ToString() + " " + second._ID + " Not Found.");
                }  
            }
        }
        private IEnumerable<ProcessPipe> _Visit(FirstJudgmentMessage _FirstJudgmentMessage, MessageType type)
        {
            if (!Equals(first_messages.GetPeekQueueItem(), default(FirstJudgmentMessage)))
            {
                first_messages.GetQueueItem();
                Repository.Instance.Update(_FirstJudgmentMessage.Proxy());
                if (string.IsNullOrEmpty(_FirstJudgmentMessage.ID))
                    yield return new ProcessPipe(new HandShakeSignl() { WriteSignalToLogicControl = 7 });
                else
                    yield return new ProcessPipe(new HandShakeSignl() { WriteSignalToLogicControl = 3 });
                Interlocked.Decrement(ref FirstRequestSend);
                Interlocked.Exchange(ref FirstTranferSignal, 0);
            }        
        }
        private IEnumerable<ProcessPipe> _Visit(FirstJudgmentMessageTest _FirstJudgmentMessageTest, MessageType type)
        {
              Repository.Instance.Update(_FirstJudgmentMessageTest.Proxy());
              yield return default(ProcessPipe);
        }
        private IEnumerable<ProcessPipe> _Visit(SecondJudgmentMessage _SecondJudgmentMessage, MessageType type)
        {
            //if(type == MessageType.Read && _SecondJudgmentMessage.Level == -1)
            //{
            //    var repository = Repository.Instance.SEARCH(_SecondJudgmentMessage._ID);
            //    if(!Equals(repository, default(RepositoryMessage)))
            //    {
            //        _SecondJudgmentMessage.Level = repository.Level;
            //        yield return new ProcessPipe(_SecondJudgmentMessage.DeepClone()).SetMessageType(MessageType.Read);
            //    }
            //    else
            //        LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + " " + _SecondJudgmentMessage._ID + "Not Found.");
            //}
            if(type == MessageType.Read && _SecondJudgmentMessage.CompareID())
                yield return new ProcessPipe(_SecondJudgmentMessage).SetMessageType(MessageType.Write);
            else 
            {
                if (type == MessageType.Write)
                    Repository.Instance.Update(_SecondJudgmentMessage.Proxy());
                second_messages.GetQueueItem();
                Interlocked.Exchange(ref OccupyTranferSignal, 0);
            }  
        }
        private IEnumerable<ProcessPipe> _Visit(HandShakeSignl _HandShakeSignl, MessageType type)
        {
            if (type == MessageType.Read)
            {
                LogHandlerManager.Instance.GetLogHandler(LogType.PLC_Status).refresh(DateTime.Now + " D7000(PLC): " + _HandShakeSignl.ReadSignalFromLogicControl + " D7050(PC): " + _HandShakeSignl.WriteSignalToLogicControl);
                //if(!_HandShakeSignl.CompareStatus())
                //    LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " D7000(PLC): " + _HandShakeSignl.ReadSignalFromLogicControl + " D7050(PC): " + _HandShakeSignl.WriteSignalToLogicControl);
                switch (_HandShakeSignl.ReadSignalFromLogicControl)
                {
                    case 0:
                        {
                            Interlocked.Exchange(ref StopTranferSignal, 0);
                            if(_HandShakeSignl.WriteSignalToLogicControl != 0)
                            {
                                FormManager.Instance.Query<_FormMain>()?.UpdateUI(new ListViewBackColorClearMsg("RepositoryView"));
                                yield return new ProcessPipe(new HandShakeSignl() { WriteSignalToLogicControl = 0 });
                            }
                            else
                                yield return new ProcessPipe(new HandShakeSignl()).SetMessageType(MessageType.Read);
                            break;
                        }
                    case 1:
                        {
                            Interlocked.Exchange(ref StopTranferSignal, 0);
                            int _sharedStorage = Interlocked.CompareExchange(ref FirstRequestSend, 0, 0);
                            if (_HandShakeSignl.WriteSignalToLogicControl != 3 && _HandShakeSignl.WriteSignalToLogicControl != 7 && _sharedStorage == 0)
                            {
                                Interlocked.Increment(ref FirstRequestSend);
                                LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " D7000(PLC): " + _HandShakeSignl.ReadSignalFromLogicControl + "  D7050(PC): " + _HandShakeSignl.WriteSignalToLogicControl + "  Level: " + _HandShakeSignl.ReadLevelFromLogicControl);
                                FormManager.Instance.Query<_FormMain>()?.UpdateUI(new ListViewBackColorClearMsg("RepositoryView"));
                                yield return new ProcessPipe(new FirstJudgmentMessage(default, (int)_HandShakeSignl.ReadLevelFromLogicControl)).SetDirection(Direction.SerialPort);
                            }   
                            //yield return new ProcessPipe(new FirstJudgmentMessage("PCBID" + ((int)_HandShakeSignl.ReadLevelFromLogicControl).ToString(), (int)_HandShakeSignl.ReadLevelFromLogicControl));
                            else
                               yield return new ProcessPipe(new HandShakeSignl()).SetMessageType(MessageType.Read);
                            break;                           
                        }
                    case 4:
                        {
                            Interlocked.Exchange(ref StopTranferSignal, 0);
                            if (_HandShakeSignl.WriteSignalToLogicControl != 4)
                            {
                                var repository = Repository.Instance.SEARCH((int)_HandShakeSignl.ReadLevelFromLogicControl);
                                Repository.Instance.DEL(repository);
                                yield return new ProcessPipe(new HandShakeSignl() { WriteSignalToLogicControl = 4 });
                               LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + this.ToString() + " Level: " + _HandShakeSignl.ReadLevelFromLogicControl + " Remove.");
                            }
                            else
                                yield return new ProcessPipe(new HandShakeSignl()).SetMessageType(MessageType.Read);
                            break;
                        }
                    case 5:
                        {
                            Interlocked.Exchange(ref StopTranferSignal, 1);
                            int _sharedStorage = Interlocked.CompareExchange(ref OccupyTranferSignal, 0, 0);
                            if(_sharedStorage == 0)
                            {
                                FormManager.Instance.Query<_FormMain>()?.UpdateUI(new ListViewBackColorMsg("RepositoryView", _HandShakeSignl.ReadLevelFromLogicControl.ToString(), Color.LightGreen));
                                yield return new ProcessPipe(new HandShakeSignl() { WriteSignalToLogicControl = 5 });
                            } 
                            else
                                yield return new ProcessPipe(new HandShakeSignl()).SetMessageType(MessageType.Read);
                            break;
                        }
                    case 6:
                        {
                            Interlocked.Exchange(ref StopTranferSignal, 1);
                            int _sharedStorage = Interlocked.CompareExchange(ref OccupyTranferSignal, 0, 0);
                            if (_sharedStorage == 0 && _HandShakeSignl.WriteSignalToLogicControl != 6)
                            {
                                Repository.Instance.Clear();
                                yield return new ProcessPipe(new HandShakeSignl() { WriteSignalToLogicControl = 6 });
                            }     
                            else
                                yield return new ProcessPipe(new HandShakeSignl()).SetMessageType(MessageType.Read);
                            break;
                        }
                    default:
                        {
                            Interlocked.Exchange(ref StopTranferSignal, 0);
                            yield return new ProcessPipe(new HandShakeSignl()).SetMessageType(MessageType.Read);
                            break;
                        }
                }
            }
            else
                yield return new ProcessPipe(new HandShakeSignl()).SetMessageType(MessageType.Read);
            
        }

    }
}

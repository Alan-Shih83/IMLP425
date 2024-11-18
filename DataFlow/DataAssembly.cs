using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow
{
    public class DataAssembly
    {
        Assembly Assembly = new Assembly();
        BasicPipleLine<ProcessPipe> BasicSendPipleLine = new BasicPipleLine<ProcessPipe>();
        BasicPipleLine<ProcessPipe> BasicReceivePipleLine = new BasicPipleLine<ProcessPipe>();
        BasicPipleLine<ProcessPipe> BasicHandledPipleLine = new BasicPipleLine<ProcessPipe>();
        public DataAssembly()
        {
            _ = BasicReceivePipleLine.Flow();
            _ = BasicSendPipleLine.Flow();
            _ = BasicHandledPipleLine.Flow(Compose);
        }
        public PipleLine<ProcessPipe> GetBasicSendPipleLine()
        {
            return BasicSendPipleLine;
        }
        public PipleLine<ProcessPipe> GetBasicReceivePipleLine()
        {
            return BasicReceivePipleLine;
        }
        public PipleLine<ProcessPipe> GetBasicHandledPipleLine()
        {
            return BasicHandledPipleLine;
        }
        public void Clear()
        {
            BasicSendPipleLine.Clear();
            BasicReceivePipleLine.Clear();
            BasicHandledPipleLine.Clear();
            Assembly.Release();
            //LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(this.ToString() + " DataAssembly Clear.");
        }
        public void Decompose(ProcessPipe pipe)
        {
            foreach (var _pipe in Assembly.Decompose(pipe))
            {
                if (!Equals(_pipe, default))
                {
                    if(_pipe.GetRandomCode() == 0)
                        BasicReceivePipleLine.Push(_pipe);
                    else
                        BasicSendPipleLine.Push(_pipe);
                }     
            }
        }
        private async Task Compose(ProcessPipe pipe)
        {
            await Task.Yield();
            if(pipe.GetNotifyType() == NotifyType.Connect || pipe.GetNotifyType() == NotifyType.Error)
                BasicReceivePipleLine.Push(pipe);
            else
            {
                foreach (var _pipe in Assembly.Compose(pipe))
                {
                    if (!Equals(_pipe, default))
                        BasicReceivePipleLine.Push(_pipe);
                }
            }
            
        }
    }
}

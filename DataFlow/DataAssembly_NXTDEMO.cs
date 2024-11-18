using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NXT_Hermes
{
    public class DataAssembly_NXTDEMO
    {
        Assembly Assembly = new Assembly();

        BasicPipleLine<ProcessPipe> BasicPipleLineSend = new BasicPipleLine<ProcessPipe>();

        BasicPipleLine<ProcessPipe> BasicPipleLineReceive = new BasicPipleLine<ProcessPipe>();

        BasicPipleLine<ProcessPipe> BasicPipleLineHandled = new BasicPipleLine<ProcessPipe>();

        public DataAssembly_NXTDEMO()
        {
            _ = BasicPipleLineSend.Flow();
            _ = BasicPipleLineReceive.Flow(PipleFlow);
            _ = BasicPipleLineHandled.Flow();
        }

        public PipleLine<ProcessPipe> GetHandleLine()
        {
            return BasicPipleLineHandled;
        }

        public PipleLine<ProcessPipe> GetReceiveLine()
        {
            return BasicPipleLineReceive;
        }

        public PipleLine<ProcessPipe> GetSendLine()
        {
            return BasicPipleLineSend;
        }

        public void Send(ProcessPipe pipe)
        {
            foreach (var _pipe in Assembly.Decompose(pipe))
            {
                if (!Equals(_pipe, default))
                    BasicPipleLineSend.Push(_pipe, _pipe.GetPriority());
            }
        }

        private async Task PipleFlow(ProcessPipe pipe)
        {
            await Task.Yield();
            foreach (var _pipe in Assembly.Compose(pipe))
            {
                if(!Equals(_pipe, default))
                    BasicPipleLineHandled.Push(_pipe);
            }
        }

    }
}

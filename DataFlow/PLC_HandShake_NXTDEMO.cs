using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NXT_Hermes
{
    public class PLC_HandShake_NXTDEMO
    {
        Client client;

        TemporaryStorage<byte[]> TemporaryStorage = new TemporaryStorage<byte[]>(new LogicControlDecoding());

        BasicPipleLine<ConnectPipe> BasicPipleLineReceive = new BasicPipleLine<ConnectPipe>();

        BasicPipleLine<ProcessPipe> BasicPipleLineSend = new BasicPipleLine<ProcessPipe>();

        BasicPipleLine<ProcessPipe> BasicPipleLineHandled = new BasicPipleLine<ProcessPipe>();

        ProcessPipeControl ProcessPipeControl = new ProcessPipeControl();

        SemaphoreManage Semaphore = new SemaphoreManage(1, 1);

        public PLC_HandShake_NXTDEMO(IPEndPoint EndPoint)
        {
            //client = new Client(EndPoint);
            //client.BasicPipleLine.SetNext(BasicPipleLineReceive);
            //_ = BasicPipleLineHandled.Flow();
            //_ = BasicPipleLineReceive.Flow(PipleFlow);
            //_ = BasicPipleLineSend.Flow(PipleFlow);
            //_ = client.BasicPipleLine.Flow();
            //client.Start();
        }

        public PipleLine<ProcessPipe> GetHandleLine()
        {
            return BasicPipleLineHandled;
        }

        public PipleLine<ProcessPipe> GetSendLine()
        {
            return BasicPipleLineSend;
        }
        
        private async Task PipleFlow(ConnectPipe Pipe)
        {
            await Task.Yield();
            foreach (var data in TemporaryStorage.Decode(Pipe.GetData() as byte[]))
            {
                ProcessPipe pipe = ProcessPipeControl.Assemble(data);
                if (!Equals(pipe, default) && ProcessPipeControl.isEmpty())
                {
                    BasicPipleLineHandled.Push(pipe);
                    Semaphore.Release();
                }
                else
                {
                    byte[] message = ProcessPipeControl.Encapsulate();
                    if (!Equals(message, default))
                        client.Send(message);
                }
            }
        }

        private async Task PipleFlow(ProcessPipe pipe)
        {
            await Task.Yield();
            Semaphore.WaitOne();
            ProcessPipeControl.SetProcessPipe(pipe);
            byte[] message = ProcessPipeControl.Encapsulate();
            if (!Equals(message, default))
                client.Send(message);
            else
                Semaphore.Release();
        }
    }
}

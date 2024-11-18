using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NXT_Hermes
{


    public class Process_NXTDEMO_Upstream
    {
       
        DataAssembly_NXTDEMO DataAssembly_NXTDEMO;

        PLC_HandShake_NXTDEMO PLC_HandShake_NXTDEMO;

        BasicPipleLine<ProcessPipe> BasicPipleLineReceive = new BasicPipleLine<ProcessPipe>();

        readonly object Lock = new object();

        public Process_NXTDEMO_Upstream(IPEndPoint endPoint)
        {
            PLC_HandShake_NXTDEMO = new PLC_HandShake_NXTDEMO(endPoint);
            DataAssembly_NXTDEMO = new DataAssembly_NXTDEMO();
            DataAssembly_NXTDEMO.GetHandleLine().SetNext(BasicPipleLineReceive);
            PLC_HandShake_NXTDEMO.GetHandleLine().SetNext(DataAssembly_NXTDEMO.GetReceiveLine());
            DataAssembly_NXTDEMO.GetSendLine().SetNext(PLC_HandShake_NXTDEMO.GetSendLine());
            _ = BasicPipleLineReceive.Flow(PipleFlow);
            //Restart();
        }
      
        public void Restart()
        {
            DataAssembly_NXTDEMO.Send(new ProcessPipe(new HandShakeSignl() { WriteSignalToLogicControl = 0}));
            DataAssembly_NXTDEMO.Send(new ProcessPipe(new Watchdog()));
        }

        private async Task PipleFlow(ProcessPipe pipe)
        {
            try
            {
                if(!Equals(pipe, default))
                {
                    //LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " Scan. ");
                    if(pipe.GetData() is Watchdog)
                    {
                        LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " Watchdog. ");
                        DataAssembly_NXTDEMO.Send(new ProcessPipe(new Watchdog()));
                    }
                    else if (pipe.GetData() is HandShakeSignl)
                    {
                        HandShakeSignl signal = pipe.GetData() as HandShakeSignl;
                        if (pipe.GetMessageType() == MessageType.Read)
                        {
                            if (signal.ReadSignalFromLogicControl == 0)
                            {
                                DataAssembly_NXTDEMO.Send(new ProcessPipe(new HandShakeSignl() { WriteSignalToLogicControl = 0 }));
                            }
                            else if (signal.ReadSignalFromLogicControl == 1 && signal.WriteSignalToLogicControl != 3)
                            {
                               // LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " " + RangeSets.GetData((int)signal.ReadLevelFromLogicControl));
                                //DataAssembly_NXTDEMO.Send(new ProcessPipe(RangeSets.GetData((int)signal.ReadLevelFromLogicControl)));
                            }
                        }
                        else
                            DataAssembly_NXTDEMO.Send(new ProcessPipe(new HandShakeSignl()).SetMessageType(MessageType.Read));
                    }
                    else
                        DataAssembly_NXTDEMO.Send(new ProcessPipe(new HandShakeSignl() { WriteSignalToLogicControl = 3 }));
                }
                else
                    LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(DateTime.Now + " NULL Object. ");
            }
            catch(Exception ex)
            {
                LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(DateTime.Now + " " + ex.Message);
            }
        }
    }
}

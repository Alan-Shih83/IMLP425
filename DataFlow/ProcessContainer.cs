using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow
{
    public interface IProcessContainer
    {
        IProcessContainer GetProcessContainer();
        IFliter GetFilter();
        IVisitor GetVisitor();
        void AddMessage(FirstJudgmentMessage first);
        void AddMessage(SecondJudgmentMessage second);
        void AddMessage(FirstJudgmentMessageTest second);
    }

    public class SerialPortProcessContainer : IProcessContainer
    {
        SerialPortStatusVisitor visitor = new SerialPortStatusVisitor();
        SerialPortFilter Filter = new SerialPortFilter();
        BasicPipleLine<ProcessPipe> BasicReceivePipleLine = new BasicPipleLine<ProcessPipe>();
        public IProcessContainer GetProcessContainer()
        {
            return this;
        }
        public IFliter GetFilter()
        {
            return Filter;
        }
        public IVisitor GetVisitor()
        {
            return visitor;
        }
        public void AddMessage(FirstJudgmentMessageTest test) { }
        public void AddMessage(FirstJudgmentMessage first) 
        {
            if (!Equals(first, default(FirstJudgmentMessage)))
                visitor.AddMessage(first);
        }
        public void AddMessage(SecondJudgmentMessage second) { }

        public SerialPortProcessContainer()
        {
            Filter.GetReceivePipleLine().SetNext(BasicReceivePipleLine);
            _ = BasicReceivePipleLine.Flow(PipleFlow);
        }
        private async Task PipleFlow(ProcessPipe pipe)
        {
            await Task.Yield();
            foreach (var pipeitem in visitor.Visit(pipe))
            {
                if (!Equals(pipeitem, default(ProcessPipe)))
                {
                    if (pipeitem.GetData() is DisConnect)
                        Clear();
                    else if (pipeitem.GetDirection() == Direction.LogicController && pipeitem.GetData() is FirstJudgmentMessage)
                        ProcessManager.Instance.Query<LogicControlProcessContainer>()?.AddMessage(pipeitem.GetData() as FirstJudgmentMessage);
                    else if (pipeitem.GetDirection() == Direction.LogicController && pipeitem.GetData() is SecondJudgmentMessage)
                        ProcessManager.Instance.Query<LogicControlProcessContainer>()?.AddMessage(pipeitem.GetData() as SecondJudgmentMessage);
                    else if(pipeitem.GetDirection() == Direction.SerialPort)
                        Filter.GetSendPipleLine().Push(pipeitem);
                }
            }
        }
        private void Clear()
        {
            BasicReceivePipleLine.Clear();
            //visitor.Clear();
            //LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(this.ToString() + " ProcessContainer Clear.");
        }

    }

    public class LogicControlProcessContainer : IProcessContainer
    {
        LogicControlFilter Filter = new LogicControlFilter();
        DataAssembly DataAssembly = new DataAssembly();
        LogicControlStatusVisitor visitor = new LogicControlStatusVisitor();
        BasicPipleLine<ProcessPipe> BasicReceivePipleLine = new BasicPipleLine<ProcessPipe>();
        public LogicControlProcessContainer()
        {
            DataAssembly.GetBasicSendPipleLine().SetNext(Filter.GetSendPipleLine());
            DataAssembly.GetBasicReceivePipleLine().SetNext(BasicReceivePipleLine);
            Filter.GetReceivePipleLine().SetNext(DataAssembly.GetBasicHandledPipleLine());
            _ = BasicReceivePipleLine.Flow(PipleFlow);
        }
        public IFliter GetFilter()
        {
            return Filter;
        }
        public IVisitor GetVisitor()
        {
            return visitor;
        }
        public IProcessContainer GetProcessContainer()
        {
            return this;
        }
        public void AddMessage(FirstJudgmentMessage first)
        {
            if(!Equals(first, default(FirstJudgmentMessage)))
                visitor.AddMessage(first);
        }
        public void AddMessage(SecondJudgmentMessage second)
        {
            if (!Equals(second, default(SecondJudgmentMessage)))
                visitor.AddMessage(second);
        }
        public void AddMessage(FirstJudgmentMessageTest test)
        {
            if (!Equals(test, default(FirstJudgmentMessageTest)))
                visitor.AddMessage(test);
        }
        private async Task PipleFlow(ProcessPipe pipe)
        {
            await Task.Yield();
            foreach (var pipeitem in visitor.Visit(pipe))
            {
                if (!Equals(pipeitem, default(ProcessPipe)))
                {
                    if (pipeitem.GetData() is DisConnect)
                        Clear();
                    else if (pipeitem.GetDirection() == Direction.SerialPort && pipeitem.GetData() is FirstJudgmentMessage)
                        ProcessManager.Instance.Query<SerialPortProcessContainer>()?.AddMessage(pipeitem.GetData() as FirstJudgmentMessage);
                    else if(pipeitem.GetDirection() == Direction.LogicController)
                        DataAssembly.Decompose(pipeitem);
                }
            }
        }
        private void Clear()
        {
            BasicReceivePipleLine.Clear();
            DataAssembly.Clear();
            visitor.Clear();
            //LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(this.ToString() + " ProcessContainer Clear.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;

namespace DataFlow
{

    public interface IPipe //: ICreator
    {
        Priority GetPriority();
        MessageType GetMessageType();
        RepositoryType GetRepositoryType();
        Direction GetDirection();
        NotifyType GetNotifyType();
        object GetData();
    }

    [Serializable]
    public class ConnectPipe : IPipe
    {
        byte[] datas;
        Priority Priority = Priority.First;
        Direction Direction = Direction.LogicController;
        NotifyType NotifyType = NotifyType.Update;
        MessageType MessageType = MessageType.Write;
        RepositoryType RepositoryType = RepositoryType.SEARCH;
        public ConnectPipe(byte[] datas)
        {
            this.datas = datas;
        }
        public Priority GetPriority()
        {
            return Priority;
        }
        public ConnectPipe SetPriority(Priority Priority)
        {
            this.Priority = Priority;
            return this;
        }
        public MessageType GetMessageType()
        {
            return MessageType;
        }
        public ConnectPipe SetMessageType(MessageType MessageType)
        {
            this.MessageType = MessageType;
            return this;
        }
        public ConnectPipe SetNotifyType(NotifyType NotifyType)
        {
            this.NotifyType = NotifyType;
            return this;
        }
        public NotifyType GetNotifyType()
        {
            return NotifyType;
        }
        public RepositoryType GetRepositoryType()
        {
            return RepositoryType;
        }
        public Direction GetDirection()
        {
            return Direction;
        }
        public ConnectPipe SetRepositoryType(RepositoryType RepositoryType)
        {
            this.RepositoryType = RepositoryType;
            return this;
        }
        public object GetData()
        {
            return datas;
        }
    }

    [Serializable]
    public class ProcessPipe : IPipe
    {
        object datas;
        uint RandomCode = 0;
        int AssembleCount = 0;
        Priority Priority = Priority.First;
        MessageType MessageType = MessageType.Write;
        NotifyType NotifyType = NotifyType.Update;
        RepositoryType RepositoryType = RepositoryType.SEARCH;
        Direction Direction = Direction.LogicController;
        public ProcessPipe(object datas)
        {
            this.datas = datas;
        }
        public uint GetRandomCode()
        {
            return RandomCode;
        }
        public ProcessPipe SetRandomCode(uint RandomCode)
        {
            this.RandomCode = RandomCode;
            return this;
        }
        public int GetAssembleCount()
        {
            return AssembleCount;
        }
        public ProcessPipe SetAssembleCount(int AssembleCount)
        {
            this.AssembleCount = AssembleCount;
            return this;
        }
        public Priority GetPriority()
        {
            return Priority;
        }
        public ProcessPipe SetPriority(Priority Priority)
        {
            this.Priority = Priority;
            return this;
        }
        public MessageType GetMessageType()
        {
            return MessageType;
        }
        public ProcessPipe SetMessageType(MessageType MessageType)
        {
            this.MessageType = MessageType;
            return this;
        }
        public ProcessPipe SetNotifyType(NotifyType NotifyType)
        {
            this.NotifyType = NotifyType;
            return this;
        }
        public NotifyType GetNotifyType()
        {
            return NotifyType;
        }
        public RepositoryType GetRepositoryType()
        {
            return RepositoryType;
        }
        public ProcessPipe SetDirection(Direction Direction)
        {
            this.Direction = Direction;
            return this;
        }
        public Direction GetDirection()
        {
            return Direction;
        }
        public ProcessPipe SetRepositoryType(RepositoryType RepositoryType)
        {
            this.RepositoryType = RepositoryType;
            return this;
        }
        public object GetData()
        {
            return datas;
        }
    }
}

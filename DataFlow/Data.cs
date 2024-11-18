using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow
{
    [Serializable]
    public class SerialData
    {
        public string message { get; set; }
        public SerialData(string message) { this.message = message; }
    }
    
    [Serializable]
    public class Watchdog
    {
        [Range(0x20, 1, MessageType.Write), Range(0x20, 1, MessageType.Read)]
        public int? _WatchDog { get; set; } = 0;
    }

    [Serializable]
    public class Connect { }

    [Serializable]
    public class DisConnect { }


    [Serializable]
    public class HandShakeSignl
    {
        [Range(0x7000, 1, MessageType.Read)]
        public int? ReadSignalFromLogicControl { get; set; }

        [Range(0x7050, 1, MessageType.Write), Range(0x7050, 1, MessageType.Read)]
        public int? WriteSignalToLogicControl { get; set; }

        [Range(0x7250, 1, MessageType.Read)]
        public int? ReadLevelFromLogicControl { get; set; }
        public bool CompareStatus()
        {
            if (ReadSignalFromLogicControl.HasValue && WriteSignalToLogicControl.HasValue)
                return Nullable.Compare<int>(ReadSignalFromLogicControl, WriteSignalToLogicControl) == 0;
            else
                return false;
        }
    }

    [Serializable]
    public class RepositoryMessage
    {
        public string ID { get; set; }
        public int Level { get; set; }
        public int check { get; set; } = 0;

        public RepositoryMessage() { }
        public RepositoryMessage(string ID, int Level) : this(ID, Level, 0) { }
        public RepositoryMessage(string ID, int Level, int check)
        {
            this.ID = ID;
            this.Level = Level;
            this.check = check;
        }

        public string[] Format()
        {
            if(check == 1)
               return new string[] { ID, Level.ToString(), "OK" };
            else if(check == 2)
               return new string[] { ID, Level.ToString(), "NG" };
            else 
               return new string[] { ID, Level.ToString(), "" };
        }

    }

    [Serializable]
    public class FirstJudgmentMessage
    {
        [Range(0x8025, 14, MessageType.Write, 1)]
        [Range(0x8045, 14, MessageType.Write, 2)]
        [Range(0x8065, 14, MessageType.Write, 3)]
        [Range(0x8085, 14, MessageType.Write, 4)]
        [Range(0x8105, 14, MessageType.Write, 5)]
        [Range(0x8125, 14, MessageType.Write, 6)]
        [Range(0x8145, 14, MessageType.Write, 7)]
        [Range(0x8165, 14, MessageType.Write, 8)]
        [Range(0x8185, 14, MessageType.Write, 9)]
        [Range(0x8205, 14, MessageType.Write, 10)]
        [Range(0x8225, 14, MessageType.Write, 11)]
        [Range(0x8245, 14, MessageType.Write, 12)]
        [Range(0x8265, 14, MessageType.Write, 13)]
        [Range(0x8285, 14, MessageType.Write, 14)]
        [Range(0x8305, 14, MessageType.Write, 15)]
        [Range(0x8325, 14, MessageType.Write, 16)]
        [Range(0x8345, 14, MessageType.Write, 17)]
        public string ID { get; set; }
        public int Level { get; set; }
        public int Request { get; set; } = 0;//1

        public FirstJudgmentMessage() { }
        
        public FirstJudgmentMessage(string ID, int Level)
        {
            this.ID = ID;
            this.Level = Level;
        }

        public RepositoryMessage Proxy()
        {
            return new RepositoryMessage(ID, Level);
        }
    }

    [Serializable]
    public class SecondJudgmentMessage
    {
        [Range(0x8025, 14, MessageType.Read, 1)]
        [Range(0x8045, 14, MessageType.Read, 2)]
        [Range(0x8065, 14, MessageType.Read, 3)]
        [Range(0x8085, 14, MessageType.Read, 4)]
        [Range(0x8105, 14, MessageType.Read, 5)]
        [Range(0x8125, 14, MessageType.Read, 6)]
        [Range(0x8145, 14, MessageType.Read, 7)]
        [Range(0x8165, 14, MessageType.Read, 8)]
        [Range(0x8185, 14, MessageType.Read, 9)]
        [Range(0x8205, 14, MessageType.Read, 10)]
        [Range(0x8225, 14, MessageType.Read, 11)]
        [Range(0x8245, 14, MessageType.Read, 12)]
        [Range(0x8265, 14, MessageType.Read, 13)]
        [Range(0x8285, 14, MessageType.Read, 14)]
        [Range(0x8305, 14, MessageType.Read, 15)]
        [Range(0x8325, 14, MessageType.Read, 16)]
        [Range(0x8345, 14, MessageType.Read, 17)]
        public string ID { get; set; }

        [Range(0x8020, 1, MessageType.Write, 1)]
        [Range(0x8040, 1, MessageType.Write, 2)]
        [Range(0x8060, 1, MessageType.Write, 3)]
        [Range(0x8080, 1, MessageType.Write, 4)]
        [Range(0x8100, 1, MessageType.Write, 5)]
        [Range(0x8120, 1, MessageType.Write, 6)]
        [Range(0x8140, 1, MessageType.Write, 7)]
        [Range(0x8160, 1, MessageType.Write, 8)]
        [Range(0x8180, 1, MessageType.Write, 9)]
        [Range(0x8200, 1, MessageType.Write, 10)]
        [Range(0x8220, 1, MessageType.Write, 11)]
        [Range(0x8240, 1, MessageType.Write, 12)]
        [Range(0x8260, 1, MessageType.Write, 13)]
        [Range(0x8280, 1, MessageType.Write, 14)]
        [Range(0x8300, 1, MessageType.Write, 15)]
        [Range(0x8320, 1, MessageType.Write, 16)]
        [Range(0x8340, 1, MessageType.Write, 17)]
        public int? check { get; set; }
        public string _ID { get; set; }
        public int Level { get; set; } = -1;

        public SecondJudgmentMessage() { }
        public SecondJudgmentMessage(string _ID, int check)
        {
            this._ID = _ID;
            this.check = check;
        }
        public bool CompareID()
        {
            return string.CompareOrdinal(this._ID, this.ID) == 0;
        }
        public RepositoryMessage Proxy()
        {
            return new RepositoryMessage(_ID, Level, (int)check);
        }

    }

    [Serializable]
    public class FirstJudgmentMessageTest
    {
        [Range(0x8025, 14, MessageType.Write, 1)]
        [Range(0x8045, 14, MessageType.Write, 2)]
        [Range(0x8065, 14, MessageType.Write, 3)]
        [Range(0x8085, 14, MessageType.Write, 4)]
        [Range(0x8105, 14, MessageType.Write, 5)]
        [Range(0x8125, 14, MessageType.Write, 6)]
        [Range(0x8145, 14, MessageType.Write, 7)]
        [Range(0x8165, 14, MessageType.Write, 8)]
        [Range(0x8185, 14, MessageType.Write, 9)]
        [Range(0x8205, 14, MessageType.Write, 10)]
        [Range(0x8225, 14, MessageType.Write, 11)]
        [Range(0x8245, 14, MessageType.Write, 12)]
        [Range(0x8265, 14, MessageType.Write, 13)]
        [Range(0x8285, 14, MessageType.Write, 14)]
        [Range(0x8305, 14, MessageType.Write, 15)]
        [Range(0x8325, 14, MessageType.Write, 16)]
        [Range(0x8345, 14, MessageType.Write, 17)]
        public string ID { get; set; }
        public int Level { get; set; }

        public FirstJudgmentMessageTest() { }

        public FirstJudgmentMessageTest(string ID, int Level)
        {
            this.ID = ID;
            this.Level = Level;
        }
        public RepositoryMessage Proxy()
        {
            return new RepositoryMessage(ID, Level, 0);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

namespace DataFlow
{
    public interface Decoding<T>
    {
        IEnumerable<T> Decode(byte[] code);
        bool isFail();
        void Clear();
    }

    public class SerialPortDecoding : Decoding<string>
    {
        private byte STX = Extensions.isExistinEnum<Command, byte>("STX");  
        private byte ETX = Extensions.isExistinEnum<Command, byte>("ETX");
        private List<byte> DecodingMessage = new List<byte>();
        public IEnumerable<string> Decode(byte[] code)
        {
            if (!Equals(code, default) && code.Length > 0)
                DecodingMessage.AddRange(code);

            if (DecodingMessage.Count() > 0)
            {
                int head_index = DecodingMessage.IndexOf(STX, 0);
                int tail_index = DecodingMessage.IndexOf(ETX, 0);
                if(tail_index > head_index)
                {
                    List<byte> message = DecodingMessage.GetRange(head_index, tail_index - head_index + 1);
                    if (!Equals(message, default))
                    {
                        string _show = Extensions.GetASCIIToHexStr(message.ToArray()) + " ( ";
                        //LogHandlerManager.Instance.GetLogHandler(LogType.Serial_Status).refresh(DateTime.Now + " " + this.ToString() + " Receive: " + message.ToArray());
                        message.Remove(STX);
                        message.Remove(ETX);
                        _show += (Encoding.ASCII.GetString(message.ToArray()) + " ) ");
                        LogHandlerManager.Instance.GetLogHandler(LogType.Serial_Status).refresh(DateTime.Now  + " RS232 Receive: " + _show);
                        LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(DateTime.Now + " RS232 Receive: " + _show);
                        yield return Encoding.ASCII.GetString(message.ToArray()).Trim();
                        DecodingMessage.RemoveRange(head_index, tail_index - head_index + 1);
                        Decode(default);
                    }
                }
            }
        }
        public bool isFail()
        {
            if (DecodingMessage.Count > 1000)
            {
                DecodingMessage.Clear();
                return true;
            }
            else
                return false;
        }
        public void Clear() { DecodingMessage.Clear(); }
    }

    public class LogicControlDecoding : Decoding<byte[]>
    {
        private byte DLE = Extensions.isExistinEnum<Command, byte>("DLE");
        private byte ACK = Extensions.isExistinEnum<Command, byte>("ACK");
        private byte ETX = Extensions.isExistinEnum<Command, byte>("ETX");

        private List<byte> DecodingMessage = new List<byte>();

        private Decapsulation Decapsulation = new Decapsulation();


        public IEnumerable<byte[]> Decode(byte[] code)
        {
            if (!Equals(code, default) && code.Length > 0)
                DecodingMessage.AddRange(code);

            if (DecodingMessage.Count() > 0)
            {
                int headindex, tailindex;
                if (FindIndex(out headindex, out tailindex))
                {
                    List<byte> message = Decapsulation.Decodeing(DecodingMessage.GetRange(headindex, tailindex - headindex + 1));
                    if (!Equals(message, default))
                    {
                        yield return message.ToArray();
                        DecodingMessage.RemoveRange(headindex, tailindex - headindex + 1);
                        Decode(default);
                    }
                }

            }
        }
        public bool isFail()
        {
            if (DecodingMessage.Count > 1000)
            {
                DecodingMessage.Clear();
                return true;
            }
            else
                return false;
        }
        public void Clear()
        {
            DecodingMessage.Clear();
        }
        private bool FindIndex(out int HeadIndex, out int TailIndex)
        {
            int headindex = 0;
            int tailindex = 0;
            int index = DecodingMessage.IndexOf(DLE, 0);

            while ((index != -1) || (tailindex < headindex))
            {
                if ((index + 1 <= DecodingMessage.Count() - 1) && DecodingMessage.ElementAt(index + 1) == ACK)
                {
                    if (headindex != 0)
                        headindex = index;
                }
                else if ((index + 1 <= DecodingMessage.Count() - 1) && DecodingMessage.ElementAt(index + 1) == ETX)
                {
                    if (index + 3 <= DecodingMessage.Count() - 1)
                        tailindex = index + 3;
                }
                index = DecodingMessage.IndexOf(DLE, index + 1);
            }

            HeadIndex = headindex;
            TailIndex = tailindex;
            if (tailindex > headindex)
                return true;
            else 
                return false;
        }
    }


   

    public class TemporaryStorage<T>
    {
        Decoding<T> Decoding = default;
        public TemporaryStorage() { }
        public TemporaryStorage(Decoding<T> Decoding) { this.Decoding = Decoding; }

        public IEnumerable<T> Decode(byte[] code)
        {
            foreach (var _T in Decoding.Decode(code))
                yield return _T;
        }
        public bool isFail() { return Decoding.isFail(); }

        public void Clear() { Decoding.Clear(); }
    }
}

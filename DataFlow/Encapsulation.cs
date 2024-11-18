using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow
{
    public abstract class MessageHandle
    {
        public abstract byte[] Handle();
    }


    public class HandleDataMessage<Typename>
    {
        public int InitialIndex { get; set; } = default(int);
        public int Word_Number { get; set; } = default(int);
        public Typename Message { get; set; } = default(Typename);
        public MessageType MessageType { get; set; } = MessageType.Read;

        public HandleDataMessage(int InitialIndex, int Word_Number, Typename Message, MessageType MessageType)
        {
            this.InitialIndex = InitialIndex;
            this.Word_Number = Word_Number;
            this.Message = Message;
            this.MessageType = MessageType;
        }

        public List<byte> Get_InitialIndex_ByteList(byte byte_number = 3)
        {
            return BitConverter.GetBytes(InitialIndex).Take(byte_number).ToList();
        }

        public List<byte> Get_Number_ByteList(byte byte_number = 2)
        {
            return BitConverter.GetBytes(Word_Number).Take(byte_number).ToList();
        }

        public List<byte> Get_Message_ByteList()
        {
            List<byte> MessageList = new List<byte>();

            try
            {
                if (MessageType == MessageType.Read)
                    return MessageList;
                else
                {
                    MethodInfo generic = Extensions.GetMethodInfo<HandleDataMessage<Typename>>(nameof(_Get_Message_ByteList), new Type[] { typeof(Typename) });
                    var result = (IEnumerable<byte>)generic.Invoke(this, new object[] { Message });

                    foreach (var item in result)
                    {
                        MessageList.Add(item);
                    }

                    MessageList.InsertRange(0, Enumerable.Repeat((byte)0x00, (Word_Number << 1) - MessageList.Count).ToList());
                    MessageList.Reverse();
                    return MessageList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<byte> _Get_Message_ByteList(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                byte[] arr = Encoding.ASCII.GetBytes(msg);
                for (int len = 0; len < arr.Length; ++len)
                {
                    //yield return 0;
                    yield return arr[len];
                }
            }
            else
                yield return 0;
        }

        public IEnumerable<byte> _Get_Message_ByteList(int? msg)
        {
            foreach (var item in __Get_Message_ByteList((int)msg))
                yield return item;
        }

        public IEnumerable<byte> _Get_Message_ByteList(double? msg)
        {
            int _msg = (int)Math.Round((double)msg);
            foreach (var item in __Get_Message_ByteList(_msg))
                yield return item;
        }

        public IEnumerable<byte> __Get_Message_ByteList(int msg)
        {
            //int value = Convert.ToByte(msg);
            //dynamic _msg = Convert.ChangeType(msg, msg.GetType());
            byte Digit_High = Convert.ToByte((msg >> 8) & 0x00FF);
            byte Digit_Low  = Convert.ToByte( msg & 0x00FF);
            List<byte> arr_high = BitConverter.GetBytes(Digit_High).Take(1).ToList();
            List<byte> arr_low  = BitConverter.GetBytes(Digit_Low).Take(1).ToList();
            arr_high.AddRange(arr_low);
            for (int len = 0; len < arr_high.Count; ++len)
            {
                yield return arr_high[len];
            }
        }
    }
    public class Encapsulation<T> : MessageHandle
    {
        private List<byte> head = new List<byte>() { Extensions.isExistinEnum<Command, byte>("DLE"), Extensions.isExistinEnum<Command, byte>("STX") };
        private List<byte> tail = new List<byte>() { Extensions.isExistinEnum<Command, byte>("DLE"), Extensions.isExistinEnum<Command, byte>("ETX") };

        private HandleDataMessage<T> HandleDataMessage = default;
        public Encapsulation(HandleDataMessage<T> HandleDataMessage)
        {
            this.HandleDataMessage = HandleDataMessage;
        }
        public override byte[] Handle()
        {
            try
            {
                List<byte> msg = HandleDataMessage.MessageType == MessageType.Read ? new List<byte>() { Extensions.isExistinEnum<Command, byte>("READ") } : new List<byte>() { Extensions.isExistinEnum<Command, byte>("WRITE") };

                msg.Add(Extensions.isExistinEnum<Command, byte>("CMP"));
                msg.AddRange(HandleDataMessage.Get_InitialIndex_ByteList());
                msg.AddRange(HandleDataMessage.Get_Number_ByteList());
                msg.AddRange(HandleDataMessage.Get_Message_ByteList());

                msg.InsertRange(0, DataCount(msg));
                msg.Insert(0, Extensions.isExistinEnum<Command, byte>("STN"));
                var checksum = CheckSum(msg);
                msg = Insert_0x10(msg);
                msg.InsertRange(0, head);
                msg.AddRange(tail);
                msg.AddRange(checksum);
                return msg.ToArray();
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private List<byte> DataCount(List<byte> data)
        {
            return BitConverter.GetBytes(data.Count).Take(2).ToList();
        }

        private  List<byte> Insert_0x10(List<byte> _data)
        {
            List<byte> data = new List<byte>();
            foreach (var item in IEnumerable_Insert_0x10(_data))
            {
                data.Add(item);
            }
            return data;
        }

        private IEnumerable<byte> IEnumerable_Insert_0x10(List<byte> data)
        {
            for (int index = 0; index < data.Count; ++index)
            {
                if (data[index] == 0x10)
                {
                    yield return 0x10;
                }
                yield return data[index];
            }
        }

        private List<byte> CheckSum(List<byte> data)
        {
            List<byte> checksum = new List<byte>();
            try
            {
                int _check_sum = data.Sum(item => item) & 0x00FF;
                var _Low = Encoding.ASCII.GetBytes((_check_sum & 0x0F).ToString("X")).Take(1).ToList();
                var _High = Encoding.ASCII.GetBytes(((_check_sum >> 4) & 0x0F).ToString("X")).Take(1).ToList();
                checksum.AddRange(_High);
                checksum.AddRange(_Low);
                return checksum;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow
{
    public class Decapsulation
    {
        List<byte> head = new List<byte>() { Extensions.isExistinEnum<Command, byte>("DLE"), Extensions.isExistinEnum<Command, byte>("ACK") };
        List<byte> tail = new List<byte>() { Extensions.isExistinEnum<Command, byte>("DLE"), Extensions.isExistinEnum<Command, byte>("ETX") };

        public List<byte> Decodeing(List<byte> msg)
        {
            List<byte> data = new List<byte>();
            try
            {
                List<byte> message = msg.ToList();
                if (Check(ref message))
                {
                    message = Remove_0x10(message);
                    message.RemoveAt(0);
                    message.RemoveAt(2);

                    int data_len = message[0] + (message[1] << 8);
                    message.RemoveRange(0, 2);

                    data = _Decoding(message);

                    if (data_len != data.Count + 1)
                        data = new List<byte>();

                    return data;
                }
                else
                    return null;
            }
            catch (Exception) { return null; }
        }

        private List<byte> _Decoding(List<byte> message)
        {
            List<byte> data = new List<byte>();
            foreach (var dataItem in IEnumerable_Decoding(message))
                data.Add(dataItem);

            return data;
        }

        private IEnumerable<byte> IEnumerable_Decoding(List<byte> message)
        {
            for (int index = 0; index < message.Count; ++index)
                yield return message[index];
        }

        private List<byte> Remove_0x10(List<byte> _data)
        {
            List<byte> data = new List<byte>();
            foreach (var item in IEnumerable_Remove_0x10(_data))
                data.Add(item);
            return data;
        }

        private IEnumerable<byte> IEnumerable_Remove_0x10(List<byte> data)
        {
            byte temp = 0;
            for (int index = 0; index < data.Count; ++index)
            {
                if (temp == 0x10 && data[index] == 0x10)
                    temp = 0;
                else if (data[index] == 0x10)
                {
                    temp = data[index];
                    yield return data[index];
                }
                else
                {
                    temp = 0;
                    yield return data[index];
                }
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

        private bool Check(ref List<byte> message)
        {
            try
            {
                if (head.SequenceEqual(message.Take(head.Count)) && tail.SequenceEqual(message.Skip(message.Count - 4).Take(tail.Count)))
                {
                    List<byte> _checksum = message.Skip(message.Count - 2).Take(2).ToList();
                    message.RemoveRange(0, head.Count);
                    message.RemoveRange(message.Count - tail.Count - _checksum.Count, tail.Count + _checksum.Count);
                    message = Remove_0x10(message);

                    List<byte> checksum = CheckSum(message);
                    if (checksum.SequenceEqual(_checksum))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}

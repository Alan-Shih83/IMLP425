using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DataFlow
{
    public class ProcessPipeControl
    {
       
        KeyValuePair<ProcessPipe, Assemble> KeyValuePair;

        QueueOperation<MessageHandle> queue = new QueueOperation<MessageHandle>();
        public void SetProcessPipe(ProcessPipe pipe)
        {
            if(!Equals(pipe, default) && !Equals(pipe.GetData(), default))
            {
                this.KeyValuePair = new KeyValuePair<ProcessPipe, Assemble>(pipe, pipe.GetData() as Assemble);
                foreach (var Message in DataHandle.GetExpandMessageItem(this.KeyValuePair.Value))
                    queue.SetQueueItem(Message);
            }
        }
        public ProcessPipe Assemble(byte[] message)
        {
            if(this.KeyValuePair.Value?.MessageType == MessageType.Read)
            {
                MethodInfo generic = Extensions.GetGenericMethodInfo<DataHandle>(nameof(DataHandle.GetMessageAssemble), new Type[] { this.KeyValuePair.Value.PropertyInfo.PropertyType }, new Type[] { typeof(byte[]) });
                dynamic result = generic.Invoke(null, new object[] { message });
                dynamic value = this.KeyValuePair.Value.PropertyInfo.GetValue(this.KeyValuePair.Value._object);
                //if (value != null)
                //    this.KeyValuePair.Value.PropertyInfo.SetValue(this.KeyValuePair.Value._object, value + result);
                //else
                //    this.KeyValuePair.Value.PropertyInfo.SetValue(this.KeyValuePair.Value._object, result);
                if (value != null && this.KeyValuePair.Value.PropertyInfo.PropertyType == typeof(string))
                    this.KeyValuePair.Value.PropertyInfo.SetValue(this.KeyValuePair.Value._object, value + result);
                else
                    this.KeyValuePair.Value.PropertyInfo.SetValue(this.KeyValuePair.Value._object, result);
            }
           
            if (queue.Count() > 0)
                return default;
            else
                return this.KeyValuePair.Key;
        }
        public byte[] Encapsulate()
        {
            MessageHandle handle = queue.GetQueueItem();
            if (!Equals(handle, default))
                return handle.Handle();
            else
                return default;
        }
        public void Clear()
        {
            queue.Clear();
            KeyValuePair = default;
        }
        public bool isEmpty()
        {
            return queue.Count() == 0;
        }
    }
}

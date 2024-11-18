using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography;

namespace DataFlow
{
    public class Assembly
    {
        HashSet<uint> RandomSets = new HashSet<uint>();

        Hashtable Storage = new Hashtable();

        StorageCreater<PropertyInfo> Creater = new StorageCreater<PropertyInfo>();

        readonly object Lock = new object();

        public IEnumerable<ProcessPipe> Decompose(ProcessPipe pipe)
        {
            IEnumerable<Assemble> Assembles = PropertySearch.Instance.GetPropertyInfo(pipe);
            if (Assembles.Count() > 0)
            {
                uint RandomCode = GetRandomCode();
                ProcessPipe ProcessPipe = pipe.DeepClone().SetAssembleCount(Assembles.Count()).SetRandomCode(RandomCode) as ProcessPipe;
                Storage.Add(ProcessPipe, Creater.Pull(Assembles.Count()));
                foreach (var assemble in Assembles)
                    yield return new ProcessPipe(assemble).SetRandomCode(RandomCode).SetMessageType(ProcessPipe.GetMessageType()).SetPriority(ProcessPipe.GetPriority());
            }
            else
            {
                LogHandlerManager.Instance.GetLogHandler(LogType.Event).refresh(this.ToString() + " Assembles.Count() == 0");
                pipe.SetRandomCode(0);
                yield return pipe.DeepClone();
            }  
        }

        public IEnumerable<ProcessPipe> Compose(ProcessPipe _pipe)
        {
            Assemble assemble = _pipe.GetData() as Assemble;
            ProcessPipe pipe = Storage.Keys.OfType<ProcessPipe>().FirstOrDefault(index => index.GetRandomCode() == _pipe.GetRandomCode());
            if (!Equals(pipe, default))
            {
                //pipe.GetKeyValuePair().Value.GetType().GetProperty(assemble.PropertyInfo.Name).SetValue(pipe.GetKeyValuePair().Value, assemble.PropertyInfo.GetValue(pipe.GetKeyValuePair().Value));
                pipe.GetData().GetType().GetProperty(assemble.PropertyInfo.Name).SetValue(pipe.GetData(), assemble.PropertyInfo.GetValue(assemble._object));
                Storage<PropertyInfo> _Storage = (Storage[pipe] as Storage<PropertyInfo>);
                if(!Equals(_Storage, default))
                {
                    _Storage.SetRecord(assemble.PropertyInfo);
                    if (_Storage.Count() == pipe.GetAssembleCount())
                    {
                        lock (Lock)
                        {
                            RandomSets.Remove(pipe.GetRandomCode());
                        }
                        Creater.Push(_Storage);
                        Storage.Remove(pipe);
                        yield return pipe.DeepClone();
                    }
                }
            }
            else
                yield return default;
        }

        public void Release()
        {
            RandomSets.Clear();
            IDictionaryEnumerator Enumerator = Storage.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                DictionaryEntry Current = (DictionaryEntry)Enumerator.Current;
                Creater.Push(Current.Value as Storage<PropertyInfo>);
            }
            Storage.Clear();
        }

        private uint GetRandomCode()
        {
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                uint identityCode = default(uint);

                byte[] randomNumber = new byte[4];
                lock (Lock)
                {
                    do
                    {
                        rngCsp.GetBytes(randomNumber);
                        foreach (var ran in randomNumber)
                        {
                            identityCode <<= 8;
                            identityCode += ran;
                        }
                    }
                    while (RandomSets.Contains(identityCode) || identityCode == 0);
                    RandomSets.Add(identityCode);
                }
                return identityCode;
            }
        }
    }
}

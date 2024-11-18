using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DataFlow
{


    public class Assemble
    {
        public MessageType MessageType { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public object _object { get; set; }
        public int Level { get; set; }
        public Assemble(PropertyInfo PropertyInfo, object _object, MessageType MessageType, int Level = -1) 
        {
            this.PropertyInfo = PropertyInfo;
            this._object = _object;
            this.MessageType = MessageType;
            this.Level = Level;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public class Range : System.Attribute
    {
        public int InitialIndex = default(int);
        public int number = default(int);
        public int MaxRange = 64;
        public int Segment = default(int);
        public int EndIndex = default(int);
        public int Level = -1;
        public MessageType status;

        public Range(int InitialIndex, int number, MessageType status)
        {
            this.InitialIndex = InitialIndex;
            this.number = number;
            this.status = status;
            this.EndIndex = InitialIndex + number;
        }
        public Range(int InitialIndex, int number, MessageType status, int Level)
        {
            this.InitialIndex = InitialIndex;
            this.number = number;
            this.status = status;
            this.EndIndex = InitialIndex + number;
            this.Level = Level;
        }
    }

    public class PropertySearch
    {
        //Storage<PropertyInfo> Storage = new Storage<PropertyInfo>(new PropertyEqualityComparer());

        static readonly Lazy<PropertySearch> singleton = new Lazy<PropertySearch>(() => new PropertySearch(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        public static PropertySearch Instance { get { return singleton.Value; } }

        PropertySearch()
        {
            //LogicControlProperty LogicControlProperty = new LogicControlProperty();
            //PropertyInfo[] properties = LogicControlProperty.GetType().GetProperties();
            //foreach (var property in properties)
            //    Storage.SetRecord(property);
        }



        public IEnumerable<Assemble>  GetPropertyInfo(ProcessPipe Pipe)
        {
            try
            {
                var properties = Pipe.GetData().GetType().GetProperties()
                                     .Where(property => property.GetCustomAttributes(typeof(Range), false).AsEnumerable().FirstOrDefault(attr => (attr as Range).status == Pipe.GetMessageType()) != default);
                
                var InnerJoinQuery = from _properties in properties
                                     where Pipe.GetMessageType() == MessageType.Write ? _properties.GetValue(Pipe.GetData()) != null : true
                                     select (_properties);

                int Level = -1;
                var info = Pipe.GetData().GetType().GetProperty("Level");
                if (!Equals(info, null))
                    Level = (int)info.GetValue(Pipe.GetData());

                List<Assemble> Assembles = new List<Assemble>();
                foreach (var property in InnerJoinQuery)
                {
                    if (property.Name.Contains("WatchDog"))
                    {
                        property.SetValue(Pipe.GetData(), GetRandomCode());
                        Assembles.Add(new Assemble(property, Pipe.GetData().DeepClone(), MessageType.Write));
                    }
                    else
                        Assembles.Add(new Assemble(property, Pipe.GetData().DeepClone(), Pipe.GetMessageType(), Level));
                }
                return Assembles;
            }
            catch(Exception ex)
            {
                LogHandlerManager.Instance.GetLogHandler(LogType.Error).refresh(this.ToString() + ex.ToString());
                return default;
            }
            
        }

        private int GetRandomCode()
        {
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                int identityCode = default(int);
                byte[] randomNumber = new byte[1];

                rngCsp.GetBytes(randomNumber);
                foreach (var ran in randomNumber)
                {
                    identityCode <<= 8;
                    identityCode += ran;
                }
                return identityCode;
            }
        }
    }
}

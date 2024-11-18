using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Text.Json;
using System.Xml.Serialization;
using System.Reflection;

namespace DataFlow
{
    public interface Format
    {
        string Serialize(object _object);
        T Deserialize<T>(string str);
    }

    public class JsonFormat : Format
    {
        public string Serialize(object _object)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
                return JsonSerializer.Serialize(_object, options);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public T Deserialize<T>(string str)
        {
            if (string.IsNullOrEmpty(str))
                return default(T);
            else
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(str);
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
        }
    }
}

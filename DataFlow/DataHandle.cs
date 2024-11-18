using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow
{
    public class DataHandle
    {

        public static T GetDefaultValue<T>()
        {
            if (Nullable.GetUnderlyingType(typeof(T)) != null)
                return (T)Activator.CreateInstance(Nullable.GetUnderlyingType(typeof(T)));
            else
                return default(T);
        }

        //public static object GetCompositeMessage(List<HandleData> HandleDataList)
        //{
        //    try
        //    {
        //        if (HandleDataList.Count > 0)
        //        {
        //            List<HandleData> query = HandleDataList.OrderBy(item => item.EncapsulationHandle.InitialIndex).ToList();
        //            object _object = query.First()._object;
        //            for (; query.Count > 0;)
        //            {
        //                PropertyInfo propertyInfo = query.First().info;
        //                //dynamic defaultValue = propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
        //                MethodInfo generic = Extensions.GetGenericMethodInfo<DataHandle>(nameof(DataHandle.GetDefaultValue), new Type[] { propertyInfo.PropertyType }, new Type[] { });
        //                dynamic defaultValue = generic.Invoke(null, null);

        //                var FindItem = query.Where(Item => Item.info.Name == propertyInfo.Name);
        //                foreach (var item in FindItem)
        //                {
        //                    dynamic value = item.info.GetValue(item._object);
        //                    if (value != null)
        //                        defaultValue += value;
        //                }
        //                _object.GetType().GetProperties().FirstOrDefault(property => property.Name == propertyInfo.Name).SetValue(_object, defaultValue);


        //                query.RemoveAll(q => q.info.Name == propertyInfo.Name);
        //            }
        //            return _object;
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public static T GetMessageAssemble<T>(byte[] msg)
        {
            //dynamic result = default(T);
            dynamic result = GetDefaultValue<T>();
            foreach (var item in MessageAssemble<T>(msg))
            {
                result += item;
            }
            return result;
        }

        private static IEnumerable<T> MessageAssemble<T>(byte[] msg)
        {
            MethodInfo generic = Extensions.GetMethodInfo<DataHandle>(nameof(_MessageAssemble), new Type[] { typeof(byte[]), typeof(T).MakeByRefType() });
            //dynamic result = typeof(T).IsValueType ? Activator.CreateInstance(typeof(T)) : null;
            dynamic result = GetDefaultValue<T>();

            for (int len = 0; len < msg.Length; len += 2)
            {
                object[] parameters = new object[] { Enumerable.Range(len, 2).Select(x => msg[x]).ToArray(), result };
                generic.Invoke(null, parameters);

                dynamic value = parameters[1];
                //T __value = (T)parameters.FirstOrDefault(_value => _value.GetType() == typeof(T));
                if (value != null)
                    yield return value;
            }
        }

        private static void _MessageAssemble(byte[] msg, out string str)
        {
            str = Encoding.ASCII.GetString(msg).Trim('\0');
            //str = (string)Convert.ChangeType(Encoding.ASCII.GetString(msg).Trim('\0'), typeof(string));
        }

        private static void _MessageAssemble(byte[] msg, out int? number)
        {
            //int _number;
            //_MessageAssemble(msg, out _number);
            //number = _number;
            number = (int)Convert.ChangeType((msg[1] << 8) + msg[0], typeof(int));
        }

        private static void _MessageAssemble(byte[] msg, out double? number)
        {
            //int _number;
            //_MessageAssemble(msg, out _number);
            //number = _number;
            number = (double)Convert.ChangeType((msg[1] << 8) + msg[0], typeof(double));
        }
        //private static void _MessageAssemble(byte[] msg, out int number)
        //{
        //    number = (msg[1] << 8) + msg[0];
        //    //number = (int)Convert.ChangeType((msg[1] << 8) + msg[0], typeof(int));
        //}

        public static IEnumerable<MessageHandle> GetExpandMessageItem(Assemble Assemble, int MaxRange = 64)
        {
            foreach (var MessageHandle in ExpandMessageItem(Assemble, MaxRange))
            {
                if(!Equals(MessageHandle, default))
                    yield return MessageHandle;
            }
        }

        private static Range GetMessageRange(Assemble Assemble)
        {
            var RangeFieldAttribute = Assemble.PropertyInfo.GetCustomAttributes(typeof(Range), false);
            if(Assemble.Level > 0)
                return RangeFieldAttribute.FirstOrDefault(index => (index as Range).status == Assemble.MessageType && (index as Range).Level == Assemble.Level) as Range;
            else
                return RangeFieldAttribute.FirstOrDefault(index => (index as Range).status == Assemble.MessageType) as Range;
        }

        private static IEnumerable<MessageHandle> ExpandMessageItem(Assemble Assemble, int MaxRange = 64)
        {
            Range range = GetMessageRange(Assemble);
            if (Equals(range, default))
                yield return default;
            else
            {
                //MethodInfo generic = Extensions.GetMethodInfo<DataHandle>(nameof(MessageDivide), new Type[] { Assemble.PropertyInfo.PropertyType, typeof(Range) });
                //var result = (IEnumerable<dynamic>)generic.Invoke(null, new object[] { Assemble.PropertyInfo.GetValue(Assemble._object), range });
                //foreach (var item in result)
                //    yield return Extensions.CreateInstance(item, typeof(Encapsulation<>)) as MessageHandle;

                MethodInfo generic = default;
                if (Assemble.PropertyInfo.PropertyType == typeof(string))
                    generic = Extensions.GetMethodInfo<DataHandle>(nameof(MessageDivide), new Type[] { Assemble.PropertyInfo.PropertyType, typeof(Range) });
                else
                    generic = Extensions.GetGenericMethodInfo<DataHandle>(nameof(MessageDivide), new Type[] { Nullable.GetUnderlyingType(Assemble.PropertyInfo.PropertyType) }, new Type[] { Nullable.GetUnderlyingType(Assemble.PropertyInfo.PropertyType), typeof(Range) });
                var result = (IEnumerable<dynamic>)generic.Invoke(null, new object[] { Assemble.PropertyInfo.GetValue(Assemble._object), range });
                foreach (var item in result)
                    yield return Extensions.CreateInstance(item, typeof(Encapsulation<>)) as MessageHandle;
            }
        }

        //private static IEnumerable<HandleDataMessage<double?>> MessageDivide(double? number, Range range)
        //{
        //    int len = (int)Math.Ceiling(range.number / (decimal)range.MaxRange);

        //    for (int _len = 0; _len < len; ++_len)
        //    {
        //        range.Segment = (range.InitialIndex + range.MaxRange >= range.EndIndex) ? (range.EndIndex - range.InitialIndex) : (range.MaxRange);
        //        if (number != -1)
        //        {
        //            yield return new HandleDataMessage<double?>(range.InitialIndex, range.Segment, number, range.status);
        //        }
        //        else
        //        {
        //            yield return new HandleDataMessage<double?>(range.InitialIndex, range.Segment, -1, range.status);
        //        }
        //        range.InitialIndex += range.Segment;
        //    }
        //}

        //private static IEnumerable<HandleDataMessage<int?>> MessageDivide(int? number, Range range)
        //{
        //    int len = (int)Math.Ceiling(range.number / (decimal)range.MaxRange);

        //    for (int _len = 0; _len < len; ++_len)
        //    {
        //        range.Segment = (range.InitialIndex + range.MaxRange >= range.EndIndex) ? (range.EndIndex - range.InitialIndex) : (range.MaxRange);
        //        if (number != -1)
        //        {
        //            yield return new HandleDataMessage<int?>(range.InitialIndex, range.Segment, number, range.status);
        //        }
        //        else
        //        {
        //            yield return new HandleDataMessage<int?>(range.InitialIndex, range.Segment, -1, range.status);
        //        }
        //        range.InitialIndex += range.Segment;
        //    }
        //}

        private static IEnumerable<HandleDataMessage<Nullable<T>>> MessageDivide<T>(T value, Range range) where T : struct
        {
            int len = (int)Math.Ceiling(range.number / (decimal)range.MaxRange);
            for (int _len = 0; _len < len; ++_len)
            {
                range.Segment = (range.InitialIndex + range.MaxRange >= range.EndIndex) ? (range.EndIndex - range.InitialIndex) : (range.MaxRange);
                yield return new HandleDataMessage<Nullable<T>>(range.InitialIndex, range.Segment, value, range.status);
                range.InitialIndex += range.Segment;
            }
        }

        private static IEnumerable<HandleDataMessage<string>> MessageDivide(string str, Range range)
        {
            int len = (int)Math.Ceiling(range.number / (decimal)range.MaxRange);

            string _str = null;
            if (!string.IsNullOrEmpty(str))
                str = new string(str.Reverse().ToArray());

            for (int _len = 0; _len < len; ++_len)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    if (str.Length >= range.MaxRange)
                    {
                        _str = str.Substring(0, range.MaxRange);
                        str = str.Remove(0, range.MaxRange);
                        range.Segment = range.MaxRange;
                    }
                    else
                    {
                        _str = str.Substring(0);
                        str = str.Remove(0, _str.Length);
                        range.Segment = (range.InitialIndex + range.MaxRange >= range.EndIndex) ? (range.EndIndex - range.InitialIndex) : (range.MaxRange);
                    }
                }
                else
                {
                    _str = null;
                    range.Segment = (range.InitialIndex + range.MaxRange >= range.EndIndex) ? (range.EndIndex - range.InitialIndex) : (range.MaxRange);
                }

                yield return new HandleDataMessage<string>(range.InitialIndex, range.Segment, _str, range.status);
                range.InitialIndex += range.Segment;
            }
        }


    }
}

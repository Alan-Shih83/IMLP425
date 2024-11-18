using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;

namespace DataFlow
{
	public static class Extensions
	{
		public static string GetASCIIToHexStr(byte[] arr)
        {
			if (Equals(default(byte[]), arr) || arr.Length == 0)
				return default(string);
			else
			{
				char[] charArray = Encoding.ASCII.GetString(arr).ToCharArray();
				StringBuilder builder = new StringBuilder(charArray.Length);
				foreach (var _c in charArray)
					builder.AppendFormat("0x{0:X2} ", (int)_c);
				return builder.ToString().Trim();
			}
		}

		public static T DeepClone<T>(this T item)
		{
			if (item != null)
			{
				using (var stream = new MemoryStream())
				{
					var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
					formatter.Serialize(stream, item);
					stream.Seek(0, SeekOrigin.Begin);
					var result = (T)formatter.Deserialize(stream);
					return result;
				}
			}
			return default(T);
		}

		public static string PropertyConvert<T>(this T item, PropertyInfo property)
		{
			dynamic value = property.GetValue(item);
			if (!Equals(value, null))
				return Convert.ChangeType(value, typeof(string)) as string;
			else
				return default(string);
		}

		//public static T GetDefaultValue<T>()
		//{
		//	if (Nullable.GetUnderlyingType(typeof(T)) != null)
		//		return (T)Activator.CreateInstance(Nullable.GetUnderlyingType(typeof(T)));
		//	else
		//		return default(T);
		//}

		public static T MinValue<T>(this Type self)
		{
			if (typeof(T) == typeof(string))
				return default(T);
			else
				return (T)self.GetField(nameof(MinValue)).GetRawConstantValue();
		}

		public static T MaxValue<T>(this Type self)
		{
			return (T)self.GetField(nameof(MaxValue)).GetRawConstantValue();
		}

		public static string GetEnumName<TEnum>(int value) where TEnum : Enum
		{
			try
			{
				var EnumNameList = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
				return EnumNameList.Count > value ? EnumNameList[value].ToString() : default(string);
			}
			catch (Exception)
			{
				return default(string);
			}
		}

		public static TValue isExistinEnum<T, TValue>(string item) where T : System.Enum 
		{

			var EnumList = Enum.GetValues(typeof(T))
						 .Cast<T>()
						 .Where(name => Enum.GetName(typeof(T), name)
						 .StartsWith(item, StringComparison.OrdinalIgnoreCase))
						 .Select(x => (TValue)Convert.ChangeType(x, typeof(TValue)));

			return EnumList.ToList().Count > 0 ? EnumList.ToList().First() : typeof(TValue).MinValue<TValue>();
		}

		public static TValue isExistinEnum<TEnum, TValue>(this Type self) where TEnum : Enum
		{
			string NameSpaceName = MethodBase.GetCurrentMethod().DeclaringType.Namespace + ".";
			string ClassName = TypeDescriptor.GetClassName(self).Trim(NameSpaceName.ToCharArray());
			return isExistinEnum<TEnum, TValue>(ClassName);
		}

		public static MethodInfo GetGenericMethodInfo<T>(string function, Type[] generic_types, params Type[] types) where T : class
		{
			try
			{
				var methods = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance)
									   .Where(m => m.Name == function
												&& m.IsGenericMethod
												&& m.GetParameters().Count() == types.Length).ToList();

				MethodInfo generic = methods.First().MakeGenericMethod(generic_types);
				return generic;
			}
			catch (Exception ex)
			{
				return null;
			}

		}

		public static MethodInfo GetMethodInfo<T>(string function, params Type[] template_Type) where T : class
		{
			try
			{
				return typeof(T).GetMethod(function, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic, null, template_Type, null);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static object CreateInstance(object arg, Type type)
		{
			Type[] typeArgs = arg.GetType().GetGenericArguments();
			Type constructed = type.MakeGenericType(typeArgs);
			return Activator.CreateInstance(constructed, new object[] { arg });
		}

		public static object CreateInstance(Type classType, Type[] typeParams, params object[] arg)
		{
			Type constructedType = classType.MakeGenericType(typeParams);
			return Activator.CreateInstance(constructedType, arg);
		}

		public static object CreateInstance(Type type) 
		{
			//string NameSpaceName = MethodBase.GetCurrentMethod().DeclaringType.Namespace + ".";
			//Type _type = type.GetType();
			return Activator.CreateInstance(type);
		}

		public static object CreateInstance(string name)
        {
			string NameSpaceName = MethodBase.GetCurrentMethod().DeclaringType.Namespace + ".";
			Type type = Type.GetType(NameSpaceName + name);
			return Activator.CreateInstance(type);
		}

		public static async Task Delay(int iSecond, CancellationTokenSource source_delay = default(CancellationTokenSource))
		{
			try
			{
				if (!EqualityComparer<CancellationTokenSource>.Default.Equals(source_delay, default(CancellationTokenSource)))
					await Task.Delay(iSecond, source_delay.Token);
				else
					await Task.Delay(iSecond);
			}
			catch (OperationCanceledException) { }
		}


	}
}

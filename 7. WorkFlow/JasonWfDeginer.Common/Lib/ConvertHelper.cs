using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace JasonWfDesigner.Common.Lib
{
    /// <summary>
    ///     自定义的转换帮助
    /// </summary>
    public class ConvertHelper
    {
        private static DateTime _unixTime = new DateTime(1970, 1, 1, 8, 0, 0);

        /// <summary>
        ///     转换对象为一个指定的数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ConvertValue<T>(object obj)
        {
            object obj2 = default(T);
            if (obj == null)
            {
                return default(T);
            }
            if (obj is string)
            {
                obj2 = ObjToStr(obj);
            }
            else if (typeof(T) == typeof(int))
            {
                obj2 = ObjToInt(obj);
            }
            else if (typeof(T) == typeof(float))
            {
                obj2 = ObjToFloat(obj);
            }
            else if (typeof(T) == typeof(short))
            {
                obj2 = ObjToShort(obj);
            }
            else if (typeof(T) == typeof(DateTime))
            {
                if (obj is long)
                {
                    obj2 = ToDateTime(ObjToLong(obj));
                }
                else
                {
                    obj2 = ObjToDate(obj, null);
                }
            }
            else if (typeof(T) == typeof(bool))
            {
                obj2 = ObjToBool(obj);
            }
            return (T)obj2;
        }

        public static string Base64Decode(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }
            return Encoding.UTF8.GetString(Convert.FromBase64String(s));
        }

        public static string Base64Encode(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
        }

        public static DateTime UnixTimeToDateTime(long unixtime)
        {
            return _unixTime.AddMilliseconds(unixtime);
        }

        public static DateTime ToDateTime(long unixtime)
        {
            return _unixTime.AddMilliseconds(unixtime);
        }

        public static double ToDoubleUnixTime(DateTime datetime)
        {
            return datetime.AddHours(-8.0).Subtract(Convert.ToDateTime("1970-1-1")).TotalMilliseconds;
        }

        public static DateTime UnixTimeToDateTimeMilliseconds(long timestamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddMilliseconds(timestamp);
        }

        public static long ToUnixTime(DateTime datetime)
        {
            return (long)(datetime - _unixTime).TotalMilliseconds;
        }

        public static long ToUnixTimeMilliseconds(DateTime datetime)
        {
            var d = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToInt64((datetime - d).TotalMilliseconds);
        }

        public static long ToUnixTimeMilliseconds(DateTime? datetime)
        {
            datetime = datetime ?? new DateTime(1970, 1, 1);
            return ToUnixTimeMilliseconds(datetime.Value);
        }

        public static string ToJoin(string[] array, string separator)
        {
            return string.Join(separator, array);
        }

        public static string ToJoinSQL(IEnumerable<string> array)
        {
            var text = string.Empty;
            var enumerable = array as string[] ?? array.ToArray();
            if (array == null || !enumerable.Any())
            {
                return text;
            }
            var stringBuilder = new StringBuilder();
            foreach (var current in enumerable)
            {
                stringBuilder.AppendFormat("'{0}',", current);
            }
            text = stringBuilder.ToString();
            return text.Substring(0, text.Length - 1);
        }

        public static string ToJoin(IEnumerable<string> array, string separator)
        {
            return string.Join(separator, array);
        }

        public static string TrimAll(string s)
        {
            return
                s.Trim()
                    .Replace("\u3000", "")
                    .Replace("\u00a0", "")
                    .Replace(" ", "")
                    .Replace("\t", "")
                    .Replace("\r", "")
                    .Replace("\n", "");
        }

        public static string ToEllipsis(string s, int length)
        {
            if (s == null || s.Length <= length)
            {
                return s;
            }
            return s.Substring(0, length) + "...";
        }

        public static string ToEllipsis(string s, int length, string spl)
        {
            if (s == null || s.Length <= length)
            {
                return s;
            }
            return s.Substring(0, length) + spl;
        }

        /// <summary>
        ///     获取一个简短格式的GUID
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static string ToGridShort(Guid g)
        {
            return g.ToString().Replace("-", "");
        }

        public static bool ObjToBool(object obj)
        {
            return obj != null && !obj.Equals(DBNull.Value) &&
                   (obj.ToString().Trim() == "1" || obj.ToString().Trim().ToLower() == "true");
        }

        public static bool? ObjToBoolNull(object obj)
        {
            if (obj == null) return null;
            return ObjToBool(obj);
        }

        public static DateTime? ObjToDateNull(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            DateTime result;
            DateTime.TryParse(obj.ToString(), out result);
            return result;
        }

        /// <summary>
        ///     转化为时间类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultTime"></param>
        /// <returns></returns>
        public static DateTime ObjToDate(object obj, DateTime? defaultTime = null)
        {
            if (!defaultTime.HasValue)
            {
                defaultTime = DateTime.MinValue;
            }
            if (obj == null)
            {
                return (DateTime)defaultTime;
            }
            DateTime result = (DateTime)defaultTime;
            DateTime.TryParse(obj.ToString(), out result);
            return result;
        }

        /// <summary>
        ///     获取当前时间是周几
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="pre"></param>
        /// <returns></returns>
        public static string DateToWeak_ZH(DateTime dt, string pre = "周")
        {
            return pre + "日一二三四五六".Substring(dt.DayOfWeek.GetHashCode(), 1);
        }

        /// <summary>
        ///     转换对象为int
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ifNullValue"></param>
        /// <returns></returns>
        public static int ObjToInt(object obj, int ifNullValue = 0)
        {
            int result = ifNullValue;
            if (obj == null)
            {
                return result;
            }
            int.TryParse(obj.ToString(), out result);
            return result;
        }

        public static short ObjToShort(object obj, short ifNullValue = 0)
        {
            short result = ifNullValue;
            if (obj == null) { return result; }
            short.TryParse(obj.ToString(), out result);
            return result;
        }

        public static short? ObjToShortNull(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            short result;
            if (short.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static long ObjToLong(object obj, long ifNullValue = 0L)
        {
            long result = ifNullValue;
            if (obj == null) { return result; }
            long.TryParse(obj.ToString(), out result);
            return result;
        }

        public static int? ObjToIntNull(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            int result;
            if (int.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static string BlobToStr(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            if (obj.Equals(DBNull.Value))
            {
                return "";
            }
            string result;
            var bytes1 = obj as byte[];
            if (bytes1 != null)
            {
                var bytes = bytes1;
                result = Encoding.Default.GetString(bytes);
            }
            else
            {
                result = obj.ToString();
            }
            return result;
        }

        private static string Byte2String(byte[] arrByte)
        {
            var stringBuilder = new StringBuilder();
            foreach (var b in arrByte)
            {
                stringBuilder.Append(b > 15 ? Convert.ToString(b, 16) : '0' + Convert.ToString(b, 16));
            }
            return stringBuilder.ToString();
        }

        public static string ObjToStr(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            if (obj.Equals(DBNull.Value))
            {
                return "";
            }
            /*if (obj is HttpCookie)
            {
                return (obj as HttpCookie).Value;
            }*/
            return Convert.ToString(obj);
        }

        public static decimal ObjToDecimal(object obj, decimal ifNullValue = 0m)
        {
            decimal result = ifNullValue;
            if (obj == null)
            {
                return result;
            }
            if (obj.Equals(DBNull.Value))
            {
                return result;
            }
            decimal.TryParse(obj.ToString(), out result);
            return result;
        }

        public static decimal? ObjToDecimalNull(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj.Equals(DBNull.Value))
            {
                return null;
            }
            decimal result;
            if (decimal.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static double ObjToDouble(object obj, double ifNullValue = 0d)
        {
            double result = ifNullValue;
            if (obj == null) { return result; }
            double.TryParse(obj.ToString(), out result);
            return result;

        }

        public static double? ObjToDoubleWithNull(object obj)
        {

            if (obj == null)
            {
                return null;
            }
            if (obj.Equals(DBNull.Value))
            {
                return null;
            }
            double result;
            if (double.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static float ObjToFloat(object obj, float ifNullValue = 0f)
        {
            float result = ifNullValue;
            if (obj == null) { return result; }
            float.TryParse(obj.ToString(), out result);
            return result;
        }


        public static Dictionary<T, F> SetValue<T, F>(Dictionary<T, F> dic, T key, F value)
        {
            if (dic == null)
            {
                throw new Exception("字典不能为null！");
            }
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }
            return dic;
        }

        public static object GetValue(bool b, object trueValue, object falseValue)
        {
            var result = trueValue;
            if (!b)
            {
                result = falseValue;
            }
            return result;
        }

        /*        private static string CapText(Match m)
                {
                    string text = m.ToString().Replace(":\"", "").Replace("\"", "");
                    string safeHtmlFragment = Sanitizer.GetSafeHtmlFragment(text);
                    string arg = safeHtmlFragment.Replace("\"", "\\\"");
                    return string.Format(":\"{0}\"", arg);
                }

                public static string FilterXssInJson(string str)
                {
                    if (string.IsNullOrEmpty(str))
                    {
                        return "";
                    }
                    Regex regex = new Regex(":\"(.*?)\"");
                    return regex.Replace(str, new MatchEvaluator(ConvertExtension.CapText));
                }

                public static string FilterXss(string str)
                {
                    return Sanitizer.GetSafeHtmlFragment(str);
                }*/

        public static string UnEscape(string str)
        {
            if (str == null)
            {
                return string.Empty;
            }
            var stringBuilder = new StringBuilder();
            var length = str.Length;
            var num = 0;
            while (num != length)
            {
                if (Uri.IsHexEncoding(str, num))
                {
                    stringBuilder.Append(Uri.HexUnescape(str, ref num));
                }
                else
                {
                    stringBuilder.Append(str[num++]);
                }
            }
            return stringBuilder.ToString();
        }

        public static string GetName(Enum et)
        {
            return Enum.GetName(et.GetType(), et);
        }

        public static string ArrayToString(Array array, string separator = "")
        {
            if (array == null)
            {
                return "";
            }
            var length = array.Length;
            if (length <= 0)
            {
                return "";
            }
            if (string.IsNullOrWhiteSpace(separator))
            {
                separator = ",";
            }
            var stringBuilder = new StringBuilder();
            for (var i = length; i > 0; i--)
            {
                var text = "";
                if (array.GetValue(i - 1) != null)
                {
                    text = array.GetValue(i - 1).ToString();
                }
                if (i == 1)
                {
                    stringBuilder.Append(text);
                }
                else
                {
                    stringBuilder.Append(text + separator);
                }
            }
            return stringBuilder.ToString();
        }

        public static string ToBase64String(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }
            var list = new List<byte>
            {
                7,
                66,
                10,
                4,
                203,
                206,
                204,
                229,
                10,
                0,
                0,
                0,
                0,
                18
            };
            var bytes = Encoding.UTF8.GetBytes(content);
            if (bytes.Length < 256)
            {
                list.Add(66);
                list.Add((byte)bytes.Length);
            }
            else
            {
                list.Add(65);
                list.Add((byte)(bytes.Length / 256));
                list.Add((byte)(bytes.Length % 256));
            }
            var array = bytes;
            for (var i = 0; i < array.Length; i++)
            {
                var item = array[i];
                list.Add(item);
            }
            return HttpUtility.UrlEncode(Convert.ToBase64String(list.ToArray()));
        }

        public static string ConvertToStr(Guid id)
        {
            return id.ToString().Replace("-", "");
        }

        public static int GetCount<T>(IEnumerable<T> list)
        {
            var num = 0;
            using (var enumerator = list.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    num++;
                }
            }
            return num;
        }

        public static int ToInt(Enum value)
        {
            return Convert.ToInt32(value);
        }

        /*        public static void Trim(System.Text.StringBuilder stringBuilder, char c)
                {
                    if (stringBuilder(c))
                    {
                        stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    }
                }

                public static bool EndsWith(System.Text.StringBuilder stringBuilder, char c)
                {
                    return stringBuilder.Length != 0 && stringBuilder[stringBuilder.Length - 1].Equals(c);
                }*/

        /// <summary>
        /// 将一个对象转换为指定类型
        /// </summary>
        /// <param name="obj">待转换的对象</param>
        /// <returns>转换后的对象</returns>
        public static T ConvertObject<T>(object obj)
        {
            var result = default(T);
            var type = typeof(T);
            Type underlyingType = Nullable.GetUnderlyingType(type);

            if (obj == null || type.IsValueType || (underlyingType ?? type).IsEnum) return result;

            if (type.IsInstanceOfType(obj)) // 如果待转换对象的类型与目标类型兼容，则无需转换
            {
                return (T)obj;
            }

            /* else if (typeof(IConvertible).IsAssignableFrom(underlyingType ?? type)) // 如果目标类型的基类型实现了IConvertible，则直接转换
             {
                 try
                 {
                     return Convert.ChangeType(obj, underlyingType ?? type, null);
                 }
                 catch
                 {
                     return underlyingType == null ? Activator.CreateInstance(type) : null;
                 }
             }*/
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                if (converter.CanConvertFrom(obj.GetType()))
                {
                    return (T)converter.ConvertFrom(obj);
                }
                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    object o = constructor.Invoke(null);
                    PropertyInfo[] propertys = type.GetProperties();
                    Type oldType = obj.GetType();
                    foreach (PropertyInfo property in propertys)
                    {
                        PropertyInfo p = oldType.GetProperty(property.Name);
                        if (property.CanWrite && p != null && p.CanRead)
                        {
                            property.SetValue(o, p.GetValue(obj, null));
                        }
                    }
                    result = (T)o;
                }
            }
            return result;
        }


    }
}
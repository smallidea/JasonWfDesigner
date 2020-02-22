using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JasonWfDesigner.Common.Lib
{
    /// <summary>
    /// json序列化、反序列化的帮助类
    /// </summary>
    public class JsonHelper
    {
        public static T ConvertToObj<T>(string json, List<JsonConverter> converts = null)
        {
            if (string.IsNullOrWhiteSpace(json) || json == "{}" || json == "[]")
            {
                return default(T);
            }

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            settings.Converters.Add(new BoolConverter());
            if(converts != null && converts.Any())
                foreach (var jsonConverter in converts)
                    settings.Converters.Add(jsonConverter);

            json = json.TrimStart('\"');
            json = json.TrimEnd('\"');
            json = json.Replace("\\", ""); //????
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public static string ConvertToStr<T>(T obj)
        {
            var timeConverter = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            };
            return JsonConvert.SerializeObject(obj, Formatting.None, timeConverter); //Formatting.None 防止带有转义字符的json字符串
        }

        public static string ConvertToStr(object obj)
        {
            //return JsonConvert.SerializeObject(obj);

            var timeConverter = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            };
            return JsonConvert.SerializeObject(obj, Formatting.None, timeConverter); //Formatting.None 防止带有转义字符的json字符串
        }

        /// <summary>
        /// 通过json文件路径获取json对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonPath"></param>
        /// <returns></returns>
        public static T GetJsonObjectFromFile<T>(string jsonPath, List<JsonConverter> converts = null)
        {
            try
            {
                string jsonString = File.ReadAllText(jsonPath);
                return ConvertToObj<T>(jsonString, converts);
               // return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 布尔类型
        /// </summary>
        internal class BoolConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(((bool)value) ? 1 : 0);
            }

            public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.Value == null)
                {
                    return false;
                }
                return reader.Value.ToString() == "1" || reader.Value.ToString().ToLower() == "true";
            }

            public override bool CanConvert(System.Type objectType)
            {
                return objectType == typeof(bool) || objectType == typeof(bool?);
            }
        }

        /*public class DateTimeConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
            }

            public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.Value == null)
                {
                    return 0L.UnixTimeToDateTimeMilliseconds();
                }
                if (reader.get_Value() is long || reader.get_Value() is int)
                {
                    return ((long)reader.get_Value()).UnixTimeToDateTimeMilliseconds();
                }
                if (reader.get_Value() is string)
                {
                    long timestamp = -1L;
                    long.TryParse(reader.get_Value().ObjToStr(), out timestamp);
                    return timestamp.UnixTimeToDateTimeMilliseconds();
                }
                return reader.get_Value();
            }

            public override bool CanConvert(System.Type objectType)
            {
                return objectType == typeof(System.DateTime) || objectType == typeof(System.DateTime?);
            }
        }*/

        /*public class DecimalConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
            }

            public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (objectType == typeof(int))
                {
                    return reader.get_Value().ObjToInt(0);
                }
                if (objectType == typeof(int?))
                {
                    return reader.get_Value().ObjToIntNull();
                }
                if (objectType == typeof(long?))
                {
                    return reader.get_Value().ObjToLong();
                }
                if (objectType == typeof(short))
                {
                    return reader.get_Value().ObjToShort();
                }
                if (objectType == typeof(short?))
                {
                    return reader.get_Value().ObjToShortNull();
                }
                if (objectType == typeof(double))
                {
                    return reader.get_Value().ObjToDouble();
                }
                if (objectType == typeof(decimal))
                {
                    return reader.get_Value().ObjToDecimal();
                }
                return reader.get_Value();
            }

            public override bool CanConvert(System.Type objectType)
            {
                return objectType == typeof(decimal) || objectType == typeof(double) || objectType == typeof(float) || objectType == typeof(int) || objectType == typeof(int?);
            }
        }*/
    }
}

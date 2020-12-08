using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Utilities.Converters
{
    /// <summary>
    /// DictionaryStringObjectConverter
    /// </summary>
    public class DictionaryStringObjectConverter : JsonConverter
    {
        private static readonly Type _dictionaryType = typeof(Dictionary<string, object>);
        private static readonly Type _listGenericType = typeof(List<>);

        /// <summary>
        /// CanConvert
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType) => objectType == _dictionaryType;

        /// <summary>
        /// ReadJson
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (objectType != _dictionaryType)
            {
                return serializer.Deserialize(reader);
            }
            else
            {
                var jsonObject = serializer.Deserialize<JObject>(reader);
                if (jsonObject == null) return null;

                var result = new Dictionary<string, object>(jsonObject.Count);
                foreach (JProperty property in jsonObject.Children())
                {
                    if (property == null || property.Name == null || property.Value == null)
                    {
                        continue;
                    }

                    if (property.Value is JArray valueArray)
                    {
                        if (valueArray.Count == 0) continue;

                        var type = GetObjectType(valueArray.First.Type);
                        if (type == null) continue;

                        result[property.Name] = valueArray.ToObject(_listGenericType.MakeGenericType(type));
                    }
                    else
                    {
                        var type = GetObjectType(property.Value.Type);

                        if (type == null || string.IsNullOrWhiteSpace(property.Name)) continue;

                        // if (type != typeof(object))
                        // {
                        result[property.Name] = property.Value.ToObject(type);
                        // }
                        // else
                        // {
                        //     JProperty child = property.Value.Children().FirstOrDefault() as JProperty;
                        //     if (child.Name == "t" && child.Value.ToString() == "GeoPoint")
                        //     {
                        //         JProperty objs = property.Value.Children().LastOrDefault() as JProperty;
                        //         result[property.Name] = objs.Value.ToObject<GeoPoint>();
                        //     }
                        //     else if (child.Name == "t" && child.Value.ToString() == "List<GeoPoint>")
                        //     {
                        //         JProperty objs = property.Value.Children().LastOrDefault() as JProperty;
                        //         result[property.Name] = objs.Value.ToObject<List<GeoPoint>>();
                        //     }
                        // }
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// WriteJson
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            if (value is Dictionary<string, object> dictionaryObject
                && dictionaryObject.GetType() == typeof(Dictionary<string, object>))
            {
                writer.WriteStartObject();
                foreach (var item in dictionaryObject)
                {
                    // if (item.Value is GeoPoint)
                    // {
                    //     writer.WritePropertyName(item.Key);
                    //     writer.WriteStartObject();
                    //     writer.WritePropertyName("t");
                    //     writer.WriteValue("GeoPoint");
                    //     writer.WritePropertyName("v");
                    //     serializer.Serialize(writer, item.Value);
                    //     writer.WriteEndObject();
                    // }
                    // else if (item.Value is List<GeoPoint>)
                    // {
                    //     writer.WritePropertyName(item.Key);
                    //     writer.WriteStartObject();
                    //     writer.WritePropertyName("t");
                    //     writer.WriteValue("List<GeoPoint>");
                    //     writer.WritePropertyName("v");
                    //     serializer.Serialize(writer, item.Value);
                    //     writer.WriteEndObject();
                    // }
                    // else
                    // {
                    writer.WritePropertyName(item.Key);
                    serializer.Serialize(writer, item.Value);
                    // }
                }
                writer.WriteEndObject();
            }
        }

        private Type GetObjectType(JTokenType type)
        {
            switch (type)
            {
                case JTokenType.String:
                    return typeof(string);

                case JTokenType.Boolean:
                    return typeof(bool);

                case JTokenType.Float:
                    return typeof(float);

                case JTokenType.Integer:
                    return typeof(int);

                case JTokenType.Date:
                    return typeof(DateTimeOffset);
                case JTokenType.Object:
                    return typeof(object);
                default:
                    return null;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Utilities.Converters
{
    public class DictionaryIntObjectConverter : JsonConverter
    {
        private static readonly Type _dictType = typeof(Dictionary<int, object>);
        private static readonly Type _listGenericType = typeof(List<>);

        public override bool CanConvert(Type objectType) => objectType == _dictType;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == _dictType)
            {
                return ReadDict(reader, serializer);
            }
            else
            {
                return serializer.Deserialize(reader);
            }
        }

        private object ReadDict(JsonReader reader, JsonSerializer serializer)
        {
            var obj = serializer.Deserialize<JObject>(reader);
            if (obj == null)
            {
                return null;
            }

            var result = new Dictionary<int, object>(obj.Count);
            foreach (JProperty property in obj.Children())
            {
                if (property == null || property.Name == null || property.Value == null)
                {
                    continue;
                }

                if (property.Value is JArray arr)
                {
                    if (arr.Count == 0)
                    {
                        continue;
                    }

                    var type = GetObjectType(arr.First.Type);
                    if (type != null && int.TryParse(property.Name, out int res))
                    {
                        result[res] = arr.ToObject(_listGenericType.MakeGenericType(type));
                    }
                }
                else
                {
                    var type = GetObjectType(property.Value.Type);
                    int.TryParse(property.Name, out int key);

                    if (type != null && key > 0)
                    {
                        // if (type == typeof(Object))
                        // {
                        //     JProperty child = property.Value.Children().FirstOrDefault() as JProperty;
                        //     if (child.Name == "t" && child.Value.ToString() == "GeoPoint")
                        //     {
                        //         JProperty objs = property.Value.Children().LastOrDefault() as JProperty;
                        //         result[key] = objs.Value.ToObject<GeoPoint>();
                        //     }
                        //     else if (child.Name == "t" && child.Value.ToString() == "List<GeoPoint>")
                        //     {
                        //         JProperty objs = property.Value.Children().LastOrDefault() as JProperty;
                        //         result[key] = objs.Value.ToObject<List<GeoPoint>>();
                        //     }
                        // }
                        // else
                        // {
                        result[key] = property.Value.ToObject(type);
                        // }
                    }
                }
            }

            return result;
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
                    return typeof(Object);
                default:
                    return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is Dictionary<int, object> dict))
            {
                return;
            }

            writer.WriteStartObject();
            foreach (var kvp in dict)
            {
                // if (kvp.Value is GeoPoint)
                // {
                //     writer.WritePropertyName(kvp.Key.ToString());
                //     writer.WriteStartObject();
                //     writer.WritePropertyName("t");
                //     writer.WriteValue("GeoPoint");
                //     writer.WritePropertyName("v");
                //     serializer.Serialize(writer, kvp.Value);
                //     writer.WriteEndObject();
                // }
                // else if (kvp.Value is List<GeoPoint>)
                // {
                //     writer.WritePropertyName(kvp.Key.ToString());
                //     writer.WriteStartObject();
                //     writer.WritePropertyName("t");
                //     writer.WriteValue("List<GeoPoint>");
                //     writer.WritePropertyName("v");
                //     serializer.Serialize(writer, kvp.Value);
                //     writer.WriteEndObject();
                // }
                // else
                // {
                writer.WritePropertyName(kvp.Key.ToString());
                serializer.Serialize(writer, kvp.Value);
                // }
            }
            writer.WriteEndObject();
        }
    }
}

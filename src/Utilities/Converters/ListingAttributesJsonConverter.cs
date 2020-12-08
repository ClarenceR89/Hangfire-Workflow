using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Utilities.Converters
{
    /// <summary>
    /// <see cref="ListingAttributesJsonConverter"/>
    /// </summary>
    public class ListingAttributesJsonConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;

        /// <summary>
        /// <see cref="CanConvert"/>
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType) => objectType == typeof(Dictionary<string, object>);

        /// <summary>
        /// <see cref="WriteJson"/>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanWrite is false. The type will skip the converter.");
        }

        /// <summary>
        /// <see cref="ReadJson"/>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new JsonSerializationException($"Invalid token type! expected StartObject, got {reader.TokenType:G}");
            }

            var obj = new Dictionary<string, object>();
            var done = false;
            string key = null;
            while (!done && reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.EndObject:
                        done = true;
                        break;

                    case JsonToken.PropertyName:
                        if (key != null)
                        {
                            throw new JsonSerializationException("Invalid token type! got a new PropertyName while already working on a PropertyName!");
                        }

                        key = reader.Value as string;
                        if (key == null)
                        {
                            throw new JsonSerializationException("Invalid PropertyName! The PropertyName has to be a non-null string!");
                        }

                        break;

                    case JsonToken.StartArray:
                        var jArray = serializer.Deserialize<JArray>(reader);

                        if (jArray == null || jArray.Count == 0) break;

                        object array = GetArrayObject(jArray);
                        obj[key] = array;
                        key = null;
                        break;

                    case JsonToken.Boolean:
                    case JsonToken.Float:
                    case JsonToken.Integer:
                    case JsonToken.String:
                        obj[key] = reader.Value;
                        key = null;
                        break;

                    case JsonToken.Date:
                        var date = reader.Value;
                        if (date is DateTime time)
                        {
                            date = new DateTimeOffset(time);
                        }

                        obj[key] = date;
                        key = null;
                        break;

                    case JsonToken.Null:
                    case JsonToken.Undefined:
                        //don't add null values
                        key = null;
                        break;

                    case JsonToken.Comment:
                        //skip comments
                        break;

                    default:
                        throw new JsonSerializationException("Invalid JsonTokenType type inside dictionary!");
                }
            }
            return obj;
        }

        private static object GetArrayObject(JArray jArray)
        {
            object array;
            switch (jArray.First?.Type)
            {
                case JTokenType.Boolean:
                    array = jArray.ToObject<List<bool>>();
                    break;

                case JTokenType.Date:
                    array = jArray.ToObject<List<DateTimeOffset>>();
                    break;

                case JTokenType.Float:
                    array = jArray.ToObject<List<float>>();
                    break;

                case JTokenType.Integer:
                    array = jArray.ToObject<List<int>>();
                    break;

                case JTokenType.String:
                    array = jArray.ToObject<List<string>>();
                    break;

                default:
                    throw new JsonSerializationException("Invalid JTokenType type inside array!");
            }

            return array;
        }
    }
}

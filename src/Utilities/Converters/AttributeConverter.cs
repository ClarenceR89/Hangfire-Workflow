using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Utilities.Converters
{
    /// <summary>
    /// AttributesConverter
    /// </summary>
    public class AttributesConverter : JsonConverter
    {
        private static readonly Type _dictionaryType = typeof(Dictionary<string, object>);

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
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader);
        }

        /// <summary>
        /// WriteJson
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Dictionary<string, object> dict)
            {
                writer.WriteStartObject();

                // Go through Dictionary and write out key-value items
                foreach (var item in dict)
                {
                    writer.WritePropertyName(item.Key);
                    serializer.Serialize(writer, item.Value);
                }

                writer.WriteEndObject();
            }
        }
    }
}

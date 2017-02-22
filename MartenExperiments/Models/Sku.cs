using System;
using System.Text.RegularExpressions;
using Autofac;
using Newtonsoft.Json;
using StringlyTyped;

namespace MartenExperiments.Models
{
    [JsonConverter(typeof(SkuConverter))]
    public class Sku : Stringly
    {
        protected override Regex Regex => new Regex(@".*", RegexOptions.Compiled);

        public static Sku Parse(string sku)=> new Stringly<Sku>(sku);
    }

    public class SkuConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                var sku = value as Sku;
                if (sku != null)
                {
                    writer.WriteValue(sku.ToString());
                }
                else
                {
                    throw new JsonSerializationException("Expected Sku object value");
                }                
            }            
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonToken.String)
            {
                try
                {
                    var strValue = (string)reader.Value;
                    return Sku.Parse(strValue);
                }
                catch (Exception ex)
                {
                    throw new JsonSerializationException($"Error parsing sku: {reader.Value}", ex);
                }
            }
            else
            {
                throw new JsonSerializationException($"Unexpected token or value when parsing Sku. Token: {reader.TokenType}, Value: {reader.Value}");
            }
        }

        public override bool CanConvert(Type objectType) =>
            objectType.IsAssignableTo<Sku>();
    }
}
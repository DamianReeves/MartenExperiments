using System;
using System.Text.RegularExpressions;
using Autofac;
using Newtonsoft.Json;
using StringlyTyped;
using TwentyTwenty.DomainDriven;

namespace MartenExperiments.Models
{
    [JsonConverter(typeof(SkuConverter))]
    public struct Sku 
    {
        private static readonly Regex _regex = new Regex(@".*", RegexOptions.Compiled);
        public string Value { get; }

        public Sku(string value)
        {
            if (!_regex.IsMatch(value))
            {
                throw new ArgumentException($"The provided value ({value}) is in an incorrect format.", nameof(value));
            }
            Value = value;
        }
        
        public static Sku Parse(string sku)=> new Sku(sku);

        public override string ToString() => Value;
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
                if (value is Sku)
                {
                    writer.WriteValue(((Sku)value).Value);
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

    public class DomainEvent : IDomainEvent
    {
        public DomainEvent()
        {            
        }
        public Guid Id { get; set; }
    }
}
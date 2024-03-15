using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SignNow.Net._Internal.Helpers.Converters
{
    /// <summary>
    /// Converter used to make the JSON property name the value of another C# property
    /// </summary>
    /// <remarks>Adapted from https://stackoverflow.com/a/51459043/10720469</remarks>
    internal class DynamicNameConverter : JsonConverter
    {
        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetRuntimeProperties().Any(prop => prop.CustomAttributes.Any(attr => attr.AttributeType == typeof(JsonDynamicNameAttribute)));
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var token = JToken.FromObject(value);
            if (token.Type != JTokenType.Object)
            {
                // We should never reach this point because CanConvert() only allows objects with JsonPropertyDynamicNameAttribute to pass through.
                throw new Exception("Token to be serialized was unexpectedly not an object.");
            }

            var o = (JObject)token;
            var propertiesWithDynamicNameAttribute = value.GetType().GetRuntimeProperties().Where(
                prop => prop.CustomAttributes.Any(attr => attr.AttributeType == typeof(JsonDynamicNameAttribute))
            );

            foreach (var property in propertiesWithDynamicNameAttribute)
            {
                var dynamicAttributeData = property.CustomAttributes.FirstOrDefault(attr => attr.AttributeType == typeof(JsonDynamicNameAttribute));

                // Determine what we should rename the property from and to.
                var currentName = property.Name;
                var propertyNameContainingNewName = (string)dynamicAttributeData.ConstructorArguments[0].Value;
                var newName = (string)value.GetType().GetRuntimeProperty(propertyNameContainingNewName).GetValue(value);

                // Perform the renaming in the JSON object.
                var currentJsonPropertyValue = o[currentName];
                var newJsonProperty = new JProperty(newName, currentJsonPropertyValue);
                currentJsonPropertyValue.Parent.Replace(newJsonProperty);
            }

            token.WriteTo(writer);
        }
    }
}

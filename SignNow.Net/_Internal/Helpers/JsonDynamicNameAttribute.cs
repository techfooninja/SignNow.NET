using System;

namespace SignNow.Net._Internal.Helpers
{
    /// <summary>
    /// Attribute for using another property's value to be the field name of this property when serializing.
    /// </summary>
    /// <remarks>Adapted from https://stackoverflow.com/a/51459043/10720469</remarks>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal class JsonDynamicNameAttribute : Attribute
    {
        public JsonDynamicNameAttribute(string objectPropertyName)
        {
            ObjectPropertyName = objectPropertyName;
        }

        public string ObjectPropertyName { get; }
    }
}

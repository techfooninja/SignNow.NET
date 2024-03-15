using Newtonsoft.Json;
using SignNow.Net._Internal.Helpers;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents a value assigned to a smart field.
    /// </summary>
    public class SignNowSmartFieldValue
    {
        /// <summary>
        /// Creates a new instance of <see cref="SignNowSmartFieldValue"/>
        /// </summary>
        public SignNowSmartFieldValue()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="SignNowSmartFieldValue"/>
        /// </summary>
        /// <param name="fieldName">Name of the smart field to set a value to.</param>
        /// <param name="value">Value to set to the smart field.</param>
        public SignNowSmartFieldValue(string fieldName, string value)
        {
            FieldName = fieldName;
            Value = value;
        }

        /// <summary>
        /// Name of the field to assign a value to.
        /// </summary>
        [JsonIgnore]
        public string FieldName { get; set; }

        /// <summary>
        /// Value to assign to the smart field.
        /// </summary>
        [JsonDynamicName(nameof(FieldName))]
        public string Value { get; set; }
    }
}

using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net._Internal.Helpers.Converters;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net._Internal.Requests
{
    /// <summary>
    /// Represents a request for pre-filling smart fields.
    /// </summary>
    internal class PrefillSmartFieldsRequest : JsonHttpContent
    {
        /// <summary>
        /// Collection of smart field values.
        /// </summary>
        [JsonProperty("data", ItemConverterType = typeof(DynamicNameConverter))]
        public IEnumerable<SignNowSmartFieldValue> Data { get; set; }
    }
}

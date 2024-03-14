using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Represents response for getting smart fields.
    /// </summary>
    public class GetSmartFieldsResponse
    {
        /// <summary>
        /// Collection of smart fields.
        /// </summary>
        [JsonProperty("integration_objects")]
        public List<SignNowSmartField> SmartFields { get; set; }
    }
}

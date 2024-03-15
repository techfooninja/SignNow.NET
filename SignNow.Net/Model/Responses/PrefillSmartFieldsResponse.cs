using System;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Represents response for pre-filling smart fields
    /// </summary>
    public class PrefillSmartFieldsResponse
    {
        /// <summary>
        /// Status of the request.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        public bool IsSuccessful
        {
            get
            {
                return string.Equals("success", this.Status, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}

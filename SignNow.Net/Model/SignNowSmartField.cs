using System;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents a smart field.
    /// </summary>
    public class SignNowSmartField
    {
        /// <summary>
        /// Unique identifier of the smart field.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Identifier for the user who added the field.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// The page number in the document where the smart field is located.
        /// </summary>
        [JsonProperty("page_number")]
        public int PageNumber { get; set; }

        /// <summary>
        /// Email for the user who added the smart field.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Font type of the text in the smart field.
        /// </summary>
        [JsonProperty("font")]
        public string Font { get; set; }

        /// <summary>
        /// Font size of the text in the smart field.
        /// </summary>
        [JsonProperty("size")]
        public string Size { get; set; }

        /// <summary>
        /// Name of the smart field.
        /// </summary>
        [JsonProperty("data")]
        public string Name { get; set; }

        /// <summary>
        /// Identifies whether it's a smart field or a Salesforce field
        /// </summary>
        [JsonProperty("api_integration_id")]
        public string ApiItegrationId { get; set; }

        /// <summary>
        /// X coordinate of the field.
        /// </summary>
        [JsonProperty("x")]
        public string X { get; set; }

        /// <summary>
        /// Y coordinate of the field.
        /// </summary>
        [JsonProperty("y")]
        public string Y { get; set; }

        /// <summary>
        /// Line height of the field.
        /// </summary>
        [JsonProperty("line_height")]
        public string LineHeight { get; set; }

        /// <summary>
        /// Timestamp smart field was created.
        /// </summary>
        [JsonProperty("created")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Created { get; set; }

        /// <summary>
        /// Identifies the document the smart field belongs to.
        /// </summary>
        [JsonProperty("document_id")]
        public string DocumentId { get; set; }

        /// <summary>
        /// Field attributes (e.g. font, style, size etc.)
        /// </summary>
        [JsonProperty("json_attributes")]
        public dynamic JsonAttributes { get; set; }
    }
}

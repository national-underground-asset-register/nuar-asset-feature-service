namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles
{
    public class StyleRuleCondition
    {
        /// <summary>
        /// Gets or sets the unique id of the condition
        /// </summary>
        [JsonIgnore]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the style rule id that this condition belongs to.
        /// </summary>
        [JsonIgnore]
        public Guid StyleRuleId { get; set; }

        /// <summary>
        /// Gets or sets the parent condition id.
        /// </summary>
        [JsonIgnore]
        public Guid ParentConditionId { get; set; }

        /// <summary>
        /// Gets or sets the field to compare against.
        /// </summary>
        [JsonProperty("field", NullValueHandling = NullValueHandling.Ignore)]
        public string? Field { get; set; }

        /// <summary>
        /// Gets or sets the operator to use for comparison.
        /// </summary>
        [JsonRequired]
        [JsonProperty("operator", NullValueHandling = NullValueHandling.Ignore)]
        public string? Operator { get; set; }

        /// <summary>
        /// Gets or sets the value to compare against the field.
        /// </summary>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the list of child conditions for this condition.
        /// </summary>
        [JsonProperty("conditions", NullValueHandling = NullValueHandling.Ignore)]
        public List<StyleRuleCondition>? Conditions { get; set; }
    }
}

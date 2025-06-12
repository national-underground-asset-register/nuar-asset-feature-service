using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models.Styles;

namespace Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers.FluentMap.Styles
{
    internal class StyleRuleConditionMap : EntityMap<StyleRuleCondition>
    {
        public StyleRuleConditionMap()
        {
            Map(x => x.Id).ToColumn("id");
            Map(x => x.StyleRuleId).ToColumn("style_rule_id");
            Map(x => x.ParentConditionId).ToColumn("parent_condition_id");
            Map(x => x.Field).ToColumn("field");
            Map(x => x.Operator).ToColumn("operator");
            Map(x => x.Value).ToColumn("value");
        }
    }
}

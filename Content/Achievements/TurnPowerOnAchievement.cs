using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Achievements
{
    public class AATurnPowerOnAchievement : ModAchievement
    {
        public override string TextureName => "ChangedSpecialMod/Content/Achievements/TurnPowerOnAchievement";
        public CustomFlagCondition ConditionTurnPowerOn { get; private set; }

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Explorer);
            ConditionTurnPowerOn = AddCondition();
        }
    }
}

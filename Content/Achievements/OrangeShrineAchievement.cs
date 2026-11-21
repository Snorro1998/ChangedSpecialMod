using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Achievements
{
    public class OrangeShrineAchievement : ModAchievement
    {
        public override string TextureName => "ChangedSpecialMod/Content/Achievements/OrangeShrineAchievement";
        public CustomFlagCondition Condition { get; private set; }

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Explorer);
            Condition = AddCondition();
        }
    }
}

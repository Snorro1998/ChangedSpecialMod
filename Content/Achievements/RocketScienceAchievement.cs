using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Achievements
{
    public class RocketScienceAchievement : ModAchievement
    {
        // It is possible to share a texture and use Index to choose which icon is used from a texture.
        public override string TextureName => "ChangedSpecialMod/Content/Achievements/RocketScienceAchievement";
        public CustomFlagCondition Condition { get; private set; }

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Explorer);
            Condition = AddCondition();
        }
    }
}

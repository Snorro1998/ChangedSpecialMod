using ChangedSpecialMod.Content.NPCs;
using Terraria.Achievements;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Achievements
{
    // Stupid problems require stupid solutions. Adding the letters in front of the name fixes the ordering.
    public class ACWolfKingAchievement : ModAchievement
    {
        public override string TextureName => "ChangedSpecialMod/Content/Achievements/WolfKingAchievement";

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Slayer);
            AddNPCKilledCondition(ModContent.NPCType<WolfKing>());
        }
    }
}

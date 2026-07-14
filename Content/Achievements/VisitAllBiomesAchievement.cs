using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Achievements
{
    public class VisitAllBiomesAchievement : ModAchievement
    {
        // It is possible to share a texture and use Index to choose which icon is used from a texture.
        public override string TextureName => "ChangedSpecialMod/Content/Achievements/VisitAllBiomesAchievement";
        public CustomFlagCondition ConditionBlackSurface { get; private set; }
        public CustomFlagCondition ConditionWhiteSurface { get; private set; }
        //public CustomFlagCondition ConditionCityRuins { get; private set; }
        public CustomFlagCondition ConditionBlackCave { get; private set; }
        public CustomFlagCondition ConditionWhiteCave { get; private set; }

        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Explorer);
            ConditionBlackSurface = AddCondition("ConditionBlackSurface");
            ConditionWhiteSurface = AddCondition("ConditionWhiteSurface");
            //ConditionCityRuins = AddCondition("ConditionCityRuins");
            ConditionBlackCave = AddCondition("ConditionBlackCave");
            ConditionWhiteCave = AddCondition("ConditionWhiteCave");
        }
    }
}

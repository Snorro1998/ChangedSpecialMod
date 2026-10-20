using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportSpirit : ModSupportData
    {
        public ModSupportSpirit(Mod mod, string modName) : base(mod, modName) { }

        public override void AddTiles()
        {
            // Briar
            AddTileType("BriarGrass", true);
            AddTileType("LivingBriarWood", true);
        }

        public override void AddItemsFruit()
        {
            AddItemFruit("EnchantedApple");
            AddItemFruit("EnchantedStarFruit");
            AddItemFruit("Durian");
            AddItemFruit("Guava");
            AddItemFruit("IceBerry");
            AddItemFruit("Glowpear", ItemSellCondition.DownedAnyMech);
            AddItemFruit("LuminBerry", ItemSellCondition.DownedAnyMech);
            AddItemFruit("BirdhouseGourd", ItemSellCondition.DownedGolem);
            AddItemFruit("CaramelApple", ItemSellCondition.DownedGolem);
        }

        public override void AddItemsSeed()
        {
            AddItemSeed("BriarGrassSeeds");
            //Soulbloom Seeds
        }
    }
}

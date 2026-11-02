using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportSpiritReforged : ModSupportData
    {
        public ModSupportSpiritReforged(Mod mod, string modName) : base(mod, modName) { }

        public override void AddTiles()
        {
            AddTileType("SaltBlock", true);
            AddTileType("SaltBlockDull", true);

            // Savanna
            AddTileType("SavannaGrass", true);
            AddTileType("SavannaDirt", true);
            AddTileType("LivingBaobab", true);
            AddTileType("LivingBaobabLeaf", true);

            // Ziggurat
            AddTileType("BronzePlatform", true);
            AddTileType("RedSandstoneBrick", true);
            AddTileType("RedSandstoneBrickCracked", true);
            AddTileType("SandySandstone", true);
        }

        public override void AddItemsFruit()
        {
            AddItemFruit("CrescentMelon");
            AddItemFruit("MidnightApple");
            AddItemFruit("Pearlberry");
            AddItemFruit("BaobabFruit");
            AddItemFruit("Caryocar");
            AddItemFruit("CustardApple");
        }
    }
}

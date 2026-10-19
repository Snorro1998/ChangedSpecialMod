using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportSpirit : ModSupportData
    {
        public ModSupportSpirit(Mod mod, string modName) : base(mod, modName) { }

        public override void AddItemsFruit()
        {
            AddItemFruit("EnchantedApple");
            AddItemFruit("EnchantedStarFruit");
            AddItemFruit("Durian");
            AddItemFruit("Guava");
            AddItemFruit("IceBerry");
            AddItemFruit("Glowpear"); // anyMech
            AddItemFruit("LuminBerry"); // anyMech
            AddItemFruit("BirdhouseGourd"); // NPC.downedGolemBoss
            AddItemFruit("CaramelApple"); // NPC.downedGolemBoss
        }

        public override void AddItemsSeed()
        {
            AddItemSeed("BriarGrassSeeds");
            //Soulbloom Seeds
        }
    }
}

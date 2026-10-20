using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportSpiritReforged : ModSupportData
    {
        public ModSupportSpiritReforged(Mod mod, string modName) : base(mod, modName) { }

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

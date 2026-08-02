using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items
{
    public class WhiteGoo : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 0, 0, 10);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
        }
    }
}
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items
{
    public class Orange : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToFood(40, 40, BuffID.WellFed, 60 * 60 * 2);
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffID.WellFed, 3600);
        }
    }
}
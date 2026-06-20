using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Utilities;
using ChangedSpecialMod.Utilities.UI.TransfurUI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Syringes
{
    public class Syringe : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.maxStack = 9999;
        }
    }
}
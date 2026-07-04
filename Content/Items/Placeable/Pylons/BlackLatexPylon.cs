using ChangedSpecialMod.Content.Tiles.Pylons;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Pylons
{
	public class BlackLatexPylon : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<BlackLatexPylonTile>());
            Item.SetShopValues(ItemRarityColor.Blue1, Terraria.Item.buyPrice(gold: 10));
        }
    }
}

using ChangedSpecialMod.Content.Items.Placeable.Crystals;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
    public class FaultyLight : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.FaultyLightTile>());
            Item.value = Item.sellPrice(copper: 30);
        }
    }
}

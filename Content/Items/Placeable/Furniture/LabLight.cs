using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture
{
    public class LabLight : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.LabLightTile>());
            Item.value = Item.sellPrice(copper: 30);
        }
    }
}

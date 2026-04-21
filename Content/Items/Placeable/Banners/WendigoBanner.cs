using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Banners
{
    public class WendigoBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Banners.WendigoBanner>());
        }
    }
}

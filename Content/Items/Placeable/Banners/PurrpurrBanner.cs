using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Banners
{
    public class PurrpurrBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Banners.PurrpurrBanner>());
        }
    }
}

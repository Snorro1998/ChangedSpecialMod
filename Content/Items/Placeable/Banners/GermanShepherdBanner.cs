using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Banners
{
    public class GermanShepherdBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Banners.GermanShepherdBanner>());
        }
    }
}

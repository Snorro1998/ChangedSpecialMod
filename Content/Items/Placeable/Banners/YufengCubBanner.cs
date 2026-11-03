using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Banners
{
    [LegacyName("FlyingDarkLatexBanner")]
    public class YufengCubBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Banners.YufengCubBanner>());
            Item.rare = ItemRarityID.Blue;
        }
    }
}

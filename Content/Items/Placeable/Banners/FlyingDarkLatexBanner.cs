using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Banners
{
    public class FlyingDarkLatexBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Banners.FlyingDarkLatexBanner>());
            Item.rare = ItemRarityID.Blue;
        }
    }
}

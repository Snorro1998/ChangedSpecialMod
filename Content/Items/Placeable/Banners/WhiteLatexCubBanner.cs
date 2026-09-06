using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Banners
{
    public class WhiteLatexCubBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Banners.WhiteLatexCubBanner>());
            Item.rare = ItemRarityID.Blue;
        }
    }
}

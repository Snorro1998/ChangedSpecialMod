using ChangedSpecialMod.Content.Tiles.Latex.White;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Plants.Latex.White
{
    public class WhiteLatexTree : BaseWhiteLatexTree
    {
        public override void SetStaticDefaults()
        {
            GrowsOnTileId = [ModContent.TileType<WhiteLatexGrassTile>()];
            texture = ModContent.Request<Texture2D>("ChangedSpecialMod/Content/Tiles/Plants/Latex/White/WhiteLatexTree");
            branchesTexture = ModContent.Request<Texture2D>("ChangedSpecialMod/Content/Tiles/Plants/Latex/White/WhiteLatexTree_Branches");
            topsTexture = ModContent.Request<Texture2D>("ChangedSpecialMod/Content/Tiles/Plants/Latex/White/WhiteLatexTree_Tops");
        }

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return ModContent.TileType<WhiteLatexTreeSapling>();
        }
    }
}

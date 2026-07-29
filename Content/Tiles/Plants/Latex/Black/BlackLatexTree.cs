using ChangedSpecialMod.Content.Tiles.Latex.Black;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Plants.Latex.Black
{
    public class BlackLatexTree : BaseBlackLatexTree
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            GrowsOnTileId = [ModContent.TileType<BlackLatexGrassTile>()];
            texture = ModContent.Request<Texture2D>("ChangedSpecialMod/Content/Tiles/Plants/Latex/Black/BlackLatexTree");
            branchesTexture = ModContent.Request<Texture2D>("ChangedSpecialMod/Content/Tiles/Plants/Latex/Black/BlackLatexTree_Branches");
            topsTexture = ModContent.Request<Texture2D>("ChangedSpecialMod/Content/Tiles/Plants/Latex/Black/BlackLatexTree_Tops");
        }

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return ModContent.TileType<BlackLatexTreeSapling>();
        }
    }
}

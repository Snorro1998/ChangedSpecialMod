using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Latex
{
    public class BlackLatexTile : BaseBlackLatexTile
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.tileMergeDirt[Type] = true;
            ChangedUtils.SetTileMerge(ModContent.TileType<BlackLatexTile>());
        }
    }
}
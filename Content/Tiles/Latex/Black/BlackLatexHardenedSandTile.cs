using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Latex.Black
{
    public class BlackLatexHardenedSandTile : BaseBlackLatexTile
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.tileMergeDirt[Type] = true;
            ChangedUtils.SetTileMerge(ModContent.TileType<BlackLatexHardenedSandTile>());

            VanillaFallbackOnModDeletion = TileID.HardenedSand;
        }
    }
}
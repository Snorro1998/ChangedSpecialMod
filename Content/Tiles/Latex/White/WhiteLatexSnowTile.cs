using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Latex.White
{
    public class WhiteLatexSnowTile : BaseWhiteLatexTile
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.tileMergeDirt[Type] = true;
            ChangedUtils.SetTileMerge(ModContent.TileType<WhiteLatexSnowTile>());
            HitSound = SoundID.Item48;
            TileID.Sets.Conversion.Grass[Type] = true;

            //VanillaFallbackOnModDeletion = TileID.SnowBlock;
        }
    }
}

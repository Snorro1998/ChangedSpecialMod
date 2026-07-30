using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Latex.Black
{
    public class BlackLatexSnowTile : BaseBlackLatexTile
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.tileMergeDirt[Type] = true;
            ChangedUtils.SetTileMerge(ModContent.TileType<BlackLatexSnowTile>());
            HitSound = SoundID.Item48;
            TileID.Sets.Conversion.Grass[Type] = true;

            //VanillaFallbackOnModDeletion = TileID.SnowBlock;
        }
    }
}

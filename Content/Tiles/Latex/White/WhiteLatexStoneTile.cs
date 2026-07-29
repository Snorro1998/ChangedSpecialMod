using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Latex.White
{
    public class WhiteLatexStoneTile : BaseWhiteLatexTile
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            ChangedUtils.SetTileMerge(ModContent.TileType<WhiteLatexStoneTile>());
            Main.tileLavaDeath[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            HitSound = SoundID.Tink;

            VanillaFallbackOnModDeletion = TileID.Stone;
        }
    }
}

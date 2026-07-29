using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Latex.White
{
    public class WhiteLatexIceTile : BaseWhiteLatexTile
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            ChangedUtils.SetTileMerge(ModContent.TileType<WhiteLatexIceTile>());
            Main.tileLavaDeath[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            TileID.Sets.Ices[Type] = true;
            TileID.Sets.IceSkateSlippery[Type] = true;
            HitSound = SoundID.Item50;

            VanillaFallbackOnModDeletion = TileID.IceBlock;
        }
    }
}

using ChangedSpecialMod.Content.Items.Placeable.Latex;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Latex
{
    public class WhiteLatexStoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            ChangedUtils.SetTileMerge(ModContent.TileType<WhiteLatexStoneTile>());
            Main.tileLavaDeath[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            DustType = DustID.SnowBlock;
            HitSound = SoundID.Tink;
            AddMapEntry(new Color(200, 200, 200));
        }

        public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor)
        {
            return true;
        }

        public override void RandomUpdate(int i, int j)
        {
            WorldGenerator.GrowCrystal(i, j, NPCs.GooType.White);
            WorldGenerator.Corrupt(i, j, NPCs.GooType.White);
            base.RandomUpdate(i, j);
        }
    }
}

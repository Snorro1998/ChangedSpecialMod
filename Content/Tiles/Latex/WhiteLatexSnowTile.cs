using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Latex
{
    public class WhiteLatexSnowTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            ChangedUtils.SetTileMerge(ModContent.TileType<WhiteLatexSnowTile>());
            Main.tileLavaDeath[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            DustType = DustID.SnowBlock;
            HitSound = SoundID.Item48;
            AddMapEntry(new Color(200, 200, 200));
        }

        public override bool HasWalkDust()
        {
            return Main.rand.NextBool(3);
        }

        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
            dustType = DustID.SnowBlock;
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

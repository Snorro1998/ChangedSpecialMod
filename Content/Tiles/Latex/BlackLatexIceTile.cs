using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Latex
{
    public class BlackLatexIceTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            ChangedUtils.SetTileMerge(ModContent.TileType<BlackLatexIceTile>());
            Main.tileLavaDeath[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            TileID.Sets.Ices[Type] = true;
            TileID.Sets.IceSkateSlippery[Type] = true;
            DustType = DustID.Asphalt;
            HitSound = SoundID.Item50;
            AddMapEntry(new Color(35, 34, 41));
        }

        public override bool HasWalkDust()
        {
            return Main.rand.NextBool(3);
        }

        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
            dustType = DustID.Asphalt;
        }

        public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor)
        {
            return true;
        }

        public override void RandomUpdate(int i, int j)
        {
            WorldGenerator.GrowCrystal(i, j, NPCs.GooType.Black);
            WorldGenerator.Corrupt(i, j, NPCs.GooType.Black);
            base.RandomUpdate(i, j);
        }
    }
}

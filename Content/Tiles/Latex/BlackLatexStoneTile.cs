using ChangedSpecialMod.Content.Items.Placeable.Latex;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Latex
{
    public class BlackLatexStoneTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            ChangedUtils.SetTileMerge(ModContent.TileType<BlackLatexStoneTile>());
            Main.tileLavaDeath[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            DustType = DustID.Asphalt;
            HitSound = SoundID.Tink;
            AddMapEntry(new Color(35, 34, 41));
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

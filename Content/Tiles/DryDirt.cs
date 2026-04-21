using ChangedSpecialMod.Content.Dusts;
using ChangedSpecialMod.Content.Items.Placeable;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles
{
    public class DryDirt : ModTile
    {
        public int ItemDrop { get; private set; }

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            ChangedUtils.SetTileMerge(ModContent.TileType<DryDirt>());
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            DustType = DustID.Mud;//ModContent.DustType<Sparkle>();
            ItemDrop = ModContent.ItemType <DryDirtBlock>();
            AddMapEntry(new Color(200, 200, 200));
        }


        public override void RandomUpdate(int i, int j)
        {
            if (Main.rand.Next(2) == 0)
            {
                var topTile = Main.tile[i, j - 1];
                var bottomTile = Main.tile[i, j + 1];

                if (!topTile.HasTile && topTile.TileType != ModContent.TileType<DryDirtPlant>())
                {
                    WorldGen.PlaceTile(i, j - 1, ModContent.TileType<DryDirtPlant>(), true, false);
                    Main.tile[i, j - 1].TileFrameX = (short)(Main.rand.Next(6) * 18);
                    NetMessage.SendTileSquare(-1, i, j - 1, TileChangeType.None);
                }
            }

            base.RandomUpdate(i, j);
        }
    }
}


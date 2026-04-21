using ChangedSpecialMod.Content.Items.Placeable;
using ChangedSpecialMod.Content.Tiles.Furniture;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles
{
    public class BlackLatexTile : ModTile
    {
        public int ItemDrop { get; private set; }

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[ModContent.TileType<BlackLatexTile>()][TileID.Stone] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            DustType = DustID.Asphalt;//ModContent.DustType<Sparkle>();
            ItemDrop = ModContent.ItemType<BlackLatexBlock>();
            AddMapEntry(new Color(35, 34, 41));
            // Set other values here
        }

        public override void RandomUpdate(int i, int j)
        {
            // If you want to slow it down
            //if (Main.rand.Next(2) == 0)
            {
                var topTile = Main.tile[i, j - 1];
                var bottomTile = Main.tile[i, j + 1];

                if (!topTile.HasTile && topTile.TileType != ModContent.TileType<CrystalGreen>() && topTile.TileType != ModContent.TileType<CrystalRed>())
                {
                    var tileType = ChangedUtils.Choose(ModContent.TileType<CrystalGreen>(), ModContent.TileType<CrystalRed>());
                    ChangedUtils.PlaceRandomTile(i, j, tileType);
                }
            }

            base.RandomUpdate(i, j);
        }
    }
}
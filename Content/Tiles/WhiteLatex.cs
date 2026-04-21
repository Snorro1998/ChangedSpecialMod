using ChangedSpecialMod.Content.Items.Placeable;
using ChangedSpecialMod.Content.Tiles.Furniture;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles
{
    public class WhiteLatexTile : ModTile
    {
        public int ItemDrop { get; private set; }

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            ChangedUtils.SetTileMerge(ModContent.TileType<WhiteLatexTile>());
            Main.tileLavaDeath[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            DustType = DustID.SnowBlock;//ModContent.DustType<Sparkle>();
            ItemDrop = ModContent.ItemType<WhiteLatexBlock>();
            AddMapEntry(new Color(200, 200, 200));
        }

        public override void RandomUpdate(int i, int j)
        {
            //if (Main.rand.Next(2) == 0)
            {
                var topTile = Main.tile[i, j - 1];
                var bottomTile = Main.tile[i, j + 1];

                if (!topTile.HasTile && topTile.TileType != ModContent.TileType<CrystalWhite>() && topTile.TileType != ModContent.TileType<PillarWhite>())
                {
                    var tileType = ChangedUtils.Choose(ModContent.TileType<PillarWhite>(), ModContent.TileType<CrystalWhite>());
                    ChangedUtils.PlaceRandomTile(i, j, tileType);
                }
            }

            base.RandomUpdate(i, j);
        }
    }
}


using ChangedSpecialMod.Content.Dusts;
using ChangedSpecialMod.Content.Items.Placeable;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles
{
    public class DryDirtGrassTile : ModTile
    {
        public int ItemDrop { get; private set; }

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileBrick[Type] = true;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Grass"]);

            ChangedUtils.SetTileMerge(ModContent.TileType<DryDirtGrassTile>());
            Main.tileLighted[Type] = true;
            DustType = DustID.Mud;
            AddMapEntry(new Color(255, 111, 0));

            TileID.Sets.Grass[Type] = true;
            TileID.Sets.Conversion.Grass[Type] = true;

            TileID.Sets.NeedsGrassFraming[Type] = true;
            TileID.Sets.NeedsGrassFramingDirt[Type] = ModContent.TileType<DryDirt>();
            TileID.Sets.CanBeDugByShovel[Type] = true;
        }


        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail && !effectOnly)
            {
                Main.tile[i, j].TileType = (ushort)ModContent.TileType<DryDirt>();
            }
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

        // Ignore style nonsense and always drop the correct item
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<DryDirtBlock>());
        }
    }
}


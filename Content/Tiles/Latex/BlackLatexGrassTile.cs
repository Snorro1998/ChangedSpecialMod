using ChangedSpecialMod.Content.Dusts;
using ChangedSpecialMod.Content.Items.Placeable;
using ChangedSpecialMod.Content.Items.Placeable.Latex;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Latex
{
    public class BlackLatexGrassTile : BaseBlackLatexTile
    {
        public int ItemDrop { get; private set; }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileBrick[Type] = true;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Grass"]);

            ChangedUtils.SetTileMerge(ModContent.TileType<BlackLatexGrassTile>());
            Main.tileLighted[Type] = true;
            ItemDrop = ModContent.ItemType<BlackLatexBlock>();

            TileID.Sets.Grass[Type] = true;
            TileID.Sets.Conversion.Grass[Type] = true;
            TileID.Sets.NeedsGrassFraming[Type] = true;
            TileID.Sets.NeedsGrassFramingDirt[Type] = ModContent.TileType<BlackLatexTile>();
        }


        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail && !effectOnly)
            {
                Main.tile[i, j].TileType = (ushort)ModContent.TileType<BlackLatexTile>();
            }
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<BlackLatexBlock>());
        }
    }
}


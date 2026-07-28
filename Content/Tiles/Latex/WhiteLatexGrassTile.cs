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
    public class WhiteLatexGrassTile : BaseWhiteLatexTile
    {
        public int ItemDrop { get; private set; }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileBrick[Type] = true;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Grass"]);

            ChangedUtils.SetTileMerge(ModContent.TileType<WhiteLatexGrassTile>());
            Main.tileLighted[Type] = true;
            ItemDrop = ModContent.ItemType<WhiteLatexBlock>();

            TileID.Sets.Grass[Type] = true;
            TileID.Sets.Conversion.Grass[Type] = true;
            TileID.Sets.NeedsGrassFraming[Type] = true;
            TileID.Sets.NeedsGrassFramingDirt[Type] = ModContent.TileType<WhiteLatexTile>();

            VanillaFallbackOnModDeletion = TileID.Grass;
        }


        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail && !effectOnly)
            {
                Main.tile[i, j].TileType = (ushort)ModContent.TileType<WhiteLatexTile>();
            }
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<WhiteLatexBlock>());
        }
    }
}


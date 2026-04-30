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
            DustType = DustID.SnowBlock;
            ItemDrop = ModContent.ItemType<WhiteLatexBlock>();
            AddMapEntry(new Color(200, 200, 200));
        }

        public override void RandomUpdate(int i, int j)
        {
            WorldGenerator.GrowCrystal(i, j, NPCs.GooType.White);
            WorldGenerator.Corrupt(i, j, NPCs.GooType.White);
            base.RandomUpdate(i, j);
        }
    }
}


using ChangedSpecialMod.Content.Items.Placeable;
using ChangedSpecialMod.Content.Tiles.Furniture;
using ChangedSpecialMod.Content.Tiles.Latex;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
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
            ChangedUtils.SetTileMerge(ModContent.TileType<BlackLatexTile>());
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
            WorldGenerator.GrowCrystal(i, j, NPCs.GooType.Black);
            WorldGenerator.Corrupt(i, j, NPCs.GooType.Black);
            base.RandomUpdate(i, j);
        }
    }
}
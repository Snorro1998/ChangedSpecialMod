using ChangedSpecialMod.Content.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ChangedSpecialMod.Content.Dusts;

namespace ChangedSpecialMod.Content.Tiles
{
    public class CautionTile : ModTile
    {
        public int ItemDrop { get; private set; }

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[ModContent.TileType<CautionTile>()][TileID.Stone] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            DustType = ModContent.DustType<Sparkle>();
            ItemDrop = ModContent.ItemType<CautionTileBlock>();
            AddMapEntry(new Color(255, 191, 28));
            // Set other values here
        }
    }
}


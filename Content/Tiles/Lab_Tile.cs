using ChangedSpecialMod.Content.Dusts;
using ChangedSpecialMod.Content.Items.Placeable;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles
{
    public class Lab_TileTile : ModTile
    {
        public int ItemDrop { get; private set; }

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            ChangedUtils.SetTileMerge(ModContent.TileType<Lab_TileTile>());
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            DustType = DustID.TintableDust;//ModContent.DustType<Sparkle>();
            ItemDrop = ModContent.ItemType <LabTileBlock>();
            AddMapEntry(new Color(200, 200, 200));
        }
    }
}


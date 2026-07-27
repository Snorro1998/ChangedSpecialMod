using ChangedSpecialMod.Content.Items.Placeable.Latex;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Latex
{
    public class WhiteLatexTile : BaseWhiteLatexTile
    {
        public int ItemDrop { get; private set; }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults(); 
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            ChangedUtils.SetTileMerge(ModContent.TileType<WhiteLatexTile>());
            Main.tileLavaDeath[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            ItemDrop = ModContent.ItemType<WhiteLatexBlock>();
        }
    }
}


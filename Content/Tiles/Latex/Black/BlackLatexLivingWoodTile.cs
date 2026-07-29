using ChangedSpecialMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Latex.Black
{
    public class BlackLatexLivingWoodTile : BaseBlackLatexTile
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            ChangedUtils.SetTileMerge(ModContent.TileType<BlackLatexLivingWoodTile>());
            Main.tileLavaDeath[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;

            VanillaFallbackOnModDeletion = TileID.LivingWood;
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ItemID.Wood);
        }
    }
}
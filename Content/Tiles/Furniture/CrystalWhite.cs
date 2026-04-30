using ChangedSpecialMod.Content.Items;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class CrystalWhite : ModTile
	{
        public override void SetStaticDefaults()
        {
            Main.tileShine[Type] = 2400;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.RandomStyleRange = 4;
            TileObjectData.newTile.StyleHorizontal = true;
            TileID.Sets.FramesOnKillWall[Type] = false;
            TileObjectData.addTile(Type);
            DustType = DustID.WhiteTorch;
            HitSound = SoundID.Item27;
            AddMapEntry(new Color(220, 220, 220));
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<Items.Placeable.Furniture.CrystalWhite>());
        }

        public override void EmitParticles(int i, int j, Tile tile, short tileFrameX, short tileFrameY, Color tileLight, bool visible)
        {
            if (!Main.dedServ && Main.rand.NextBool(200))
            {
                var player = Main.LocalPlayer;
                if (player.inventory.Any(x => x.type == ModContent.ItemType<DiscoCrystal>()))
                {
                    var dust = Dust.NewDustPerfect(new Vector2(i * 16, j * 16), DustID.WhiteTorch, null);
                }
            }

            base.EmitParticles(i, j, tile, tileFrameX, tileFrameY, tileLight, visible);
        }
    }
}

using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class Generator : ModTile
	{
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = false;
            // Copy default settings
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 4;
            TileObjectData.addTile(Type);
            DustType = DustID.WoodFurniture;

            AnimationFrameHeight = 72;
        }

        /*
        // I don't know why, but without this it won't drop an item if it is turned on
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            noItem = true;
            var coords = TileObjectData.TopLeft(i, j);
            var isEnabled = Main.tile[i, j].TileFrameX > 0;

            // Breaking top left tile
            if (coords.X == i && coords.Y == j && isEnabled && !fail)
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Placeable.Furniture.Generator>());
            }

            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
        }
        */

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter >= 10) // speed of animation (lower = faster)
            {
                frameCounter = 0;
                frame = (frame + 1) % 3;
            }
        }

        public override bool RightClick(int i, int j)
        {
            Main.NewText("Use the switch to toggle the power");
            SoundEngine.PlaySound(Assets.Sounds.SoundBuzzer2);
            return true;
        }

        public override void HitWire(int i, int j)
        {
            var coords = TileObjectData.TopLeft(i, j);
            var topLeftTile = Main.tile[coords.X, coords.Y];
            var tileFrameX = topLeftTile.TileFrameX == 0 ? 72 : 0;

            // Toggled off
            if (tileFrameX == 0)
                SoundEngine.PlaySound(Assets.Sounds.SoundBuzzer2);
            // Toggled on
            else
                SoundEngine.PlaySound(Assets.Sounds.SoundChime2);

            bool wireSkip = Wiring.running;

            for (int y = 0; y < 4; y++)
            {
                var yPos = coords.Y + y;
                for (int x = 0; x < 4; x++)
                {
                    var xPos = coords.X + x;
                    var tile = Main.tile[xPos, yPos];
                    tile.TileFrameX = (short)(tileFrameX + x * 18);
                    if (wireSkip) Wiring.SkipWire(xPos, yPos);
                }
            }

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendTileSquare(-1, coords.X, coords.Y, 4, TileChangeType.None);
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.Generator>();
        }

        // Ignore style nonsense and always drop the correct item
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<Items.Placeable.Furniture.Generator>());
        }
    }
}

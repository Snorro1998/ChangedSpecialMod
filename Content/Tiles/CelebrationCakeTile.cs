using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles
{
    public class CelebrationCakeTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileLighted[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.StyleLineSkip = 2;
            TileObjectData.addTile(Type);
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<CelebrationCake>();
        }

        public override bool RightClick(int i, int j)
        {
            var coords = TileObjectData.TopLeft(i, j);
            var topLeftTile = Main.tile[coords.X, coords.Y];
            var tileFrameX = topLeftTile.TileFrameX == 0 ? (2 * 18) : 0;

            bool toggledOn = tileFrameX != 0;

            for (int y = 0; y < 2; y++)
            {
                var yPos = coords.Y + y;
                for (int x = 0; x < 2; x++)
                {
                    var xPos = coords.X + x;
                    var tile = Main.tile[xPos, yPos];
                    tile.TileFrameX = (short)(tileFrameX + x * 18);
                }
            }

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendTileSquare(-1, coords.X, coords.Y, 2, TileChangeType.None);

            return true;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].TileFrameX >= 36)
            {
                r = 1f;
                g = 1f;
                b = 1f;
            }
            else
            {
                r = 0f;
                g = 0f;
                b = 0f;
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            base.PostDraw(i, j, spriteBatch);

            var tile = Main.tile[i, j];

            if (tile.TileFrameX < 54 || tile.TileFrameY != 0)
                return;

            Texture2D texture = Mod.Assets.Request<Texture2D>("Assets/Textures/Dust/Torch").Value;

            for (int k = 0; k < 3; k++)
            {
                spriteBatch.Draw(
                    texture, 
                    new Vector2(i * 16 - Main.screenPosition.X + k * 8 - 13, j * 16 - Main.screenPosition.Y - 3) + ChangedUtils.TileDrawOffset, 
                    new Rectangle(0, ChangedUtils.Choose(0, 10, 20), 10, 10), 
                    Color.White
                );
            }
        }

        // Ignore style nonsense and always drop the correct item
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<Items.Placeable.Furniture.CelebrationCake>());
        }
    }
}
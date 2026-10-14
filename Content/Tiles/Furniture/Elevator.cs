using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class Elevator : ModTile
	{
        public static string retroTexturePath = "ChangedSpecialMod/Content/Tiles/Furniture/ElevatorRetro";
        public const int ElevatorReach = 20;

		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = false;
			TileID.Sets.FramesOnKillWall[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.addTile(Type);
			DustType = DustID.WoodFurniture;
            AddMapEntry(new Color(200, 200, 200), Language.GetText("Mods.ChangedSpecialMod.Tiles.Elevator"));
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            if (!player.IsWithinSnappngRangeToTile(i, j, 5 * 16))
                return false;

            // Disable usage if you are fighting the wolf king or behemoth and play a buzzer sound
            if (ChangedUtils.HasWolfKingFightStarted()/* || NPC.AnyNPCs(ModContent.NPCType<Behemoth>())*/)
            {
                SoundEngine.PlaySound(Assets.Sounds.SoundBuzzer1);
                return true;
            }

            var coords = TileObjectData.TopLeft(i, j);
            var topLeftTile = Main.tile[coords.X, coords.Y];
            // Going up
            int direction = -1;

            if (j >= coords.Y + 2)
                direction = 1;

            for (int y = 0; y < ElevatorReach; y++)
            {
                var yPos = coords.Y + direction * y;
                var topLeft = TileObjectData.TopLeft(i, yPos);
                // The tile is a different elevator and is at the same x position
                if (Main.tile[i, yPos].TileType == ModContent.TileType<Elevator>() && topLeft != Point16.NegativeOne && topLeft.X == coords.X && topLeft.Y != coords.Y)
                {
                    SoundEngine.PlaySound(Assets.Sounds.SoundElevator);
                    // Need something for this probably
                    player.RemoveAllGrapplingHooks();
                    ChangedUtils.TeleportPlayer(player.whoAmI, topLeft.X + 1, topLeft.Y);
                    return true;
                }
            }

            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            if (!player.IsWithinSnappngRangeToTile(i, j, 5 * 16))
                return;

            var coords = TileObjectData.TopLeft(i, j);
            var topLeftTile = Main.tile[coords.X, coords.Y];
            // Going up
            int itemType = ModContent.ItemType<Items.Placeable.Furniture.ElevatorUp>();

            if (j >= coords.Y + 2)
                itemType = ModContent.ItemType<Items.Placeable.Furniture.ElevatorDown>();

            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = itemType;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (ChangedSpecialModClientConfig.Instance.RetroVisualsTiles)
            {
                RetroDraw(i, j, spriteBatch);
                return false;
            }
            return base.PreDraw(i, j, spriteBatch);
        }

        private void RetroDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];

            if (tile.IsTileInvisible && !Main.ShouldShowInvisibleWalls())
                return;

            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            int height = 16;
            int frameYOffset = 0;

            var texture = TextureAssets.Tile[Type].Value;
            if (ModContent.RequestIfExists(retroTexturePath, out Asset<Texture2D> retroTexture))
                texture = retroTexture.Value;

            // Firstly we draw the original texture and then glow mask texture
            spriteBatch.Draw(
                texture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY + frameYOffset, 16, height),
                Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
        }
    }
}

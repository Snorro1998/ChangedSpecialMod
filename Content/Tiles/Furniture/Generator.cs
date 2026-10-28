using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Content.Achievements;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static ChangedSpecialMod.ChangedSpecialMod;
using Microsoft.Xna.Framework;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class Generator : ModTile
	{
        public static string retroTexturePath = "ChangedSpecialMod/Content/Tiles/Furniture/GeneratorRetro";
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 4;
            TileObjectData.addTile(Type);
            DustType = DustID.WoodFurniture;

            AnimationFrameHeight = 72;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter >= 10)
            {
                frameCounter = 0;
                frame = (frame + 1) % 3;
            }
        }

        public override bool RightClick(int i, int j)
        {
            Main.NewText(Language.GetTextValue("Mods.ChangedSpecialMod.Messages.TogglePower"));
            SoundEngine.PlaySound(Assets.Sounds.SoundBuzzer2);
            return true;
        }

        public override void HitWire(int i, int j)
        {
            var coords = TileObjectData.TopLeft(i, j);
            var topLeftTile = Main.tile[coords.X, coords.Y];
            var tileFrameX = topLeftTile.TileFrameX == 0 ? 72 : 0;

            bool toggledOn = tileFrameX != 0;

            if (Main.netMode == NetmodeID.SinglePlayer)
                SoundEngine.PlaySound(toggledOn ? Assets.Sounds.SoundChime2 : Assets.Sounds.SoundBuzzer2);

            else if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)MessageType.PlaySwitchSound);
                packet.Write((short)i);
                packet.Write((short)j);
                packet.Write(toggledOn);
                packet.Send();
            }

            ModContent.GetInstance<AATurnPowerOnAchievement>().ConditionTurnPowerOn.Complete();
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
            zero.Y += 2;
            int height = 16;
            int frameYOffset = Main.tileFrame[tile.TileType] * 18 * 4;

            var texture = TextureAssets.Tile[Type].Value;
            if (ModContent.RequestIfExists(retroTexturePath, out Asset<Texture2D> retroTexture))
                texture = retroTexture.Value;

            spriteBatch.Draw(
                texture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY + frameYOffset, 16, height),
                Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
        }
    }
}

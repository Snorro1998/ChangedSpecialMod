using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ChangedSpecialMod
{
    public class CustomPlayerLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.LastVanillaLayer);

        private int PlayerAnimationWhiteTaur(int legFrame)
        {
            int frameIndex = 0;
            if (legFrame > 5)
            {
                if (legFrame == 6 || legFrame == 7)
                    frameIndex = 3;
                else if (legFrame == 8 || legFrame == 9)
                    frameIndex = 4;
                else if (legFrame == 10 || legFrame == 11)
                    frameIndex = 5;
                else if (legFrame == 12 || legFrame == 13)
                    frameIndex = 6;
                else if (legFrame == 14 || legFrame == 15)
                    frameIndex = 7;
                else if (legFrame == 16 || legFrame == 17)
                    frameIndex = 8;
                else
                    frameIndex = 9;
            }

            else if (legFrame == 5)
                frameIndex = 10;
            else
                frameIndex = 0;

            return frameIndex;
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            var changedPlayer = player.ChangedPlayer();
            var baseTexturePath = "ChangedSpecialMod/Content/NPCs/";
            var texturePath = $"{baseTexturePath}Transparent";
            var nFrames = 3;

            var transfurCurrent = changedPlayer?.TransfurTypeCurrent;
            if (transfurCurrent != null)
            {
                texturePath = transfurCurrent.texturePath;
                nFrames = transfurCurrent.nFrames;
            }

            Texture2D texture = ModContent.Request<Texture2D>(texturePath).Value;
            Vector2 playerPos = player.Center;
            playerPos.Y += player.gfxOffY;
            Vector2 position = playerPos - Main.screenPosition;
            position = new Vector2((int)position.X, (int)position.Y);
            var effects = player.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            var frameWidth = texture.Width;
            var legFrame = player.legFrame.Y / 56;
            var frameIndex = (player.legFrame.Y / (4 * 32)) % nFrames;// 0;

            if (transfurCurrent != null && transfurCurrent.npcType == ModContent.NPCType<WhiteLatexTaur>())
                frameIndex = PlayerAnimationWhiteTaur(legFrame);
            else
            {
                //6..19 walk
                if (legFrame > 5)
                {
                    if (legFrame == 6 || legFrame == 7 || legFrame == 13 || legFrame == 14)
                        frameIndex = 0;
                    else if (legFrame == 8 || legFrame == 11 || legFrame == 12 || legFrame == 15 || legFrame == 18 || legFrame == 19)
                        frameIndex = 1;
                    else
                        frameIndex = 2;
                }

                else if (legFrame == 5)
                    frameIndex = 2;
                else
                    frameIndex = 0;
            }

            var frameHeight = texture.Height / nFrames;
            var sourceRect = new Rectangle(0, frameIndex * frameHeight, frameWidth, frameHeight);

            var yOff = (48 - frameHeight) / 2;
            position.Y += yOff;

            Vector2 drawCenter = player.Center;
            Vector2 drawPositionInWorld = drawCenter;
            Color tmpColor = Lighting.GetColor((int)drawCenter.X / 16, (int)(drawCenter.Y / 16f));

            // Body
            drawInfo.DrawDataCache.Add(new DrawData(
                texture,
                position,
                sourceRect,
                tmpColor,
                0f,
                new Vector2(frameWidth / 2, frameHeight / 2),
                1f,
                effects,
                0f
            ));
        }
    }
}

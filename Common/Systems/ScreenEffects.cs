using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace ChangedSpecialMod.Common.Systems
{
    public class ScreenEffect : ModSystem
    {
        public override void ModifyInterfaceLayers(
            List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = 1;// layers.FindIndex(
                //layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex,
                    new LegacyGameInterfaceLayer(
                        "ChangedSpecialMod: Green Gas",
                        delegate
                        {
                            if (Main.gameMenu)
                                return true;

                            // Get the toxic meter from the local player
                            var player = Main.LocalPlayer;
                            var toxicMeter = player.ChangedPlayer().gasAmount;

                            // Calculate opacity (0 to 0.5)
                            float opacity = MathHelper.Clamp(
                                toxicMeter / 240f * 0.3f,
                                0f,
                                0.3f
                            );

                            // Draw the green screen overlay
                            if (opacity > 0f)
                            {
                                Main.spriteBatch.Draw(
                                    TextureAssets.MagicPixel.Value,
                                    new Rectangle(
                                        0,
                                        0,
                                        Main.screenWidth,
                                        Main.screenHeight
                                    ),
                                    Color.Blue * opacity
                                );
                            }

                            return true;
                        },
                        InterfaceScaleType.UI
                    )
                );
            }
        }
    }

    /*
    public class BlindnessSystem : ModSystem
    {
        private const int RegionSize = 50;

        public override void ModifyInterfaceLayers(
            List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = 1;// layers.FindIndex(
                //layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex,
                    new LegacyGameInterfaceLayer(
                        "MyMod: Blindness Effect",
                        delegate
                        {
                            if (Main.gameMenu)
                                return true;

                            var player = Main.LocalPlayer;
                            var modPlayer = player.ChangedPlayer();

                            //if (!modPlayer.blinded)
                            //   return true;

                            // Get player's current tile coordinates
                            int playerTileX = (int)(player.Center.X / 16f);
                            int playerTileY = (int)(player.Center.Y / 16f);

                            // Determine player's current region
                            int regionX = playerTileX / RegionSize;
                            int regionY = playerTileY / RegionSize;

                            // Region boundaries in world pixels
                            float left = regionX * RegionSize * 16f;
                            float right = (regionX + 1) * RegionSize * 16f;

                            float top = regionY * RegionSize * 16f;
                            float bottom = (regionY + 1) * RegionSize * 16f;

                            // Convert world coordinates to screen coordinates
                            left -= Main.screenPosition.X;
                            right -= Main.screenPosition.X;

                            top -= Main.screenPosition.Y;
                            bottom -= Main.screenPosition.Y;

                            int screenWidth = Main.screenWidth;
                            int screenHeight = Main.screenHeight;

                            // Clamp visible region to screen boundaries
                            int visibleLeft = (int)MathHelper.Clamp((int)left, 0, screenWidth);
                            int visibleRight = (int)MathHelper.Clamp((int)right, 0, screenWidth);
                            int visibleTop = (int)MathHelper.Clamp((int)top, 0, screenHeight);
                            int visibleBottom = (int)MathHelper.Clamp((int)bottom, 0, screenHeight);

                            Texture2D pixel = TextureAssets.MagicPixel.Value;

                            // Black area above the region
                            DrawBlack(0, 0, screenWidth, visibleTop, pixel);

                            // Black area below the region
                            DrawBlack(
                                0, visibleBottom,
                                screenWidth,
                                screenHeight - visibleBottom,
                                pixel
                            );

                            // Black area left of the region
                            DrawBlack(
                                0, visibleTop,
                                visibleLeft,
                                visibleBottom - visibleTop,
                                pixel
                            );

                            // Black area right of the region
                            DrawBlack(
                                visibleRight, visibleTop,
                                screenWidth - visibleRight,
                                visibleBottom - visibleTop,
                                pixel
                            );

                            return true;
                        },
                        InterfaceScaleType.UI
                    )
                );
            }
        }

        private static void DrawBlack(
            int x,
            int y,
            int width,
            int height,
            Texture2D pixel)
        {
            if (width <= 0 || height <= 0)
                return;

            Main.spriteBatch.Draw(
                pixel,
                new Rectangle(x, y, width, height),
                Color.Black
            );
        }
    }
    */
}
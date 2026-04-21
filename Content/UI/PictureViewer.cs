using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;


namespace ChangedSpecialMod.Content.UI
{
    public class PictureViewer
    {
        public const float MaxViewingDistance = 160f;
        public static int imageIndex = 0;
        public static int[] pictures = null;

        public static void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            var changedPlayer = player.ChangedPlayer();

            var pictureViewerOpen = changedPlayer.pictureViewerOpen;
            var pictureIndex = changedPlayer.pictureIndex;

            if (!pictureViewerOpen || pictures == null)
            {
                imageIndex = 0;
                return;
            }

            // Close if index is too great, the inventory is open or if somehow on the main menu
            if (imageIndex >= pictures.Length || Main.playerInventory || Main.gameMenu)
            {
                changedPlayer.pictureViewerOpen = false;
                pictures = null;
                imageIndex = 0;
                return;
            }

            var playerPos = new Vector2(player.Center.X, player.Center.Y + player.gfxOffY);
            Vector2 drawPosition = playerPos - Main.screenPosition;
            drawPosition.X -= 72;
            drawPosition.Y -= 48;
            Texture2D texture = ModContent.Request<Texture2D>($"ChangedSpecialMod/Assets/Textures/Pictures/{pictures[imageIndex]}").Value;
            spriteBatch.Draw(texture, drawPosition, null, Color.White, 0f, texture.Size() * 0.5f, 2, SpriteEffects.None, 0f);
            
            // Should continue to the next picture, but for now just close it
            if ((Main.mouseLeft && Main.mouseLeftRelease) || (Main.mouseRight && Main.mouseRightRelease))
            {
                imageIndex++;
                if (pictures != null && imageIndex < pictures.Length)
                    SoundEngine.PlaySound(Sounds.SoundBook);
            }
        }
    }
}

using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.UI;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.UI;

namespace ChangedSpecialMod.Utilities
{
    public class PictureSystem : ModSystem
    {
        private static Dictionary<int, Texture2D> pics = new Dictionary<int, Texture2D>();

        // Unused transformation sequence
        public static int[] ImagesScientists = new int[] { 62, 64, 65, 66 };
        public static int[] ImagesBacteria = new int[] { 111, 112, 113, 114 };
        public static int[] ImagesCorridor = new int[] { 147, 148, 149, 150 };
        public static int[] ImagesCrystalToBabby = new int[] { 181, 182, 183, 184 };
        public static int[] ImagesPuroSilly = new int[] { 236, 237, 238, 239 };
        public static int[] ImagesOutsideBuilding = new int[] { 517, 518, 519, 520 };
        public static int[] ImagesColinPuro = new int[] { 547, 548, 549, 550 };

        // This is stupid but it fixes the images flickering when viewing them for the first time
        public override void OnModLoad()
        {
            AddPictures(ImagesBacteria);
            AddPictures(ImagesCorridor);
            AddPictures(ImagesCrystalToBabby);
            AddPictures(ImagesPuroSilly);
            AddPictures(ImagesOutsideBuilding);
            AddPictures(ImagesColinPuro);
        }

        public override void Unload()
        {
            pics.Clear();
        }

        public static Texture2D GetPicture(int i)
        {
            if (pics != null && pics.ContainsKey(i))
                return pics[i];
            return null;
        }

        private void AddPictures(int[] p)
        {
            for (int i = 0; i < p.Length; i++)
            {
                var pic = p[i];
                Texture2D texture = ModContent.Request<Texture2D>($"ChangedSpecialMod/Assets/Textures/Pictures/{pic}").Value;
                if (texture != null)
                    pics.Add(pic, texture);
            }
        }

        public static bool OpenPictureViewer(int i, int j, int[] pictures)
        {
            Player player = Main.LocalPlayer;
            ChangedUtils.CancelSignsAndChests(player);
            // Close inventory screen if open
            Main.playerInventory = false;
            var changedPlayer = player.ChangedPlayer();

            if (changedPlayer.pictureViewerOpen)
            {
                if (PictureViewer.pictures != pictures)
                {
                    SoundEngine.PlaySound(Sounds.SoundBook);
                    PictureViewer.pictures = pictures;
                    PictureViewer.imageIndex = 0;
                    changedPlayer.pictureViewerOpen = true;
                }
                else
                {
                    PictureViewer.imageIndex++;
                    if (PictureViewer.imageIndex <  pictures.Length)
                        SoundEngine.PlaySound(Sounds.SoundBook);
                }
            }

            else
            {
                SoundEngine.PlaySound(Sounds.SoundBook);
                PictureViewer.pictures = pictures;
                PictureViewer.imageIndex = 0;
                changedPlayer.pictureViewerOpen = true;
            }

            return true;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");
            if (mouseIndex != -1)
            {
                layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("Picture Viewer UI", () =>
                {
                    PictureViewer.Draw(Main.spriteBatch);
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
}

using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.UI
{
    public class CustomHeartOverlay : ModResourceOverlay
    {
        private Dictionary<string, Asset<Texture2D>> vanillaAssetCache = new();
        private Asset<Texture2D> blackHeartTexture;
        private Asset<Texture2D> whiteHeartTexture;

        private void DrawClassicFancyOverlay(ResourceOverlayDrawContext context, GooType gooType)
        {
            context.texture = blackHeartTexture ??= ModContent.Request<Texture2D>("ChangedSpecialMod/Content/UI/WhiteHeart");
            if (gooType == GooType.Black)
                context.texture = whiteHeartTexture ??= ModContent.Request<Texture2D>("ChangedSpecialMod/Content/UI/BlackHeart");

            context.Draw();
        }

        public override void PostDrawResource(ResourceOverlayDrawContext context)
        {
            var changedPlayer = Main.LocalPlayer.ChangedPlayer();

            if (!ChangedSpecialModClientConfig.Instance.CustomHealthBar || changedPlayer.TransfurTypeCurrent == null)
                return;

            Asset<Texture2D> asset = context.texture;

            string fancyFolder = "Images/UI/PlayerResourceSets/FancyClassic/";
            string barsFolder = "Images/UI/PlayerResourceSets/HorizontalBars/";

            bool drawingBarsPanels = CompareAssets(asset, barsFolder + "HP_Panel_Middle");

            if (asset == TextureAssets.Heart || asset == TextureAssets.Heart2)
            {
                // Draw over the Classic hearts
                DrawClassicFancyOverlay(context, changedPlayer.TransfurTypeCurrent.gooType);
            }
            else if (CompareAssets(asset, fancyFolder + "Heart_Fill") || CompareAssets(asset, fancyFolder + "Heart_Fill_B"))
            {
                // Draw over the Fancy hearts
                DrawClassicFancyOverlay(context, changedPlayer.TransfurTypeCurrent.gooType);
            }
        }

        private bool CompareAssets(Asset<Texture2D> existingAsset, string compareAssetPath)
        {
            if (!vanillaAssetCache.TryGetValue(compareAssetPath, out var asset))
                asset = vanillaAssetCache[compareAssetPath] = Main.Assets.Request<Texture2D>(compareAssetPath);

            return existingAsset == asset;
        }
    }
}

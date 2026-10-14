using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Content.Dusts;
using ChangedSpecialMod.Content.Items.Placeable;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles
{
    public class Lab_TileTile : ModTile
    {
        public static string retroTexturePath = "ChangedSpecialMod/Content/Tiles/Lab_TileTileRetro";

        public int ItemDrop { get; private set; }

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            ChangedUtils.SetTileMerge(ModContent.TileType<Lab_TileTile>());
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            DustType = DustID.TintableDust;//ModContent.DustType<Sparkle>();
            ItemDrop = ModContent.ItemType <LabTileBlock>();
            AddMapEntry(new Color(200, 200, 200));
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


using ChangedSpecialMod.Common.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class MountBookest : ModTile
	{
		public static string retroTexturePath = "ChangedSpecialMod/Content/Tiles/Furniture/MountBookestRetro";

        public override void SetStaticDefaults() 
		{
			// Properties
			Main.tileTable[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true;

            DustType = DustID.WoodFurniture;
            AdjTiles = new int[] { TileID.Tables };

			// Placement
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.addTile(Type);

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

			// Etc
			AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Table"));
		}

		public override void NumDust(int x, int y, bool fail, ref int num) 
		{
			num = fail ? 1 : 3;
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

            // If you are using ModTile.SpecialDraw or PostDraw or PreDraw, use this snippet and add zero to all calls to spriteBatch.Draw
            // The reason for this is to accommodate the shift in drawing coordinates that occurs when using the different Lighting modes
            // While at 100% world zoom, press Shift+F9 to change lighting modes quickly to verify your code works for all lighting modes
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            // Because height of third tile is different we change it
            int height = 16;//tile.TileFrameY % AnimationFrameHeight == 36 ? 18 : 16;

            // Offset along the Y axis depending on the current frame
            int frameYOffset = 0;// (Main.tileFrame[Type] + 1) * AnimationFrameHeight;

            var texture = TextureAssets.Tile[Type].Value;
            if (ModContent.RequestIfExists(retroTexturePath, out Asset<Texture2D> retroTexture))
                texture = retroTexture.Value;

            // Firstly we draw the original texture and then glow mask texture
            spriteBatch.Draw(
                texture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY + frameYOffset, 16, height),
                Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
            // Make sure to draw with Color.White or at least a color that is fully opaque
            // Achieve opaqueness by increasing the alpha channel closer to 255. (lowering closer to 0 will achieve transparency)
            /*
            var zero = Main.drawToScreen ? Vector2.Zero : new(Main.offScreenRange, Main.offScreenRange);
            var tileDrawOffset = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
            var drawPos = new Vector2(i * 16, j * 16) - Main.screenPosition + zero + tileDrawOffset;

			Vector2 position = drawPos;// pos.ToWorldCoordinates() - Main.screenPosition;
            Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
            Color tileColor = Lighting.GetColor(pos);

			var texture = TextureAssets.Tile[Type].Value;
			if (ModContent.RequestIfExists(retroTexturePath, out Asset<Texture2D> retroTexture))
				texture = retroTexture.Value;

            Main.spriteBatch.Draw(texture, position, frame, tileColor);
			*/
        }
    }
}

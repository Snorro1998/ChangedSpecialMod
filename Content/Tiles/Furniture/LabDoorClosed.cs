using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Content.Dusts;
using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	//TODO: Smart Cursor Outlines and tModLoader support
	public class LabDoorClosed : ModTile
	{
        public static string retroTexturePath = "ChangedSpecialMod/Content/Tiles/Furniture/LabDoorClosedRetro";
        public override void SetStaticDefaults()
        {
            // Properties
            Main.tileFrameImportant[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.NotReallySolid[Type] = true;
            TileID.Sets.DrawsWalls[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.OpenDoorID[Type] = ModContent.TileType<LabDoorOpen>();

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);

            DustType = DustID.TintableDust; //ModContent.DustType<Sparkle>();
            AdjTiles = [TileID.ClosedDoor];

            // Names
            AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Door"));

            // Placement
            // In addition to copying from the TileObjectData.Something templates, modders can copy from specific tile types. CopyFrom won't copy subtile data, so style specific properties won't be copied, such as how Obsidian doors are immune to lava.
            TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.ClosedDoor, 0));
            /* This is what is copied from the ClosedDoor tile
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.CoordinateHeights = [16, 16, 16];
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.newTile.StyleLineSkip = 3; // When a door closes, each tile randomize between 3 different options. StyleLineSkip ensures that those tiles are interpreted as the correct style.
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(0, 1);
			TileObjectData.addAlternate(0);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(0, 2);
			TileObjectData.addAlternate(0);
			*/
            TileObjectData.addTile(Type);
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<LabDoor>();
        }

        /*
        public override bool RightClick(int i, int j)
        {
            SoundEngine.PlaySound(Assets.Sounds.SoundDoor, new Vector2(i * 16, j * 16));
            return base.RightClick(i, j);
        }
        */

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
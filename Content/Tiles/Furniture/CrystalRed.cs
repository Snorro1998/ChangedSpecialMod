using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Content.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class CrystalRed : ModTile
	{
        public static string retroTexturePath = "ChangedSpecialMod/Content/Tiles/Furniture/CrystalRedRetro";
        public override void SetStaticDefaults()
        {
            Main.tileShine[Type] = 2400;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.RandomStyleRange = 4;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = 2;
            TileID.Sets.FramesOnKillWall[Type] = false;
            TileObjectData.addTile(Type);
            DustType = DustID.RedTorch;
            HitSound = SoundID.Item27;
            AddMapEntry(new Color(255, 26, 80));
        }

        // Ignore style nonsense and always drop the correct item
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<Items.Placeable.Crystals.CrystalRed>());
        }

        public override void EmitParticles(int i, int j, Tile tile, short tileFrameX, short tileFrameY, Color tileLight, bool visible)
        {
            if (!Main.dedServ && Main.rand.NextBool(200))
            {
                var player = Main.LocalPlayer;
                if (player.inventory.Any(x => x.type == ModContent.ItemType<DiscoCrystal>()))
                {
                    var dust = Dust.NewDustPerfect(new Vector2(i * 16, j * 16), DustID.RedTorch, null);
                }
            }

            base.EmitParticles(i, j, tile, tileFrameX, tileFrameY, tileLight, visible);
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
            int frameYOffset = 0;

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

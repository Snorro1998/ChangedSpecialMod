using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ChangedSpecialMod.Content.Tiles
{
    public class MusicBoxHappyBirthdayTile : BaseMusicBoxTile
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.tileLighted[Type] = true;
        }
        public override string Texture => "ChangedSpecialMod/Content/Tiles/MusicBoxes/MusicBoxHappyBirthdayTile";
        public override int CursorItemIconID => ModContent.ItemType<MusicBoxHappyBirthday>();

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].TileFrameX >= 36)
            {
                r = 1f;
                g = 1f;
                b = 1f;
            }
            else
            {
                r = 0f;
                g = 0f;
                b = 0f;
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            base.PostDraw(i, j, spriteBatch);

            var tile = Main.tile[i, j];

            if (tile.TileFrameX < 54 || tile.TileFrameY != 0)
                return;

            Texture2D texture = Mod.Assets.Request<Texture2D>("Assets/Textures/Dust/Torch").Value;

            for (int k = 0; k < 3; k++)
            {
                spriteBatch.Draw(
                    texture,
                    new Vector2(i * 16 - Main.screenPosition.X + k * 8 - 13, j * 16 - Main.screenPosition.Y + 5) + ChangedUtils.TileDrawOffset,
                    new Rectangle(0, ChangedUtils.Choose(0, 10, 20), 10, 10),
                    Color.White
                );
            }
        }
    }
}
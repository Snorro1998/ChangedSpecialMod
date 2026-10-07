using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items
{
    public class QuestBook : ModItem
    {
        private string textureName = "QuestBook";

        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
            Item.SetNameOverride(BookQuestSystem.GetBookName());
            textureName = ChangedUtils.Choose("QuestBook", "QuestBook1", "QuestBook2", "QuestBook3", "QuestBook4");
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            try
            {
                Texture2D texture = ChangedSpecialMod.Instance.Assets.Request<Texture2D>($"Content/Items/{textureName}").Value;
                var itemFrame = texture.Frame();
                spriteBatch.Draw(texture, position, itemFrame, Color.White, 0f, origin, scale, SpriteEffects.None, 0);
                return false;
            }
            catch
            {
                return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            }
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            try
            {
                Texture2D texture = ChangedSpecialMod.Instance.Assets.Request<Texture2D>($"Content/Items/{textureName}").Value;
                var itemFrame = texture.Frame();

                Vector2 drawOrigin = itemFrame.Size() / 2f;
                Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, drawOrigin.Y);

                spriteBatch.Draw(
                    texture,
                    drawPosition,
                    null,
                    lightColor,
                    rotation,
                    drawOrigin,
                    scale,
                    SpriteEffects.None,
                    0f
                );

                return false;
            }
            catch
            {
                return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
            }
        }
    }
}

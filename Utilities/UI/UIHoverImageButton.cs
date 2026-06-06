using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace ChangedSpecialMod.Utilities.UI
{
    internal class UIHoverImageButton : UIImageButton
    {
        internal string hoverText;
        private Asset<Texture2D> _tx;

        public UIHoverImageButton(Asset<Texture2D> texture, string hoverText) : base(texture)
        {
            this.hoverText = hoverText;
            _tx = texture;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            spriteBatch.Draw(_tx.Value, dimensions.Position(), Color.White);
            base.DrawSelf(spriteBatch);

            if (IsMouseHovering)
            {
                UICommon.TooltipMouseText(hoverText);
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace ChangedSpecialMod.Utilities.UI.TransfurUI
{
    // This ExampleUIHoverImageButton class inherits from UIImageButton.
    // Inheriting is a great tool for UI design.
    // By inheriting, we get the Image drawing, MouseOver sound, and fading for free from UIImageButton
    // We've added some code to allow the Button to show a text tooltip while hovered
    internal class UIHoverImageButton : UIImageButton
    {
        // Tooltip text that will be shown on hover
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
            //if (_borderTexture != null && base.IsMouseHovering)
             //   spriteBatch.Draw(_borderTexture.Value, dimensions.Position(), Color.White);
            // When you override UIElement methods, don't forget call the base method
            // This helps to keep the basic behavior of the UIElement
            base.DrawSelf(spriteBatch);

            // IsMouseHovering becomes true when the mouse hovers over the current UIElement
            if (IsMouseHovering)
            {
                // Show the tooltip when hovered.
                UICommon.TooltipMouseText(hoverText);
                // Another option is "Main.hoverItemName = hoverText;". Read the docs for information on the slight behavioral differences.
            }
        }
    }
}

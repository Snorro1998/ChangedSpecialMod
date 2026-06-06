using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using Terraria.UI;
using Terraria.UI;


namespace ChangedSpecialMod.Common.Configs
{
    class CornerElement : ConfigElement
    {
        //Texture2D circleTexture;
        //string[] valueStrings;

        public override void OnBind()
        {
            base.OnBind();
            //circleTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Toggle", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            //valueStrings = Enum.GetNames(MemberInfo.Type);
            //TextDisplayFunction = () => Label + ": " + GetStringValue();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //CalculatedStyle dimensions = GetDimensions();
            //var circleSourceRectangle = new Rectangle(0, 0, (circleTexture.Width - 2) / 2, circleTexture.Height);
            //spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)(dimensions.X + dimensions.Width - 25), (int)(dimensions.Y + 4), 22, 22), Color.LightGreen);
            //Corner corner = GetValue();
            //var circlePositionOffset = new Vector2((int)corner % 2 * 8, (int)corner / 2 * 8);
            //spriteBatch.Draw(circleTexture, new Vector2(dimensions.X + dimensions.Width - 25, dimensions.Y + 4) + circlePositionOffset, circleSourceRectangle, Color.White);
        }
    }
}

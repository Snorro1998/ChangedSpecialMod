using ChangedSpecialMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Biomes
{
	public class CityRuinsWaterStyle : ModWaterStyle
	{
		public override int ChooseWaterfallStyle() {
			return ModContent.GetInstance<CityRuinsWaterfallStyle>().Slot;
		}

		public override int GetSplashDust() {
            return ModContent.DustType<ChangedSolution>();
        }

		public override int GetDropletGore() {
			return Terraria.ID.GoreID.WaterDrip;
        }

		public override void LightColorMultiplier(ref float r, ref float g, ref float b) {
			r = 1f;
			g = 1f;
			b = 1f;
		}

		public override Color BiomeHairColor() {
			return Color.White;
		}

		public override byte GetRainVariant() {
			return (byte)Main.rand.Next(3);
		}

		public override Asset<Texture2D> GetRainTexture() {
			return ModContent.Request<Texture2D>("ChangedSpecialMod/Content/Biomes/CityRuinsRain");
		}
	}
}
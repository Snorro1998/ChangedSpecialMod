using Terraria.ModLoader;

namespace ChangedSpecialMod.Backgrounds
{
	public class CityRuinsUndergroundBackgroundStyle : ModUndergroundBackgroundStyle
	{
		public override void FillTextureArray(int[] textureSlots) {
			textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/CityRuinsBiomeUnderground0");
			textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/CityRuinsBiomeUnderground1");
			textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/CityRuinsBiomeUnderground2");
			textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/CityRuinsBiomeUnderground3");
		}
	}
}
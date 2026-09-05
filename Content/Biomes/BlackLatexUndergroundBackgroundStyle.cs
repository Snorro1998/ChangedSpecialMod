using Terraria.ModLoader;

namespace ChangedSpecialMod.Backgrounds
{
	public class BlackLatexUndergroundBackgroundStyle : ModUndergroundBackgroundStyle
	{
        public override void FillTextureArray(int[] textureSlots)
        {
			textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/BlackLatexUnderground/0");
			textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/BlackLatexUnderground/1");
			textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/BlackLatexUnderground/2");
			textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/BlackLatexUnderground/3");
			textureSlots[4] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/BlackLatexUnderground/4");
        }
    }
}
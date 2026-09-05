using Terraria.ModLoader;

namespace ChangedSpecialMod.Backgrounds
{
	public class WhiteLatexUndergroundBackgroundStyle : ModUndergroundBackgroundStyle
	{
        public override void FillTextureArray(int[] textureSlots)
        {
			textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/WhiteLatexUnderground/0");
			textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/WhiteLatexUnderground/1");
			textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/WhiteLatexUnderground/2");
			textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/WhiteLatexUnderground/3");
			textureSlots[4] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/WhiteLatexUnderground/4");
        }
    }
}
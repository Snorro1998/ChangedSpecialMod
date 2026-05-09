using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Furniture.Paintings
{
	public class Painting5 : BasePainting3X3
    {
        public override string Texture => "ChangedSpecialMod/Content/Tiles/Furniture/Paintings/Painting5";

        public override bool RightClick(int i, int j)
        {
            SoundEngine.PlaySound(Assets.Sounds.SoundHahaha);
            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.Painting5>();
        }
    }
}

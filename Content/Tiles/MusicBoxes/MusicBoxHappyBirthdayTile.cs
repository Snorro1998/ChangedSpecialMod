using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles
{
    public class MusicBoxHappyBirthdayTile : BaseMusicBoxTile
    {
        public override string Texture => "ChangedSpecialMod/Content/Tiles/MusicBoxes/MusicBoxHappyBirthdayTile";
        public override int CursorItemIconID => ModContent.ItemType<MusicBoxHappyBirthday>();
    }
}
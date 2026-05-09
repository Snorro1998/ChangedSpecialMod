using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles
{
    public class MusicBoxGreenhouseTile : BaseMusicBoxTile
    {
        public override string Texture => "ChangedSpecialMod/Content/Tiles/MusicBoxes/MusicBoxGreenhouseTile";
        public override int CursorItemIconID => ModContent.ItemType<MusicBoxGreenhouse>();
    }
}
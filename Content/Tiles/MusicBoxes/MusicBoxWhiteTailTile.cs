using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles
{
    public class MusicBoxWhiteTailTile : BaseMusicBoxTile
    {
        public override string Texture => "ChangedSpecialMod/Content/Tiles/MusicBoxes/MusicBoxWhiteTailTile";
        public override int CursorItemIconID => ModContent.ItemType<MusicBoxWhiteTail>();
    }
}
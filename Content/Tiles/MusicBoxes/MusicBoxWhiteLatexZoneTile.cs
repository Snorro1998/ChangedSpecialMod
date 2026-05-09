using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles
{
    public class MusicBoxWhiteLatexZoneTile : BaseMusicBoxTile
    {
        public override string Texture => "ChangedSpecialMod/Content/Tiles/MusicBoxes/MusicBoxWhiteLatexZoneTile";
        public override int CursorItemIconID => ModContent.ItemType<MusicBoxWhiteLatexZone>();
    }
}
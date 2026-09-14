using ChangedSpecialMod.Content.Items.Placeable.Furniture.MusicBox;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.MusicBoxes
{
    public class MusicBoxWhiteLatexZoneTile : BaseMusicBoxTile
    {
        public override string Texture => "ChangedSpecialMod/Content/Tiles/MusicBoxes/MusicBoxWhiteLatexZoneTile";
        public override int CursorItemIconID => ModContent.ItemType<MusicBoxWhiteLatexZone>();
    }
}
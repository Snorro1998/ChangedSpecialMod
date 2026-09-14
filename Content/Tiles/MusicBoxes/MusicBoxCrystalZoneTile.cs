using ChangedSpecialMod.Content.Items.Placeable.Furniture.MusicBox;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.MusicBoxes
{
    public class MusicBoxCrystalZoneTile : BaseMusicBoxTile
    {
        public override string Texture => "ChangedSpecialMod/Content/Tiles/MusicBoxes/MusicBoxCrystalZoneTile";
        public override int CursorItemIconID => ModContent.ItemType<MusicBoxCrystalZone>();
    }
}
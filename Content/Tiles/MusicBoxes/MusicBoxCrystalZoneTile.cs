using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles
{
    public class MusicBoxCrystalZoneTile : BaseMusicBoxTile
    {
        public override string Texture => "ChangedSpecialMod/Content/Tiles/MusicBoxes/MusicBoxCrystalZoneTile";
        public override int CursorItemIconID => ModContent.ItemType<MusicBoxCrystalZone>();
    }
}
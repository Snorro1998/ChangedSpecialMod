using ChangedSpecialMod.Content.Items.Placeable.Furniture.MusicBox;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.MusicBoxes
{
    public class MusicBoxVentsTile : BaseMusicBoxTile
    {
        public override string Texture => "ChangedSpecialMod/Content/Tiles/MusicBoxes/MusicBoxVentsTile";
        public override int CursorItemIconID => ModContent.ItemType<MusicBoxVents>();
    }
}
using ChangedSpecialMod.Content.Items.Placeable.Furniture.MusicBox;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.MusicBoxes
{
    public class MusicBoxPuroDanceTile : BaseMusicBoxTile
    {
        public override string Texture => "ChangedSpecialMod/Content/Tiles/MusicBoxes/MusicBoxPuroDanceTile";
        public override int CursorItemIconID => ModContent.ItemType<MusicBoxPuroDance>();
    }
}
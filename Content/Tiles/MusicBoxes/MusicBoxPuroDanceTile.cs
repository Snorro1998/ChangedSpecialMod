using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles
{
    public class MusicBoxPuroDanceTile : BaseMusicBoxTile
    {
        public override string Texture => "ChangedSpecialMod/Content/Tiles/MusicBoxes/MusicBoxPuroDanceTile";
        public override int CursorItemIconID => ModContent.ItemType<MusicBoxPuroDance>();
    }
}
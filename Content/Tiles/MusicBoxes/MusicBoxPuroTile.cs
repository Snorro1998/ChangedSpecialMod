using ChangedSpecialMod.Content.Items.Placeable.Furniture.MusicBox;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.MusicBoxes
{
    public class MusicBoxPuroTile : BaseMusicBoxTile
    {
        public override string Texture => "ChangedSpecialMod/Content/Tiles/MusicBoxes/MusicBoxPuroTile";
        public override int CursorItemIconID => ModContent.ItemType<MusicBoxPuro>();
    }
}
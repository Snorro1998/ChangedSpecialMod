using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles
{
    public class MusicBoxWolfKingTile : BaseMusicBoxTile
    {
        public override string Texture => "ChangedSpecialMod/Content/Tiles/MusicBoxes/MusicBoxWolfKingTile";
        public override int CursorItemIconID => ModContent.ItemType<MusicBoxWolfKing>();
    }
}
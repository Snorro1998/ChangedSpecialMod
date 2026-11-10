using ChangedSpecialMod.Content.Items.Placeable.Furniture.MusicBox;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.MusicBoxes
{
    public class MusicBoxGasTile : BaseMusicBoxTile
    {
        public override string Texture => "ChangedSpecialMod/Content/Tiles/MusicBoxes/MusicBoxGasTile";
        public override int CursorItemIconID => ModContent.ItemType<MusicBoxWolfKing>();

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Main.gameInactive || Main.gamePaused)
                return;

            var tile = Framing.GetTileSafely(i, j);
            var coords = TileObjectData.TopLeft(i, j);

            if (coords == Point16.NegativeOne)
                return;

            var topLeftTile = Framing.GetTileSafely(coords);
            if (topLeftTile.TileFrameX >= 36 && Main.rand.NextBool(60))
            {
                var xPos = -8;
                if (Main.rand.NextBool(2))
                    xPos += 16;
                var particlePos = coords.ToWorldCoordinates(xPos, -16);
                var particleType = Utils.SelectRandom(Main.rand, 11, 12, 13);
                var particle = Gore.NewGorePerfect(new EntitySource_TileUpdate(coords.X, coords.Y), particlePos, new Vector2(0, -1), particleType, Main.rand.NextFloat(0.1f, 0.9f));
            }
        }
    }
}
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
    public class PuroStatue : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileSpelunker[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IsAMechanism[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);

            DustType = DustID.Silver;

            AddMapEntry(new Color(144, 148, 144), Language.GetText("MapObject.Statue"));
        }

        public override void HitWire(int i, int j)
        {
            (int x, int y) = TileObjectData.TopLeft(i, j);

            const int TileWidth = 2;
            const int TileHeight = 3;

            for (int yy = y; yy < y + TileHeight; yy++)
            {
                for (int xx = x; xx < x + TileWidth; xx++)
                {
                    Wiring.SkipWire(xx, yy);
                }
            }

            float spawnX = (x + TileWidth * 0.5f);
            float spawnY = (y + TileHeight * 0.65f);
            var entitySource = new EntitySource_TileUpdate(x, y, context: "OrangeStatue");
            ChangedUtils.SpawnOranges(entitySource, null, (int)spawnX, (int)spawnY);
        }
    }
}

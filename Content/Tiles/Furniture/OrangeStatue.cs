using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
    // ExampleStatue shows off correctly using wiring to spawn items and NPC.
    // See StatueWorldGen to see how ExampleStatue is added as an option for naturally spawning statues during worldgen.
    public class OrangeStatue : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileSpelunker[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IsAMechanism[Type] = true; // Ensures that this tile and connected pressure plate won't be removed during the "Remove Broken Traps" worldgen step

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);

            DustType = DustID.Silver;

            AddMapEntry(new Color(144, 148, 144), Language.GetText("MapObject.Statue"));
        }

        // This hook allows you to make anything happen when this statue is powered by wiring.
        // In this example, powering the statue either spawns a random coin with a 95% chance, or, with a 5% chance - a goldfish.
        public override void HitWire(int i, int j)
        {
            // Find the coordinates of top left tile square
            (int x, int y) = TileObjectData.TopLeft(i, j);

            const int TileWidth = 2;
            const int TileHeight = 3;

            // Here we call SkipWire on all tile coordinates covered by this tile. This ensures a wire signal won't run multiple times.
            for (int yy = y; yy < y + TileHeight; yy++)
            {
                for (int xx = x; xx < x + TileWidth; xx++)
                {
                    Wiring.SkipWire(xx, yy);
                }
            }

            // Calculate the center of this tile to use as an entity spawning position.
            // Note that we use 0.65 for height because even though the statue takes 3 blocks, its appearance is shorter.
            float spawnX = (x + TileWidth * 0.5f);
            float spawnY = (y + TileHeight * 0.65f);

            var entitySource = new EntitySource_TileUpdate(x, y, context: "OrangeStatue");
            ChangedUtils.SpawnOranges(entitySource, null, (int)spawnX, (int)spawnY);
        }
    }
}

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace ChangedSpecialMod.Content.NPCs.AIStates.Passive
{
    public static class StateTryWaterPlants
    {
        private static List<int> PlantList = new List<int>
        {
            TileID.Saplings,
            TileID.Pumpkins,
            TileID.Sunflower,
            TileID.ImmatureHerbs,
            TileID.MatureHerbs,
            TileID.BloomingHerbs,
            TileID.PottedPlants1,
            TileID.PottedPlants2,
            TileID.PottedCrystalPlants,
            TileID.PottedLavaPlants,
            TileID.PottedLavaPlantTendrils,
        };

        private static bool ShouldWater(int tileType)
        {
            if (tileType < 0 || (tileType >= TileID.Sets.TreeSapling.Length || tileType >= TileID.Sets.CommonSapling.Length))
                return false;

            var isTreeSapling = TileID.Sets.TreeSapling[tileType];
            var isCommonSapling = TileID.Sets.CommonSapling[tileType];
            return PlantList.Contains(tileType) || isTreeSapling || isCommonSapling;
        }

        public static void Update(NPC npc)
        {
            Point point = (npc.Bottom + Vector2.UnitX * npc.direction * 24 + Vector2.UnitY * -2f).ToTileCoordinates();
            bool isPlant = WorldGen.InWorld(point.X, point.Y, 1);
            if (isPlant)
            {
                Tile checkTile = Main.tile[point.X, point.Y];
                isPlant = ShouldWater((int)checkTile.TileType);

                if (isPlant)
                {
                    var wateringTime = Main.rand.Next(1, 3) * 120;
                    npc.ai[0] = 30f;
                    npc.ai[1] = wateringTime;
                    npc.netUpdate = true;

                    bool growSuccess = WorldGen.GrowTree(point.X, point.Y);
                    bool isPlayerNear = WorldGen.PlayerLOS(point.X, point.Y);

                    // If growing the tree was a success and the player is near, show growing effects
                    if (growSuccess && isPlayerNear)
                    {
                        WorldGen.TreeGrowFXCheck(point.X, point.Y);
                    }
                }
            }
        }
    }
}

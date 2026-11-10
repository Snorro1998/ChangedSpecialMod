using ChangedSpecialMod.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;

namespace ChangedSpecialMod.Common.WorldGeneration
{
    public enum WorldRegionType
    {
        Sky,

        Corruption,
        Crimson,
        Hell,

        Forest, // Purity
        Desert,
        Jungle,
        Snow,

        LivingTree,
        Dungeon,
        Structure,

        Shimmer,
        Mushroom,
        Granite,
        Marble,
        Beehive
    }

    public class RegionObject
    {
        public Rectangle area;
        public WorldRegionType regionType;
        public bool surfaceLayer = false;
        public List<RegionObject> labyrinthConnections = new List<RegionObject>();

        public RegionObject(Rectangle area)
        {
            this.area = area;
            SetRegionType();
        }

        private static readonly List<int> forestTiles = new()
            {
                TileID.Grass,
                TileID.Dirt,
                TileID.Stone
            };

        private static readonly List<int> snowTiles = new()
            {
                TileID.SnowBlock,
                TileID.IceBlock
            };

        private static readonly List<int> JungleTiles = new()
            {
                TileID.JungleGrass,
                TileID.Mud
            };

        private static readonly List<int> DesertTiles = new()
            {
                TileID.Sand,
                TileID.HardenedSand,
                TileID.Sandstone
            };

        private static readonly List<int> MushroomTiles = new()
            {
                TileID.MushroomGrass
            };

        private static readonly List<int> CorruptTiles = new()
            {
                TileID.Ebonstone,
                TileID.Ebonsand,
                TileID.CorruptGrass
            };

        private static readonly List<int> CrimsonTiles = new()
        {
            TileID.Crimstone,
            TileID.Crimsand,
            TileID.CrimsonGrass
        };

        private static readonly List<int> HellTiles = new()
        {
            TileID.Ash
        };

        private static readonly List<int> dungeonTiles = new()
        {
            TileID.BlueDungeonBrick,
            TileID.GreenDungeonBrick,
            TileID.PinkDungeonBrick
        };

        private static readonly List<int> livingTreeTiles = new()
        {
            TileID.LivingWood,
            TileID.LeafBlock,
            TileID.LivingMahogany,
            TileID.LivingMahoganyLeaves
        };

        private static readonly List<int> graniteTiles = new()
        {
            TileID.Granite
        };

        private static readonly List<int> marbleTiles = new()
        {
            TileID.Marble
        };

        private static readonly List<int> hiveTiles = new()
        {
            TileID.Hive
        };

        private static readonly List<int> structureTiles = new()
        {
            // Chests
            TileID.Containers,
            TileID.Containers2,

            // Pyramid
            TileID.SandstoneBrick,

            // Temple
            TileID.LihzahrdBrick,

            // Not used by vanilla, but often used by other mods for buildings
            TileID.WoodBlock,
            TileID.GrayBrick,
            TileID.StoneSlab,
            TileID.BlueDynastyShingles,
            TileID.RedDynastyShingles,
            TileID.RichMahogany,
            TileID.BorealWood,

            TileID.IceBrick
        };

        public bool IsNonStructureRegion()
        {
            return regionType != WorldRegionType.Dungeon &&
                regionType != WorldRegionType.LivingTree &&
                regionType != WorldRegionType.Beehive &&
                regionType != WorldRegionType.Granite &&
                regionType != WorldRegionType.Marble &&
                regionType != WorldRegionType.Shimmer &&
                regionType != WorldRegionType.Mushroom &&
                regionType != WorldRegionType.Structure;
        }

        public bool IsValidNotSky()
        {
            return IsNonStructureRegion() && regionType != WorldRegionType.Sky;
        }

        public bool IsUnderground()
        {
            return area.Top > Main.rockLayer;
        }

        public void SetRegionType()
        {
            // Make a copy so external mod tiles don't permanently modify structureTiles.
            var allStructureTiles = new HashSet<int>(structureTiles);

            var externalMods = ModSupportSystem.externalMods;
            if (externalMods != null)
            {
                foreach (var mod in externalMods)
                {
                    allStructureTiles.UnionWith(mod.tilesAvoidedByWorldGen);
                }
            }

            if (ModSupportSystem.AvoidTiles != null)
            {
                foreach (var tileType in ModSupportSystem.AvoidTiles)
                    allStructureTiles.Add(tileType);
            }

            var dungeonCount = 0;
            var livingTreeCount = 0;
            var structureCount = 0;

            var mushroomCount = 0;
            var graniteCount = 0;
            var marbleCount = 0;
            var hiveCount = 0;

            var corruptCount = 0;
            var crimsonCount = 0;
            var hellCount = 0;

            var forestCount = 0;
            var snowCount = 0;
            var jungleCount = 0;
            var desertCount = 0;

            var xCenter = area.Left + area.Width / 2;
            var yCenter = area.Top + area.Height / 2;
            var areaCenter = new Vector2(xCenter, yCenter);
            var shimPos = GenVars.shimmerPosition;
            var shimmerPosition = new Vector2((float)shimPos.X, (float)shimPos.Y);

            if (Vector2.DistanceSquared(areaCenter, shimmerPosition) < 120 * 120)
            {
                regionType = WorldRegionType.Shimmer;
                return;
            }

            var dungeonPos = GenVars.dungeonLocation;
            var dungeonPosition = new Vector2((float)shimPos.X, (float)shimPos.Y);

            if (Vector2.DistanceSquared(areaCenter, dungeonPosition) < 120 * 120)
            {
                regionType = WorldRegionType.Dungeon;
                return;
            }

            // TODO add ocean region
            if (area.Left < GenVars.leftBeachEnd || area.Right > GenVars.rightBeachStart)
            {
                regionType = WorldRegionType.Structure;
                return;
            }

            // Scan every tile in the region.
            for (var x = area.Left; x < area.Right; x++)
            {
                for (var y = area.Top; y < area.Bottom; y++)
                {
                    var tile = Main.tile[x, y];

                    if (!tile.HasTile)
                        continue;

                    var tileType = tile.TileType;

                    if (dungeonTiles.Contains(tileType))
                        dungeonCount++;
                    else if (livingTreeTiles.Contains(tileType))
                        livingTreeCount++;
                    else if (allStructureTiles.Contains(tileType))
                        structureCount++;

                    else if (CorruptTiles.Contains(tileType))
                        corruptCount++;
                    else if (CrimsonTiles.Contains(tileType))
                        crimsonCount++;
                    else if (HellTiles.Contains(tileType))
                        hellCount++;

                    else if (MushroomTiles.Contains(tileType))
                        mushroomCount++;
                    else if (graniteTiles.Contains(tileType))
                        graniteCount++;
                    else if (marbleTiles.Contains(tileType))
                        marbleCount++;
                    else if (hiveTiles.Contains(tileType))
                        hiveCount++;

                    else if (forestTiles.Contains(tileType))
                        forestCount++;
                    else if (snowTiles.Contains(tileType))
                        snowCount++;
                    else if (JungleTiles.Contains(tileType))
                        jungleCount++;
                    else if (DesertTiles.Contains(tileType))
                        desertCount++;

                }
            }

            // Use Forest as the default when no special category was found.
            regionType = WorldRegionType.Sky;

            // Structures have the highest priority
            if (dungeonCount > 0)
                regionType = WorldRegionType.Dungeon;
            else if (structureCount > 0)
                regionType = WorldRegionType.Structure;
            else if (livingTreeCount > 0)
                regionType = WorldRegionType.LivingTree;

            // Mini biomes
            else if (hiveCount > 0)
                regionType = WorldRegionType.Beehive;
            else if (mushroomCount > 0)
                regionType = WorldRegionType.Mushroom;
            else if (graniteCount > 0)
                regionType = WorldRegionType.Granite;
            else if (marbleCount > 0)
                regionType = WorldRegionType.Marble;

            // Hell
            else if (hellCount > 0)
                regionType = WorldRegionType.Hell;

            // World evil
            else if (corruptCount > 0)
                regionType = WorldRegionType.Corruption;
            else if (crimsonCount > 0)
                regionType = WorldRegionType.Crimson;

            // For normal biomes, check which is the most present
            else if (jungleCount > 0 && jungleCount > desertCount && jungleCount > snowCount && jungleCount > forestCount)
                regionType = WorldRegionType.Jungle;
            else if (desertCount > 0 && desertCount > snowCount && desertCount > forestCount)
                regionType = WorldRegionType.Desert;
            else if (snowCount > 0 && snowCount > forestCount)
                regionType = WorldRegionType.Snow;
            else if (forestCount > 0)
                regionType = WorldRegionType.Forest;
        }

        public void DebugMarkBorders()
        {
            var tileType = TileID.Chlorophyte;

            if (regionType == WorldRegionType.Dungeon ||
                regionType == WorldRegionType.Structure)
            {
                tileType = TileID.Adamantite;
            }
            else if (regionType == WorldRegionType.Shimmer)
                tileType = TileID.ShimmerBlock;
            else if (regionType == WorldRegionType.LivingTree)
                tileType = TileID.WoodBlock;

            else if (regionType == WorldRegionType.Beehive)
                tileType = TileID.Hive;
            else if (regionType == WorldRegionType.Mushroom)
                tileType = TileID.MushroomGrass;
            else if (regionType == WorldRegionType.Granite)
                tileType = TileID.GraniteBlock;
            else if (regionType == WorldRegionType.Marble)
                tileType = TileID.MarbleBlock;

            else if (regionType == WorldRegionType.Hell)
                tileType = TileID.HellstoneBrick;
            else if (regionType == WorldRegionType.Corruption)
                tileType = TileID.Ebonstone;
            else if (regionType == WorldRegionType.Crimson)
                tileType = TileID.Crimstone;

            else if (regionType == WorldRegionType.Jungle)
                tileType = TileID.JungleGrass;
            else if (regionType == WorldRegionType.Desert)
                tileType = TileID.HardenedSand;
            else if (regionType == WorldRegionType.Snow)
                tileType = TileID.IceBlock;
            else if (regionType == WorldRegionType.Forest)
                tileType = TileID.Grass;

            if (surfaceLayer)
                tileType = TileID.GoldBrick;

            // Mark the region border for debugging.
            for (var tileX = area.Left; tileX < area.Right; tileX++)
            {
                DebugMarkBorderTiles(tileX, area.Top, tileType);
                DebugMarkBorderTiles(tileX, area.Bottom - 1, tileType);
            }

            for (var tileY = area.Top; tileY < area.Bottom; tileY++)
            {
                DebugMarkBorderTiles(area.Left, tileY, tileType);
                DebugMarkBorderTiles(area.Right - 1, tileY, tileType);
            }
        }

        public void DebugMarkBorderTiles(int x, int y, int tileType)
        {
            var tile = Main.tile[x, y];

            // Only replace solid tiles so the debug border doesn't create new blocks.
            if (tile.HasTile && Main.tileSolid[tile.TileType])
            {
                tile.TileType = (ushort)tileType;
            }
        }
    }
}

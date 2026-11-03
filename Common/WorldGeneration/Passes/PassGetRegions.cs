using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.WorldBuilding;

namespace ChangedSpecialMod.Common.WorldGeneration.Passes
{
    public class PassGetRegions : WorldGenerationPass
    {
        private int regionSize = 50; // 30

        public PassGetRegions()
        {
            PassName = "ChangedAnalyzeWorld";
            GenerationMessage = "GetRegions"; //Finding suitable places for structures
        }

        public override bool Enabled()
        {
            var config = ChangedSpecialModClientConfig.Instance;
            return config.WorldGenStepOrangeShrine || config.WorldGenStepLatexCaves || config.WorldGenStepSurfaceLabs;
        }

        public override void Generate()
        {
            // Mark surface regions
            var nX = (int)Math.Ceiling((float)Main.maxTilesX / (float)regionSize);
            var nY = (int)Math.Ceiling((float)Main.maxTilesY / (float)regionSize);

            WorldMaker.worldRegionsNew = new RegionObject[nX, nY];

            for (var x2 = 0; x2 < nX; x2++)
            {
                for (var y2 = 0; y2 < nY; y2++)
                {
                    // Convert region coordinates to tile coordinates.
                    int x = x2 * regionSize;
                    int y = y2 * regionSize;

                    // Regions at the edge of the world may be smaller.
                    int width = Math.Min(regionSize, Main.maxTilesX - x);
                    int height = Math.Min(regionSize, Main.maxTilesY - y);

                    var area = new Rectangle(x, y, width, height);
                    var region = new RegionObject(area);
                    WorldMaker.worldRegionsNew[x2, y2] = region;
                }
            }

            // Mark surface regions
            for (int x2 = 0; x2 < nX; x2++)
            {
                for (int y2 = 1; y2 < nY; y2++)
                {
                    var region = WorldMaker.worldRegionsNew[x2, y2];
                    var regionAbove = WorldMaker.worldRegionsNew[x2, y2 - 1];

                    if (region.regionType != WorldRegionType.Structure &&
                    region.area.Top >= (int)GenVars.worldSurfaceLow &&
                    regionAbove.regionType == WorldRegionType.Sky
                    )
                    {
                        region.surfaceLayer = true;
                    }
                }
            }

            /*
            for (int x2 = 0; x2 < nX; x2++)
            {
                for (int y2 = 0; y2 < nY; y2++)
                {
                    WorldMaker.worldRegionsNew[x2, y2].DebugMarkBorders();
                }
            }
            */
        }
    }
}

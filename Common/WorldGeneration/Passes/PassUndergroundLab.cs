using ChangedSpecialMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;

namespace ChangedSpecialMod.Common.WorldGeneration.Passes
{
    public class PassUndergroundLab : WorldGenerationPass
    {
        public PassUndergroundLab()
        {
            PassName = "ChangedUndergroundLab";
            GenerationMessage = "UndergroundLab";
        }

        public override bool Enabled()
        {
            return true;
        }

        public override void Generate()
        {
            List<(int, int)> placedPositions = new List<(int, int)>();

            // World is divided in 50x50 regions
            var regions = WorldMaker.worldRegionsNew;
            var nRegionsX = regions.GetLength(0);
            var nRegionsY = regions.GetLength(1);

            var cell1 = WorldMaker.StructureLabRoom1;
            var cell2 = WorldMaker.StructureLabRoom2;
            var cell3 = WorldMaker.StructureLabRoomLibrary;
            var cell4 = WorldMaker.StructureLabRoomWater;
            var cell5 = WorldMaker.StructureLabRoomGas;
            var cell6 = WorldMaker.StructureLabRoomVents;

            var nXBuilding = 3;
            var nYBuilding = 2;

            var nFromWorldBottom = 5;

            var maxAttempts = 100;
            
            for (int i = 0; i < maxAttempts; i++)
            {
                var nXMin = 0;
                var nXMax = nRegionsX - nXBuilding;

                var nYMin = nRegionsY / 2;
                var nYMax = nRegionsY - nYBuilding - nFromWorldBottom;

                var xLeftPos = WorldGen.genRand.Next(nXMin, nXMax);
                var yTopPos = WorldGen.genRand.Next(nYMin, nYMax);

                var grid1 = regions[xLeftPos, yTopPos];
                var grid2 = regions[xLeftPos + 1, yTopPos];
                var grid3 = regions[xLeftPos + 2, yTopPos];
                var grid4 = regions[xLeftPos, yTopPos + 1];
                var grid5 = regions[xLeftPos + 1, yTopPos + 1];
                var grid6 = regions[xLeftPos + 2, yTopPos + 1];

                // valid regions
                var valid1 = grid1.IsValidNotSky();
                var valid2 = grid2.IsValidNotSky();
                var valid3 = grid3.IsValidNotSky();
                var valid4 = grid4.IsValidNotSky();
                var valid5 = grid5.IsValidNotSky();
                var valid6 = grid6.IsValidNotSky();

                var isValidPosition = valid1 && valid2 && valid3 && valid4 && valid5 && valid6;

                if (!isValidPosition)
                    continue;

                cell1.BuildAtPosition(ref placedPositions, grid1.area.Left, grid1.area.Top);
                cell2.BuildAtPosition(ref placedPositions, grid2.area.Left, grid2.area.Top);
                cell3.BuildAtPosition(ref placedPositions, grid3.area.Left, grid3.area.Top);
                cell4.BuildAtPosition(ref placedPositions, grid4.area.Left, grid4.area.Top);
                cell5.BuildAtPosition(ref placedPositions, grid5.area.Left, grid5.area.Top);
                cell6.BuildAtPosition(ref placedPositions, grid6.area.Left, grid6.area.Top);

                break;
            }
        }
    }
}

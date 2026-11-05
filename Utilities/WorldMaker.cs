using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Common.WorldGeneration;
using ChangedSpecialMod.Common.WorldGeneration.Passes;
using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Furniture;
using ChangedSpecialMod.Content.Walls;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ChangedSpecialMod.Utilities
{
    public enum LabType
    {
        CityRuins,
        Black,
        White
    }

    public class LabStruct
    {
        public Vector2 Position = Vector2.Zero;
        public LabType labType;
        public Building Lab = null;

        public LabStruct(Vector2 position)
        {
            Position = position;
            labType = LabType.CityRuins;
            Lab = new Building();
        }
    }

    public enum WorldPass
    {
        SpreadingGrass,
        SurfaceOreandStone,
        WaterPlants,
        AfterFinalCleanup
    }

    public class WorldMaker : ModSystem
    {
        //public static List<RegionObject> worldRegions;
        public static RegionObject[,] worldRegionsNew;

        private static Dictionary<WorldPass, string> WorldGenPasses = new Dictionary<WorldPass, string>()
        {
            { WorldPass.SpreadingGrass, "Spreading Grass" },
            { WorldPass.SurfaceOreandStone, "Surface Ore and Stone" },
            // Ancients Awakened generates its biomes before this
            { WorldPass.WaterPlants, "Water Plants" },
            
            // Not a generation pass, but for everything after it
            { WorldPass.AfterFinalCleanup, "After Final Cleanup" },
        };

        private WorldPass highestPass = WorldPass.SurfaceOreandStone;

        public static StructureRuinedHouse StructureOrangeShrine;
        public static StructureRuinedHouse StructureOrangeShrine2;
        public static StructureRuinedHouse StructureLabRoom1;

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            if (ChangedSpecialModClientConfig.Instance.ForceLastGenerationPass)
                return;

            // Spreading Grass
            // [Changed] Analyze world
            // [Changed] Surface labs
            // [Changed] Orange Shrines

            // Final cleanup
            // [Changed] Latex Caves
            // [Changed] Special Seed
            // [Changed] Remove Unwanted Decals

            GetHighestPass();

            var config = ChangedSpecialModClientConfig.Instance;
            var passLabsEnabled = config.WorldGenStepSurfaceLabs;
            var passLatexCavesEnabled = config.WorldGenStepLatexCaves;
            var passOrangeShrinesEnabled = config.WorldGenStepOrangeShrine;
            var anyWorldGenEnabled = passLabsEnabled || passLatexCavesEnabled || passOrangeShrinesEnabled;

            var passName = string.Empty;
            if (WorldGenPasses.ContainsKey(highestPass))
                passName = WorldGenPasses[highestPass];
            int taskIndex = tasks.FindIndex(genpass => genpass.Name.Equals(passName)) - 1; 
            if (taskIndex >= 0)
            {
                var passes = new List<WorldGenerationPass>()
                {
                    new PassGetRegions(),
                    new PassSurfaceLabs(),
                    new PassOrangeShrines()
                };

                foreach (var pass in passes)
                {
                    pass.AddPassIfEnabled(ref tasks, ref taskIndex);
                }
            }

            /*
            taskIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Cactus, Palm Trees, & Coral"));
            if (taskIndex != -1)
            {
                var pass1 = new PassRemoveUnwantedDecals();
                pass1.AddPassIfEnabled(ref tasks, ref taskIndex);
            }
            */

            taskIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
            if (taskIndex != -1)
            {
                var passes = new List<WorldGenerationPass>()
                {
                    new PassLatexCaves()
                };

                var specialPass = GetSpecialSeedPass();
                if (specialPass != null)
                    passes.Add(specialPass);

                passes.Add(new PassRemoveUnwantedDecals());

                foreach (var pass in passes)
                {
                    pass.AddPassIfEnabled(ref tasks, ref taskIndex);
                }
            }
        }

        public override void PostWorldGen()
        {
            if (!ChangedSpecialModClientConfig.Instance.ForceLastGenerationPass)
                return;

            var passes = new List<WorldGenerationPass>()
            {
                new PassGetRegions(),
                new PassSurfaceLabs(),
                new PassOrangeShrines(),
                new PassRemoveUnwantedDecals(),
                new PassLatexCaves()
            };

            var specialPass = GetSpecialSeedPass();
            if (specialPass != null)
                passes.Add(specialPass);

            passes.Add(new PassRemoveUnwantedDecals());

            foreach (var pass in passes)
            {
                pass.GenerateIfEnabled();
            }
        }

        private WorldGenerationPass GetSpecialSeedPass()
        {
            var worldSeedName = WorldGen.currentWorldSeed.ToLower();
            var seedBlackLatex = worldSeedName.Contains("blackgoozone");
            var seedWhiteLatex = worldSeedName.Contains("whitegoojungle");
            var seedDryDirt = worldSeedName.Contains("drydrydirt");
            var seedOrange = worldSeedName.Contains("muchorange");

            if (seedBlackLatex)
                return new PassAddLatexEverywhere(Content.NPCs.GooType.Black);
            else if (seedWhiteLatex)
                return new PassAddLatexEverywhere(Content.NPCs.GooType.White);
            else if (seedDryDirt)
                return new PassAddLatexEverywhere(Content.NPCs.GooType.None);
            else if (seedOrange)
                return new PassPaintWholeWorldOrange(PaintID.DeepOrangePaint);

            return null;
        }

        private void GetHighestPass()
        {
            foreach (var mod in ModSupportSystem.externalMods)
            {
                var tmpPass = mod.VanillaPassAfterBiomeGeneration;
                if (tmpPass > highestPass)
                    highestPass = tmpPass;
            }
        }

        public override void PostSetupContent()
        {
            // Structure init loads image files by using texture.getdata, which has to happen on the main thread
            Main.QueueMainThreadAction(() =>
            {
                StructureOrangeShrine = new StructureRuinedHouse("OrangeShrine");

                Dictionary<Color, int> tileTypes = new Dictionary<Color, int>
                {
                    { Colors2.Red, TileID.MarbleBlock },
                    { Colors2.Green, TileID.Marble },
                    { Colors2.Blue, TileID.MarbleColumn },
                };

                Dictionary<Color, int> wallTypes = new Dictionary<Color, int>
                {
                    { Colors2.Green, WallID.MarbleBlock }
                };

                StructureOrangeShrine2 = new StructureRuinedHouse("Shrine", tileTypes, wallTypes);

                tileTypes = new Dictionary<Color, int>
                {
                    { Colors2.Green, ModContent.TileType<Lab_TileTile>() },
                    { Colors2.Blue, TileID.Platforms },
                };

                wallTypes = new Dictionary<Color, int>
                {
                    { Colors2.Green, ModContent.WallType<LabWall>() }
                };

                Dictionary<Color, int> furnitureTypes = new Dictionary<Color, int>
                {
                    { Colors2.Yellow, ModContent.TileType<LabDoorATile>() },
                    { Colors2.Blue, ModContent.TileType<Content.Tiles.Furniture.Elevator>() },
                    { Colors2.Purple, ModContent.TileType<FaultyLightTile>() },
                };

                StructureLabRoom1 = new StructureRuinedHouse("LabRoom1", tileTypes, wallTypes, furnitureTypes);
            });
        }
    }
}

using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Common.WorldGeneration;
using ChangedSpecialMod.Common.WorldGeneration.Passes;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Furniture;
using ChangedSpecialMod.Content.Tiles.Furniture.Paintings;
using ChangedSpecialMod.Content.Tiles.Latex.Black;
using ChangedSpecialMod.Content.Walls;
using Microsoft.Xna.Framework;
using System;
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

        public LabStruct(Vector2 position, bool hasBasementDoor = false)
        {
            Position = position;
            labType = LabType.CityRuins;
            Lab = new Building(hasBasementDoor);
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
        public static StructureRuinedHouse StructureLabRoom2;
        public static StructureRuinedHouse StructureLabRoomLibrary;
        public static StructureRuinedHouse StructureLabRoomVents;
        public static StructureRuinedHouse StructureLabRoomGas;
        public static StructureRuinedHouse StructureLabRoomWater;

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

                passes.Add(new PassUndergroundLab());
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
                new PassLatexCaves(),
                new PassUndergroundLab(),
                new PassRemoveUnwantedDecals()
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
            try
            {
                // Structure init loads image files by using texture.getdata, which has to happen on the main thread
                Main.QueueMainThreadAction(() =>
                {
                    StructureOrangeShrine = new StructureRuinedHouse("OrangeShrine");

                    Dictionary<Color, int> tileTypes = new Dictionary<Color, int>
                    {
                        { Colors2.Green, TileID.Marble },
                        { Colors2.Blue, TileID.MarbleColumn },
                        { Colors2.Red, TileID.MarbleBlock },
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
                        { Colors2.Red, TileID.Spikes },

                        { Colors2.Purple, TileID.FrozenSlimeBlock },
                        { Colors2.BlackLatex, ModContent.TileType<BlackLatexTile>() },
                    };

                    wallTypes = new Dictionary<Color, int>
                    {
                        { Colors2.Green, ModContent.WallType<LabWall>() },
                        { Colors2.Yellow, ModContent.WallType<LabCrystalRoomWall>() },
                        { Colors2.Red, ModContent.WallType<LabLibraryRoomWall>() },
                        { Colors2.Purple, ModContent.WallType<LabVentsRoomWall>() },
                        { Colors2.Purple2, ModContent.WallType<LabWaterRoomWall>() },

                        { Colors2.Blue, ModContent.WallType<LabGasRoomWall>() },
                        { Colors2.Blue2, WallID.Waterfall },

                    };

                    Dictionary<Color, int> furnitureTypes = new Dictionary<Color, int>
                    {
                        { Colors2.Yellow, ModContent.TileType<LabDoorATile>() },
                        { Colors2.Yellow2, ModContent.TileType<LabDoorBTile>() },
                        { Colors2.Yellow3, ModContent.TileType<LabDoorCTile>() },
                        { Colors2.Yellow4, ModContent.TileType<LabDoorDTile>() },
                        { Colors2.Yellow5, ModContent.TileType<LabDoorETile>() },
                        { Colors2.Yellow6, ModContent.TileType<LabDoorFTile>() },
                        { Colors2.Yellow7, ModContent.TileType<LabDoorGTile>() },
                        { Colors2.Yellow8, ModContent.TileType<LabDoorHTile>() },
                        { Colors2.Yellow9, ModContent.TileType<LabDoorITile>() },
                        { Colors2.Yellow10, ModContent.TileType<LabDoorJTile>() },
                        { Colors2.Yellow11, ModContent.TileType<LabDoorKTile>() },
                        { Colors2.Yellow12, ModContent.TileType<LabDoorLTile>() },

                        { Colors2.Red, ModContent.TileType<RedGasTank>() },
                        { Colors2.Red2, ModContent.TileType<CrystalRed>() },
                        
                        { Colors2.Blue, ModContent.TileType<BlueGasTank>() },
                        { Colors2.Blue2, ModContent.TileType<Elevator>() },
                        { Colors2.Blue3, TileID.WaterDrip },

                        { Colors2.Green, ModContent.TileType<LabDoorClosed>() },
                        { Colors2.Green2, ModContent.TileType<CrystalGreen>() },

                        { Colors2.Purple, ModContent.TileType<FaultyLightTile>() },
                        { Colors2.Purple2, ModContent.TileType<LabLightTile>() },
                        
                        { Colors2.Brown1, TileID.Bookcases },
                        { Colors2.Brown2, ModContent.TileType<MountBookest>() },
                        { Colors2.Brown3, ModContent.TileType<Fan>() },
                        { Colors2.Brown4, ModContent.TileType<StackOfBoxes>() },
                        { Colors2.Brown5, ModContent.TileType<LabTable>() },
                        { Colors2.Brown6, ModContent.TileType<Cryopod>() },
                        { Colors2.Brown7, TileID.AlchemyTable },

                        { Colors2.PaintingShhh, ModContent.TileType<Painting4>() },
                        { Colors2.PaintingPoofball, ModContent.TileType<Painting1>() },
                        { Colors2.PaintingPuro, ModContent.TileType<Painting9>() },
                        { Colors2.PaintingEffectOfOranges, ModContent.TileType<Painting3>() },
                    };

                    StructureLabRoom1 = new StructureRuinedHouse("LabRoom1", tileTypes, wallTypes, furnitureTypes, "SignTextBlackGooRoom");
                    StructureLabRoom2 = new StructureRuinedHouse("LabRoom2", tileTypes, wallTypes, furnitureTypes, "SignTextCrystalZoneRoom");
                    StructureLabRoomLibrary = new StructureRuinedHouse("LabRoomLibrary", tileTypes, wallTypes, furnitureTypes, "SignTextLibraryRoom");
                    StructureLabRoomVents = new StructureRuinedHouse("LabRoomVents", tileTypes, wallTypes, furnitureTypes);
                    StructureLabRoomGas = new StructureRuinedHouse("LabRoomGas", tileTypes, wallTypes, furnitureTypes);
                    StructureLabRoomWater = new StructureRuinedHouse("LabRoomWater", tileTypes, wallTypes, furnitureTypes);
                });
            }
            catch(Exception e)
            {
                Mod.Logger.Error($"Something went wrong while trying to load the structure data: {e.Message}");
            }
        }
    }
}

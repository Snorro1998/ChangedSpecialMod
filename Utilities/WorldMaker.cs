using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Common.WorldGeneration;
using ChangedSpecialMod.Common.WorldGeneration.Passes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.Items.Food;
using ChangedSpecialMod.Content.Items.Weapons;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Content.Tiles.Furniture;
using ChangedSpecialMod.Content.Tiles.Furniture.Paintings;
using ChangedSpecialMod.Content.Tiles.Latex.Black;
using ChangedSpecialMod.Content.Walls;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Reflection;
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
        public static StructureRuinedHouse StructureOrangeDesert;
        public static StructureRuinedHouse StructureOrangeSnow;

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

                    StructureOrangeShrine2 = new StructureRuinedHouse("OrangeShrineMarble", tileTypes, wallTypes);

                    tileTypes = new Dictionary<Color, int>
                    {
                        { Colors2.Green, TileID.Sandstone },
                        { Colors2.Blue, TileID.Platforms },
                        { Colors2.Red, TileID.HardenedSand }
                    };

                    wallTypes = new Dictionary<Color, int>
                    {
                        { Colors2.Green, WallID.Sandstone }
                    };

                    StructureOrangeDesert = new StructureRuinedHouse("OrangeShrineDesert", tileTypes, wallTypes);

                    tileTypes = new Dictionary<Color, int>
                    {
                        { Colors2.Green, TileID.IceBlock },
                        { Colors2.Blue, TileID.Platforms },
                        { Colors2.Red, TileID.SnowBlock }
                    };

                    wallTypes = new Dictionary<Color, int>
                    {
                        { Colors2.Green, WallID.IceUnsafe }
                    };

                    StructureOrangeSnow = new StructureRuinedHouse("OrangeShrineSnow", tileTypes, wallTypes);

                    tileTypes = new Dictionary<Color, int>
                    {
                        { Colors2.Green, ModContent.TileType<SturdyLabTileTile>() },
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
                        { Colors2.Yellow13, ModContent.TileType<LabDoorHomeTile>() },

                        { Colors2.Red, ModContent.TileType<RedGasTank>() },
                        { Colors2.Red2, ModContent.TileType<CrystalRed>() },
                        
                        { Colors2.Blue, ModContent.TileType<BlueGasTank>() },
                        { Colors2.Blue2, ModContent.TileType<LabElevatorTile>() },
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
                        { Colors2.Brown8, ModContent.TileType<Generator>() },
                        { Colors2.Brown9, TileID.Switches },
                        { Colors2.Brown10, ModContent.TileType<WateringCan>() },

                        { Colors2.PaintingShhh, ModContent.TileType<Painting4>() },
                        { Colors2.PaintingPoofball, ModContent.TileType<Painting1>() },
                        { Colors2.PaintingPuro, ModContent.TileType<Painting9>() },
                        { Colors2.PaintingEffectOfOranges, ModContent.TileType<Painting3>() },
                    };

                    var itemList = new List<int>() { ModContent.ItemType<Orange>() };
                    StructureLabRoom1 = new StructureRuinedHouse("LabRoom1", tileTypes, wallTypes, furnitureTypes, "SignTextBlackGooRoom", itemList);
                    StructureLabRoom2 = new StructureRuinedHouse("LabRoom2", tileTypes, wallTypes, furnitureTypes, "SignTextCrystalZoneRoom");
                    
                    var lostBookID = ModContent.ItemType<QuestBook>();
                    itemList = new List<int>() 
                    { 
                        ModContent.ItemType<Encyclopedia>(),
                        lostBookID,  
                        lostBookID,  
                        lostBookID,
                        lostBookID,
                        lostBookID,
                        lostBookID,
                    };
                    
                    StructureLabRoomLibrary = new StructureRuinedHouse("LabRoomLibrary", tileTypes, wallTypes, furnitureTypes, "SignTextLibraryRoom", itemList);

                    itemList = new List<int>() { ItemID.MiningHelmet };
                    StructureLabRoomVents = new StructureRuinedHouse("LabRoomVents", tileTypes, wallTypes, furnitureTypes, "SignText1", itemList);

                    itemList = new List<int>()
                    {
                        ModContent.ItemType<FartGun>(),
                        ItemID.WhoopieCushion,
                    };
                    StructureLabRoomGas = new StructureRuinedHouse("LabRoomGas", tileTypes, wallTypes, furnitureTypes, "SignTextGasRoom", itemList);
                    StructureLabRoomWater = new StructureRuinedHouse("LabRoomWater", tileTypes, wallTypes, furnitureTypes);
                });
            }
            catch(Exception e)
            {
                Mod.Logger.Error($"Something went wrong while trying to load the structure data: {e.Message}");
            }
        }

        public override void Load()
        {
            IL_WorldGen.GenerateWorld_RunTasksAndFinish +=
                Edit_GenerateWorld_RunTasksAndFinish;
        }

        public override void Unload()
        {
            IL_WorldGen.GenerateWorld_RunTasksAndFinish -=
                Edit_GenerateWorld_RunTasksAndFinish;
        }

        private void Edit_GenerateWorld_RunTasksAndFinish(ILContext il)
        {
            try
            {
                var cursor = new ILCursor(il);

                // Find the call to SystemLoader.ModifyWorldGenTasks.
                if (!cursor.TryGotoNext(
                    MoveType.After,
                    instruction => instruction.MatchCall(
                        typeof(SystemLoader).GetMethod(
                            "ModifyWorldGenTasks",
                            BindingFlags.Public | BindingFlags.Static
                        )
                    )))
                {
                    throw new Exception(
                        "Could not find ModifyWorldGenTasks!"
                    );
                }

                // Inject our code immediately after all mods
                // have modified the generation-pass list.
                cursor.EmitDelegate(FinalizeWorldGenPasses);
            }
            catch (Exception exception)
            {
                MonoModHooks.DumpIL(Mod, il);
                Mod.Logger.Error(
                    "Failed to edit GenerateWorld_RunTasksAndFinish",
                    exception
                );
            }
        }

        private static void FinalizeWorldGenPasses()
        {
            FieldInfo generatorField = typeof(WorldGen).GetField(
                "_generator",
                BindingFlags.NonPublic | BindingFlags.Static
            );

            Terraria.WorldBuilding.WorldGenerator generator =
                (Terraria.WorldBuilding.WorldGenerator)generatorField.GetValue(null);


            // Dump all registered generation passes.
            FieldInfo passesField = typeof(Terraria.WorldBuilding.WorldGenerator).GetField(
                "_passes",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            List<GenPass> passes = (List<GenPass>)passesField.GetValue(generator);

            /*
            ChangedSpecialMod.Instance.Logger.Info(
                "========== World Generation Passes =========="
            );

            for (int i = 0; i < passes.Count; i++)
            {
                ChangedSpecialMod.Instance.Logger.Info(
                    $"[{i}] {passes[i].Name}"
                );
            }
            */

            var config = ChangedSpecialModClientConfig.Instance;
            var changedPassNames = new List<string>()
            {
                "ChangedAnalyzeWorld"
            };

            if (config.WorldGenStepSurfaceLabs)
                changedPassNames.Add("ChangedStructures");
            if (config.WorldGenStepOrangeShrine)
                changedPassNames.Add("ChangedOrangeShrines");
            if (config.WorldGenStepLatexCaves)
                changedPassNames.Add("ChangedLatexPools");
            if (config.WorldGenStepUndergroundLab)
                changedPassNames.Add("ChangedUndergroundLab");

            for (int i = 0; i < changedPassNames.Count; i++)
            {
                var previousPassName = i > 0 ? changedPassNames[i - 1] : ModSupportSystem.GetHighestWorldGenerationPassIndex(passes);
                var currentPassName = changedPassNames[i];

                if (previousPassName == null || currentPassName == null)
                    continue;

                int previousPassIndex = passes.FindLastIndex(
                    pass => pass.Name == previousPassName
                );

                int currentPassIndex = passes.FindLastIndex(
                    pass => pass.Name == currentPassName
                );

                if (previousPassIndex == -1 || currentPassIndex == -1)
                    continue;

                if (currentPassIndex > previousPassIndex)
                    continue;

                if (i == 0)
                {
                    ChangedSpecialMod.Instance.Logger.Info($"Moved world generation passes further back. They will now happen after {previousPassName}");
                }

                // Remove our pass from its current position.
                GenPass currentPass = passes[currentPassIndex];
                passes.RemoveAt(currentPassIndex);

                // Recalculate the target index after removing our pass.
                previousPassIndex = passes.FindIndex(
                    pass => pass.Name == previousPassName
                );

                // Insert our pass immediately after the latest external pass.
                passes.Insert(previousPassIndex + 1, currentPass);
            }
            /*
            var latestPassName = ModSupportSystem.GetHighestWorldGenerationPassIndex(passes);
            int analyzeWorldPassIndex = passes.FindIndex(
                pass => pass.Name == "ChangedAnalyzeWorld"
            );

            // Find the latest pass from another mod.
            int latestPassIndex = passes.FindLastIndex(
                pass => pass.Name == latestPassName
            );

            // Do nothing if no Changed passes are enabled
            if (analyzeWorldPassIndex == -1)
            {
                return;
            }

            // Do nothing if it couldn't find the last pass index, meaning the tasks are probably in the right order.
            if (latestPassIndex == -1)
            {
                return;
            }

            // Already positioned correctly.
            if (analyzeWorldPassIndex > latestPassIndex)
            {
                //ChangedSpecialMod.Instance.Logger.Info("MakeLaboratories is already positioned correctly.");
                return;
            }

            // Remove our pass from its current position.
            GenPass ourPass = passes[analyzeWorldPassIndex];
            passes.RemoveAt(analyzeWorldPassIndex);

            // Recalculate the target index after removing our pass.
            latestPassIndex = passes.FindIndex(
                pass => pass.Name == latestPassName
            );

            // Insert our pass immediately after the latest external pass.
            passes.Insert(latestPassIndex + 1, ourPass);

            ChangedSpecialMod.Instance.Logger.Info(
                $"Moved MakeLaboratories to position {latestPassIndex + 1}, immediately after {latestPassName}."
            );
            */
        }
    }
}

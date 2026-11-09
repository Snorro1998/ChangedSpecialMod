using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Items.Food;
using ChangedSpecialMod.Content.Items.Weapons;
using ChangedSpecialMod.Content.Tiles.Furniture;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Common.WorldGeneration.Passes
{
    public class TileData
    {
        // -1 = ignore, -2 = remove
        public int LiquidType = -1;
        public int LiquidAmount = -1;

        public int TileType = -1;
        public int TileColor = -1;

        public int WallType = -1;
        public int WallColor = -1;

        public bool HasWire = false;

        public int FurnitureTileID = -1;

        public bool IsTopLeftOfChest = false;
        public bool IsTopLeftOfDocumentPaper = false;
    }

    // Non-retarded colors that just work
    public static class Colors2
    {
        public static Color Red = new Color(255, 0, 0);
        public static Color Red2 = new Color(250, 0, 0);

        public static Color Orange = new Color(255, 128, 0);

        public static Color Yellow = new Color(255, 255, 0);
        public static Color Yellow2 = new Color(250, 250, 0);
        public static Color Yellow3 = new Color(245, 245, 0);
        public static Color Yellow4 = new Color(240, 240, 0);
        public static Color Yellow5 = new Color(235, 235, 0);
        public static Color Yellow6 = new Color(230, 230, 0);
        public static Color Yellow7 = new Color(225, 225, 0);
        public static Color Yellow8 = new Color(220, 220, 0);
        public static Color Yellow9 = new Color(215, 215, 0);
        public static Color Yellow10 = new Color(210, 210, 0);
        public static Color Yellow11 = new Color(205, 205, 0);
        public static Color Yellow12 = new Color(200, 200, 0);

        public static Color Green = new Color(0, 255, 0);
        public static Color Green2 = new Color(0, 250, 0);

        public static Color Blue = new Color(0, 0, 255);
        public static Color Blue2 = new Color(0, 0, 250);
        public static Color Blue3 = new Color(0, 0, 245);

        public static Color Purple = new Color(128, 0, 128);
        public static Color Purple2 = new Color(138, 0, 138);

        public static Color DeepOrange = new Color(200, 100, 0);

        public static Color Gray = new Color(128, 128, 128);

        public static Color BlackLatex = new Color(20, 20, 20);

        // Most of them are not brown
        public static Color Brown1 = new Color(190, 140, 110);
        public static Color Brown2 = new Color(180, 140, 110);
        public static Color Brown3 = new Color(170, 140, 110);
        public static Color Brown4 = new Color(160, 140, 110);
        public static Color Brown5 = new Color(150, 140, 110);
        public static Color Brown6 = new Color(140, 140, 110);
        public static Color Brown7 = new Color(130, 140, 110);
        public static Color Brown8 = new Color(120, 140, 110);
        public static Color Brown9 = new Color(110, 140, 110);
        public static Color Brown10 = new Color(100, 140, 110);

        public static Color PaintingShhh = new Color(100, 50, 30);
        public static Color PaintingPoofball = new Color(95, 50, 30);
        public static Color PaintingPuro = new Color(90, 50, 30);
        public static Color PaintingEffectOfOranges = new Color(85, 50, 30);
    }

    public class StructureRuinedHouse
    {
        public string Name;
        public TileData[,] Layout = null;
        public string SignText = null;

        // Overwriteable
        public Dictionary<Color, int> TileTypes = new Dictionary<Color, int>()
        {
            { Colors2.Green, TileID.SlimeBlock },
            { Colors2.Blue, TileID.Platforms },
        };

        // Overwriteable
        public Dictionary<Color, int> WallTypes = new Dictionary<Color, int>()
        {
            { Colors2.Gray, WallID.None },
            { Colors2.Green, WallID.Slime },
        };

        // Things placed after blocks, because they need to be anchored
        public Dictionary<Color, int> FurnitureTypes = new Dictionary<Color, int>()
        {
            { Colors2.Red, TileID.Lamps },
            { Colors2.Green, ModContent.TileType<OrangeStatue>() },
            { Colors2.Blue, ModContent.TileType<PuroStatue>() },
        };

        // Always the same. Used for both tiles and walls
        public Dictionary<Color, int> TileColors = new Dictionary<Color, int>()
        {
            { Colors2.Green, PaintID.GreenPaint },
            { Colors2.DeepOrange, PaintID.DeepOrangePaint },
            { Colors2.BlackLatex, PaintID.BlackPaint },
        };

        // Always the same
        public Dictionary<Color, int> LiquidTypes = new Dictionary<Color, int>()
        {
            { Colors2.Blue, LiquidID.Water},
            { Colors2.Red, LiquidID.Lava},
            { Colors2.Yellow, LiquidID.Honey},
            { Colors2.Purple, LiquidID.Shimmer}
        };

        public StructureRuinedHouse(string name, Dictionary<Color, int> tileTypes = null, Dictionary<Color, int> wallTypes = null, Dictionary<Color, int> furnitureTypes = null, string signText = "SignText1")
        {
            Name = name;
            if (tileTypes != null)
                TileTypes = tileTypes;
            if (wallTypes != null)
                WallTypes = wallTypes;
            if (furnitureTypes != null)
                FurnitureTypes = furnitureTypes;

            try
            {
                LoadWalls();
                LoadWires();
                LoadWallColors();
                LoadTiles();
                LoadTileColors();
                LoadLiquids();
                LoadFurniture();
                LoadChests();
            }
            catch
            {
                ChangedSpecialMod.Instance.Logger.Error("Structure cctor crashed while loading images!");
            }

            SignText = signText;
        }

        public bool BuildAtPosition(ref List<(int, int)> placedPositions, int forceXPosition = -1, int forceYPosistion = -1, RegionObject region = null)
        {
            if (Layout == null)
                return false;

            var nX = Layout.GetLength(0);
            var nY = Layout.GetLength(1);

            ChangedSpecialMod.Instance.Logger.Info($"building something w:{nX}, h:{nY}");

            int distFromBorderX = 200;

            // Move further inward if Calamity is enabled so it won't end up in the abyss
            // We might have to also add something for the sunken sea under the desert
            if (ModSupportSystem.IsModActive("CalamityMod"))
                distFromBorderX = 400;

            int distFromBorderY = 300;

            int xPosMin = distFromBorderX;
            int xPosMax = Main.maxTilesX - distFromBorderX - nX;
            int yPosMin = (int)(Main.maxTilesY * 0.4f);
            int yPosMax = Main.maxTilesY - distFromBorderY - nY;

            int xPos = ChangedUtils.WorldGenRandNext(xPosMin, xPosMax);
            int yPos = ChangedUtils.WorldGenRandNext(yPosMin, yPosMax);

            if (region != null)
            {
                xPos = region.area.Left + ChangedUtils.WorldGenRandNext(0, 30);
                yPos = region.area.Top + ChangedUtils.WorldGenRandNext(0, 30);
            }

            if (forceXPosition == -1)
            {
                foreach (var position in placedPositions)
                {
                    var dist = Vector2.DistanceSquared(new Vector2(position.Item1, position.Item2), new Vector2(xPos, yPos));
                    var minDist = 300;
                    if (dist < minDist * minDist)
                        return false;
                }
            }
            else
            {
                xPos = forceXPosition;
                yPos = forceYPosistion;
            }


            // First iteration. Walls and blocks
            for (int x2 = 0; x2 < nX; x2++)
            {
                for (int y2 = 0; y2 < nY; y2++)
                {
                    var xPosWorld = xPos + x2;
                    var yPosWorld = yPos + y2;

                    if (!WorldGen.InWorld(xPosWorld, yPosWorld))
                        continue;

                    var tmpTile = Layout[x2, y2];
                    var worldTile = Main.tile[xPosWorld, yPosWorld];

                    if (tmpTile == null)
                        continue;

                    var liquidType = tmpTile.LiquidType;
                    var liquidAmount = tmpTile.LiquidAmount;
                    var tileType = tmpTile.TileType;
                    var tileColor = tmpTile.TileColor;
                    var wallType = tmpTile.WallType;
                    var wallColor = tmpTile.WallColor;
                    var hasWire = tmpTile.HasWire;

                    if (liquidType < -1)
                        worldTile.LiquidType = LiquidID.Water;
                    else if (liquidType >= 0)
                        worldTile.LiquidType = liquidType;

                    if (liquidAmount < -1)
                        worldTile.LiquidAmount = 0;
                    else if (liquidAmount >= 0)
                        worldTile.LiquidAmount = (byte)liquidAmount;

                    if (tileType < -1)
                    {
                        WorldGenerator.DestroyChestAtPosition(xPosWorld, yPosWorld);
                        WorldGen.KillTile(xPosWorld, yPosWorld, false, false, true);
                    }
                    else if (tileType >= 0)
                    {
                        WorldGenerator.DestroyChestAtPosition(xPosWorld, yPosWorld);
                        WorldGen.KillTile(xPosWorld, yPosWorld, false, false, true);
                        WorldGen.PlaceTile(xPosWorld, yPosWorld, tileType, false, true);
                    }

                    if (tileColor < -1)
                        worldTile.TileColor = PaintID.None;
                    else if (tileColor >= 0)
                        worldTile.TileColor = (byte)tileColor;

                    if (wallType < -1)
                        worldTile.WallType = WallID.None;
                    else if (wallType >= 0)
                        worldTile.WallType = (ushort)wallType;

                    if (wallColor < -1)
                        worldTile.WallColor = PaintID.None;
                    else if (wallColor >= 0)
                        worldTile.WallColor = (byte)wallColor;

                    if (hasWire)
                        worldTile.RedWire = true;
                }
            }

            // Second iteration. For things that need to be anchored to tiles or walls (furniture, chests, etc)
            for (int x2 = 0; x2 < nX; x2++)
            {
                for (int y2 = 0; y2 < nY; y2++)
                {
                    var xPosWorld = xPos + x2;
                    var yPosWorld = yPos + y2;

                    if (!WorldGen.InWorld(xPosWorld, yPosWorld))
                        continue;

                    var tmpTile = Layout[x2, y2];
                    var worldTile = Main.tile[xPosWorld, yPosWorld];

                    if (tmpTile == null)
                        continue;

                    var placeChest = tmpTile.IsTopLeftOfChest;
                    var placeSign = tmpTile.IsTopLeftOfDocumentPaper;
                    var furnitureType = tmpTile.FurnitureTileID;

                    if (placeChest)
                        CreateChest(xPosWorld, yPosWorld);
                    else if (placeSign)
                        WorldGenerator.PlaceDocumentPaper(xPosWorld, yPosWorld, SignText);
                    
                    if (furnitureType >= 0)
                    {
                        var tile1 = Main.tile[xPosWorld, yPosWorld];

                        // Skip if it is already there
                        if (tile1.HasTile && tile1.TileType == furnitureType)
                            continue;

                        var nStyles = ChangedUtils.GetNumberOfStylesPerItem(furnitureType);
                        if (nStyles > 0)
                        {
                            var style = ChangedUtils.WorldGenRandNext(0, nStyles);
                            var placementResult = WorldGen.PlaceObject(xPosWorld, yPosWorld, furnitureType, true, style, 1, -1, ChangedUtils.WorldGenRandNext(1, 3));
                        }
                        else
                        {
                            WorldGen.PlaceTile(xPosWorld, yPosWorld, furnitureType, true);
                            tile1 = Main.tile[xPosWorld, yPosWorld];
                        }
                        

                        // Placed succesfully
                        if (tile1.HasTile && tile1.TileType == furnitureType)
                        {
                            // Dirty fan fix
                            if (furnitureType == ModContent.TileType<Content.Tiles.Furniture.Fan>())
                            {
                                var topLeft = TileObjectData.TopLeft(xPosWorld, yPosWorld);

                                Main.tile[topLeft.X, topLeft.Y].TileFrameX = 54;
                                Main.tile[topLeft.X + 1, topLeft.Y].TileFrameX = 54 + 18;
                                Main.tile[topLeft.X + 2, topLeft.Y].TileFrameX = 54 + 36;

                                Main.tile[topLeft.X, topLeft.Y + 1].TileFrameX = 54;
                                Main.tile[topLeft.X + 1, topLeft.Y + 1].TileFrameX = 54 + 18;
                                Main.tile[topLeft.X + 2, topLeft.Y + 1].TileFrameX = 54 + 36;
                            }
                            // Dirty lamp fix
                            if (furnitureType == ModContent.TileType<LabLightTile>() || furnitureType == ModContent.TileType<FaultyLightTile>())
                            {
                                Main.tile[xPosWorld, yPosWorld].TileFrameX = 18;
                                Main.tile[xPosWorld, yPosWorld + 1].TileFrameX = 18;
                            }
                        }
                    }
                }
            }

            placedPositions.Add((xPos, yPos));
            return true;
        }

        private void CreateChest(int xx, int yy)
        {
            // Place a dead man's chest
            var chestIndex = WorldGen.PlaceChest(xx, yy, TileID.Containers2, false, 4);
            if (chestIndex >= 0)
            {
                var chest = Main.chest[chestIndex];
                var items = new List<int>();
                var itemAmounts = new List<int>();

                var primaryItems = new List<int>()
                {
                    ItemID.Extractinator,
                    ItemID.BandofRegeneration,
                    ItemID.MagicMirror,
                    ItemID.CloudinaBottle,
                    ItemID.HermesBoots,
                    ItemID.Mace,
                    ItemID.ShoeSpikes
                };

                items.Add(Utils.SelectRandom(WorldGen.genRand, primaryItems.ToArray()));
                itemAmounts.Add(1);

                WorldGenerator.AddToPool(ref items, ref itemAmounts, ItemID.SuspiciousLookingEye, 5, 1, 1);
                WorldGenerator.AddToPool(ref items, ref itemAmounts, ItemID.Dynamite, 3, 1, 1);
                WorldGenerator.AddToPool(ref items, ref itemAmounts, ItemID.JestersArrow, 4, 25, 50);
                WorldGenerator.AddToPool(ref items, ref itemAmounts, ItemID.Topaz, 3, 1, 4);
                WorldGenerator.AddRandomToPool(ref items, ref itemAmounts, new int[] { ItemID.SilverBar, ItemID.TungstenBar }, 2, 3, 10);
                WorldGenerator.AddRandomToPool(ref items, ref itemAmounts, new int[] { ItemID.GoldBar, ItemID.PlatinumBar }, 2, 3, 10);
                WorldGenerator.AddRandomToPool(ref items, ref itemAmounts, new int[] { ItemID.FlamingArrow, ItemID.ThrowingKnife }, 2, 25, 50);
                WorldGenerator.AddToPool(ref items, ref itemAmounts, ItemID.HealingPotion, 2, 3, 5);
                WorldGenerator.AddToPool(ref items, ref itemAmounts, ItemID.OrangeTorch, 2, 15, 30);
                WorldGenerator.AddToPool(ref items, ref itemAmounts, ModContent.ItemType<BasketballWeapon>(), 3, 1, 1);
                WorldGenerator.AddToPool(ref items, ref itemAmounts, ItemID.GolfBallDyedOrange, 3, 1, 1);

                // Set this to a lower value if other mods also add things to chests
                // Spirit reforged doesn't check and just crashes the world generation
                // So we just lower the number of items if that mod is enabled.
                var nItemsChest = 40;
                if (ModSupportSystem.IsModActive("SpiritReforged"))
                    nItemsChest = 30;


                for (int j = items.Count; j < nItemsChest; j++)
                {
                    if (Main.rand.NextBool(3))
                    {
                        items.Add(ItemID.BloodOrange);
                        itemAmounts.Add(1);
                    }
                    else
                    {
                        items.Add(ModContent.ItemType<Orange>());
                        itemAmounts.Add(1);
                    }
                }

                for (int j = 0; j < nItemsChest; j++)
                {
                    var item = items[j];
                    chest.item[j].SetDefaults(item, false);
                    chest.item[j].stack = itemAmounts[j];
                }
            }
        }

        private Texture2D LoadTexture(string textureName)
        {
            if (Name == null || textureName == null)
                return null;
            ChangedSpecialMod.Instance.RequestAssetIfExists<Texture2D>($"Content/Structures/{Name}/{textureName}", out ReLogic.Content.Asset<Texture2D> texture);
            return texture.Value;
        }

        public void LoadWires()
        {
            Texture2D texture = LoadTexture("Wires");
            if (texture != null)
            {
                Color[] pixels = new Color[texture.Width * texture.Height];
                texture.GetData(pixels);

                if (Layout == null)
                    Layout = new TileData[texture.Width, texture.Height];

                for (int y = 0; y < texture.Height; y++)
                {
                    for (int x = 0; x < texture.Width; x++)
                    {
                        Color color = pixels[y * texture.Width + x];
                        if (Layout[x, y] == null)
                            Layout[x, y] = new TileData();

                        if (color == Color.Red)
                            Layout[x, y].HasWire = true;
                    }
                }
            }
        }

        public void LoadWalls()
        {
            Texture2D texture = LoadTexture("Walls");
            if (texture != null)
            {
                Color[] pixels = new Color[texture.Width * texture.Height];
                texture.GetData(pixels);

                if (Layout == null)
                    Layout = new TileData[texture.Width, texture.Height];

                for (int y = 0; y < texture.Height; y++)
                {
                    for (int x = 0; x < texture.Width; x++)
                    {
                        Color color = pixels[y * texture.Width + x];
                        if (Layout[x, y] == null)
                            Layout[x, y] = new TileData();

                        if (color == Color.Gray)
                            Layout[x, y].WallType = -2;
                        else if (WallTypes.ContainsKey(color))
                            Layout[x, y].WallType = (ushort)WallTypes[color];
                    }
                }
            }
        }

        public void LoadWallColors()
        {
            Texture2D texture = LoadTexture("WallColors");
            if (texture != null)
            {
                Color[] pixels = new Color[texture.Width * texture.Height];
                texture.GetData(pixels);

                if (Layout == null)
                    Layout = new TileData[texture.Width, texture.Height];

                for (int y = 0; y < texture.Height; y++)
                {
                    for (int x = 0; x < texture.Width; x++)
                    {
                        Color color = pixels[y * texture.Width + x];
                        if (Layout[x, y] == null)
                            Layout[x, y] = new TileData();

                        if (TileColors.ContainsKey(color))
                            Layout[x, y].WallColor = TileColors[color];
                    }
                }
            }
        }

        public void LoadTiles()
        {
            Texture2D texture = LoadTexture("Blocks");
            if (texture != null)
            {
                Color[] pixels = new Color[texture.Width * texture.Height];
                texture.GetData(pixels);

                if (Layout == null)
                    Layout = new TileData[texture.Width, texture.Height];

                for (int y = 0; y < texture.Height; y++)
                {
                    for (int x = 0; x < texture.Width; x++)
                    {
                        Color color = pixels[y * texture.Width + x];
                        if (Layout[x, y] == null)
                            Layout[x, y] = new TileData();

                        if (color == Color.Gray)
                            Layout[x, y].TileType = -2;
                        else if (TileTypes.ContainsKey(color))
                            Layout[x, y].TileType = (ushort)TileTypes[color];
                    }
                }
            }
        }

        public void LoadTileColors()
        {
            Texture2D texture = LoadTexture("BlockColors");
            if (texture != null)
            {
                Color[] pixels = new Color[texture.Width * texture.Height];
                texture.GetData(pixels);

                if (Layout == null)
                    Layout = new TileData[texture.Width, texture.Height];

                for (int y = 0; y < texture.Height; y++)
                {
                    for (int x = 0; x < texture.Width; x++)
                    {
                        Color color = pixels[y * texture.Width + x];
                        if (Layout[x, y] == null)
                            Layout[x, y] = new TileData();

                        if (TileColors.ContainsKey(color))
                            Layout[x, y].TileColor = TileColors[color];
                    }
                }
            }
        }

        public void LoadChests()
        {
            Texture2D texture = LoadTexture("Chests");
            if (texture != null)
            {
                Color[] pixels = new Color[texture.Width * texture.Height];
                texture.GetData(pixels);

                if (Layout == null)
                    Layout = new TileData[texture.Width, texture.Height];

                for (int y = 0; y < texture.Height; y++)
                {
                    for (int x = 0; x < texture.Width; x++)
                    {
                        Color color = pixels[y * texture.Width + x];
                        if (Layout[x, y] == null)
                            Layout[x, y] = new TileData();

                        if (color == Colors2.Green)
                            Layout[x, y].IsTopLeftOfChest = true;
                        else if (color == Colors2.Blue)
                            Layout[x, y].IsTopLeftOfDocumentPaper = true;
                    }
                }
            }
        }

        public void LoadFurniture()
        {
            Texture2D texture = LoadTexture("Furniture");
            if (texture != null)
            {
                Color[] pixels = new Color[texture.Width * texture.Height];
                texture.GetData(pixels);

                if (Layout == null)
                    Layout = new TileData[texture.Width, texture.Height];

                for (int y = 0; y < texture.Height; y++)
                {
                    for (int x = 0; x < texture.Width; x++)
                    {
                        Color color = pixels[y * texture.Width + x];
                        if (Layout[x, y] == null)
                            Layout[x, y] = new TileData();

                        if (FurnitureTypes.ContainsKey(color))
                            Layout[x, y].FurnitureTileID = FurnitureTypes[color];
                    }
                }
            }
        }

        public void LoadLiquids()
        {
            Texture2D texture = LoadTexture("Liquids");
            if (texture != null)
            {
                Color[] pixels = new Color[texture.Width * texture.Height];
                texture.GetData(pixels);

                if (Layout == null)
                    Layout = new TileData[texture.Width, texture.Height];

                for (int y = 0; y < texture.Height; y++)
                {
                    for (int x = 0; x < texture.Width; x++)
                    {
                        Color color = pixels[y * texture.Width + x];
                        var tile = Layout[x, y];
                        if (tile == null)
                            tile = new TileData();

                        if (color == Color.Gray)
                        {
                            Layout[x, y].LiquidType = -2;
                            Layout[x, y].LiquidAmount = -2;
                        }
                        else if (LiquidTypes.ContainsKey(color))
                        {
                            tile.LiquidAmount = 255;
                            tile.LiquidType = (ushort)LiquidTypes[color];
                        }

                        Layout[x, y] = tile;
                    }
                }
            }
        }
    }

    public class PassOrangeShrines : WorldGenerationPass
    {
        public PassOrangeShrines()
        {
            PassName = "ChangedOrangeShrines";
            GenerationMessage = "OrangeShrines";
        }

        public override bool Enabled()
        {
            return ChangedSpecialModClientConfig.Instance.WorldGenStepOrangeShrine;
        }

        public override void Generate()
        {
            var nSucces = 0;
            var nShrines = 10;

            if (Main.maxTilesX >= WorldGen.WorldSizeLargeX)
                nShrines = 20;
            else if (Main.maxTilesX >= WorldGen.WorldSizeMediumX)
                nShrines = 15;

            var nIce = 2;
            var nJungle = 2;
            var nDesert = 2;
            var nOther = 4;

            if (nShrines == 15)
            {
                nIce = 3;
                nJungle = 3;
                nDesert = 3;
                nOther = 6;
            }

            else if (nShrines == 20)
            {
                nIce = 4;
                nJungle = 4;
                nDesert = 4;
                nOther = 8;
            }

            var placedPositions = new List<(int, int)>();

            Queue<StructureRuinedHouse> structureList = new Queue<StructureRuinedHouse>();
            structureList.Enqueue(WorldMaker.StructureOrangeShrine);
            structureList.Enqueue(WorldMaker.StructureOrangeShrine2);

            var regions = WorldMaker.worldRegionsNew;
            var nX = regions.GetLength(0);
            var nY = regions.GetLength(1);

            var iceRegions = new List<RegionObject>();
            var jungleRegions = new List<RegionObject>();
            var desertRegions = new List<RegionObject>();
            var otherRegions = new List<RegionObject>();

            for (int x = 0; x < nX; x++)
            {
                for (int y = 0; y < nY; y++)
                {
                    var region = regions[x, y];
                    if (region == null || !region.IsValidNotSky() || !region.IsUnderground()) 
                        continue;
                    if (region.regionType == WorldRegionType.Snow)
                        iceRegions.Add(region);
                    else if (region.regionType == WorldRegionType.Jungle)
                        jungleRegions.Add(region);
                    else if (region.regionType == WorldRegionType.Desert)
                        desertRegions.Add(region);
                    else if (region.regionType != WorldRegionType.Hell)
                        otherRegions.Add(region);
                }
            }

            iceRegions = iceRegions.OrderBy(_ => ChangedUtils.WorldGenRandNext(0, Int32.MaxValue)).ToList();
            jungleRegions = jungleRegions.OrderBy(_ => ChangedUtils.WorldGenRandNext(0, Int32.MaxValue)).ToList();
            desertRegions = desertRegions.OrderBy(_ => ChangedUtils.WorldGenRandNext(0, Int32.MaxValue)).ToList();
            otherRegions = otherRegions.OrderBy(_ => ChangedUtils.WorldGenRandNext(0, Int32.MaxValue)).ToList();

            var shrineDefault = WorldMaker.StructureOrangeShrine;
            var shrineMarble = WorldMaker.StructureOrangeShrine2;
            var shrineSnow = WorldMaker.StructureOrangeSnow;
            var shrineDesert = WorldMaker.StructureOrangeDesert;

            nSucces = 0;
            for (int i = 0; i < iceRegions.Count; i++)
            {
                if (nSucces >= nIce)
                    break;
                
                var region = iceRegions[i];
                var succes = shrineSnow.BuildAtPosition(ref placedPositions, -1, -1, region);
                if (succes)
                    nSucces++;
            }

            nSucces = 0;
            for (int i = 0; i < jungleRegions.Count; i++)
            {
                if (i >= nJungle)
                    break;

                var region = jungleRegions[i];
                var succes = shrineDefault.BuildAtPosition(ref placedPositions, -1, -1, region);
                if (succes)
                    nSucces++;
            }

            nSucces = 0;
            for (int i = 0; i < desertRegions.Count; i++)
            {
                if (i >= nDesert)
                    break;

                var region = desertRegions[i];
                var succes = shrineDesert.BuildAtPosition(ref placedPositions, -1, -1, region);
                if (succes)
                    nSucces++;
            }

            nSucces = 0;
            for (int i = 0; i < otherRegions.Count; i++)
            {
                if (i >= nOther)
                    break;

                var region = otherRegions[i];
                var shrine = i % 2 == 0 ? shrineDefault : shrineMarble;
                var succes = shrine.BuildAtPosition(ref placedPositions, -1, -1, region);
                if (succes)
                    nSucces++;
            }
        }
    }
}

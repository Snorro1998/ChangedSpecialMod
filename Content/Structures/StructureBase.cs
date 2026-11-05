using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Common.WorldGeneration.Passes;
using ChangedSpecialMod.Content.Items.Food;
using ChangedSpecialMod.Content.Items.Weapons;
using ChangedSpecialMod.Content.Tiles.Furniture;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Structures
{
    public abstract class StructureBase
    {
        public string Name { get; protected set; }
        public TileData[,] Layout = null;

        public virtual Dictionary<Color, int> TileTypes { get; set; } = new()
        {
            { Colors2.Green, TileID.SlimeBlock },
            { Colors2.Blue, TileID.Platforms },
        };

        public virtual Dictionary<Color, int> WallTypes { get; set; } = new()
        {
            { Colors2.Gray, WallID.None },
            { Colors2.Green, WallID.Slime },
        };

        public virtual Dictionary<Color, int> FurnitureTypes { get; set; } = new()
        {
            { Colors2.Red, TileID.Lamps },
            { Colors2.Green, ModContent.TileType<OrangeStatue>() },
            { Colors2.Blue, ModContent.TileType<PuroStatue>() },
        };

        public Dictionary<Color, int> TileColors { get; } = new()
        {
            { Colors2.Green, PaintID.GreenPaint },
            { Colors2.DeepOrange, PaintID.DeepOrangePaint },
        };

        public Dictionary<Color, int> LiquidTypes { get; } = new()
        {
            { Colors2.Blue, LiquidID.Water },
            { Colors2.Red, LiquidID.Lava },
            { Colors2.Yellow, LiquidID.Honey },
            { Colors2.Purple, LiquidID.Shimmer },
        };

        protected StructureBase(
            string name,
            Dictionary<Color, int> tileTypes = null,
            Dictionary<Color, int> wallTypes = null,
            Dictionary<Color, int> furnitureTypes = null)
        {
            Name = name;

            if (tileTypes != null)
                TileTypes = tileTypes;

            if (wallTypes != null)
                WallTypes = wallTypes;

            if (furnitureTypes != null)
                FurnitureTypes = furnitureTypes;

            // Load the layers that make up the layout
            LoadWalls();
            LoadWires();
            LoadWallColors();
            LoadTiles();
            LoadTileColors();
            LoadLiquids();
            LoadFurniture();
            LoadChests();
        }

        public bool BuildAtPosition(ref List<(int, int)> placedPositions)
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

            foreach (var position in placedPositions)
            {
                var dist = Vector2.DistanceSquared(new Vector2(position.Item1, position.Item2), new Vector2(xPos, yPos));
                var minDist = 300;
                if (dist < minDist * minDist)
                    return false;
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
                        worldTile.TileType = TileID.Dirt;
                        worldTile.IsHalfBlock = false;
                        worldTile.Slope = SlopeType.Solid;
                        worldTile.HasTile = false;
                    }
                    else if (tileType >= 0)
                    {
                        worldTile.TileType = (ushort)tileType;
                        worldTile.HasTile = true;
                        worldTile.Slope = SlopeType.Solid;
                        worldTile.IsHalfBlock = false;
                        worldTile.IsActuated = false;
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
                    var furnitureType = tmpTile.FurnitureTileID;

                    if (placeChest)
                        CreateChest(xPosWorld, yPosWorld);
                    if (furnitureType >= 0)
                        WorldGen.PlaceTile(xPosWorld, yPosWorld, furnitureType, true);
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
}

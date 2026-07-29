using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Tiles.Latex.Black
{
    public class BlackLatexVines : BaseBlackLatexTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileCut[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoFail[Type] = true;
            TileID.Sets.IsVine[Type] = true;
            TileID.Sets.ReplaceTileBreakDown[Type] = true;
            TileID.Sets.VineThreads[Type] = true;
            TileID.Sets.DrawFlipMode[Type] = 1;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);

            HitSound = SoundID.Grass;

            AddMapEntry(new Color(65, 56, 83));
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Main.instance.TilesRenderer.CrawlToTopOfVineAndAddSpecialPoint(j, i);
            return false;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            //GIVE VINE ROPE IF SPECIAL VINE BOOK
            if (WorldGen.genRand.NextBool() && Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].cordage)
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i * 16 + 8f, j * 16 + 8f), ItemID.VineRope);
            }
        }
        /*
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            float brightness = 0.9f;
            brightness += 0.2f;
            brightness = MathHelper.Clamp(brightness, 0.5f, 0.9f);
            Color orange = new Color(237, 93, 83);
            Color cyan = new Color(66, 189, 181);
            Color value = Color.Lerp(orange, cyan, (MathF.Sin(j / 30f + Main.GameUpdateCount * 0.017f + -i / 40f) + 1f) / 2f);
            Color value1 = Color.Lerp(orange, cyan, (MathF.Sin((-j - 100) / 40f + Main.GameUpdateCount * 0.014f + i / 20f) + 1f) / 2f);
            r = (value.R + value1.R) / 600f;
            g = (value.G + value1.G) / 600f;
            b = (value.B + value1.B) / 600f;
            r *= brightness;
            g *= brightness;
            b *= brightness;
        }
        */

        public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor)
        {
            //sightColor = Color.Cyan;
            return true;
        }
    }

    // This class handles spawning and growing ExampleVine (RandomUpdate). Vines can either grow from the tip of an existing vine or spawn from the tile it grows from.
    // Because this behavior needs to act on both ExampleVine and ExampleBlock tiles, we put this logic in a GlobalTile rather than in both ModTile classes.
    // This class also handle transforming vines to ExampleVine if the anchor tile changes (TileFrame).
    public class BlackLatexVineGlobalTile : GlobalTile
    {
        private int Vine;
        private int ExampleBlock; // TODO: Replace with ExampleGrass eventually.

        public override void SetStaticDefaults()
        {
            // Caching these tile type values to make the code more readable
            Vine = ModContent.TileType<BlackLatexVines>();
            ExampleBlock = ModContent.TileType<BlackLatexGrassTile>();
        }

        // Random growth behavior:
        public override void RandomUpdate(int i, int j, int type)
        {
            Tile tile = Main.tile[i, j];
            if (!tile.HasUnactuatedTile)
            {
                return; // Don't grow on actuated tiles.
            }

            // Vine tiles usually grow on themselves (from the tip) or on any tile they spawn from (grass tiles usually). GrowMoreVines checks that the nearby area isn't already full of vines.
            if ((tile.TileType == Vine || tile.TileType == ExampleBlock) && WorldGen.GrowMoreVines(i, j))
            {
                int growChance = 70;
                if (tile.TileType == Vine)
                {
                    growChance = 7; // 10 times more likely to extend an existing vine than start a new vine
                }

                int below = j + 1;
                Tile tileBelow = Main.tile[i, below];
                if (WorldGen.genRand.NextBool(growChance) && !tileBelow.HasTile && tileBelow.LiquidType != LiquidID.Lava)
                {
                    // We check that the vine can grow longer and is not already broken.
                    bool vineIsHangingOffValidTile = false;
                    for (int above = j; above > j - 10; above--)
                    {
                        Tile tileAbove = Main.tile[i, above];
                        if (tileAbove.BottomSlope)
                        {
                            return;
                        }

                        if (tileAbove.HasTile && tileAbove.TileType == ExampleBlock && !tileAbove.BottomSlope)
                        {
                            vineIsHangingOffValidTile = true;
                            break;
                        }
                    }

                    if (vineIsHangingOffValidTile)
                    {
                        // If all the checks succeed, place the tile, copy paint from the tile we grew from, and sync the tile change.
                        tileBelow.TileType = (ushort)Vine;
                        tileBelow.HasTile = true;
                        tileBelow.CopyPaintAndCoating(tile);
                        WorldGen.SquareTileFrame(i, below);
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendTileSquare(-1, i, below);
                        }
                    }
                }
            }
        }

        // Transforming vines to ExampleVine if necessary behavior
        public override bool TileFrame(int i, int j, int type, ref bool resetFrame, ref bool noBreak)
        {
            // This code handles transforming any vine to ExampleVine if the anchored tile happens to change to ExampleBlock. This can happen with spreading grass tiles or Clentaminator solutions. Without this code the vine would just break in those situations.
            if (!TileID.Sets.IsVine[type])
            {
                return true;
            }

            Tile tile = Main.tile[i, j];
            Tile tileAbove = Main.tile[i, j - 1];

            // We determine the tile type of the tile above this tile. If the tile doesn't exist, is actuated, or has a slopped bottom, the vine will be destroyed (-1).
            int aboveTileType = tileAbove.HasUnactuatedTile && !tileAbove.BottomSlope ? tileAbove.TileType : -1;

            // If this tile isn't the same as the one above, we need to verify that the above tile is valid.
            if (type != aboveTileType)
            {
                // If the above tile is a valid ExampleVine anchor, but this tile isn't ExampleVine, we change this tile into ExampleVine.
                if ((aboveTileType == ExampleBlock || aboveTileType == Vine) && type != Vine)
                {
                    tile.TileType = (ushort)Vine;
                    WorldGen.SquareTileFrame(i, j);
                    return true;
                }

                // Finally, we need to handle the case where there is not longer a valid placement for ExampleVine.
                // Due to the ordering of hooks with respect to vanilla code, it is not easy to do this in a mod-compatible manner directly. Vanilla vine code or vine code from other mods might convert the vine to a new tile type, but we can't know that here.
                // If the anchor tile is invalid, we kill the tile, otherwise we change the vine tile to TileID.Vines and let the vanilla code that will run after this handle the remaining logic.
                if (type == Vine && aboveTileType != ExampleBlock)
                {
                    if (aboveTileType == -1)
                    {
                        WorldGen.KillTile(i, j);
                    }
                    else
                    {
                        tile.TileType = TileID.Vines;
                    }
                }
            }

            return true;
        }
    }
}


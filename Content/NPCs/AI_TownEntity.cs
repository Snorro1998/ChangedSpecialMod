using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
    public static class AI_TownEntity
    {
        public static List<int> PlantList = new List<int>
        {
            //TileID.Saplings,
            //TileID.Cactus,
            //TileID.Trees,
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

    private static void AI_007_TownEntities_TeleportToHome(NPC npc, int homeFloorX, int homeFloorY)
        {
            bool flag = false;
            for (int i = 0; i < 3; i++)
            {
                int num2 = homeFloorX + i switch
                {
                    1 => -1,
                    0 => 0,
                    _ => 1,
                };
                if (npc.type == NPCID.OldMan || !Collision.SolidTiles(num2 - 1, num2 + 1, homeFloorY - 3, homeFloorY - 1))
                {
                    npc.velocity.X = 0f;
                    npc.velocity.Y = 0f;
                    npc.position.X = num2 * 16 + 8 - npc.width / 2;
                    npc.position.Y = (float)(homeFloorY * 16 - npc.height) - 0.1f;
                    npc.netUpdate = true;
                    AI_007_TryForcingSitting(npc, homeFloorX, homeFloorY);
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                npc.homeless = true;
                WorldGen.QuickFindHome(npc.whoAmI);
            }
        }

        private static void AI_007_TryForcingSitting(NPC npc, int homeFloorX, int homeFloorY)
        {
            Tile tile = Main.tile[homeFloorX, homeFloorY - 1];
            bool flag = !NPCID.Sets.CannotSitOnFurniture[npc.type] && !NPCID.Sets.IsTownSlime[npc.type] && npc.ai[0] != 5f;
            if (flag)
            {
                flag &= tile != null && tile.HasTile && TileID.Sets.CanBeSatOnForNPCs[tile.TileType];
            }
            if (flag)
            {
                flag &= tile.TileType != 15 || tile.TileFrameY < 1080 || tile.TileFrameY > 1098;
            }
            if (flag)
            {
                Point point = (npc.Bottom + Vector2.UnitY * -2f).ToTileCoordinates();
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].aiStyle == 7 && Main.npc[i].townNPC && Main.npc[i].ai[0] == 5f && (Main.npc[i].Bottom + Vector2.UnitY * -2f).ToTileCoordinates() == point)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            if (flag)
            {
                npc.ai[0] = 5f;
                npc.ai[1] = 900 + Main.rand.Next(10800);
                npc.SitDown(new Point(homeFloorX, homeFloorY - 1), out var targetDirection, out var bottom);
                npc.direction = targetDirection;
                npc.Bottom = bottom;
                npc.velocity = Vector2.Zero;
                npc.localAI[3] = 0f;
                npc.netUpdate = true;
            }
        }


        private static void AI_007_TownEntities_GetWalkPrediction(NPC npc, int myTileX, int homeFloorX, bool canBreathUnderWater, bool currentlyDrowning, int tileX, int tileY, out bool keepwalking, out bool avoidFalling)
        {
            keepwalking = false;
            avoidFalling = true;
            bool flag = myTileX >= homeFloorX - 35 && myTileX <= homeFloorX + 35;
            if (npc.townNPC && npc.ai[1] < 30f)
            {
                keepwalking = !Utils.PlotTileLine(npc.Top, npc.Bottom, npc.width, DelegateMethods.SearchAvoidedByNPCs);
                if (!keepwalking)
                {
                    Rectangle hitbox = npc.Hitbox;
                    hitbox.X -= 20;
                    hitbox.Width += 40;
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].active && Main.npc[i].friendly && i != npc.whoAmI && Main.npc[i].velocity.X == 0f && hitbox.Intersects(Main.npc[i].Hitbox))
                        {
                            keepwalking = true;
                            break;
                        }
                    }
                }
            }
            if (!keepwalking && currentlyDrowning)
            {
                keepwalking = true;
            }
            if (avoidFalling && (NPCID.Sets.TownCritter[npc.type] || (!flag && npc.direction == Math.Sign(homeFloorX - myTileX))))
            {
                avoidFalling = false;
            }
            if (!avoidFalling)
            {
                return;
            }
            bool flag2 = false;
            Point p = default(Point);
            int num = 0;
            for (int j = -1; j <= 4; j++)
            {
                Tile tileSafely = Framing.GetTileSafely(tileX, tileY + j);
                if (tileSafely.LiquidAmount > 0)
                {
                    num++;
                    if (tileSafely.LiquidType == LiquidID.Lava)
                    {
                        flag2 = true;
                        break;
                    }
                }
                if (tileSafely.HasUnactuatedTile && Main.tileSolid[tileSafely.TileType])
                {
                    if (num > 0)
                    {
                        p.X = tileX;
                        p.Y = tileY + j;
                    }
                    avoidFalling = false;
                    break;
                }
            }
            avoidFalling |= flag2;
            double num2 = Math.Ceiling((float)npc.height / 16f);
            if ((double)num >= num2)
            {
                avoidFalling = true;
            }
            if (!avoidFalling && p.X != 0 && p.Y != 0)
            {
                Vector2 vector = p.ToWorldCoordinates(8f, 0f) + new Vector2(-npc.width / 2, -npc.height);
                avoidFalling = Collision.DrownCollision(vector, npc.width, npc.height, 1f);
            }
        }

        private static bool AI_007_TownEntities_CheckIfWillDrown(bool currentlyDrowning)
        {
            return currentlyDrowning;
        }

        private static void AI_007_AttemptToPlayIdleAnimationsForPets(NPC npc, int petIdleChance)
        {
            if (npc.velocity.X == 0f && Main.netMode != 1 && Main.rand.Next(petIdleChance) == 0)
            {
                int num = 3;
                if (npc.type == 638)
                {
                    num = 2;
                }
                if (NPCID.Sets.IsTownSlime[npc.type])
                {
                    num = 0;
                }
                npc.ai[0] = ((num == 0) ? 20 : Main.rand.Next(20, 20 + num));
                npc.ai[1] = 200 + Main.rand.Next(300);
                if (npc.ai[0] == 20f && npc.type == 637)
                {
                    npc.ai[1] = 500 + Main.rand.Next(200);
                }
                if (npc.ai[0] == 21f && npc.type == 638)
                {
                    npc.ai[1] = 100 + Main.rand.Next(100);
                }
                if (npc.ai[0] == 22f && npc.type == 656)
                {
                    npc.ai[1] = 200 + Main.rand.Next(200);
                }
                if (npc.ai[0] == 20f && NPCID.Sets.IsTownSlime[npc.type])
                {
                    npc.ai[1] = 180 + Main.rand.Next(240);
                }
                npc.ai[2] = 0f;
                npc.localAI[3] = 0f;
                npc.netUpdate = true;
            }
        }

        private static void AI_007_TownEntities_Shimmer_TeleportToLandingSpot(NPC npc)
        {
            Vector2? vector = AI_007_TownEntities_Shimmer_ScanForBestSpotToLandOn(npc);
            if (vector.HasValue)
            {
                Vector2 vector2 = npc.position;
                npc.position = vector.Value;
                Vector2 movementVector = npc.position - vector2;
                int num = 560;
                if (movementVector.Length() >= (float)num)
                {
                    npc.ai[2] = 30f;
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerTownNPCSend, new ParticleOrchestraSettings
                    {
                        PositionInWorld = vector2 + npc.Size / 2f,
                        MovementVector = movementVector
                    });
                }
                npc.netUpdate = true;
            }
        }

        private static Vector2? AI_007_TownEntities_Shimmer_ScanForBestSpotToLandOn(NPC npc)
        {
            Point point = npc.Top.ToTileCoordinates();
            int num = 30;
            Vector2? result = null;
            bool flag = npc.homeless && (npc.homeTileX == -1 || npc.homeTileY == -1);
            for (int i = 1; i < num; i += 2)
            {
                Vector2? vector = ShimmerHelper.FindSpotWithoutShimmer(npc, point.X, point.Y, i, flag);
                if (vector.HasValue)
                {
                    result = vector.Value;
                    break;
                }
            }
            if (!result.HasValue && npc.homeTileX != -1 && npc.homeTileY != -1)
            {
                for (int j = 1; j < num; j += 2)
                {
                    Vector2? vector2 = ShimmerHelper.FindSpotWithoutShimmer(npc, npc.homeTileX, npc.homeTileY, j, flag);
                    if (vector2.HasValue)
                    {
                        result = vector2.Value;
                        break;
                    }
                }
            }
            if (!result.HasValue)
            {
                int num2 = (flag ? 30 : 0);
                num = 60;
                flag = true;
                for (int k = num2; k < num; k += 2)
                {
                    Vector2? vector3 = ShimmerHelper.FindSpotWithoutShimmer(npc, point.X, point.Y, k, flag);
                    if (vector3.HasValue)
                    {
                        result = vector3.Value;
                        break;
                    }
                }
            }
            if (!result.HasValue && npc.homeTileX != -1 && npc.homeTileY != -1)
            {
                num = 60;
                flag = true;
                for (int l = 30; l < num; l += 2)
                {
                    Vector2? vector4 = ShimmerHelper.FindSpotWithoutShimmer(npc, npc.homeTileX, npc.homeTileY, l, flag);
                    if (vector4.HasValue)
                    {
                        result = vector4.Value;
                        break;
                    }
                }
            }
            return result;
        }

        private static void AI_007_FindGoodRestingSpot(NPC npc, int myTileX, int myTileY, out int floorX, out int floorY)
        {
            floorX = npc.homeTileX;
            floorY = npc.homeTileY;
            if (floorX == -1 || floorY == -1)
            {
                return;
            }
            while (!WorldGen.SolidOrSlopedTile(floorX, floorY) && floorY < Main.maxTilesY - 20)
            {
                floorY++;
            }
            if (Main.dayTime || (npc.ai[0] == 5f && Math.Abs(myTileX - floorX) < 7 && Math.Abs(myTileY - floorY) < 7))
            {
                return;
            }
            Point point = new Point(floorX, floorY);
            Point point2 = new Point(-1, -1);
            int num = -1;
            if (npc.type == 638 || npc.type == 656 || NPCID.Sets.IsTownSlime[npc.type] || npc.ai[0] == 5f)
            {
                return;
            }
            int num2 = 7;
            int num3 = 6;
            int num4 = 1;
            int num5 = 1;
            int num6 = 1;
            for (int i = point.X - num2; i <= point.X + num2; i += num5)
            {
                for (int num7 = point.Y + num4; num7 >= point.Y - num3; num7 -= num6)
                {
                    Tile tile = Main.tile[i, num7];
                    if (tile != null && tile.HasTile && TileID.Sets.CanBeSatOnForNPCs[tile.TileType])
                    {
                        int num8 = Math.Abs(i - point.X) + Math.Abs(num7 - point.Y);
                        if (num == -1 || num8 < num)
                        {
                            num = num8;
                            point2.X = i;
                            point2.Y = num7;
                        }
                    }
                }
            }
            if (num == -1)
            {
                return;
            }
            Tile tile2 = Main.tile[point2.X, point2.Y];
            if (tile2.TileType == 497 || tile2.TileType == 15)
            {
                if (tile2.TileFrameY % 40 != 0)
                {
                    point2.Y--;
                }
                point2.Y += 2;
            }
            else if (tile2.TileType >= TileID.Count)
            {
                TileRestingInfo info = new TileRestingInfo(npc, point2, Vector2.Zero, npc.direction);
                TileLoader.ModifySittingTargetInfo(point2.X, point2.Y, tile2.TileType, ref info);
                point2 = info.AnchorTilePosition;
                point2.Y++;
            }
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].active && Main.npc[j].aiStyle == 7 && Main.npc[j].townNPC && Main.npc[j].ai[0] == 5f && (Main.npc[j].Bottom + Vector2.UnitY * -2f).ToTileCoordinates() == point2)
                {
                    return;
                }
            }
            floorX = point2.X;
            floorY = point2.Y;
        }

        private static bool AI_007_TownEntities_IsInAGoodRestingSpot(NPC npc, int tileX, int tileY, int idealRestX, int idealRestY)
        {
            if (!Main.dayTime && npc.ai[0] == 5f)
            {
                if (Math.Abs(tileX - idealRestX) <= 7)
                {
                    return Math.Abs(tileY - idealRestY) <= 7;
                }
                return false;
            }
            if ((npc.type == NPCID.Frog || npc.type == NPCID.GoldFrog || npc.type == NPCID.BoundTownSlimeYellow) && npc.wet)
            {
                return false;
            }
            if (tileX == idealRestX)
            {
                return tileY == idealRestY;
            }
            return false;
        }


        // Added custom states which start from 30
        // 30: Watering plants
        public static void AI_007_TownEntities(NPC npc)
        {
            // Chance he will water plants if walking near them
            int waterPlantsChance = 90;
            // Chance he will sit down if walking near a chair
            int sitDownChance = 300;

            bool shouldStayInside = Main.raining;

            NPC.ShimmeredTownNPCs[npc.type] = npc.IsShimmerVariant;
            if (npc.type == NPCID.TaxCollector && npc.GivenName == "Andrew")
            {
                npc.defDefense = 200;
            }
            if (npc.type == NPCID.TownDog || npc.type == NPCID.TownBunny || NPCID.Sets.IsTownSlime[npc.type])
            {
                sitDownChance = 0;
            }
            if (!Main.dayTime || Main.eclipse || Main.slimeRain)
            {
                shouldStayInside = true;
            }
            float num2 = 1f;
            if (Main.masterMode)
            {
                npc.defense = (npc.dryadWard ? (npc.defDefense + 14) : npc.defDefense);
            }
            else if (Main.expertMode)
            {
                npc.defense = (npc.dryadWard ? (npc.defDefense + 10) : npc.defDefense);
            }
            else
            {
                npc.defense = (npc.dryadWard ? (npc.defDefense + 6) : npc.defDefense);
            }
            if (npc.isLikeATownNPC)
            {
                if (NPC.combatBookWasUsed)
                {
                    num2 += 0.2f;
                    npc.defense += 6;
                }
                if (NPC.combatBookVolumeTwoWasUsed)
                {
                    num2 += 0.2f;
                    npc.defense += 6;
                }
                if (NPC.downedBoss1)
                {
                    num2 += 0.1f;
                    npc.defense += 3;
                }
                if (NPC.downedBoss2)
                {
                    num2 += 0.1f;
                    npc.defense += 3;
                }
                if (NPC.downedBoss3)
                {
                    num2 += 0.1f;
                    npc.defense += 3;
                }
                if (NPC.downedQueenBee)
                {
                    num2 += 0.1f;
                    npc.defense += 3;
                }
                if (Main.hardMode)
                {
                    num2 += 0.4f;
                    npc.defense += 12;
                }
                if (NPC.downedQueenSlime)
                {
                    num2 += 0.15f;
                    npc.defense += 6;
                }
                if (NPC.downedMechBoss1)
                {
                    num2 += 0.15f;
                    npc.defense += 6;
                }
                if (NPC.downedMechBoss2)
                {
                    num2 += 0.15f;
                    npc.defense += 6;
                }
                if (NPC.downedMechBoss3)
                {
                    num2 += 0.15f;
                    npc.defense += 6;
                }
                if (NPC.downedPlantBoss)
                {
                    num2 += 0.15f;
                    npc.defense += 8;
                }
                if (NPC.downedEmpressOfLight)
                {
                    num2 += 0.15f;
                    npc.defense += 8;
                }
                if (NPC.downedGolemBoss)
                {
                    num2 += 0.15f;
                    npc.defense += 8;
                }
                if (NPC.downedAncientCultist)
                {
                    num2 += 0.15f;
                    npc.defense += 8;
                }
                NPCLoader.BuffTownNPC(ref num2, ref npc.defense);
            }
            if (npc.type == NPCID.SantaClaus && Main.netMode != NetmodeID.MultiplayerClient && !Main.xMas)
            {
                var hit = new NPC.HitInfo();
                hit.Damage = 9999;
                hit.Knockback = 0f;
                npc.StrikeNPC(hit);
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, 9999f);
                }
            }
            if ((npc.type == NPCID.Penguin || npc.type == NPCID.PenguinBlack) && npc.localAI[0] == 0f)
            {
                npc.localAI[0] = Main.rand.Next(1, 5);
            }
            if (npc.type == NPCID.Mechanic)
            {
                int num3 = NPC.lazyNPCOwnedProjectileSearchArray[npc.whoAmI];
                bool flag2 = false;
                if (Main.projectile.IndexInRange(num3))
                {
                    Projectile projectile = Main.projectile[num3];
                    if (projectile.active && projectile.type == 582 && projectile.ai[1] == (float)npc.whoAmI)
                    {
                        flag2 = true;
                    }
                }
                npc.localAI[0] = flag2.ToInt();
            }
            // Switch to swimming variant when entering the water, which is a completely different npc
            if ((npc.type == NPCID.Duck || npc.type == 364 || npc.type == 602 || npc.type == 608) && Main.netMode != 1 && (npc.velocity.Y > 4f || npc.velocity.Y < -4f || npc.wet))
            {
                int num4 = npc.direction;
                npc.Transform(npc.type + 1);
                npc.TargetClosest();
                npc.direction = num4;
                npc.netUpdate = true;
                return;
            }

            switch (npc.type)
            {
                case NPCID.Golfer:
                    NPC.savedGolfer = true;
                    break;
                case NPCID.TaxCollector:
                    NPC.savedTaxCollector = true;
                    break;
                case NPCID.GoblinTinkerer:
                    NPC.savedGoblin = true;
                    break;
                case NPCID.Wizard:
                    NPC.savedWizard = true;
                    break;
                case NPCID.Mechanic:
                    NPC.savedMech = true;
                    break;
                case NPCID.Stylist:
                    NPC.savedStylist = true;
                    break;
                case NPCID.Angler:
                    NPC.savedAngler = true;
                    break;
                case NPCID.DD2Bartender:
                    NPC.savedBartender = true;
                    break;
            }

            npc.dontTakeDamage = false;
            // Shimmering
            if (npc.ai[0] == 25f)
            {
                npc.dontTakeDamage = true;
                if (npc.ai[1] == 0f)
                {
                    npc.velocity.X = 0f;
                }
                npc.shimmerWet = false;
                npc.wet = false;
                npc.lavaWet = false;
                npc.honeyWet = false;
                if (npc.ai[1] == 0f && Main.netMode == NetmodeID.MultiplayerClient)
                {
                    return;
                }
                if (npc.ai[1] == 0f && npc.ai[2] < 1f)
                {
                    AI_007_TownEntities_Shimmer_TeleportToLandingSpot(npc);
                }
                if (npc.ai[2] > 0f)
                {
                    npc.ai[2] -= 1f;
                    if (npc.ai[2] <= 0f)
                    {
                        npc.ai[1] = 1f;
                    }
                    return;
                }
                npc.ai[1] += 1f;
                if (npc.ai[1] >= 30f)
                {
                    if (!Collision.WetCollision(npc.position, npc.width, npc.height))
                    {
                        npc.shimmerTransparency = MathHelper.Clamp(npc.shimmerTransparency - 1f / 60f, 0f, 1f);
                    }
                    else
                    {
                        npc.ai[1] = 30f;
                    }
                    npc.velocity = new Vector2(0f, -4f * npc.shimmerTransparency);
                }
                Rectangle hitbox = npc.Hitbox;
                hitbox.Y += 20;
                hitbox.Height -= 20;
                float num5 = Main.rand.NextFloatDirection();
                Lighting.AddLight(npc.Center, Main.hslToRgb((float)Main.timeForVisualEffects / 360f % 1f, 0.6f, 0.65f).ToVector3() * Utils.Remap(npc.ai[1], 30f, 90f, 0f, 0.7f));
                if (Main.rand.NextFloat() > Utils.Remap(npc.ai[1], 30f, 60f, 1f, 0.5f))
                {
                    Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(hitbox) + Main.rand.NextVector2Circular(8f, 0f) + new Vector2(0f, 4f), 309, new Vector2(0f, -2f).RotatedBy(num5 * ((float)Math.PI * 2f) * 0.11f), 0, default(Color), 1.7f - Math.Abs(num5) * 1.3f);
                }
                if (npc.ai[1] > 60f && Main.rand.Next(15) == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 vector = Main.rand.NextVector2FromRectangle(npc.Hitbox);
                        ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ShimmerBlock, new ParticleOrchestraSettings
                        {
                            PositionInWorld = vector,
                            MovementVector = npc.DirectionTo(vector).RotatedBy((float)Math.PI * 9f / 20f * (float)(Main.rand.Next(2) * 2 - 1)) * Main.rand.NextFloat()
                        });
                    }
                }
                npc.TargetClosest();
                NPCAimedTarget targetData = npc.GetTargetData();
                if (npc.ai[1] >= 75f && npc.shimmerTransparency <= 0f && Main.netMode != 1)
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] = 0f;
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                    Math.Sign(targetData.Center.X - npc.Center.X);
                    npc.velocity = new Vector2(0f, -4f);
                    npc.localAI[0] = 0f;
                    npc.localAI[1] = 0f;
                    npc.localAI[2] = 0f;
                    npc.localAI[3] = 0f;
                    npc.netUpdate = true;
                    npc.townNpcVariationIndex = ((npc.townNpcVariationIndex != 1) ? 1 : 0);
                    NetMessage.SendData(56, -1, -1, null, npc.whoAmI);
                    npc.Teleport(npc.position, 12);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerTownNPC, new ParticleOrchestraSettings
                    {
                        PositionInWorld = npc.Center
                    });
                }
                return;
            }
            if (npc.type >= NPCID.None && NPCID.Sets.TownCritter[npc.type] && npc.target == 255)
            {
                npc.TargetClosest();
                if (npc.position.X < Main.player[npc.target].position.X)
                {
                    npc.direction = 1;
                    npc.spriteDirection = npc.direction;
                }
                if (npc.position.X > Main.player[npc.target].position.X)
                {
                    npc.direction = -1;
                    npc.spriteDirection = npc.direction;
                }
                if (npc.homeTileX == -1)
                {
                    npc.UpdateHomeTileState(npc.homeless, (int)((npc.position.X + (float)(npc.width / 2)) / 16f), npc.homeTileY);
                }
            }
            else if (npc.homeTileX == -1 && npc.homeTileY == -1 && npc.velocity.Y == 0f && !npc.shimmering)
            {
                npc.UpdateHomeTileState(npc.homeless, (int)npc.Center.X / 16, (int)(npc.position.Y + (float)npc.height + 4f) / 16);
            }
            bool flag3 = false;
            int num6 = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
            int num7 = (int)(npc.position.Y + (float)npc.height + 1f) / 16;
            AI_007_FindGoodRestingSpot(npc, num6, num7, out var floorX, out var floorY);
            if (npc.type == NPCID.TaxCollector)
            {
                NPC.taxCollector = true;
            }
            npc.directionY = -1;
            if (npc.direction == 0)
            {
                npc.direction = 1;
            }
            // If he is not shimmering?
            if (npc.ai[0] != 24f)
            {
                for (int j = 0; j < 255; j++)
                {
                    // If being talked to
                    if (Main.player[j].active && Main.player[j].talkNPC == npc.whoAmI)
                    {
                        flag3 = true;
                        if (npc.ai[0] != 0f)
                        {
                            npc.netUpdate = true;
                        }
                        npc.ai[0] = 0f;
                        npc.ai[1] = 300f;
                        npc.localAI[3] = 100f;
                        if (Main.player[j].position.X + (float)(Main.player[j].width / 2) < npc.position.X + (float)(npc.width / 2))
                        {
                            npc.direction = -1;
                        }
                        else
                        {
                            npc.direction = 1;
                        }
                    }
                }
            }
            // Kills the old man
            if (npc.ai[3] == 1f)
            {
                npc.life = -1;
                npc.HitEffect();
                npc.active = false;
                npc.netUpdate = true;
                if (npc.type == NPCID.OldMan)
                {
                    //SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                }
                return;
            }
            // Old man always gets killed when you have defeated skeletron
            if (npc.type == NPCID.OldMan && Main.netMode != 1)
            {
                npc.UpdateHomeTileState(homeless: false, Main.dungeonX, Main.dungeonY);
                if (NPC.downedBoss3)
                {
                    npc.ai[3] = 1f;
                    npc.netUpdate = true;
                }
            }
            if (npc.type == NPCID.TravellingMerchant)
            {
                npc.homeless = true;
                if (!Main.dayTime)
                {
                    if (!npc.shimmering)
                    {
                        npc.UpdateHomeTileState(npc.homeless, (int)(npc.Center.X / 16f), (int)(npc.position.Y + (float)npc.height + 2f) / 16);
                    }
                    if (!flag3 && npc.ai[0] == 0f)
                    {
                        npc.ai[0] = 1f;
                        npc.ai[1] = 200f;
                    }
                    shouldStayInside = false;
                }
            }
            // Angler walks away from the ocean if homeless and in the water
            if (npc.type == NPCID.Angler && npc.homeless && npc.wet)
            {
                if (npc.Center.X / 16f < 380f || npc.Center.X / 16f > (float)(Main.maxTilesX - 380))
                {
                    npc.UpdateHomeTileState(npc.homeless, Main.spawnTileX, Main.spawnTileY);
                    npc.ai[0] = 1f;
                    npc.ai[1] = 200f;
                }
                if (npc.position.X / 16f < 300f)
                {
                    npc.direction = 1;
                }
                else if (npc.position.X / 16f > (float)(Main.maxTilesX - 300))
                {
                    npc.direction = -1;
                }
            }
            if (!WorldGen.InWorld(num6, num7) || (Main.netMode == 1 && !Main.sectionManager.TileLoaded(num6, num7)))
            {
                return;
            }
            if (!npc.homeless && Main.netMode != 1 && npc.townNPC && (shouldStayInside || (npc.type == NPCID.OldMan && Main.tileDungeon[Main.tile[num6, num7].TileType])) && !AI_007_TownEntities_IsInAGoodRestingSpot(npc, num6, num7, floorX, floorY))
            {
                bool shouldTeleportHome = true;
                for (int k = 0; k < 2; k++)
                {
                    if (!shouldTeleportHome)
                    {
                        break;
                    }
                    Rectangle rectangle = new Rectangle((int)(npc.position.X + (float)(npc.width / 2) - (float)(NPC.sWidth / 2) - (float)NPC.safeRangeX), (int)(npc.position.Y + (float)(npc.height / 2) - (float)(NPC.sHeight / 2) - (float)NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                    if (k == 1)
                    {
                        rectangle = new Rectangle(floorX * 16 + 8 - NPC.sWidth / 2 - NPC.safeRangeX, floorY * 16 + 8 - NPC.sHeight / 2 - NPC.safeRangeY, NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                    }
                    for (int l = 0; l < 255; l++)
                    {
                        if (Main.player[l].active && new Rectangle((int)Main.player[l].position.X, (int)Main.player[l].position.Y, Main.player[l].width, Main.player[l].height).Intersects(rectangle))
                        {
                            shouldTeleportHome = false;
                            break;
                        }
                    }
                }
                if (shouldTeleportHome)
                {
                    AI_007_TownEntities_TeleportToHome(npc, floorX, floorY);
                }
            }
            bool isMouseOrRat = npc.type == NPCID.Mouse || npc.type == NPCID.GoldMouse || npc.type == NPCID.Rat;
            bool isTurtle = npc.type == NPCID.Turtle || npc.type == NPCID.TurtleJungle || npc.type == NPCID.SeaTurtle;
            bool isFrogOrYellowTownSlime = npc.type == NPCID.Frog || npc.type == NPCID.GoldFrog || npc.type == NPCID.BoundTownSlimeYellow;
            bool flag8 = NPCID.Sets.IsTownSlime[npc.type];
            _ = NPCID.Sets.IsTownPet[npc.type];
            bool flag9 = isTurtle || isFrogOrYellowTownSlime;
            bool flag10 = isTurtle || isFrogOrYellowTownSlime;
            bool flag11 = flag8;
            bool flag12 = flag8;
            float num8 = 200f;
            if (NPCID.Sets.DangerDetectRange[npc.type] != -1)
            {
                num8 = NPCID.Sets.DangerDetectRange[npc.type];
            }
            bool flag13 = false;
            bool flag14 = false;
            float num9 = -1f;
            float num10 = -1f;
            int num11 = 0;
            int num12 = -1;
            int num13 = -1;
            bool keepwalking;
            if (!isTurtle && Main.netMode != 1 && !flag3)
            {
                for (int m = 0; m < 200; m++)
                {
                    if (!Main.npc[m].active || Main.npc[m].friendly || Main.npc[m].damage <= 0 || !(Main.npc[m].Distance(npc.Center) < num8) || (npc.type == NPCID.SkeletonMerchant && NPCID.Sets.Skeletons[Main.npc[m].type]) || (!Main.npc[m].noTileCollide && !Collision.CanHit(npc.Center, 0, 0, Main.npc[m].Center, 0, 0)) || !NPCLoader.CanHitNPC(Main.npc[m], npc))
                    {
                        continue;
                    }
                    bool flag15 = Main.npc[m].CanBeChasedBy(npc);
                    flag13 = true;
                    float num14 = Main.npc[m].Center.X - npc.Center.X;
                    if (npc.type == 614)
                    {
                        if (num14 < 0f && (num9 == -1f || num14 > num9))
                        {
                            num10 = num14;
                            num13 = m;
                        }
                        if (num14 > 0f && (num10 == -1f || num14 < num10))
                        {
                            num9 = num14;
                            num12 = m;
                        }
                        continue;
                    }
                    if (num14 < 0f && (num9 == -1f || num14 > num9))
                    {
                        num9 = num14;
                        if (flag15)
                        {
                            num12 = m;
                        }
                    }
                    if (num14 > 0f && (num10 == -1f || num14 < num10))
                    {
                        num10 = num14;
                        if (flag15)
                        {
                            num13 = m;
                        }
                    }
                }
                if (flag13)
                {
                    num11 = ((num9 == -1f) ? 1 : ((num10 != -1f) ? (num10 < 0f - num9).ToDirectionInt() : (-1)));
                    float num15 = 0f;
                    if (num9 != -1f)
                    {
                        num15 = 0f - num9;
                    }
                    if (num15 == 0f || (num10 < num15 && num10 > 0f))
                    {
                        num15 = num10;
                    }
                    if (npc.ai[0] == 8f)
                    {
                        if (npc.direction == -num11)
                        {
                            npc.ai[0] = 1f;
                            npc.ai[1] = 300 + Main.rand.Next(300);
                            npc.ai[2] = 0f;
                            npc.localAI[3] = 0f;
                            npc.netUpdate = true;
                        }
                    }
                    else if (npc.ai[0] != 10f && npc.ai[0] != 12f && npc.ai[0] != 13f && npc.ai[0] != 14f && npc.ai[0] != 15f)
                    {
                        if (NPCID.Sets.PrettySafe[npc.type] != -1 && (float)NPCID.Sets.PrettySafe[npc.type] < num15)
                        {
                            flag13 = false;
                            flag14 = NPCID.Sets.AttackType[npc.type] > -1;
                        }
                        else if (npc.ai[0] != 1f)
                        {
                            int tileX = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                            int tileY = (int)((npc.position.Y + (float)npc.height - 16f) / 16f);
                            bool currentlyDrowning = npc.wet && !flag9;
                            AI_007_TownEntities_GetWalkPrediction(npc, num6, floorX, flag9, currentlyDrowning, tileX, tileY, out keepwalking, out var avoidFalling);
                            if (!avoidFalling)
                            {
                                if (npc.ai[0] == 3f || npc.ai[0] == 4f || npc.ai[0] == 16f || npc.ai[0] == 17f)
                                {
                                    NPC nPC = Main.npc[(int)npc.ai[2]];
                                    if (nPC.active)
                                    {
                                        nPC.ai[0] = 1f;
                                        nPC.ai[1] = 120 + Main.rand.Next(120);
                                        nPC.ai[2] = 0f;
                                        nPC.localAI[3] = 0f;
                                        nPC.direction = -num11;
                                        nPC.netUpdate = true;
                                    }
                                }
                                npc.ai[0] = 1f;
                                npc.ai[1] = 120 + Main.rand.Next(120);
                                npc.ai[2] = 0f;
                                npc.localAI[3] = 0f;
                                npc.direction = -num11;
                                npc.netUpdate = true;
                            }
                        }
                        else if (npc.ai[0] == 1f && npc.direction != -num11)
                        {
                            npc.direction = -num11;
                            npc.netUpdate = true;
                        }
                    }
                }
            }
            if (npc.ai[0] == 0f)
            {
                if (npc.localAI[3] > 0f)
                {
                    npc.localAI[3] -= 1f;
                }
                int num16 = 120;
                if (npc.type == NPCID.TownDog)
                {
                    num16 = 60;
                }
                if ((isFrogOrYellowTownSlime || flag8) && npc.wet)
                {
                    npc.ai[0] = 1f;
                    npc.ai[1] = 200 + Main.rand.Next(500, 700);
                    npc.ai[2] = 0f;
                    npc.localAI[3] = 0f;
                    npc.netUpdate = true;
                }
                else if (shouldStayInside && !flag3 && !NPCID.Sets.TownCritter[npc.type])
                {
                    if (Main.netMode != 1)
                    {
                        if (num6 == floorX && num7 == floorY)
                        {
                            if (npc.velocity.X != 0f)
                            {
                                npc.netUpdate = true;
                            }
                            if (npc.velocity.X > 0.1f)
                            {
                                npc.velocity.X -= 0.1f;
                            }
                            else if (npc.velocity.X < -0.1f)
                            {
                                npc.velocity.X += 0.1f;
                            }
                            else
                            {
                                npc.velocity.X = 0f;
                                AI_007_TryForcingSitting(npc, floorX, floorY);
                            }
                            if (NPCID.Sets.IsTownPet[npc.type])
                            {
                                AI_007_AttemptToPlayIdleAnimationsForPets(npc, num16 * 4);
                            }
                        }
                        // Switch to walking
                        else
                        {
                            if (num6 > floorX)
                            {
                                npc.direction = -1;
                            }
                            else
                            {
                                npc.direction = 1;
                            }
                            npc.ai[0] = 1f;
                            npc.ai[1] = 200 + Main.rand.Next(200);
                            npc.ai[2] = 0f;
                            npc.localAI[3] = 0f;
                            npc.netUpdate = true;
                        }
                    }
                }
                else
                {
                    if (isMouseOrRat)
                    {
                        npc.velocity.X *= 0.5f;
                    }
                    if (npc.velocity.X > 0.1f)
                    {
                        npc.velocity.X -= 0.1f;
                    }
                    else if (npc.velocity.X < -0.1f)
                    {
                        npc.velocity.X += 0.1f;
                    }
                    else
                    {
                        npc.velocity.X = 0f;
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (!flag3 && NPCID.Sets.IsTownPet[npc.type] && npc.ai[1] >= 100f && npc.ai[1] <= 150f)
                        {
                            AI_007_AttemptToPlayIdleAnimationsForPets(npc, num16);
                        }
                        // Reducing the counter for whatever he is doing, walking etc
                        if (npc.ai[1] > 0f)
                        {
                            npc.ai[1] -= 1f;
                        }
                        bool flag16 = true;
                        int tileX2 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                        int tileY2 = (int)((npc.position.Y + (float)npc.height - 16f) / 16f);
                        bool currentlyDrowning2 = npc.wet && !flag9;
                        AI_007_TownEntities_GetWalkPrediction(npc, num6, floorX, flag9, currentlyDrowning2, tileX2, tileY2, out keepwalking, out var avoidFalling2);
                        if (npc.wet && !flag9)
                        {
                            bool currentlyDrowning3 = Collision.DrownCollision(npc.position, npc.width, npc.height, 1f, includeSlopes: true);
                            // Keep walking if drowning
                            if (AI_007_TownEntities_CheckIfWillDrown(currentlyDrowning3))
                            {
                                npc.ai[0] = 1f;
                                npc.ai[1] = 200 + Main.rand.Next(300);
                                npc.ai[2] = 0f;
                                if (NPCID.Sets.TownCritter[npc.type])
                                {
                                    npc.ai[1] += Main.rand.Next(200, 400);
                                }
                                npc.localAI[3] = 0f;
                                npc.netUpdate = true;
                            }
                        }
                        if (avoidFalling2)
                        {
                            flag16 = false;
                        }
                        if (npc.ai[1] <= 0f)
                        {
                            // Switch to walk state
                            if (flag16 && !avoidFalling2)
                            {
                                npc.ai[0] = 1f;
                                npc.ai[1] = 200 + Main.rand.Next(300);
                                npc.ai[2] = 0f;
                                if (NPCID.Sets.TownCritter[npc.type])
                                {
                                    npc.ai[1] += Main.rand.Next(200, 400);
                                }
                                npc.localAI[3] = 0f;
                                npc.netUpdate = true;
                            }
                            else
                            {
                                npc.direction *= -1;
                                npc.ai[1] = 60 + Main.rand.Next(120);
                                npc.netUpdate = true;
                            }
                        }
                    }
                }
                if (Main.netMode != 1 && (!shouldStayInside || AI_007_TownEntities_IsInAGoodRestingSpot(npc, num6, num7, floorX, floorY)))
                {
                    if (num6 < floorX - 25 || num6 > floorX + 25)
                    {
                        if (npc.localAI[3] == 0f)
                        {
                            if (num6 < floorX - 50 && npc.direction == -1)
                            {
                                npc.direction = 1;
                                npc.netUpdate = true;
                            }
                            else if (num6 > floorX + 50 && npc.direction == 1)
                            {
                                npc.direction = -1;
                                npc.netUpdate = true;
                            }
                        }
                    }
                    else if (Main.rand.Next(80) == 0 && npc.localAI[3] == 0f)
                    {
                        npc.localAI[3] = 200f;
                        npc.direction *= -1;
                        npc.netUpdate = true;
                    }
                }
            }
            // Walking
            else if (npc.ai[0] == 1f)
            {
                if (Main.netMode != 1 && shouldStayInside && AI_007_TownEntities_IsInAGoodRestingSpot(npc, num6, num7, floorX, floorY) && !NPCID.Sets.TownCritter[npc.type])
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] = 200 + Main.rand.Next(200);
                    npc.localAI[3] = 60f;
                    npc.netUpdate = true;
                }
                else
                {
                    bool flag17 = !flag9 && Collision.DrownCollision(npc.position, npc.width, npc.height, 1f, includeSlopes: true);
                    if (!flag17)
                    {
                        if (Main.netMode != 1 && !npc.homeless && !Main.tileDungeon[Main.tile[num6, num7].TileType] && (num6 < floorX - 35 || num6 > floorX + 35))
                        {
                            if (npc.position.X < (float)(floorX * 16) && npc.direction == -1)
                            {
                                npc.ai[1] -= 5f;
                            }
                            else if (npc.position.X > (float)(floorX * 16) && npc.direction == 1)
                            {
                                npc.ai[1] -= 5f;
                            }
                        }
                        npc.ai[1] -= 1f;
                    }
                    if (npc.ai[1] <= 0f)
                    {
                        npc.ai[0] = 0f;
                        npc.ai[1] = 300 + Main.rand.Next(300);
                        npc.ai[2] = 0f;
                        if (NPCID.Sets.TownCritter[npc.type])
                        {
                            npc.ai[1] -= Main.rand.Next(100);
                        }
                        else
                        {
                            npc.ai[1] += Main.rand.Next(900);
                        }
                        npc.localAI[3] = 60f;
                        npc.netUpdate = true;
                    }
                    if (npc.closeDoor && ((npc.position.X + (float)(npc.width / 2)) / 16f > (float)(npc.doorX + 2) || (npc.position.X + (float)(npc.width / 2)) / 16f < (float)(npc.doorX - 2)))
                    {
                        Tile tileSafely = Framing.GetTileSafely(npc.doorX, npc.doorY);
                        if (TileLoader.CloseDoorID(tileSafely) >= 0)
                        {
                            if (WorldGen.CloseDoor(npc.doorX, npc.doorY))
                            {
                                npc.closeDoor = false;
                                NetMessage.SendData(19, -1, -1, null, 1, npc.doorX, npc.doorY, npc.direction);
                            }
                            if ((npc.position.X + (float)(npc.width / 2)) / 16f > (float)(npc.doorX + 4) || (npc.position.X + (float)(npc.width / 2)) / 16f < (float)(npc.doorX - 4) || (npc.position.Y + (float)(npc.height / 2)) / 16f > (float)(npc.doorY + 4) || (npc.position.Y + (float)(npc.height / 2)) / 16f < (float)(npc.doorY - 4))
                            {
                                npc.closeDoor = false;
                            }
                        }
                        else if (tileSafely.TileType == TileID.TallGateOpen)
                        {
                            if (WorldGen.ShiftTallGate(npc.doorX, npc.doorY, closing: true))
                            {
                                npc.closeDoor = false;
                                NetMessage.SendData(19, -1, -1, null, 5, npc.doorX, npc.doorY);
                            }
                            if ((npc.position.X + (float)(npc.width / 2)) / 16f > (float)(npc.doorX + 4) || (npc.position.X + (float)(npc.width / 2)) / 16f < (float)(npc.doorX - 4) || (npc.position.Y + (float)(npc.height / 2)) / 16f > (float)(npc.doorY + 4) || (npc.position.Y + (float)(npc.height / 2)) / 16f < (float)(npc.doorY - 4))
                            {
                                npc.closeDoor = false;
                            }
                        }
                        else
                        {
                            npc.closeDoor = false;
                        }
                    }
                    float movementSpeed = 1f;
                    float acceleration = 0.07f;
                    if (npc.type == NPCID.ExplosiveBunny && flag13)
                    {
                        movementSpeed = 1.5f;
                        acceleration = 0.1f;
                    }
                    else if (npc.type == NPCID.Squirrel || npc.type == NPCID.SquirrelGold || npc.type == 538 || (npc.type >= 639 && npc.type <= 645))
                    {
                        movementSpeed = 1.5f;
                    }
                    else if (isTurtle)
                    {
                        if (npc.wet)
                        {
                            acceleration = 1f;
                            movementSpeed = 2f;
                        }
                        else
                        {
                            acceleration = 0.07f;
                            movementSpeed = 0.5f;
                        }
                    }
                    if (npc.type == NPCID.SeaTurtle)
                    {
                        if (npc.wet)
                        {
                            acceleration = 1f;
                            movementSpeed = 2.5f;
                        }
                        else
                        {
                            acceleration = 0.07f;
                            movementSpeed = 0.2f;
                        }
                    }
                    if (isMouseOrRat)
                    {
                        movementSpeed = 2f;
                        acceleration = 1f;
                    }
                    if (npc.friendly && (flag13 || flag17))
                    {
                        movementSpeed = 1.5f;
                        float num19 = 1f - (float)npc.life / (float)npc.lifeMax;
                        movementSpeed += num19 * 0.9f;
                        acceleration = 0.1f;
                    }
                    if (flag11 && npc.wet)
                    {
                        movementSpeed = 2f;
                        acceleration = 0.2f;
                    }
                    if (isFrogOrYellowTownSlime && npc.wet)
                    {
                        if (Math.Abs(npc.velocity.X) < 0.05f && Math.Abs(npc.velocity.Y) < 0.05f)
                        {
                            npc.velocity.X += movementSpeed * 10f * (float)npc.direction;
                        }
                        else
                        {
                            npc.velocity.X *= 0.9f;
                        }
                    }
                    else if (npc.velocity.X < 0f - movementSpeed || npc.velocity.X > movementSpeed)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity *= 0.8f;
                        }
                    }
                    else if (npc.velocity.X < movementSpeed && npc.direction == 1)
                    {
                        npc.velocity.X += acceleration;
                        if (npc.velocity.X > movementSpeed)
                        {
                            npc.velocity.X = movementSpeed;
                        }
                    }
                    else if (npc.velocity.X > 0f - movementSpeed && npc.direction == -1)
                    {
                        npc.velocity.X -= acceleration;
                        if (npc.velocity.X > movementSpeed)
                        {
                            npc.velocity.X = movementSpeed;
                        }
                    }
                    bool flag18 = true;
                    if ((float)(npc.homeTileY * 16 - 32) > npc.position.Y)
                    {
                        flag18 = false;
                    }
                    if (!flag18 && npc.velocity.Y == 0f)
                    {
                        Collision.StepDown(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY);
                    }
                    if (npc.velocity.Y >= 0f)
                    {
                        Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY, 1, flag18, 1);
                    }
                    if (npc.velocity.Y == 0f)
                    {
                        int num20 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                        int num21 = (int)((npc.position.Y + (float)npc.height - 16f) / 16f);
                        int num22 = 180;
                        AI_007_TownEntities_GetWalkPrediction(npc, num6, floorX, flag9, flag17, num20, num21, out var keepwalking3, out var avoidFalling3);
                        bool flag19 = false;
                        bool flag20 = false;
                        if (npc.wet && !flag9 && npc.townNPC && (flag20 = AI_007_TownEntities_CheckIfWillDrown(flag17)) && npc.localAI[3] <= 0f)
                        {
                            avoidFalling3 = true;
                            npc.localAI[3] = num22;
                            int num23 = 0;
                            for (int n = 0; n <= 10 && Framing.GetTileSafely(num20 - npc.direction, num21 - n).LiquidAmount != 0; n++)
                            {
                                num23++;
                            }
                            float num24 = 0.3f;
                            float num25 = (float)Math.Sqrt((float)(num23 * 16 + 16) * 2f * num24);
                            if (num25 > 26f)
                            {
                                num25 = 26f;
                            }
                            npc.velocity.Y = 0f - num25;
                            npc.localAI[3] = npc.position.X;
                            flag19 = true;
                        }
                        if (avoidFalling3 && !flag19)
                        {
                            int num26 = (int)((npc.position.X + (float)(npc.width / 2)) / 16f);
                            int num27 = 0;
                            for (int num28 = -1; num28 <= 1; num28++)
                            {
                                Tile tileSafely2 = Framing.GetTileSafely(num26 + num28, num21 + 1);
                                if (tileSafely2.HasUnactuatedTile && Main.tileSolid[tileSafely2.TileType])
                                {
                                    num27++;
                                }
                            }
                            if (num27 <= 2)
                            {
                                if (npc.velocity.X != 0f)
                                {
                                    npc.netUpdate = true;
                                }
                                keepwalking3 = (avoidFalling3 = false);
                                npc.ai[0] = 0f;
                                npc.ai[1] = 50 + Main.rand.Next(50);
                                npc.ai[2] = 0f;
                                npc.localAI[3] = 40f;
                            }
                        }
                        if (npc.position.X == npc.localAI[3] && !flag19)
                        {
                            npc.direction *= -1;
                            npc.netUpdate = true;
                            npc.localAI[3] = num22;
                        }
                        if (flag17 && !flag19)
                        {
                            if (npc.localAI[3] > (float)num22)
                            {
                                npc.localAI[3] = num22;
                            }
                            if (npc.localAI[3] > 0f)
                            {
                                npc.localAI[3] -= 1f;
                            }
                        }
                        else
                        {
                            npc.localAI[3] = -1f;
                        }
                        Tile tileSafely3 = Framing.GetTileSafely(num20, num21);
                        Tile tileSafely4 = Framing.GetTileSafely(num20, num21 - 1);
                        Tile tileSafely5 = Framing.GetTileSafely(num20, num21 - 2);
                        bool flag21 = npc.height / 16 < 3;
                        if ((npc.townNPC || NPCID.Sets.AllowDoorInteraction[npc.type]) && tileSafely5.HasUnactuatedTile && (TileLoader.IsClosedDoor(tileSafely5) || tileSafely5.TileType == 388) && (Main.rand.Next(10) == 0 || shouldStayInside))
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                if (WorldGen.OpenDoor(num20, num21 - 2, npc.direction))
                                {
                                    npc.closeDoor = true;
                                    npc.doorX = num20;
                                    npc.doorY = num21 - 2;
                                    NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, num20, num21 - 2, npc.direction);
                                    npc.netUpdate = true;
                                    npc.ai[1] += 80f;
                                }
                                else if (WorldGen.OpenDoor(num20, num21 - 2, -npc.direction))
                                {
                                    npc.closeDoor = true;
                                    npc.doorX = num20;
                                    npc.doorY = num21 - 2;
                                    NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, num20, num21 - 2, -npc.direction);
                                    npc.netUpdate = true;
                                    npc.ai[1] += 80f;
                                }
                                else if (WorldGen.ShiftTallGate(num20, num21 - 2, closing: false))
                                {
                                    npc.closeDoor = true;
                                    npc.doorX = num20;
                                    npc.doorY = num21 - 2;
                                    NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 4, num20, num21 - 2);
                                    npc.netUpdate = true;
                                    npc.ai[1] += 80f;
                                }
                                else
                                {
                                    npc.direction *= -1;
                                    npc.netUpdate = true;
                                }
                            }
                        }
                        else
                        {
                            if ((npc.velocity.X < 0f && npc.direction == -1) || (npc.velocity.X > 0f && npc.direction == 1))
                            {
                                bool flag22 = false;
                                bool flag23 = false;
                                if (tileSafely5.HasUnactuatedTile && Main.tileSolid[tileSafely5.TileType] && !Main.tileSolidTop[tileSafely5.TileType] && (!flag21 || (tileSafely4.HasUnactuatedTile && Main.tileSolid[tileSafely4.TileType] && !Main.tileSolidTop[tileSafely4.TileType])))
                                {
                                    if (!Collision.SolidTilesVersatile(num20 - npc.direction * 2, num20 - npc.direction, num21 - 5, num21 - 1) && !Collision.SolidTiles(num20, num20, num21 - 5, num21 - 3))
                                    {
                                        npc.velocity.Y = -6f;
                                        npc.netUpdate = true;
                                    }
                                    else if (isMouseOrRat)
                                    {
                                        if (WorldGen.SolidTile((int)(npc.Center.X / 16f) + npc.direction, (int)(npc.Center.Y / 16f)))
                                        {
                                            npc.direction *= -1;
                                            npc.velocity.X *= 0f;
                                            npc.netUpdate = true;
                                        }
                                    }
                                    else if (flag13)
                                    {
                                        flag23 = true;
                                        flag22 = true;
                                    }
                                    else if (!flag20)
                                    {
                                        flag22 = true;
                                    }
                                }
                                else if (tileSafely4.HasUnactuatedTile && Main.tileSolid[tileSafely4.TileType] && !Main.tileSolidTop[tileSafely4.TileType])
                                {
                                    if (!Collision.SolidTilesVersatile(num20 - npc.direction * 2, num20 - npc.direction, num21 - 4, num21 - 1) && !Collision.SolidTiles(num20, num20, num21 - 4, num21 - 2))
                                    {
                                        npc.velocity.Y = -5f;
                                        npc.netUpdate = true;
                                    }
                                    else if (flag13)
                                    {
                                        flag23 = true;
                                        flag22 = true;
                                    }
                                    else
                                    {
                                        flag22 = true;
                                    }
                                }
                                else if (npc.position.Y + (float)npc.height - (float)(num21 * 16) > 20f && tileSafely3.HasUnactuatedTile && Main.tileSolid[tileSafely3.TileType] && !tileSafely3.TopSlope)
                                {
                                    if (!Collision.SolidTilesVersatile(num20 - npc.direction * 2, num20, num21 - 3, num21 - 1))
                                    {
                                        npc.velocity.Y = -4.4f;
                                        npc.netUpdate = true;
                                    }
                                    else if (flag13)
                                    {
                                        flag23 = true;
                                        flag22 = true;
                                    }
                                    else
                                    {
                                        flag22 = true;
                                    }
                                }
                                else if (avoidFalling3)
                                {
                                    if (!flag20)
                                    {
                                        flag22 = true;
                                    }
                                    if (flag13)
                                    {
                                        flag23 = true;
                                    }
                                }
                                else if (flag12 && !Collision.SolidTilesVersatile(num20 - npc.direction * 2, num20 - npc.direction, num21 - 2, num21 - 1))
                                {
                                    npc.velocity.Y = -5f;
                                    npc.netUpdate = true;
                                }
                                if (flag23)
                                {
                                    keepwalking3 = false;
                                    npc.velocity.X = 0f;
                                    npc.ai[0] = 8f;
                                    npc.ai[1] = 240f;
                                    npc.netUpdate = true;
                                }
                                if (flag22)
                                {
                                    npc.direction *= -1;
                                    npc.velocity.X *= -1f;
                                    npc.netUpdate = true;
                                }
                                if (keepwalking3)
                                {
                                    npc.ai[1] = 90f;
                                    npc.netUpdate = true;
                                }
                                if (npc.velocity.Y < 0f)
                                {
                                    npc.localAI[3] = npc.position.X;
                                }
                            }
                            if (npc.velocity.Y < 0f && npc.wet)
                            {
                                npc.velocity.Y *= 1.2f;
                            }
                            if (npc.velocity.Y < 0f && NPCID.Sets.TownCritter[npc.type] && !isMouseOrRat)
                            {
                                npc.velocity.Y *= 1.2f;
                            }
                        }
                    }
                    else if (flag12 && !npc.wet)
                    {
                        int num29 = (int)(npc.Center.X / 16f);
                        int num30 = (int)((npc.position.Y + (float)npc.height - 16f) / 16f);
                        int num31 = 0;
                        for (int num32 = -1; num32 <= 1; num32++)
                        {
                            for (int num33 = 1; num33 <= 6; num33++)
                            {
                                Tile tileSafely6 = Framing.GetTileSafely(num29 + num32, num30 + num33);
                                if (tileSafely6.LiquidAmount > 0 || (tileSafely6.HasUnactuatedTile && Main.tileSolid[tileSafely6.TileType]))
                                {
                                    num31++;
                                }
                            }
                        }
                        if (num31 <= 2)
                        {
                            if (npc.velocity.X != 0f)
                            {
                                npc.netUpdate = true;
                            }
                            npc.velocity.X *= 0.2f;
                            npc.ai[0] = 0f;
                            npc.ai[1] = 50 + Main.rand.Next(50);
                            npc.ai[2] = 0f;
                            npc.localAI[3] = 40f;
                        }
                    }
                }
            }
            else if (npc.ai[0] == 2f || npc.ai[0] == 11f)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.localAI[3] -= 1f;
                    if (Main.rand.Next(60) == 0 && npc.localAI[3] == 0f)
                    {
                        npc.localAI[3] = 60f;
                        npc.direction *= -1;
                        npc.netUpdate = true;
                    }
                }
                npc.ai[1] -= 1f;
                npc.velocity.X *= 0.8f;
                if (npc.ai[1] <= 0f)
                {
                    npc.localAI[3] = 40f;
                    npc.ai[0] = 0f;
                    npc.ai[1] = 60 + Main.rand.Next(60);
                    npc.netUpdate = true;
                }
            }
            // handles switching back to normal. Don't forget to add any custom state to this!
            else if (npc.ai[0] == 3f || npc.ai[0] == 4f || npc.ai[0] == 5f || npc.ai[0] == 8f || npc.ai[0] == 9f || npc.ai[0] == 16f || npc.ai[0] == 17f || npc.ai[0] == 20f || npc.ai[0] == 21f || npc.ai[0] == 22f || npc.ai[0] == 23f || npc.ai[0] == 30f)
            {
                npc.velocity.X *= 0.8f;
                npc.ai[1] -= 1f;
                if (npc.ai[0] == 8f && npc.ai[1] < 60f && flag13)
                {
                    npc.ai[1] = 180f;
                    npc.netUpdate = true;
                }
                // Sitting
                if (npc.ai[0] == 5f)
                {
                    Point coords = (npc.Bottom + Vector2.UnitY * -2f).ToTileCoordinates();
                    Tile tile = Main.tile[coords.X, coords.Y];
                    if (!TileID.Sets.CanBeSatOnForNPCs[tile.TileType])
                    {
                        npc.ai[1] = 0f;
                    }
                    else
                    {
                        Main.sittingManager.AddNPC(npc.whoAmI, coords);
                    }
                }
                if (npc.ai[1] <= 0f)
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] = 60 + Main.rand.Next(60);
                    npc.ai[2] = 0f;
                    npc.localAI[3] = 30 + Main.rand.Next(60);
                    npc.netUpdate = true;
                }
            }
            else if (npc.ai[0] == 6f || npc.ai[0] == 7f || npc.ai[0] == 18f || npc.ai[0] == 19f)
            {
                if (npc.ai[0] == 18f && (npc.localAI[3] < 1f || npc.localAI[3] > 2f))
                {
                    npc.localAI[3] = 2f;
                }
                npc.velocity.X *= 0.8f;
                npc.ai[1] -= 1f;
                int num34 = (int)npc.ai[2];
                if (num34 < 0 || num34 > 255 || !Main.player[num34].CanBeTalkedTo || Main.player[num34].Distance(npc.Center) > 200f || !Collision.CanHitLine(npc.Top, 0, 0, Main.player[num34].Top, 0, 0))
                {
                    npc.ai[1] = 0f;
                }
                if (npc.ai[1] > 0f)
                {
                    int num35 = ((npc.Center.X < Main.player[num34].Center.X) ? 1 : (-1));
                    if (num35 != npc.direction)
                    {
                        npc.netUpdate = true;
                    }
                    npc.direction = num35;
                }
                else
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] = 60 + Main.rand.Next(60);
                    npc.ai[2] = 0f;
                    npc.localAI[3] = 30 + Main.rand.Next(60);
                    npc.netUpdate = true;
                }
            }
            // Attack throw projectile
            else if (npc.ai[0] == 10f)
            {
                int npcProjectileType = 0;
                int num37 = 0;
                float knockBack = 0f;
                float num38 = 0f;
                int num39 = 0;
                int num40 = 0;
                int maxValue = 0;
                float num41 = 0f;
                float num42 = NPCID.Sets.DangerDetectRange[npc.type];
                float num43 = 0f;
                if ((float)NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
                {
                    npc.frameCounter = 0.0;
                    npc.localAI[3] = 0f;
                }
                if (npc.type == NPCID.Demolitionist)
                {
                    npcProjectileType = 30;
                    num38 = 6f;
                    num37 = 20;
                    num39 = 10;
                    num40 = 180;
                    maxValue = 120;
                    num41 = 16f;
                    knockBack = 7f;
                }
                else if (npc.type == NPCID.BestiaryGirl)
                {
                    npcProjectileType = 880;
                    num38 = 24f;
                    num37 = 15;
                    num39 = 1;
                    num41 = 0f;
                    knockBack = 7f;
                    num40 = 15;
                    maxValue = 10;
                    if (npc.ShouldBestiaryGirlBeLycantrope())
                    {
                        npcProjectileType = 929;
                        num37 = (int)((float)num37 * 1.5f);
                    }
                }
                else if (npc.type == NPCID.DD2Bartender)
                {
                    npcProjectileType = 669;
                    num38 = 6f;
                    num37 = 24;
                    num39 = 10;
                    num40 = 120;
                    maxValue = 60;
                    num41 = 16f;
                    knockBack = 9f;
                }
                else if (npc.type == NPCID.Golfer)
                {
                    npcProjectileType = 721;
                    num38 = 8f;
                    num37 = 15;
                    num39 = 5;
                    num40 = 20;
                    maxValue = 10;
                    num41 = 16f;
                    knockBack = 9f;
                }
                else if (npc.type == NPCID.PartyGirl)
                {
                    npcProjectileType = 588;
                    num38 = 6f;
                    num37 = 30;
                    num39 = 10;
                    num40 = 60;
                    maxValue = 120;
                    num41 = 16f;
                    knockBack = 6f;
                }
                else if (npc.type == NPCID.Merchant)
                {
                    npcProjectileType = 48;
                    num38 = 9f;
                    num37 = 12;
                    num39 = 10;
                    num40 = 60;
                    maxValue = 60;
                    num41 = 16f;
                    knockBack = 1.5f;
                }
                else if (npc.type == NPCID.Angler)
                {
                    npcProjectileType = 520;
                    num38 = 12f;
                    num37 = 10;
                    num39 = 10;
                    num40 = 0;
                    maxValue = 1;
                    num41 = 16f;
                    knockBack = 3f;
                }
                else if (npc.type == NPCID.SkeletonMerchant)
                {
                    npcProjectileType = 21;
                    num38 = 14f;
                    num37 = 14;
                    num39 = 10;
                    num40 = 0;
                    maxValue = 1;
                    num41 = 16f;
                    knockBack = 3f;
                }
                else if (npc.type == NPCID.GoblinTinkerer)
                {
                    npcProjectileType = 24;
                    num38 = 5f;
                    num37 = 15;
                    num39 = 10;
                    num40 = 60;
                    maxValue = 60;
                    num41 = 16f;
                    knockBack = 1f;
                }
                else if (npc.type == NPCID.Mechanic)
                {
                    npcProjectileType = 582;
                    num38 = 10f;
                    num37 = 11;
                    num39 = 1;
                    num40 = 30;
                    maxValue = 30;
                    knockBack = 3.5f;
                }
                else if (npc.type == NPCID.Nurse)
                {
                    npcProjectileType = 583;
                    num38 = 8f;
                    num37 = 8;
                    num39 = 1;
                    num40 = 15;
                    maxValue = 10;
                    knockBack = 2f;
                    num41 = 10f;
                }
                else if (npc.type == NPCID.SantaClaus)
                {
                    npcProjectileType = 589;
                    num38 = 7f;
                    num37 = 22;
                    num39 = 1;
                    num40 = 10;
                    maxValue = 1;
                    knockBack = 2f;
                    num41 = 10f;
                }
                NPCLoader.TownNPCAttackStrength(npc, ref num37, ref knockBack);
                NPCLoader.TownNPCAttackCooldown(npc, ref num40, ref maxValue);
                NPCLoader.TownNPCAttackProj(npc, ref npcProjectileType, ref num39);
                NPCLoader.TownNPCAttackProjSpeed(npc, ref num38, ref num41, ref num43);
                if (Main.expertMode)
                {
                    num37 = (int)((float)num37 * Main.GameModeInfo.TownNPCDamageMultiplier);
                }
                num37 = (int)((float)num37 * num2);
                npc.velocity.X *= 0.8f;
                npc.ai[1] -= 1f;
                npc.localAI[3] += 1f;
                if (npc.localAI[3] == (float)num39 && Main.netMode != 1)
                {
                    Vector2 vec = -Vector2.UnitY;
                    if (num11 == 1 && npc.spriteDirection == 1 && num13 != -1)
                    {
                        vec = npc.DirectionTo(Main.npc[num13].Center + new Vector2(0f, (0f - num41) * MathHelper.Clamp(npc.Distance(Main.npc[num13].Center) / num42, 0f, 1f)));
                    }
                    if (num11 == -1 && npc.spriteDirection == -1 && num12 != -1)
                    {
                        vec = npc.DirectionTo(Main.npc[num12].Center + new Vector2(0f, (0f - num41) * MathHelper.Clamp(npc.Distance(Main.npc[num12].Center) / num42, 0f, 1f)));
                    }
                    if (vec.HasNaNs() || Math.Sign(vec.X) != npc.spriteDirection)
                    {
                        vec = new Vector2(npc.spriteDirection, -1f);
                    }
                    vec *= num38;
                    vec += Utils.RandomVector2(Main.rand, 0f - num43, num43);
                    int num44 = 1000;
                    num44 = ((npc.type == NPCID.Mechanic) ?
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X + (float)(npc.spriteDirection * 16), npc.Center.Y - 2f, vec.X, vec.Y, npcProjectileType, num37, knockBack, Main.myPlayer, 0f, npc.whoAmI, npc.townNpcVariationIndex)
                        : ((npc.type != NPCID.SantaClaus) ? Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X + (float)(npc.spriteDirection * 16), npc.Center.Y - 2f, vec.X, vec.Y, npcProjectileType, num37, knockBack, Main.myPlayer)
                        : Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X + (float)(npc.spriteDirection * 16), npc.Center.Y - 2f, vec.X, vec.Y, npcProjectileType, num37, knockBack, Main.myPlayer, 0f, Main.rand.Next(5))));
                    Main.projectile[num44].npcProj = true;
                    Main.projectile[num44].noDropItem = true;
                    if (npc.type == NPCID.Golfer)
                    {
                        Main.projectile[num44].timeLeft = 480;
                    }
                }
                if (npc.ai[1] <= 0f)
                {
                    npc.ai[0] = ((npc.localAI[2] == 8f && flag13) ? 8 : 0);
                    npc.ai[1] = num40 + Main.rand.Next(maxValue);
                    npc.ai[2] = 0f;
                    npc.localAI[1] = (npc.localAI[3] = num40 / 2 + Main.rand.Next(maxValue));
                    npc.netUpdate = true;
                }
            }
            // Shoot attack
            else if (npc.ai[0] == 12f)
            {
                int num45 = 0;
                int num46 = 0;
                float num47 = 0f;
                int num48 = 0;
                int num49 = 0;
                int maxValue2 = 0;
                float knockBack2 = 0f;
                float num50 = 0f;
                bool flag24 = false;
                float num51 = 0f;
                if ((float)NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
                {
                    npc.frameCounter = 0.0;
                    npc.localAI[3] = 0f;
                }
                int num52 = -1;
                if (num11 == 1 && npc.spriteDirection == 1)
                {
                    num52 = num13;
                }
                if (num11 == -1 && npc.spriteDirection == -1)
                {
                    num52 = num12;
                }
                if (npc.type == NPCID.ArmsDealer)
                {
                    num45 = 14;
                    num47 = 13f;
                    num46 = 24;
                    num49 = 14;
                    maxValue2 = 4;
                    knockBack2 = 3f;
                    num48 = 1;
                    num51 = 0.5f;
                    if ((float)NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
                    {
                        npc.frameCounter = 0.0;
                        npc.localAI[3] = 0f;
                    }
                    if (Main.hardMode)
                    {
                        num46 = 15;
                        if (npc.localAI[3] > (float)num48)
                        {
                            num48 = 10;
                            flag24 = true;
                        }
                        if (npc.localAI[3] > (float)num48)
                        {
                            num48 = 20;
                            flag24 = true;
                        }
                        if (npc.localAI[3] > (float)num48)
                        {
                            num48 = 30;
                            flag24 = true;
                        }
                    }
                }
                else if (npc.type == NPCID.Painter)
                {
                    num45 = 587;
                    num47 = 10f;
                    num46 = 8;
                    num49 = 10;
                    maxValue2 = 1;
                    knockBack2 = 1.75f;
                    num48 = 1;
                    num51 = 0.5f;
                    if (npc.localAI[3] > (float)num48)
                    {
                        num48 = 12;
                        flag24 = true;
                    }
                    if (npc.localAI[3] > (float)num48)
                    {
                        num48 = 24;
                        flag24 = true;
                    }
                    if (Main.hardMode)
                    {
                        num46 += 2;
                    }
                }
                else if (npc.type == NPCID.TravellingMerchant)
                {
                    num45 = 14;
                    num47 = 13f;
                    num46 = 24;
                    num49 = 12;
                    maxValue2 = 5;
                    knockBack2 = 2f;
                    num48 = 1;
                    num51 = 0.2f;
                    if (Main.hardMode)
                    {
                        num46 = 30;
                        num45 = 357;
                    }
                }
                else if (npc.type == NPCID.Guide)
                {
                    num47 = 10f;
                    num46 = 8;
                    num48 = 1;
                    if (Main.hardMode)
                    {
                        num45 = 2;
                        num49 = 15;
                        maxValue2 = 10;
                        num46 += 6;
                    }
                    else
                    {
                        num45 = 1;
                        num49 = 30;
                        maxValue2 = 20;
                    }
                    knockBack2 = 2.75f;
                    num50 = 4f;
                    num51 = 0.7f;
                }
                else if (npc.type == NPCID.WitchDoctor)
                {
                    num45 = 267;
                    num47 = 14f;
                    num46 = 20;
                    num48 = 1;
                    num49 = 10;
                    maxValue2 = 1;
                    knockBack2 = 3f;
                    num50 = 6f;
                    num51 = 0.4f;
                }
                else if (npc.type == NPCID.Steampunker)
                {
                    num45 = 242;
                    num47 = 13f;
                    num46 = ((!Main.hardMode) ? 11 : 15);
                    num49 = 10;
                    maxValue2 = 1;
                    knockBack2 = 2f;
                    num48 = 1;
                    if (npc.localAI[3] > (float)num48)
                    {
                        num48 = 8;
                        flag24 = true;
                    }
                    if (npc.localAI[3] > (float)num48)
                    {
                        num48 = 16;
                        flag24 = true;
                    }
                    num51 = 0.3f;
                }
                else if (npc.type == NPCID.Pirate)
                {
                    num45 = 14;
                    num47 = 14f;
                    num46 = 24;
                    num49 = 10;
                    maxValue2 = 1;
                    knockBack2 = 2f;
                    num48 = 1;
                    num51 = 0.7f;
                    if (npc.localAI[3] > (float)num48)
                    {
                        num48 = 16;
                        flag24 = true;
                    }
                    if (npc.localAI[3] > (float)num48)
                    {
                        num48 = 24;
                        flag24 = true;
                    }
                    if (npc.localAI[3] > (float)num48)
                    {
                        num48 = 32;
                        flag24 = true;
                    }
                    if (npc.localAI[3] > (float)num48)
                    {
                        num48 = 40;
                        flag24 = true;
                    }
                    if (npc.localAI[3] > (float)num48)
                    {
                        num48 = 48;
                        flag24 = true;
                    }
                    if (npc.localAI[3] == 0f && num52 != -1 && npc.Distance(Main.npc[num52].Center) < (float)NPCID.Sets.PrettySafe[npc.type])
                    {
                        num51 = 0.1f;
                        num45 = 162;
                        num46 = 50;
                        knockBack2 = 10f;
                        num47 = 24f;
                    }
                }
                else if (npc.type == NPCID.Cyborg)
                {
                    num45 = Utils.SelectRandom<int>(Main.rand, 134, 133, 135);
                    num48 = 1;
                    switch (num45)
                    {
                        case 135:
                            num47 = 12f;
                            num46 = 30;
                            num49 = 30;
                            maxValue2 = 10;
                            knockBack2 = 7f;
                            num51 = 0.2f;
                            break;
                        case 133:
                            num47 = 10f;
                            num46 = 25;
                            num49 = 10;
                            maxValue2 = 1;
                            knockBack2 = 6f;
                            num51 = 0.2f;
                            break;
                        case 134:
                            num47 = 13f;
                            num46 = 20;
                            num49 = 20;
                            maxValue2 = 10;
                            knockBack2 = 4f;
                            num51 = 0.1f;
                            break;
                    }
                }
                NPCLoader.TownNPCAttackStrength(npc, ref num46, ref knockBack2);
                NPCLoader.TownNPCAttackCooldown(npc, ref num49, ref maxValue2);
                NPCLoader.TownNPCAttackProj(npc, ref num45, ref num48);
                NPCLoader.TownNPCAttackProjSpeed(npc, ref num47, ref num50, ref num51);
                NPCLoader.TownNPCAttackShoot(npc, ref flag24);
                if (Main.expertMode)
                {
                    num46 = (int)((float)num46 * Main.GameModeInfo.TownNPCDamageMultiplier);
                }
                num46 = (int)((float)num46 * num2);
                npc.velocity.X *= 0.8f;
                npc.ai[1] -= 1f;
                npc.localAI[3] += 1f;
                if (npc.localAI[3] == (float)num48 && Main.netMode != 1)
                {
                    Vector2 vec2 = Vector2.Zero;
                    if (num52 != -1)
                    {
                        vec2 = npc.DirectionTo(Main.npc[num52].Center + new Vector2(0f, 0f - num50));
                    }
                    if (vec2.HasNaNs() || Math.Sign(vec2.X) != npc.spriteDirection)
                    {
                        vec2 = new Vector2(npc.spriteDirection, 0f);
                    }
                    vec2 *= num47;
                    vec2 += Utils.RandomVector2(Main.rand, 0f - num51, num51);
                    int num53 = 1000;
                    num53 = ((npc.type != 227) ? Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X + (float)(npc.spriteDirection * 16), npc.Center.Y - 2f, vec2.X, vec2.Y, num45, num46, knockBack2, Main.myPlayer) : Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X + (float)(npc.spriteDirection * 16), npc.Center.Y - 2f, vec2.X, vec2.Y, num45, num46, knockBack2, Main.myPlayer, 0f, (float)Main.rand.Next(12) / 6f));
                    Main.projectile[num53].npcProj = true;
                    Main.projectile[num53].noDropItem = true;
                }
                if (npc.localAI[3] == (float)num48 && flag24 && num52 != -1)
                {
                    Vector2 vector2 = npc.DirectionTo(Main.npc[num52].Center);
                    if (vector2.Y <= 0.5f && vector2.Y >= -0.5f)
                    {
                        npc.ai[2] = vector2.Y;
                    }
                }
                if (npc.ai[1] <= 0f)
                {
                    npc.ai[0] = ((npc.localAI[2] == 8f && flag13) ? 8 : 0);
                    npc.ai[1] = num49 + Main.rand.Next(maxValue2);
                    npc.ai[2] = 0f;
                    npc.localAI[1] = (npc.localAI[3] = num49 / 2 + Main.rand.Next(maxValue2));
                    npc.netUpdate = true;
                }
            }
            // Nurse throwing healing needles
            else if (npc.ai[0] == 13f)
            {
                npc.velocity.X *= 0.8f;
                if ((float)NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
                {
                    npc.frameCounter = 0.0;
                }
                npc.ai[1] -= 1f;
                npc.localAI[3] += 1f;
                if (npc.localAI[3] == 1f && Main.netMode != 1)
                {
                    Vector2 vec3 = npc.DirectionTo(Main.npc[(int)npc.ai[2]].Center + new Vector2(0f, -20f));
                    if (vec3.HasNaNs() || Math.Sign(vec3.X) == -npc.spriteDirection)
                    {
                        vec3 = new Vector2(npc.spriteDirection, -1f);
                    }
                    vec3 *= 8f;
                    int num54 = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X + (float)(npc.spriteDirection * 16), npc.Center.Y - 2f, vec3.X, vec3.Y, 584, 0, 0f, Main.myPlayer, npc.ai[2]);
                    Main.projectile[num54].npcProj = true;
                    Main.projectile[num54].noDropItem = true;
                }
                if (npc.ai[1] <= 0f)
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] = 10 + Main.rand.Next(10);
                    npc.ai[2] = 0f;
                    npc.localAI[3] = 5 + Main.rand.Next(10);
                    npc.netUpdate = true;
                }
            }
            // Magic Projectile attack
            else if (npc.ai[0] == 14f)
            {
                int num55 = 0;
                int attackDamage = 0;
                float num57 = 0f;
                int attackDelay = 0;
                int num59 = 0;
                int maxValue3 = 0;
                float knockBack3 = 0f;
                float num60 = 0f;
                float num61 = NPCID.Sets.DangerDetectRange[npc.type];
                float num62 = 1f;
                float num63 = 0f;
                if ((float)NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
                {
                    npc.frameCounter = 0.0;
                    npc.localAI[3] = 0f;
                }
                int num64 = -1;
                if (num11 == 1 && npc.spriteDirection == 1)
                {
                    num64 = num13;
                }
                if (num11 == -1 && npc.spriteDirection == -1)
                {
                    num64 = num12;
                }
                if (npc.type == NPCID.Clothier)
                {
                    num55 = 585;
                    num57 = 10f;
                    attackDamage = 16;
                    attackDelay = 30;
                    num59 = 20;
                    maxValue3 = 15;
                    knockBack3 = 2f;
                    num63 = 1f;
                }
                else if (npc.type == NPCID.Wizard)
                {
                    num55 = 15;
                    num57 = 6f;
                    attackDamage = 18;
                    attackDelay = 15;
                    num59 = 15;
                    maxValue3 = 5;
                    knockBack3 = 3f;
                    num60 = 20f;
                }
                else if (npc.type == NPCID.Truffle)
                {
                    num55 = 590;
                    attackDamage = 40;
                    attackDelay = 15;
                    num59 = 10;
                    maxValue3 = 1;
                    knockBack3 = 3f;
                    for (; npc.localAI[3] > (float)attackDelay; attackDelay += 15)
                    {
                    }
                }
                else if (npc.type == NPCID.Princess)
                {
                    num55 = 950;
                    attackDamage = ((!Main.hardMode) ? 15 : 20);
                    attackDelay = 15;
                    num59 = 0;
                    maxValue3 = 0;
                    knockBack3 = 3f;
                    for (; npc.localAI[3] > (float)attackDelay; attackDelay += 10)
                    {
                    }
                }
                else if (npc.type == NPCID.Dryad)
                {
                    num55 = 586;
                    attackDelay = 24;
                    num59 = 10;
                    maxValue3 = 1;
                    knockBack3 = 3f;
                }
                NPCLoader.TownNPCAttackStrength(npc, ref attackDamage, ref knockBack3);
                NPCLoader.TownNPCAttackCooldown(npc, ref num59, ref maxValue3);
                NPCLoader.TownNPCAttackProj(npc, ref num55, ref attackDelay);
                NPCLoader.TownNPCAttackProjSpeed(npc, ref num57, ref num60, ref num63);
                NPCLoader.TownNPCAttackMagic(npc, ref num62);
                if (Main.expertMode)
                {
                    attackDamage = (int)((float)attackDamage * Main.GameModeInfo.TownNPCDamageMultiplier);
                }
                attackDamage = (int)((float)attackDamage * num2);
                npc.velocity.X *= 0.8f;
                npc.ai[1] -= 1f;
                npc.localAI[3] += 1f;
                if (npc.localAI[3] == (float)attackDelay && Main.netMode != 1)
                {
                    Vector2 vec4 = Vector2.Zero;
                    if (num64 != -1)
                    {
                        vec4 = npc.DirectionTo(Main.npc[num64].Center + new Vector2(0f, (0f - num60) * MathHelper.Clamp(npc.Distance(Main.npc[num64].Center) / num61, 0f, 1f)));
                    }
                    if (vec4.HasNaNs() || Math.Sign(vec4.X) != npc.spriteDirection)
                    {
                        vec4 = new Vector2(npc.spriteDirection, 0f);
                    }
                    vec4 *= num57;
                    vec4 += Utils.RandomVector2(Main.rand, 0f - num63, num63);
                    if (npc.type == NPCID.Wizard)
                    {
                        int num65 = Utils.SelectRandom<int>(Main.rand, 1, 1, 1, 1, 2, 2, 3);
                        for (int num66 = 0; num66 < num65; num66++)
                        {
                            Vector2 vector3 = Utils.RandomVector2(Main.rand, -3.4f, 3.4f);
                            int num67 = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X + (float)(npc.spriteDirection * 16), npc.Center.Y - 2f, vec4.X + vector3.X, vec4.Y + vector3.Y, num55, attackDamage, knockBack3, Main.myPlayer, 0f, 0f, npc.townNpcVariationIndex);
                            Main.projectile[num67].npcProj = true;
                            Main.projectile[num67].noDropItem = true;
                        }
                    }
                    else if (npc.type == NPCID.Truffle)
                    {
                        if (num64 != -1)
                        {
                            Vector2 vector4 = Main.npc[num64].position - Main.npc[num64].Size * 2f + Main.npc[num64].Size * Utils.RandomVector2(Main.rand, 0f, 1f) * 5f;
                            int num68 = 10;
                            while (num68 > 0 && WorldGen.SolidTile(Framing.GetTileSafely((int)vector4.X / 16, (int)vector4.Y / 16)))
                            {
                                num68--;
                                vector4 = Main.npc[num64].position - Main.npc[num64].Size * 2f + Main.npc[num64].Size * Utils.RandomVector2(Main.rand, 0f, 1f) * 5f;
                            }
                            int num69 = Projectile.NewProjectile(npc.GetSource_FromAI(), vector4.X, vector4.Y, 0f, 0f, num55, attackDamage, knockBack3, Main.myPlayer, 0f, 0f, npc.townNpcVariationIndex);
                            Main.projectile[num69].npcProj = true;
                            Main.projectile[num69].noDropItem = true;
                        }
                    }
                    else if (npc.type == NPCID.Princess)
                    {
                        if (num64 != -1)
                        {
                            Vector2 vector5 = Main.npc[num64].position + Main.npc[num64].Size * Utils.RandomVector2(Main.rand, 0f, 1f) * 1f;
                            int num70 = 5;
                            while (num70 > 0 && WorldGen.SolidTile(Framing.GetTileSafely((int)vector5.X / 16, (int)vector5.Y / 16)))
                            {
                                num70--;
                                vector5 = Main.npc[num64].position + Main.npc[num64].Size * Utils.RandomVector2(Main.rand, 0f, 1f) * 1f;
                            }
                            int num71 = Projectile.NewProjectile(npc.GetSource_FromAI(), vector5.X, vector5.Y, 0f, 0f, num55, attackDamage, knockBack3, Main.myPlayer, 0f, 0f, npc.townNpcVariationIndex);
                            Main.projectile[num71].npcProj = true;
                            Main.projectile[num71].noDropItem = true;
                        }
                    }
                    else if (npc.type == NPCID.Dryad)
                    {
                        int num72 = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X + (float)(npc.spriteDirection * 16), npc.Center.Y - 2f, vec4.X, vec4.Y, num55, attackDamage, knockBack3, Main.myPlayer, 0f, npc.whoAmI, npc.townNpcVariationIndex);
                        Main.projectile[num72].npcProj = true;
                        Main.projectile[num72].noDropItem = true;
                    }
                    else
                    {
                        int num73 = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X + (float)(npc.spriteDirection * 16), npc.Center.Y - 2f, vec4.X, vec4.Y, num55, attackDamage, knockBack3, Main.myPlayer);
                        Main.projectile[num73].npcProj = true;
                        Main.projectile[num73].noDropItem = true;
                    }
                }
                if (num62 > 0f)
                {
                    Vector3 vector6 = npc.GetMagicAuraColor().ToVector3() * num62;
                    Lighting.AddLight(npc.Center, vector6.X, vector6.Y, vector6.Z);
                }
                if (npc.ai[1] <= 0f)
                {
                    npc.ai[0] = ((npc.localAI[2] == 8f && flag13) ? 8 : 0);
                    npc.ai[1] = num59 + Main.rand.Next(maxValue3);
                    npc.ai[2] = 0f;
                    npc.localAI[1] = (npc.localAI[3] = num59 / 2 + Main.rand.Next(maxValue3));
                    npc.netUpdate = true;
                }
            }
            // Melee (Swing) attack
            else if (npc.ai[0] == 15f)
            {
                int num74 = 0;
                int maxValue4 = 0;
                if ((float)NPCID.Sets.AttackTime[npc.type] == npc.ai[1])
                {
                    npc.frameCounter = 0.0;
                    npc.localAI[3] = 0f;
                }
                int num75 = 0;
                float num76 = 0f;
                int num77 = 0;
                int num78 = 0;
                if (num11 == 1)
                {
                    _ = npc.spriteDirection;
                }
                if (num11 == -1)
                {
                    _ = npc.spriteDirection;
                }
                if (npc.type == NPCID.DyeTrader)
                {
                    num75 = 11;
                    num77 = (num78 = 32);
                    num74 = 12;
                    maxValue4 = 6;
                    num76 = 4.25f;
                }
                else if (npc.type == NPCID.TaxCollector)
                {
                    num75 = 9;
                    num77 = (num78 = 28);
                    num74 = 9;
                    maxValue4 = 3;
                    num76 = 3.5f;
                    if (npc.GivenName == "Andrew")
                    {
                        num75 *= 2;
                        num76 *= 2f;
                    }
                }
                else if (npc.type == NPCID.Stylist)
                {
                    num75 = 10;
                    num77 = (num78 = 32);
                    num74 = 15;
                    maxValue4 = 8;
                    num76 = 5f;
                }
                else if (NPCID.Sets.IsTownPet[npc.type])
                {
                    num75 = 10;
                    num77 = (num78 = 32);
                    num74 = 15;
                    maxValue4 = 8;
                    num76 = 3f;
                }
                NPCLoader.TownNPCAttackStrength(npc, ref num75, ref num76);
                NPCLoader.TownNPCAttackCooldown(npc, ref num74, ref maxValue4);
                NPCLoader.TownNPCAttackSwing(npc, ref num77, ref num78);
                if (Main.expertMode)
                {
                    num75 = (int)((float)num75 * Main.GameModeInfo.TownNPCDamageMultiplier);
                }
                num75 = (int)((float)num75 * num2);
                npc.velocity.X *= 0.8f;
                npc.ai[1] -= 1f;
                if (Main.netMode != 1)
                {
                    Tuple<Vector2, float> swingStats = npc.GetSwingStats(NPCID.Sets.AttackTime[npc.type] * 2, (int)npc.ai[1], npc.spriteDirection, num77, num78);
                    Rectangle itemRectangle = new Rectangle((int)swingStats.Item1.X, (int)swingStats.Item1.Y, num77, num78);
                    if (npc.spriteDirection == -1)
                    {
                        itemRectangle.X -= num77;
                    }
                    itemRectangle.Y -= num78;
                    npc.TweakSwingStats(NPCID.Sets.AttackTime[npc.type] * 2, (int)npc.ai[1], npc.spriteDirection, ref itemRectangle);
                    int myPlayer = Main.myPlayer;
                    for (int num79 = 0; num79 < 200; num79++)
                    {
                        NPC nPC2 = Main.npc[num79];
                        if (nPC2.active && nPC2.immune[myPlayer] == 0 && !nPC2.dontTakeDamage && !nPC2.friendly && nPC2.damage > 0 && itemRectangle.Intersects(nPC2.Hitbox) && (nPC2.noTileCollide || Collision.CanHit(npc.position, npc.width, npc.height, nPC2.position, nPC2.width, nPC2.height)))
                        {
                            var hit = new NPC.HitInfo();
                            hit.Damage = num75;
                            hit.Knockback = num76;
                            hit.HitDirection = npc.spriteDirection;
                            nPC2.StrikeNPC(hit);

                            if (Main.netMode != 0)
                            {
                                NetMessage.SendData(28, -1, -1, null, num79, num75, num76, npc.spriteDirection);
                            }
                            nPC2.netUpdate = true;
                            nPC2.immune[myPlayer] = (int)npc.ai[1] + 2;
                        }
                    }
                }
                if (npc.ai[1] <= 0f)
                {
                    bool flag25 = false;
                    if (flag13)
                    {
                        int num80 = -num11;
                        if (!Collision.CanHit(npc.Center, 0, 0, npc.Center + Vector2.UnitX * num80 * 32f, 0, 0) || npc.localAI[2] == 8f)
                        {
                            flag25 = true;
                        }
                        if (flag25)
                        {
                            int num81 = NPCID.Sets.AttackTime[npc.type];
                            int num82 = ((num11 == 1) ? num13 : num12);
                            int num83 = ((num11 == 1) ? num12 : num13);
                            if (num82 != -1 && !Collision.CanHit(npc.Center, 0, 0, Main.npc[num82].Center, 0, 0))
                            {
                                num82 = ((num83 == -1 || !Collision.CanHit(npc.Center, 0, 0, Main.npc[num83].Center, 0, 0)) ? (-1) : num83);
                            }
                            if (num82 != -1)
                            {
                                npc.ai[0] = 15f;
                                npc.ai[1] = num81;
                                npc.ai[2] = 0f;
                                npc.localAI[3] = 0f;
                                npc.direction = ((npc.position.X < Main.npc[num82].position.X) ? 1 : (-1));
                                npc.netUpdate = true;
                            }
                            else
                            {
                                flag25 = false;
                            }
                        }
                    }
                    if (!flag25)
                    {
                        npc.ai[0] = ((npc.localAI[2] == 8f && flag13) ? 8 : 0);
                        npc.ai[1] = num74 + Main.rand.Next(maxValue4);
                        npc.ai[2] = 0f;
                        npc.localAI[1] = (npc.localAI[3] = num74 / 2 + Main.rand.Next(maxValue4));
                        npc.netUpdate = true;
                    }
                }
            }
            // Shimmering
            else if (npc.ai[0] == 24f)
            {
                npc.velocity.X *= 0.8f;
                npc.ai[1] -= 1f;
                npc.localAI[3] += 1f;
                npc.direction = 1;
                npc.spriteDirection = 1;
                Vector3 vector7 = npc.GetMagicAuraColor().ToVector3();
                Lighting.AddLight(npc.Center, vector7.X, vector7.Y, vector7.Z);
                if (npc.ai[1] <= 0f)
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] = 480f;
                    npc.ai[2] = 0f;
                    npc.localAI[1] = 480f;
                    npc.netUpdate = true;
                }
            }
            if (flag11 && npc.wet)
            {
                int num84 = (int)(npc.Center.X / 16f);
                int num85 = 5;
                if (npc.collideX || (num84 < num85 && npc.direction == -1) || (num84 > Main.maxTilesX - num85 && npc.direction == 1))
                {
                    npc.direction *= -1;
                    npc.velocity.X *= -0.25f;
                    npc.netUpdate = true;
                }
                npc.velocity.Y *= 0.9f;
                npc.velocity.Y -= 0.5f;
                if (npc.velocity.Y < -15f)
                {
                    npc.velocity.Y = -15f;
                }
            }
            if (flag10 && npc.wet)
            {
                if (isFrogOrYellowTownSlime)
                {
                    npc.ai[1] = 50f;
                }
                int num86 = (int)(npc.Center.X / 16f);
                int num87 = 5;
                if (npc.collideX || (num86 < num87 && npc.direction == -1) || (num86 > Main.maxTilesX - num87 && npc.direction == 1))
                {
                    npc.direction *= -1;
                    npc.velocity.X *= -0.25f;
                    npc.netUpdate = true;
                }
                if (Collision.GetWaterLine(npc.Center.ToTileCoordinates(), out var waterLineHeight))
                {
                    float num88 = npc.Center.Y + 1f;
                    if (npc.Center.Y > waterLineHeight)
                    {
                        npc.velocity.Y -= 0.8f;
                        if (npc.velocity.Y < -4f)
                        {
                            npc.velocity.Y = -4f;
                        }
                        if (num88 + npc.velocity.Y < waterLineHeight)
                        {
                            npc.velocity.Y = waterLineHeight - num88;
                        }
                    }
                    else
                    {
                        npc.velocity.Y = MathHelper.Min(npc.velocity.Y, waterLineHeight - num88);
                    }
                }
                else
                {
                    npc.velocity.Y -= 0.2f;
                }
            }
            if (Main.netMode != 1 && npc.isLikeATownNPC && !flag3)
            {
                bool flag26 = npc.ai[0] < 2f && !flag13 && !npc.wet;
                bool flag27 = (npc.ai[0] < 2f || npc.ai[0] == 8f) && (flag13 || flag14);
                if (npc.localAI[1] > 0f)
                {
                    npc.localAI[1] -= 1f;
                }
                if (npc.localAI[1] > 0f)
                {
                    flag27 = false;
                }
                if (flag27 && npc.type == 124 && npc.localAI[0] == 1f)
                {
                    flag27 = false;
                }
                if (flag27 && npc.type == 20)
                {
                    flag27 = false;
                    for (int num89 = 0; num89 < 200; num89++)
                    {
                        NPC nPC3 = Main.npc[num89];
                        if (nPC3.active && nPC3.townNPC && !(npc.Distance(nPC3.Center) > 1200f) && nPC3.FindBuffIndex(165) == -1)
                        {
                            flag27 = true;
                            break;
                        }
                    }
                }
                // Talk to another npc if near them
                if (npc.CanTalk && flag26 && npc.ai[0] == 0f && npc.velocity.Y == 0f && Main.rand.Next(300) == 0)
                {
                    int talkTime = 420;
                    talkTime = ((Main.rand.Next(2) != 0) ? (talkTime * Main.rand.Next(1, 3)) : (talkTime * Main.rand.Next(1, 4)));
                    int num91 = 100;
                    int num92 = 20;
                    for (int talkToNPCId = 0; talkToNPCId < 200; talkToNPCId++)
                    {
                        NPC nPC4 = Main.npc[talkToNPCId];
                        bool flag28 = (nPC4.ai[0] == 1f && nPC4.closeDoor) || (nPC4.ai[0] == 1f && nPC4.ai[1] > 200f) || nPC4.ai[0] > 1f || nPC4.wet;
                        if (nPC4 != npc && nPC4.active && nPC4.CanBeTalkedTo && !flag28 && nPC4.Distance(npc.Center) < (float)num91 && nPC4.Distance(npc.Center) > (float)num92 && Collision.CanHit(npc.Center, 0, 0, nPC4.Center, 0, 0))
                        {
                            int num94 = (npc.position.X < nPC4.position.X).ToDirectionInt();
                            // Talking
                            npc.ai[0] = 3f;
                            npc.ai[1] = talkTime;
                            npc.ai[2] = talkToNPCId;
                            npc.direction = num94;
                            npc.netUpdate = true;
                            // Being talked to
                            nPC4.ai[0] = 4f;
                            nPC4.ai[1] = talkTime;
                            nPC4.ai[2] = npc.whoAmI;
                            nPC4.direction = -num94;
                            nPC4.netUpdate = true;
                            break;
                        }
                    }
                }
                // Rock paper scissors?
                else if (npc.CanTalk && flag26 && npc.ai[0] == 0f && npc.velocity.Y == 0f && Main.rand.Next(1800) == 0)
                {
                    int num95 = 420;
                    num95 = ((Main.rand.Next(2) != 0) ? (num95 * Main.rand.Next(1, 3)) : (num95 * Main.rand.Next(1, 4)));
                    int num96 = 100;
                    int num97 = 20;
                    for (int num98 = 0; num98 < 200; num98++)
                    {
                        NPC nPC5 = Main.npc[num98];
                        bool flag29 = (nPC5.ai[0] == 1f && nPC5.closeDoor) || (nPC5.ai[0] == 1f && nPC5.ai[1] > 200f) || nPC5.ai[0] > 1f || nPC5.wet;
                        if (nPC5 != npc && nPC5.active && nPC5.CanBeTalkedTo && !NPCID.Sets.IsTownPet[nPC5.type] && !flag29 && nPC5.Distance(npc.Center) < (float)num96 && nPC5.Distance(npc.Center) > (float)num97 && Collision.CanHit(npc.Center, 0, 0, nPC5.Center, 0, 0))
                        {
                            int num99 = (npc.position.X < nPC5.position.X).ToDirectionInt();
                            npc.ai[0] = 16f;
                            npc.ai[1] = num95;
                            npc.ai[2] = num98;
                            npc.localAI[2] = Main.rand.Next(4);
                            npc.localAI[3] = Main.rand.Next(3 - (int)npc.localAI[2]);
                            npc.direction = num99;
                            npc.netUpdate = true;
                            nPC5.ai[0] = 17f;
                            nPC5.ai[1] = num95;
                            nPC5.ai[2] = npc.whoAmI;
                            nPC5.localAI[2] = 0f;
                            nPC5.localAI[3] = 0f;
                            nPC5.direction = -num99;
                            nPC5.netUpdate = true;
                            break;
                        }
                    }
                }
                // Throw confetti if there is a party
                else if (!NPCID.Sets.IsTownPet[npc.type] && flag26 && npc.ai[0] == 0f && npc.velocity.Y == 0f && Main.rand.Next(1200) == 0 && (npc.type == 208 || (BirthdayParty.PartyIsUp && NPCID.Sets.AttackType[npc.type] == NPCID.Sets.AttackType[208])))
                {
                    int num100 = 300;
                    int num101 = 150;
                    for (int num102 = 0; num102 < 255; num102++)
                    {
                        Player player = Main.player[num102];
                        if (player.active && !player.dead && player.Distance(npc.Center) < (float)num101 && Collision.CanHitLine(npc.Top, 0, 0, player.Top, 0, 0))
                        {
                            int num103 = (npc.position.X < player.position.X).ToDirectionInt();
                            npc.ai[0] = 6f;
                            npc.ai[1] = num100;
                            npc.ai[2] = num102;
                            npc.direction = num103;
                            npc.netUpdate = true;
                            break;
                        }
                    }
                }
                // Bartender holding beer
                else if (flag26 && npc.ai[0] == 0f && npc.velocity.Y == 0f && Main.rand.Next(600) == 0 && npc.type == 550)
                {
                    int num104 = 300;
                    int num105 = 150;
                    for (int num106 = 0; num106 < 255; num106++)
                    {
                        Player player2 = Main.player[num106];
                        if (player2.active && !player2.dead && player2.Distance(npc.Center) < (float)num105 && Collision.CanHitLine(npc.Top, 0, 0, player2.Top, 0, 0))
                        {
                            int num107 = (npc.position.X < player2.position.X).ToDirectionInt();
                            npc.ai[0] = 18f;
                            npc.ai[1] = num104;
                            npc.ai[2] = num106;
                            npc.direction = num107;
                            npc.netUpdate = true;
                            break;
                        }
                    }
                }
                else if (!NPCID.Sets.IsTownPet[npc.type] && flag26 && npc.ai[0] == 0f && npc.velocity.Y == 0f && Main.rand.Next(1800) == 0)
                {
                    npc.ai[0] = 2f;
                    npc.ai[1] = 45 * Main.rand.Next(1, 2);
                    npc.netUpdate = true;
                }
                else if (flag26 && npc.ai[0] == 0f && npc.velocity.Y == 0f && Main.rand.Next(600) == 0 && npc.type == NPCID.Pirate && !flag14)
                {
                    npc.ai[0] = 11f;
                    npc.ai[1] = 30 * Main.rand.Next(1, 4);
                    npc.netUpdate = true;
                }
                else if (flag26 && npc.ai[0] == 0f && npc.velocity.Y == 0f && Main.rand.Next(1200) == 0)
                {
                    int num108 = 220;
                    int num109 = 150;
                    for (int num110 = 0; num110 < 255; num110++)
                    {
                        Player player3 = Main.player[num110];
                        if (player3.CanBeTalkedTo && player3.Distance(npc.Center) < (float)num109 && Collision.CanHitLine(npc.Top, 0, 0, player3.Top, 0, 0))
                        {
                            int num111 = (npc.position.X < player3.position.X).ToDirectionInt();
                            npc.ai[0] = 7f;
                            npc.ai[1] = num108;
                            npc.ai[2] = num110;
                            npc.direction = num111;
                            npc.netUpdate = true;
                            break;
                        }
                    }
                }
                // Our new code. Randomly try to water plants
                else if (flag26 && npc.ai[0] == 1f && npc.velocity.Y == 0f && waterPlantsChance > 0 && Main.rand.Next(waterPlantsChance) == 0)
                {
                    Point point = (npc.Bottom + Vector2.UnitX * npc.direction * 24 + Vector2.UnitY * -2f).ToTileCoordinates();
                    //Point point = (npc.Bottom + Vector2.UnitX * npc.direction * 2 + Vector2.UnitY * -2f).ToTileCoordinates();
                    bool isPlant = WorldGen.InWorld(point.X, point.Y, 1);
                    if (isPlant)
                    {
                        Tile checkTile = Main.tile[point.X, point.Y];
                        isPlant = PlantList.Contains((int)checkTile.TileType);

                        if (isPlant)
                        {
                            var wateringTime = Main.rand.Next(1, 3) * 120;
                            npc.ai[0] = 30f;
                            npc.ai[1] = wateringTime;
                            npc.netUpdate = true;
                        }
                    }
                }
                // Try to sit down
                else if (flag26 && npc.ai[0] == 1f && npc.velocity.Y == 0f && sitDownChance > 0 && Main.rand.Next(sitDownChance) == 0)
                {
                    Point point = (npc.Bottom + Vector2.UnitY * -2f).ToTileCoordinates();
                    bool isNotOccupied = WorldGen.InWorld(point.X, point.Y, 1);
                    if (isNotOccupied)
                    {
                        // Check if another NPC is sitting here
                        for (int num112 = 0; num112 < 200; num112++)
                        {
                            if (Main.npc[num112].active && Main.npc[num112].aiStyle == 7 && Main.npc[num112].townNPC && Main.npc[num112].ai[0] == 5f && (Main.npc[num112].Bottom + Vector2.UnitY * -2f).ToTileCoordinates() == point)
                            {
                                isNotOccupied = false;
                                break;
                            }
                        }
                        // Check if any player is sitting here
                        for (int num113 = 0; num113 < 255; num113++)
                        {
                            if (Main.player[num113].active && Main.player[num113].sitting.isSitting && Main.player[num113].Center.ToTileCoordinates() == point)
                            {
                                isNotOccupied = false;
                                break;
                            }
                        }
                    }
                    // Nobody is sitting here
                    if (isNotOccupied)
                    {
                        Tile tile2 = Main.tile[point.X, point.Y];
                        // Is there actually a chair here we can sit on
                        isNotOccupied = TileID.Sets.CanBeSatOnForNPCs[tile2.TileType];
                        // Disable sitting if actuated? Not sure
                        if (isNotOccupied && tile2.TileType == TileID.Chairs && tile2.TileFrameY >= 1080 && tile2.TileFrameY <= 1098)
                        {
                            isNotOccupied = false;
                        }
                        if (isNotOccupied)
                        {
                            npc.ai[0] = 5f;
                            npc.ai[1] = 900 + Main.rand.Next(10800);
                            npc.SitDown(point, out var targetDirection, out var bottom);
                            npc.direction = targetDirection;
                            npc.Bottom = bottom;
                            npc.velocity = Vector2.Zero;
                            npc.localAI[3] = 0f;
                            npc.netUpdate = true;
                        }
                    }
                }
                else if (flag26 && npc.ai[0] == 1f && npc.velocity.Y == 0f && Main.rand.Next(600) == 0 && Utils.PlotTileLine(npc.Top, npc.Bottom, npc.width, DelegateMethods.SearchAvoidedByNPCs))
                {
                    Point point2 = (npc.Center + new Vector2(npc.direction * 10, 0f)).ToTileCoordinates();
                    bool flag31 = WorldGen.InWorld(point2.X, point2.Y, 1);
                    if (flag31)
                    {
                        Tile tileSafely7 = Framing.GetTileSafely(point2.X, point2.Y);
                        if (!tileSafely7.HasUnactuatedTile || !TileID.Sets.InteractibleByNPCs[tileSafely7.TileType])
                        {
                            flag31 = false;
                        }
                    }
                    if (flag31)
                    {
                        npc.ai[0] = 9f;
                        npc.ai[1] = 40 + Main.rand.Next(90);
                        npc.velocity = Vector2.Zero;
                        npc.localAI[3] = 0f;
                        npc.netUpdate = true;
                    }
                }
                // Check if nurse should throw healing needles to other town npcs
                if (Main.netMode != 1 && npc.ai[0] < 2f && npc.velocity.Y == 0f && npc.type == NPCID.Nurse && npc.breath > 0)
                {
                    int num114 = -1;
                    for (int num115 = 0; num115 < 200; num115++)
                    {
                        NPC nPC6 = Main.npc[num115];
                        if (nPC6.active && nPC6.townNPC && nPC6.life != nPC6.lifeMax && (num114 == -1 || nPC6.lifeMax - nPC6.life > Main.npc[num114].lifeMax - Main.npc[num114].life) && Collision.CanHitLine(npc.position, npc.width, npc.height, nPC6.position, nPC6.width, nPC6.height) && npc.Distance(nPC6.Center) < 500f)
                        {
                            num114 = num115;
                        }
                    }
                    if (num114 != -1)
                    {
                        npc.ai[0] = 13f;
                        npc.ai[1] = 34f;
                        npc.ai[2] = num114;
                        npc.localAI[3] = 0f;
                        npc.direction = ((npc.position.X < Main.npc[num114].position.X) ? 1 : (-1));
                        npc.netUpdate = true;
                    }
                }
                if (flag27 && npc.velocity.Y == 0f && NPCID.Sets.AttackType[npc.type] == 0 && NPCID.Sets.AttackAverageChance[npc.type] > 0 && Main.rand.Next(NPCID.Sets.AttackAverageChance[npc.type] * 2) == 0)
                {
                    int num116 = NPCID.Sets.AttackTime[npc.type];
                    int num117 = ((num11 == 1) ? num13 : num12);
                    int num118 = ((num11 == 1) ? num12 : num13);
                    if (num117 != -1 && !Collision.CanHit(npc.Center, 0, 0, Main.npc[num117].Center, 0, 0))
                    {
                        num117 = ((num118 == -1 || !Collision.CanHit(npc.Center, 0, 0, Main.npc[num118].Center, 0, 0)) ? (-1) : num118);
                    }
                    bool flag32 = num117 != -1;
                    if (flag32 && npc.type == 633)
                    {
                        flag32 = Vector2.Distance(npc.Center, Main.npc[num117].Center) <= 50f;
                    }
                    if (flag32)
                    {
                        npc.localAI[2] = npc.ai[0];
                        npc.ai[0] = 10f;
                        npc.ai[1] = num116;
                        npc.ai[2] = 0f;
                        npc.localAI[3] = 0f;
                        npc.direction = ((npc.position.X < Main.npc[num117].position.X) ? 1 : (-1));
                        npc.netUpdate = true;
                    }
                }
                else if (flag27 && npc.velocity.Y == 0f && NPCID.Sets.AttackType[npc.type] == 1 && NPCID.Sets.AttackAverageChance[npc.type] > 0 && Main.rand.Next(NPCID.Sets.AttackAverageChance[npc.type] * 2) == 0)
                {
                    int num119 = NPCID.Sets.AttackTime[npc.type];
                    int num120 = ((num11 == 1) ? num13 : num12);
                    int num121 = ((num11 == 1) ? num12 : num13);
                    if (num120 != -1 && !Collision.CanHitLine(npc.Center, 0, 0, Main.npc[num120].Center, 0, 0))
                    {
                        num120 = ((num121 == -1 || !Collision.CanHitLine(npc.Center, 0, 0, Main.npc[num121].Center, 0, 0)) ? (-1) : num121);
                    }
                    if (num120 != -1)
                    {
                        Vector2 vector8 = npc.DirectionTo(Main.npc[num120].Center);
                        if (vector8.Y <= 0.5f && vector8.Y >= -0.5f)
                        {
                            npc.localAI[2] = npc.ai[0];
                            npc.ai[0] = 12f;
                            npc.ai[1] = num119;
                            npc.ai[2] = vector8.Y;
                            npc.localAI[3] = 0f;
                            npc.direction = ((npc.position.X < Main.npc[num120].position.X) ? 1 : (-1));
                            npc.netUpdate = true;
                        }
                    }
                }
                if (flag27 && npc.velocity.Y == 0f && NPCID.Sets.AttackType[npc.type] == 2 && NPCID.Sets.AttackAverageChance[npc.type] > 0 && Main.rand.Next(NPCID.Sets.AttackAverageChance[npc.type] * 2) == 0)
                {
                    int num122 = NPCID.Sets.AttackTime[npc.type];
                    int num123 = ((num11 == 1) ? num13 : num12);
                    int num124 = ((num11 == 1) ? num12 : num13);
                    if (num123 != -1 && !Collision.CanHitLine(npc.Center, 0, 0, Main.npc[num123].Center, 0, 0))
                    {
                        num123 = ((num124 == -1 || !Collision.CanHitLine(npc.Center, 0, 0, Main.npc[num124].Center, 0, 0)) ? (-1) : num124);
                    }
                    if (num123 != -1)
                    {
                        npc.localAI[2] = npc.ai[0];
                        npc.ai[0] = 14f;
                        npc.ai[1] = num122;
                        npc.ai[2] = 0f;
                        npc.localAI[3] = 0f;
                        npc.direction = ((npc.position.X < Main.npc[num123].position.X) ? 1 : (-1));
                        npc.netUpdate = true;
                    }
                    else if (npc.type == 20)
                    {
                        npc.localAI[2] = npc.ai[0];
                        npc.ai[0] = 14f;
                        npc.ai[1] = num122;
                        npc.ai[2] = 0f;
                        npc.localAI[3] = 0f;
                        npc.netUpdate = true;
                    }
                }
                if (flag27 && npc.velocity.Y == 0f && NPCID.Sets.AttackType[npc.type] == 3 && NPCID.Sets.AttackAverageChance[npc.type] > 0 && Main.rand.Next(NPCID.Sets.AttackAverageChance[npc.type] * 2) == 0)
                {
                    int num125 = NPCID.Sets.AttackTime[npc.type];
                    int num126 = ((num11 == 1) ? num13 : num12);
                    int num127 = ((num11 == 1) ? num12 : num13);
                    if (num126 != -1 && !Collision.CanHit(npc.Center, 0, 0, Main.npc[num126].Center, 0, 0))
                    {
                        num126 = ((num127 == -1 || !Collision.CanHit(npc.Center, 0, 0, Main.npc[num127].Center, 0, 0)) ? (-1) : num127);
                    }
                    if (num126 != -1)
                    {
                        npc.localAI[2] = npc.ai[0];
                        npc.ai[0] = 15f;
                        npc.ai[1] = num125;
                        npc.ai[2] = 0f;
                        npc.localAI[3] = 0f;
                        npc.direction = ((npc.position.X < Main.npc[num126].position.X) ? 1 : (-1));
                        npc.netUpdate = true;
                    }
                }
            }
            /*
            if (npc.type == ModContent.NPCType<Prototype>())
            {
                Lighting.AddLight(npc.Center, Color.LightBlue.ToVector3());
            }
            */
        }
    }
}

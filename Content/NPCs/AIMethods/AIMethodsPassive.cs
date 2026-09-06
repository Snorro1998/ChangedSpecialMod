using ChangedSpecialMod.Content.NPCs.AIStates.Passive;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using static ChangedSpecialMod.Content.NPCs.AIStyles.AIPassive;

namespace ChangedSpecialMod.Content.NPCs.AIMethods
{
    public static class AIMethodsPassive
    {
        public static void TryPickRandomAction(NPC npc, bool noEnemyNearbyAndNotWet, States AIState, bool canSnooze, bool canWaterPlants, bool flag14, int sitDownChance, int waterPlantsChance)
        {
            // Talk to another npc if near them
            if (npc.CanTalk && noEnemyNearbyAndNotWet && AIState == States.Idle && npc.velocity.Y == 0f && Main.rand.Next(300) == 0)
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
            else if (npc.CanTalk && noEnemyNearbyAndNotWet && AIState == States.Idle && npc.velocity.Y == 0f && Main.rand.Next(1800) == 0)
            {
                int num95 = 420;
                num95 = ((Main.rand.Next(2) != 0) ? (num95 * Main.rand.Next(1, 3)) : (num95 * Main.rand.Next(1, 4)));
                int num96 = 100;
                int num97 = 20;
                for (int npcIndex = 0; npcIndex < Main.maxNPCs; npcIndex++)
                {
                    NPC nPC5 = Main.npc[npcIndex];
                    bool flag29 = (nPC5.ai[0] == 1f && nPC5.closeDoor) || (nPC5.ai[0] == 1f && nPC5.ai[1] > 200f) || nPC5.ai[0] > 1f || nPC5.wet;
                    if (nPC5 != npc && nPC5.active && nPC5.CanBeTalkedTo && !NPCID.Sets.IsTownPet[nPC5.type] && !flag29 && nPC5.Distance(npc.Center) < (float)num96 && nPC5.Distance(npc.Center) > (float)num97 && Collision.CanHit(npc.Center, 0, 0, nPC5.Center, 0, 0))
                    {
                        int num99 = (npc.position.X < nPC5.position.X).ToDirectionInt();
                        npc.ai[0] = 16f;
                        npc.ai[1] = num95;
                        npc.ai[2] = npcIndex;
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
            else if (!NPCID.Sets.IsTownPet[npc.type] && noEnemyNearbyAndNotWet && AIState == States.Idle && npc.velocity.Y == 0f && Main.rand.Next(1200) == 0 && (npc.type == NPCID.PartyGirl || (BirthdayParty.PartyIsUp && NPCID.Sets.AttackType[npc.type] == NPCID.Sets.AttackType[NPCID.PartyGirl])))
            {
                int num100 = 300;
                int num101 = 150;
                for (int playerIndex = 0; playerIndex < Main.maxPlayers; playerIndex++)
                {
                    Player player = Main.player[playerIndex];
                    if (player.active && !player.dead && player.Distance(npc.Center) < (float)num101 && Collision.CanHitLine(npc.Top, 0, 0, player.Top, 0, 0))
                    {
                        int num103 = (npc.position.X < player.position.X).ToDirectionInt();
                        npc.ai[0] = 6f;
                        npc.ai[1] = num100;
                        npc.ai[2] = playerIndex;
                        npc.direction = num103;
                        npc.netUpdate = true;
                        break;
                    }
                }
            }

            // Blink eyes
            else if (!NPCID.Sets.IsTownPet[npc.type] && noEnemyNearbyAndNotWet && AIState == States.Idle && npc.velocity.Y == 0f && Main.rand.Next(1800) == 0)
            {
                npc.ai[0] = 2f;
                npc.ai[1] = 45 * Main.rand.Next(1, 2);
                npc.netUpdate = true;
            }

            // Snooze
            else if (noEnemyNearbyAndNotWet && AIState == States.Idle && npc.velocity.Y == 0f && Main.rand.Next(600) == 0 && canSnooze && !flag14)
            {
                npc.ai[0] = 11f;
                npc.ai[1] = 30 * Main.rand.Next(1, 4);
                npc.netUpdate = true;
            }
            else if (noEnemyNearbyAndNotWet && AIState == States.Idle && npc.velocity.Y == 0f && Main.rand.Next(1200) == 0)
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
            // New code. Randomly try to water plants
            else if (canWaterPlants && noEnemyNearbyAndNotWet && AIState == States.Walking && npc.velocity.Y == 0f && waterPlantsChance > 0 && Main.rand.Next(waterPlantsChance) == 0)
                StateTryWaterPlants.Update(npc);
            // Try to sit down
            else if (noEnemyNearbyAndNotWet && AIState == States.Walking && npc.velocity.Y == 0f && sitDownChance > 0 && Main.rand.Next(sitDownChance) == 0)
                StateTrySitDown.Update(npc);
            else if (noEnemyNearbyAndNotWet && AIState == States.Walking && npc.velocity.Y == 0f && Main.rand.Next(600) == 0 && Utils.PlotTileLine(npc.Top, npc.Bottom, npc.width, DelegateMethods.SearchAvoidedByNPCs))
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
        }

        public static void CalculateDefenseAndAttackMultiplier(NPC npc, ref float num2)
        {
            var dryadWardDefense = 6;
            if (Main.masterMode)
                dryadWardDefense = 14;
            else if (Main.expertMode)
                dryadWardDefense = 10;

            npc.defense = (npc.dryadWard ? (npc.defDefense + dryadWardDefense) : npc.defDefense);

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
        }

        public static void TeleportToHome(NPC npc, int homeFloorX, int homeFloorY)
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
                    TryForcingSitting(npc, homeFloorX, homeFloorY);
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

        public static void TryForcingSitting(NPC npc, int homeFloorX, int homeFloorY)
        {
            Tile tile = Main.tile[homeFloorX, homeFloorY - 1];
            bool flag = !NPCID.Sets.CannotSitOnFurniture[npc.type] && !NPCID.Sets.IsTownSlime[npc.type] && npc.ai[0] != 5f;
            if (flag)
            {
                flag &= tile != null && tile.HasTile && TileID.Sets.CanBeSatOnForNPCs[tile.TileType];
            }
            if (flag)
            {
                flag &= tile.TileType != TileID.Chairs || tile.TileFrameY < 1080 || tile.TileFrameY > 1098;
            }
            if (flag)
            {
                Point point = (npc.Bottom + Vector2.UnitY * -2f).ToTileCoordinates();
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].aiStyle == NPCAIStyleID.Passive && Main.npc[i].townNPC && Main.npc[i].ai[0] == 5f && (Main.npc[i].Bottom + Vector2.UnitY * -2f).ToTileCoordinates() == point)
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

        public static void GetWalkPrediction(NPC npc, int myTileX, int homeFloorX, bool canBreathUnderWater, bool currentlyDrowning, int tileX, int tileY, out bool keepwalking, out bool avoidFalling)
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

        public static bool CheckIfWillDrown(bool currentlyDrowning)
        {
            return currentlyDrowning;
        }

        public static void AttemptToPlayIdleAnimationsForPets(NPC npc, int petIdleChance)
        {
            if (npc.velocity.X == 0f && Main.netMode != NetmodeID.MultiplayerClient && Main.rand.Next(petIdleChance) == 0)
            {
                int num = 3;
                if (npc.type == NPCID.TownDog)
                {
                    num = 2;
                }
                if (NPCID.Sets.IsTownSlime[npc.type])
                {
                    num = 0;
                }
                npc.ai[0] = ((num == 0) ? 20 : Main.rand.Next(20, 20 + num));
                npc.ai[1] = 200 + Main.rand.Next(300);
                if (npc.ai[0] == 20f && npc.type == NPCID.TownCat)
                {
                    npc.ai[1] = 500 + Main.rand.Next(200);
                }
                if (npc.ai[0] == 21f && npc.type == NPCID.TownDog)
                {
                    npc.ai[1] = 100 + Main.rand.Next(100);
                }
                if (npc.ai[0] == 22f && npc.type == NPCID.TownBunny)
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

        public static void ShimmerTeleportToLandingSpot(NPC npc)
        {
            Vector2? vector = ShimmerScanForBestSpotToLandOn(npc);
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

        public static Vector2? ShimmerScanForBestSpotToLandOn(NPC npc)
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

        public static void FindGoodRestingSpot(NPC npc, int myTileX, int myTileY, out int floorX, out int floorY)
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
            if (npc.type == NPCID.TownDog || npc.type == NPCID.TownBunny || NPCID.Sets.IsTownSlime[npc.type] || npc.ai[0] == 5f)
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
            if (tile2.TileType == TileID.Toilets || tile2.TileType == TileID.Chairs)
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
                if (Main.npc[j].active && Main.npc[j].aiStyle == NPCAIStyleID.Passive && Main.npc[j].townNPC && Main.npc[j].ai[0] == 5f && (Main.npc[j].Bottom + Vector2.UnitY * -2f).ToTileCoordinates() == point2)
                {
                    return;
                }
            }
            floorX = point2.X;
            floorY = point2.Y;
        }

        public static bool IsInAGoodRestingSpot(NPC npc, int tileX, int tileY, int idealRestX, int idealRestY)
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
    }
}

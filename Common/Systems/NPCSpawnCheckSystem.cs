using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.NPCs.Drunk;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace ChangedSpecialMod.Common.Systems
{
    public class NPCSpawnCheckSystem : ModSystem
    {
        public override void PreUpdateWorld()
        {
            DoBossSpawnChecks();
        }

        public static void DoBossSpawnChecks()
        {
            WhiteTailSpawnCheck();
            WolfKingSpawnCheck();
            BehemothSpawnCheck();
            //if (Main.netMode != 2)
            //    SandstormCheck();
        }

        public static bool ShouldSandstormDustPersist()
        {
            if (Sandstorm.Happening && Main.player[Main.myPlayer].ZoneSandstorm && (Main.bgStyle == 2 || Main.bgStyle == 5))
            {
                return Main.bgDelay < 50;
            }
            return false;
        }

        private static void SandstormCheck()
        {

            if (Main.gamePaused)
            {
                return;
            }
            int sandTileCount = Main.SceneMetrics.SandTileCount;
            Player player = Main.player[Main.myPlayer];
            bool flag = true;// Sandstorm.ShouldSandstormDustPersist();
            Sandstorm.HandleEffectAndSky(flag && Main.UseStormEffects);
            if (sandTileCount < 100 || (double)player.position.Y > Main.worldSurface * 16.0 || player.ZoneBeach)
            {
                return;
            }
            int maxValue = 1;
            if (!flag || Main.rand.Next(maxValue) != 0)
            {
                return;
            }
            int num = Math.Sign(Main.windSpeedCurrent);
            float num2 = Math.Abs(Main.windSpeedCurrent);
            if (num2 < 0.01f)
            {
                return;
            }
            float num3 = (float)num * MathHelper.Lerp(0.9f, 1f, num2);
            float num4 = 2000f / (float)sandTileCount;
            float value = 3f / num4;
            value = MathHelper.Clamp(value, 0.77f, 1f);
            int num5 = (int)num4;
            float num6 = (float)Main.screenWidth / (float)Main.maxScreenW;
            int num7 = (int)(1000f * num6);
            float num8 = 20f * Sandstorm.Severity;
            float num9 = (float)num7 * (Main.gfxQuality * 0.5f + 0.5f) + (float)num7 * 0.1f - (float)Dust.SandStormCount;
            if (num9 <= 0f)
            {
                return;
            }
            float num10 = (float)Main.screenWidth + 1000f;
            float num11 = Main.screenHeight;
            Vector2 vector = Main.screenPosition + player.velocity;
            WeightedRandom<Color> weightedRandom = new WeightedRandom<Color>();
            weightedRandom.Add(new Color(200, 160, 20, 180), Main.SceneMetrics.GetTileCount(53) + Main.SceneMetrics.GetTileCount(396) + Main.SceneMetrics.GetTileCount(397));
            weightedRandom.Add(new Color(103, 98, 122, 180), Main.SceneMetrics.GetTileCount(112) + Main.SceneMetrics.GetTileCount(400) + Main.SceneMetrics.GetTileCount(398));
            weightedRandom.Add(new Color(135, 43, 34, 180), Main.SceneMetrics.GetTileCount(234) + Main.SceneMetrics.GetTileCount(401) + Main.SceneMetrics.GetTileCount(399));
            weightedRandom.Add(new Color(213, 196, 197, 180), Main.SceneMetrics.GetTileCount(116) + Main.SceneMetrics.GetTileCount(403) + Main.SceneMetrics.GetTileCount(402));
            float num12 = MathHelper.Lerp(0.2f, 0.35f, Sandstorm.Severity);
            float num13 = MathHelper.Lerp(0.5f, 0.7f, Sandstorm.Severity);
            float amount = (value - 0.77f) / 0.23000002f;
            int maxValue2 = (int)MathHelper.Lerp(1f, 10f, amount);
            for (int i = 0; (float)i < num8; i++)
            {
                if (Main.rand.Next(num5 / 4) != 0)
                {
                    continue;
                }
                Vector2 position = new Vector2(Main.rand.NextFloat() * num10 - 500f, Main.rand.NextFloat() * -50f);
                if (Main.rand.Next(3) == 0 && num == 1)
                {
                    position.X = Main.rand.Next(500) - 500;
                }
                else if (Main.rand.Next(3) == 0 && num == -1)
                {
                    position.X = Main.rand.Next(500) + Main.screenWidth;
                }
                if (position.X < 0f || position.X > (float)Main.screenWidth)
                {
                    position.Y += Main.rand.NextFloat() * num11 * 0.9f;
                }
                position += vector;
                int num14 = (int)position.X / 16;
                int num15 = (int)position.Y / 16;
                if (!WorldGen.InWorld(num14, num15, 10) || Main.tile[num14, num15] == null)
                {
                    continue;
                }
                Tile tile = Main.tile[num14, num15];
                if (tile.WallType != 0)
                {
                    continue;
                }
                for (int j = 0; j < 1; j++)
                {
                    Dust dust = Main.dust[Dust.NewDust(position, 10, 10, 268)];
                    dust.velocity.Y = 2f + Main.rand.NextFloat() * 0.2f;
                    dust.velocity.Y *= dust.scale;
                    dust.velocity.Y *= 0.35f;
                    dust.velocity.X = num3 * 5f + Main.rand.NextFloat() * 1f;
                    dust.velocity.X += num3 * num13 * 20f;
                    dust.fadeIn += num13 * 0.2f;
                    dust.velocity *= 1f + num12 * 0.5f;
                    dust.color = weightedRandom;
                    dust.velocity *= 1f + num12;
                    dust.velocity *= value;
                    dust.scale = 0.9f;
                    num9 -= 1f;
                    if (num9 <= 0f)
                    {
                        break;
                    }
                    if (Main.rand.Next(maxValue2) != 0)
                    {
                        j--;
                        position += Utils.RandomVector2(Main.rand, -10f, 10f) + dust.velocity * -1.1f;
                        num14 = (int)position.X / 16;
                        num15 = (int)position.Y / 16;
                        if (WorldGen.InWorld(num14, num15, 10) && Main.tile[num14, num15] != null)
                        {
                            tile = Main.tile[num14, num15];
                            //_ = ref tile.wall;
                        }
                    }
                }
                if (num9 <= 0f)
                {
                    break;
                }
            }
        }

        private static void WhiteTailSpawnCheck()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient || DownedBossSystem.DownedWhiteTail)
                return;

            if (Main.npc.Any(x => x.active && x.type == ModContent.NPCType<WhiteTail>()))
                return;

            List<float> playerChanges = new List<float>();

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                var player = Main.player[i];
                if (!player.active || !BiomeChecks.InChangedSurfaceBiome(player))
                {
                    playerChanges.Add(0);
                    continue;
                }

                var nMinKills = 20;     // Minimum monster kills needed to spawn
                var nMaxKills = 40;     // Guaranteed to spawn

                // Black
                var nGoop = ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<BlackGoop>());
                var nCub = ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<DarkLatexCub>());
                var nAdult = ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<MaleDarkLatex>());
                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<FemaleDarkLatex>());
                var nFlying = ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<FlyingDarkLatex>());

                // White
                nGoop += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<WhiteGoop>());
                nCub += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<WhiteLatexCub>());
                nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<WhiteKnight>());

                // Drunk
                if (ChangedUtils.IsDrunk(player))
                {
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<BackLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<DarkLatexCubOfDoom>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<PuroWormHead>());
                    nCub += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<QuackLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<SnackLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<StackLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<WackLatex>());

                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<BrightLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<FightLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<FlightLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<HideLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<MightLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<SideLatex>());
                    nAdult += ChangedUtils.GetBestiaryKillCount(ModContent.NPCType<WideLatex>());
                }

                // Increase the change even further if the player has more then 200 hp
                var playerHP = player.statLifeMax2;
                var playerHPMultiplier = Math.Max(0, playerHP - 200) / 200 * nMinKills;

                var nKills = 0.3f * nGoop + 0.5f * nCub + nAdult + 1.5f * nFlying + playerHPMultiplier;
                var chance = 0.0f;

                if (nKills < nMinKills)
                {
                    playerChanges.Add(0);
                    continue;
                }

                chance = 0.5f + (nKills - nMinKills) * (2.5f / (nMaxKills - nMinKills));
                playerChanges.Add(chance);
            }

            int highestIndex = -1;
            float highestValue = 0;
            for (int i = 0; i < playerChanges.Count; i++)
            {
                var playerChance = playerChanges[i];
                if (playerChance > highestValue)
                {
                    highestValue = playerChance;
                    highestIndex = i;
                }
            }

            if (highestIndex == -1)
                return;

            var irandom = Math.Max(1, (int)(2000 / highestValue));

            if (Main.rand.NextBool(irandom))
                NPC.SpawnOnPlayer(highestIndex, ModContent.NPCType<WhiteTail>());
        }

        public static bool WolfKingSpawnCheck(bool summon = false, int playerIndex = -1)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return false;

            // Don't spawn if already killed or the previous bosses haven't been killed yet
            if (!summon && (!DownedBossSystem.DownedWhiteTail || DownedBossSystem.DownedWolfKing))
                return false;

            // Don't spawn if he is already present
            if (NPC.AnyNPCs(ModContent.NPCType<WolfKing>()) || NPC.AnyNPCs(ModContent.NPCType<WolfKingSpawn>()))
                return false;

            int blockCheckSpacing = 6;
            if (summon && playerIndex != -1)
            {
                for (int y = 0; y < Main.worldSurface; y += blockCheckSpacing)
                {
                    for (int x = 0; x < Main.maxTilesX; x += blockCheckSpacing)
                    {
                        if (x < Main.maxTilesX)
                        {
                            var tile = Main.tile[x, y];
                            if (ChangedUtils.IsBlackLatexWall(tile))
                            {
                                var player = Main.player[playerIndex];//GetClosestPlayer(x, y);
                                ChangedUtils.TeleportPlayer(player.whoAmI, x, y);

                                SoundEngine.PlaySound(SoundID.NPCDeath64, player.Center);
                                ChangedUtils.SpawnWolfKing(x, y, player);
                                return true;
                            }
                        }
                    }
                }
                Main.NewText("Failed to find a suitable place for the boss fight. Please make a room with slime walls covered in black paint and try again");
            }
            else
            {
                foreach (var player in Main.ActivePlayers)
                {
                    int minRangeFromPlayer = 80;
                    int maxRange = 120;                         // Same range as the clentaminator, so probably enough to be off-screen
                    var xPos = (int)(player.position.X / 16);
                    var yPos = (int)(player.position.Y / 16);

                    for (int y = -maxRange; y <= maxRange; y += blockCheckSpacing)
                    {
                        for (int x = -maxRange; x <= maxRange; x += blockCheckSpacing)
                        {
                            if (Math.Abs(x) <= minRangeFromPlayer)
                                continue;

                            var tmpX = xPos + x;
                            var tmpY = yPos + y;

                            if (tmpX < 0 || tmpX >= Main.maxTilesX)
                                continue;

                            if (tmpY < 0 || tmpY >= Main.maxTilesY)
                                continue;

                            var tile = Main.tile[tmpX, tmpY];

                            if (ChangedUtils.IsBlackLatexWall(tile))
                            {
                                ChangedUtils.SpawnWolfKing(tmpX, tmpY, player);
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static void BehemothSpawnCheck()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            // Don't spawn if already killed or the previous bosses haven't been killed yet
            if (DownedBossSystem.DownedBehemoth || !DownedBossSystem.DownedWhiteTail || !DownedBossSystem.DownedWolfKing)
                return;

            // Don't spawn if he is already present
            if (NPC.AnyNPCs(ModContent.NPCType<Behemoth>()) || NPC.AnyNPCs(ModContent.NPCType<BehemothSpawn>()))
                return;

            foreach (var player in Main.ActivePlayers)
            {                
                int minRangeFromPlayer = 80;
                int maxRange = 120;                         // Same range as the clentaminator, so probably enough to be off-screen
                int blockCheckSpacing = 6;                  // Only check every 8 blocks for performance
                var xPos = (int)(player.position.X / 16);
                var yPos = (int)(player.position.Y / 16);

                for (int y = -maxRange; y <= maxRange; y += blockCheckSpacing)
                {
                    for (int x = -maxRange; x <= maxRange; x += blockCheckSpacing)
                    {
                        if (Math.Abs(x) <= minRangeFromPlayer)
                            continue;

                        var tmpX = xPos + x;
                        var tmpY = yPos + y;

                        if (tmpX < 0 || tmpX >= Main.maxTilesX)
                            continue;

                        if (tmpY < 0 || tmpY >= Main.maxTilesY)
                            continue;

                        var tile = Main.tile[tmpX, tmpY];

                        if (ChangedUtils.IsWhiteLatexWall(tile))
                        {
                            ChangedUtils.SpawnBehemoth(tmpX, tmpY, player);
                            return;
                        }
                    }
                }
            }
        }
    }
}

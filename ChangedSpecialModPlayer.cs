using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Mounts;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod
{
    public enum TransfurType
    {
        None = 0,
        BlackGoop,
        BlackCub,
        BlackAdult,
        BlackFast,
        BlackStrong,

        WhiteGoop,
        WhiteCub,
        WhiteAdult,
        WhiteFast,
        WhiteStrong,

        SquidCub,
        SquidAdult,

        GermanShepherd,
    }

    public class Transfur
    {
        public int npcType = -1;
        public string texturePath = "ChangedSpecialMod/Content/NPCs/WhiteGoop";
        public int nFrames = 3;
        public GooType gooType = GooType.Invalid;

        public float lifeMultiplier = 1f;
        public int extraDefense = 0;
        public float speedMultiplier = 1f;
        // Used by goops
        public float speedMultiplierAirborn = -1f;
        public float damageBonus = 0;
        public float jumpHeightMultiplier = 1;
        public float jumpSpeedMultiplier = 1;

        public bool ignoreWater = false;
        public bool waterBreathing = false;

        public bool tentacleAbility = false;

        public void UpdatePlayerStats(Player player)
        {
            var speedMulti = 1f;
            if (speedMultiplierAirborn != -1)
            {
                // Should have a grounded check, because this is also true at the apex of the jump
                if (player.velocity.Y != 0)
                    speedMulti = speedMultiplierAirborn;
                else
                    speedMulti = speedMultiplier;
            }
            else
                speedMulti = speedMultiplier;

            Player.jumpSpeed *= jumpSpeedMultiplier;
            Player.jumpHeight = (int)(Player.jumpHeight * jumpHeightMultiplier);
            player.statDefense += extraDefense;
            player.statLifeMax2 = (int)(player.statLifeMax2 * lifeMultiplier);
            player.moveSpeed *= speedMulti;
            player.accRunSpeed = (int)(player.accRunSpeed * speedMulti);
            //player.maxRunSpeed = (int)(player.maxRunSpeed * speedMulti);
            player.GetDamage<MeleeDamageClass>() += damageBonus;


            // Dont set it to false in case other mods also affect this value
            // It is set to false every update anyways
            if (ignoreWater)
                player.ignoreWater = true;
            if (player.wet && waterBreathing)
                player.AddBuff(BuffID.Gills, 1);
        }
    }

    public class CustomPlayerLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.LastVanillaLayer);

        private int PlayerAnimationWhiteTaur(int legFrame)
        {
            int frameIndex = 0;
            if (legFrame > 5)
            {
                if (legFrame == 6 || legFrame == 7)
                    frameIndex = 3;
                else if (legFrame == 8 || legFrame == 9)
                    frameIndex = 4;
                else if (legFrame == 10 || legFrame == 11)
                    frameIndex = 5;
                else if (legFrame == 12 || legFrame == 13)
                    frameIndex = 6;
                else if (legFrame == 14 || legFrame == 15)
                    frameIndex = 7;
                else if (legFrame == 16 || legFrame == 17)
                    frameIndex = 8;
                else
                    frameIndex = 9;
            }

            else if (legFrame == 5)
                frameIndex = 10;
            else
                frameIndex = 0;

            return frameIndex;
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            var changedPlayer = player.ChangedPlayer();
            var baseTexturePath = "ChangedSpecialMod/Content/NPCs/";
            var texturePath = $"{baseTexturePath}Transparent";
            var nFrames = 3;

            var transfurCurrent = changedPlayer?.TransfurTypeCurrent;
            if (transfurCurrent != null)
            {
                texturePath = transfurCurrent.texturePath;
                nFrames = transfurCurrent.nFrames;
            }

            Texture2D texture = ModContent.Request<Texture2D>(texturePath).Value;
            Vector2 playerPos = player.Center;
            playerPos.Y += player.gfxOffY;
            Vector2 position = playerPos - Main.screenPosition;
            position = new Vector2((int)position.X, (int)position.Y);
            var effects = player.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            var frameWidth = texture.Width;
            var legFrame = player.legFrame.Y / 56;
            var frameIndex = (player.legFrame.Y / (4 * 32)) % nFrames;// 0;

            if (transfurCurrent != null && transfurCurrent.npcType == ModContent.NPCType<WhiteLatexTaur>())
                frameIndex = PlayerAnimationWhiteTaur(legFrame);
            else
            {
                //6..19 walk
                if (legFrame > 5)
                {
                    if (legFrame == 6 || legFrame == 7 || legFrame == 13 || legFrame == 14)
                        frameIndex = 0;
                    else if (legFrame == 8 || legFrame == 11 || legFrame == 12 || legFrame == 15 || legFrame == 18 || legFrame == 19)
                        frameIndex = 1;
                    else
                        frameIndex = 2;
                }

                else if (legFrame == 5)
                    frameIndex = 2;
                else
                    frameIndex = 0;
            }

            var frameHeight = texture.Height / nFrames;
            var sourceRect = new Rectangle(0, frameIndex * frameHeight, frameWidth, frameHeight);

            var yOff = (48 - frameHeight) / 2;
            position.Y += yOff;

            Vector2 drawCenter = player.Center;
            Vector2 drawPositionInWorld = drawCenter;
            Color tmpColor = Lighting.GetColor((int)drawCenter.X / 16, (int)(drawCenter.Y / 16f));

            // Body
            drawInfo.DrawDataCache.Add(new DrawData(
                texture,
                position,
                sourceRect,
                tmpColor,
                0f,
                new Vector2(frameWidth / 2, frameHeight / 2),
                1f,
                effects,
                0f
            ));
        }
    }

    public class ChangedSpecialModPlayer : ModPlayer
    {
        public Dictionary<GooType, List<Transfur>> EvolutionsLines = new Dictionary<GooType, List<Transfur>>();
        public Transfur TransfurTypeCurrent = null;

        public bool pictureViewerOpen = false;
        public int pictureIndex = -1;
        public bool hidePlayer = false;
        public TransfurType transfurIndex = TransfurType.None;
        public GooType transfurGooType = GooType.None;
        //0 black
        //1 white

        private void InitTransfurTypes()
        {
            var baseTexturePath = "ChangedSpecialMod/Content/NPCs/";
            EvolutionsLines = new Dictionary<GooType, List<Transfur>>();

            // Black
            var evolutionLine = new List<Transfur>();
            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<BlackGoop>(),
                texturePath = $"{baseTexturePath}BlackGoop",
                nFrames = 4,
                gooType = GooType.Black,
                lifeMultiplier = 0.6f,
                speedMultiplier = 0.75f,
                speedMultiplierAirborn = 2f,
                jumpSpeedMultiplier = 1.75f,
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<DarkLatexCub>(),
                texturePath = $"{baseTexturePath}DarkLatexCub",
                nFrames = 4,
                gooType = GooType.Black,
                lifeMultiplier = 0.8f,
                speedMultiplier = 1.2f,
                jumpHeightMultiplier = 1.5f
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<MaleDarkLatex>(),
                texturePath = $"{baseTexturePath}MaleDarkLatex",
                gooType = GooType.Black,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<WingedDarkLatex>(),
                texturePath = $"{baseTexturePath}WingedDarkLatex",
                nFrames = 4,
                gooType = GooType.Black,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
                speedMultiplier = 2f
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<Wendigo>(),
                texturePath = $"{baseTexturePath}Wendigo",
                nFrames = 4,
                gooType = GooType.Black,
                lifeMultiplier = 2f,
                extraDefense = 10,
                speedMultiplier = 0.75f,
                damageBonus = 0.3f
            });

            EvolutionsLines.Add(GooType.Black, evolutionLine);

            // White
            evolutionLine = new List<Transfur>();
            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<WhiteGoop>(),
                texturePath = $"{baseTexturePath}WhiteGoop",
                nFrames = 4,
                gooType = GooType.White,
                lifeMultiplier = 0.6f,
                speedMultiplier = 0.75f,
                speedMultiplierAirborn = 2f,
                jumpSpeedMultiplier = 1.75f,
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<WhiteLatexCub>(),
                texturePath = $"{baseTexturePath}WhiteLatexCub",
                nFrames = 3,
                gooType = GooType.White,
                lifeMultiplier = 0.8f,
                speedMultiplier = 1.2f,
                jumpHeightMultiplier = 1.5f
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<WhiteKnight>(),
                texturePath = $"{baseTexturePath}WhiteKnight",
                gooType = GooType.White,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
            });

            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<WhiteLatexTaur>(),
                texturePath = $"{baseTexturePath}WhiteLatexTaur",
                nFrames = 13,
                gooType = GooType.White,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
                speedMultiplier = 2f
            });

            EvolutionsLines.Add(GooType.White, evolutionLine);

            // Squid Dog
            evolutionLine = new List<Transfur>();
            evolutionLine.Add(new Transfur()
            {
                npcType = ModContent.NPCType<SquidDog>(),
                texturePath = $"{baseTexturePath}SquidDog",
                nFrames = 4,
                gooType = GooType.None,
                lifeMultiplier = 1.25f,
                extraDefense = 5,
                ignoreWater = true,
                waterBreathing = true,
                tentacleAbility = true
            });

            EvolutionsLines.Add(GooType.None, evolutionLine);
        }

        private List<Transfur> GetEvolutionLine(GooType gooType)
        {
            if (EvolutionsLines == null || !EvolutionsLines.ContainsKey(gooType))
                return null;
            return EvolutionsLines[gooType];
        }

        private void Devolve()
        {
            if (TransfurTypeCurrent == null)
                return;

            List<Transfur> targetList = GetEvolutionLine(TransfurTypeCurrent.gooType);

            if (targetList == null)
                return;

            var index = targetList.IndexOf(TransfurTypeCurrent);
            if (index == -1)
                return;

            if (index <= 0)
                SetTransfur(null);
            else
                SetTransfur(targetList[index - 1]);

        }

        private void Evolve()
        {
            if (TransfurTypeCurrent == null)
                return;

            List<Transfur> targetList = GetEvolutionLine(TransfurTypeCurrent.gooType);

            if (targetList == null)
                return;

            var index = targetList.IndexOf(TransfurTypeCurrent);
            if (index == -1)
                return;

            if (index >= targetList.Count - 1)
                return;
            else
                SetTransfur(targetList[index + 1]);

        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybindSystem.TransfurDevolveKeybind.JustPressed)
                Devolve();

            if (KeybindSystem.TransfurEvolveKeybind.JustPressed)
                Evolve();

            if (KeybindSystem.TransfurAttackKeybind.Current)
                TransfurAttack();
        }

        private void TransfurAttack()
        {
            if (TransfurTypeCurrent == null || !TransfurTypeCurrent.tentacleAbility)
                return;

            Vector2 aimDirection = Main.MouseWorld - Player.Center;
            aimDirection.Normalize();

            var projectileType = ModContent.ProjectileType<PlayerMollashProjectileStraight>();
            var mollashProjectile = Main.projectile.FirstOrDefault(x => x.active && x.type == projectileType);
            if (mollashProjectile != null)
                return;

            var items = Player.inventory.Where(x => x != null && !x.IsAir && x.ammo == AmmoID.None && !x.consumable).ToList();
            var highestItemDamage = items.Any() ? (int)(items.OrderByDescending(x => x.damage).FirstOrDefault().damage * 0.75f) : 10;

            var whipDamage = Math.Max(10, highestItemDamage);
            Projectile.NewProjectile(
                Player.GetSource_FromAI(),
                Player.Center,
                Vector2.Zero,
                projectileType,
                whipDamage,
                2f,
                -1,
                Player.whoAmI,
                aimDirection.ToRotation(),
                Player.direction
            );
        }

        public void SetTransfurType(GooType gooType)
        {
            if (EvolutionsLines == null || EvolutionsLines.Count == 0)
                InitTransfurTypes();

            if (TransfurTypeCurrent != null)
            {
                SetTransfur(null);
                return;
            }

            List<Transfur> targetList = GetEvolutionLine(gooType);

            if (targetList == null)
                return;

            SetTransfur(targetList.FirstOrDefault());
        }

        private void SetTransfur(Transfur transfur)
        {
            if (!Main.dedServ)
            {
                var dustTransfur = TransfurTypeCurrent;
                if (dustTransfur == null)
                    dustTransfur = transfur;

                if (dustTransfur != null)
                {
                    var dustType = dustTransfur.gooType == GooType.Black ? DustID.Asphalt : DustID.SnowSpray;
                    var nParticles = 40;
                    for (int i = 0; i < nParticles; i++)
                    {
                        var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, dustType, 0, 0, 1);
                        dust.velocity.X += Main.rand.NextFloat(-0.05f, 0.05f);
                        dust.velocity.Y += Main.rand.NextFloat(-0.05f, 0.05f);
                    }
                }
            }

            if (transfur != null)
                SoundEngine.PlaySound(Sounds.SoundTransfur, Player.Center);

            TransfurTypeCurrent = transfur;
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            var player = drawInfo.drawPlayer;
            var changedPlayer = player.ChangedPlayer();
            if (changedPlayer.TransfurTypeCurrent != null)
            {
                drawInfo.colorHair = Color.Transparent;
                drawInfo.colorEyeWhites = Color.Transparent;
                drawInfo.colorEyes = Color.Transparent;
                drawInfo.colorHead = Color.Transparent;
                drawInfo.colorBodySkin = Color.Transparent;
                drawInfo.colorLegs = Color.Transparent;
                drawInfo.colorShirt = Color.Transparent;
                drawInfo.colorUnderShirt = Color.Transparent;
                drawInfo.colorPants = Color.Transparent;
                drawInfo.colorShoes = Color.Transparent;
                drawInfo.colorArmorHead = Color.Transparent;
                drawInfo.colorArmorBody = Color.Transparent;
                drawInfo.colorMount = Color.Transparent;
                drawInfo.colorArmorLegs = Color.Transparent;
                drawInfo.colorElectricity = Color.Transparent;
                drawInfo.colorDisplayDollSkin = Color.Transparent;

                drawInfo.headGlowColor = Color.Transparent;
                drawInfo.bodyGlowColor = Color.Transparent;
                drawInfo.armGlowColor = Color.Transparent;
                drawInfo.legsGlowColor = Color.Transparent;
                drawInfo.ArkhalisColor = Color.Transparent;

                drawInfo.selectionGlowColor = Color.Transparent;
                drawInfo.itemColor = Color.Transparent;
                drawInfo.floatingTubeColor = Color.Transparent;
            }
            base.ModifyDrawInfo(ref drawInfo);
        }

        public void AddDebuff()
        {
            if (ChangedUtils.HasWolfKingFightStarted())
            {
                var wolfPosition = Vector2.Zero;
                var wolfKingSpawn = Main.npc.FirstOrDefault(x => x.active && x.type == ModContent.NPCType<WolfKingSpawn>());
                if (wolfKingSpawn != null)
                    wolfPosition = wolfKingSpawn.Center;
                else
                {
                    var wolfKing = Main.npc.FirstOrDefault(x => x.active && x.type == ModContent.NPCType<WolfKing>());
                    if (wolfKing != null)
                        wolfPosition = wolfKing.Center;
                }

                if (wolfPosition != Vector2.Zero)
                {
                    foreach (var player in Main.ActivePlayers)
                    {
                        var playerPosition = player.Center;
                        var distance = Vector2.DistanceSquared(wolfPosition, playerPosition);
                        var minDistance = 16 * 200;
                        if (distance < minDistance * minDistance)
                        {
                            player.noBuilding = true;
                            player.AddBuff(BuffID.NoBuilding, 60);
                        }
                    }
                }
            }
        }
        public override void PreUpdateBuffs()
        {
            base.PreUpdateBuffs();
            AddDebuff();
        }

        public void BehemothSpawnCheck()
        {
            // Don't spawn if already killed or the previous bosses haven't been killed yet
            if (DownedBossSystem.DownedBehemoth || !DownedBossSystem.DownedWhiteTail || !DownedBossSystem.DownedWolfKing)
                return;

            // Don't spawn if he is already present
            if (NPC.AnyNPCs(ModContent.NPCType<Behemoth>()) || NPC.AnyNPCs(ModContent.NPCType<BehemothSpawn>()))
                return;

            foreach (var player in Main.ActivePlayers)
            {
                {
                    int minRangeFromPlayer = 80;
                    int maxRange = 120;                         // Same range as the clentaminator, so probably enough to be off-screen
                    int blockCheckSpacing = 8;                  // Only check every 8 blocks for performance
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

        public override void PreUpdate()
        {
            base.PreUpdate();
            ChangedUtils.WolfKingSpawnCheck();
            BehemothSpawnCheck();
        }

        public void MakeCrystalsShinier()
        {
            var indexRed = ModContent.TileType<Content.Tiles.Furniture.CrystalRed>();
            var indexGreen = ModContent.TileType<Content.Tiles.Furniture.CrystalRed>();
            if (Player.townNPCs > 0 && BirthdayParty.PartyIsUp)
            {
                Main.tileShine[indexRed] = 800;
                Main.tileShine[indexGreen] = 800;
            }
            else
            {
                Main.tileShine[indexRed] = 2400;
                Main.tileShine[indexGreen] = 2400;
            }
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            var changedNPC = npc.Changed();

            if (npc.HasBuff(BuffID.Lovestruck) && changedNPC.GooType != GooType.Invalid && !npc.boss)
                return false;

            if (TransfurTypeCurrent != null && changedNPC.GooType == TransfurTypeCurrent.gooType)
                return false;

            return base.CanBeHitByNPC(npc, ref cooldownSlot);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (TransfurTypeCurrent != null && TransfurTypeCurrent.npcType == target.type)
                Evolve();

            base.OnHitNPC(target, hit, damageDone);
        }

        public override void PostUpdateEquips()
        {
            var changedPlayer = Player.ChangedPlayer();
            if (TransfurTypeCurrent != null)
            {
                TransfurTypeCurrent.UpdatePlayerStats(Player);
                if (Player.mount.Active)
                    SetTransfur(null);
            }   
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            // dmg = attack dmg − def × f
            // classic f = 0.5
            // expert f = 0.75
            // master f = 1
            // we stick to classic for now
            // attack has random chance to do - or + 15 percent

            // runspeed = 10 in hardmode, damage is 50?
            // so probably playerspeed is half mount runspeed
            if (Player.mount.Active && (Player.mount.Type == ModContent.MountType<WhiteLatexTaurMount>() && Math.Abs(Player.velocity.X) > Player.mount.RunSpeed * 0.8f))
            {
                var canSpawnHurtProjectile = true;
                foreach (var projectile in Main.ActiveProjectiles)
                {
                    if (projectile.type == ModContent.ProjectileType<InvisibleProjectile>() && projectile.ai[0] == npc.whoAmI)
                    {
                        canSpawnHurtProjectile = false;
                    }
                }

                modifiers.SourceDamage *= 0.85f;
                modifiers.Knockback *= 0f;


                var projectileVelocity = new Vector2(Math.Sign(Player.velocity.X) * 0.1f, 2);
                var projectileDamage = (int)(Math.Abs(Player.velocity.X) * 10);

                var dmgThatWillBeDealt = projectileDamage * 1.15 - npc.defense * 0.5f;
                
                // Set received damage to 0 if we likely instakill it
                if (dmgThatWillBeDealt > npc.life)
                    modifiers.Cancel();

                // Don't cancel the damage from the npc if he is immune or nearly immune to knockback
                var projectileKb = Math.Abs(Player.velocity.X) * 2;
                var appliedKb = projectileKb * npc.knockBackResist;
                if (appliedKb > 4)
                    modifiers.Cancel();

                if (canSpawnHurtProjectile)
                {
                    // Since we can't do something like NPC.StrikeNPC in vanilla, we spawn in an invisble projectile to do it
                    var hurtProjectile = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), npc.Center, projectileVelocity, ModContent.ProjectileType<InvisibleProjectile>(), projectileDamage, projectileKb, Player.whoAmI, npc.whoAmI);
                }
            }
        }
    }
}

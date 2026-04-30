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
            var texturePath = "ChangedSpecialMod/Content/NPCs/Transparent";
            var nFrames = 3;

            if (changedPlayer.transfurIndex != TransfurType.None)
            {
                switch(changedPlayer.transfurIndex)
                {
                    // Black
                    default:
                        texturePath = "ChangedSpecialMod/Content/NPCs/MaleDarkLatex";
                        break;
                    case TransfurType.BlackGoop:
                        texturePath = "ChangedSpecialMod/Content/NPCs/BlackGoop";
                        nFrames = 4;
                        break;
                    case TransfurType.BlackCub:
                        texturePath = "ChangedSpecialMod/Content/NPCs/DarkLatexCub";
                        nFrames = 4;
                        break;
                    case TransfurType.BlackAdult:
                        texturePath = "ChangedSpecialMod/Content/NPCs/MaleDarkLatex";
                        break;
                    case TransfurType.BlackFast:
                        texturePath = "ChangedSpecialMod/Content/NPCs/WingedDarkLatex";
                        nFrames = 4;
                        break;
                    case TransfurType.BlackStrong:
                        texturePath = "ChangedSpecialMod/Content/NPCs/Wendigo";
                        nFrames = 4;
                        break;

                    // White
                    case TransfurType.WhiteGoop:
                        texturePath = "ChangedSpecialMod/Content/NPCs/WhiteGoop";
                        nFrames = 4;
                        break;
                    case TransfurType.WhiteCub:
                        texturePath = "ChangedSpecialMod/Content/NPCs/WhiteLatexCub";
                        break;
                    case TransfurType.WhiteAdult:
                        texturePath = "ChangedSpecialMod/Content/NPCs/WhiteKnight";
                        break;
                    case TransfurType.WhiteFast:
                        texturePath = "ChangedSpecialMod/Content/NPCs/WhiteLatexTaur";
                        nFrames = 13;
                        break;

                    // Squid Dog
                    case TransfurType.SquidCub:
                        texturePath = "ChangedSpecialMod/Content/NPCs/SquidDogCub";
                        nFrames = 4;
                        break;
                    case TransfurType.SquidAdult:
                        texturePath = "ChangedSpecialMod/Content/NPCs/SquidDog";
                        nFrames = 4;
                        break;

                    case TransfurType.GermanShepherd:
                        texturePath = "ChangedSpecialMod/Content/NPCs/GermanShepherd";
                        nFrames = 4;
                        break;
                }
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

            if (changedPlayer.transfurIndex == TransfurType.WhiteFast)
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
        public bool pictureViewerOpen = false;
        public int pictureIndex = -1;
        public bool hidePlayer = false;
        public TransfurType transfurIndex = TransfurType.None;
        public GooType transfurGooType = GooType.None;
        //0 black
        //1 white

        private Dictionary<TransfurType, TransfurType> DevolveTypes = new Dictionary<TransfurType, TransfurType>()
        {
            { TransfurType.BlackCub, TransfurType.BlackGoop },
            { TransfurType.BlackAdult, TransfurType.BlackCub },
            { TransfurType.BlackFast, TransfurType.BlackAdult },
            { TransfurType.BlackStrong, TransfurType.BlackFast },

            { TransfurType.WhiteCub, TransfurType.WhiteGoop },
            { TransfurType.WhiteAdult, TransfurType.WhiteCub },
            { TransfurType.WhiteFast, TransfurType.WhiteAdult },
        };

        private void Devolve()
        {
            var targetType = transfurIndex;

            if (DevolveTypes.ContainsKey(targetType))
                targetType = DevolveTypes[targetType];
            else
                targetType = TransfurType.None;

            if (targetType != transfurIndex)
            {
                SetTransfur(targetType);
            }
        }

        private void Evolve()
        {
            if (transfurIndex == TransfurType.None)
                return;

            var targetType = transfurIndex;

            if (transfurIndex == TransfurType.BlackGoop && DownedBossSystem.HasBlackCubTF)
                targetType = TransfurType.BlackCub;
            else if (transfurIndex == TransfurType.BlackCub && DownedBossSystem.HasBlackAdultTF)
                targetType = TransfurType.BlackAdult;
            else if (transfurIndex == TransfurType.BlackAdult && DownedBossSystem.HasBlackFastTF)
                targetType = TransfurType.BlackFast;
            else if (transfurIndex == TransfurType.BlackFast && DownedBossSystem.HasBlackStrongTF)
                targetType = TransfurType.BlackStrong;

            else if (transfurIndex == TransfurType.WhiteGoop && DownedBossSystem.HasWhiteCubTF)
                targetType = TransfurType.WhiteCub;
            else if (transfurIndex == TransfurType.WhiteCub && DownedBossSystem.HasWhiteAdultTF)
                targetType = TransfurType.WhiteAdult;
            else if (transfurIndex == TransfurType.WhiteAdult && DownedBossSystem.HasWhiteFastTF)
                targetType = TransfurType.WhiteFast;

            // Type was changed
            if (targetType != transfurIndex)
                SetTransfur(targetType);
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
            if (transfurIndex == TransfurType.None)
                return;

            Vector2 aimDirection = Main.MouseWorld - Player.Center;
            aimDirection.Normalize();

            if (transfurIndex == TransfurType.SquidAdult)
            {
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
        }

        public void SetOrRemoveTransfur(TransfurType transfur)
        {
            if (transfurIndex == TransfurType.None)
                SetTransfur(transfur);
            else
                SetTransfur(TransfurType.None);
        }

        private void SetTransfur(TransfurType transfur)
        {
            transfurIndex = transfur;

            var gooType = GooType.None;
            var dustType = DustID.Asphalt;

            if (transfurIndex == TransfurType.BlackGoop || transfurIndex == TransfurType.BlackCub || 
                transfurIndex == TransfurType.BlackAdult || transfurIndex == TransfurType.BlackFast || 
                transfurIndex == TransfurType.BlackStrong)
            {
                gooType = GooType.Black;
            }

            if (transfurIndex == TransfurType.WhiteGoop || transfurIndex == TransfurType.WhiteCub || 
                transfurIndex == TransfurType.WhiteAdult || transfurIndex == TransfurType.WhiteFast)
            {
                gooType = GooType.White;
                dustType = DustID.SnowBlock;
            }

            transfurGooType = gooType;

            if (!Main.dedServ)
            {
                var nParticles = 40;
                for (int i = 0; i < nParticles; i++)
                {
                    var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, dustType, 0, 0, 1);
                    dust.velocity.X += Main.rand.NextFloat(-0.05f, 0.05f);
                    dust.velocity.Y += Main.rand.NextFloat(-0.05f, 0.05f);
                }
            }



            if (transfurIndex != TransfurType.None)
                SoundEngine.PlaySound(Sounds.SoundTransfur, Player.Center);
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            var player = drawInfo.drawPlayer;
            var changedPlayer = player.ChangedPlayer();
            if (changedPlayer.transfurIndex != TransfurType.None)
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

            if (transfurGooType == GooType.Black && changedNPC.GooType == GooType.Black)
                return false;
            else if (transfurGooType == GooType.White && changedNPC.GooType == GooType.White)
                return false;

            return base.CanBeHitByNPC(npc, ref cooldownSlot);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (transfurIndex != TransfurType.None)
            {
                // Black
                if (transfurIndex == TransfurType.BlackGoop && target.type == ModContent.NPCType<BlackGoop>())
                {
                    DownedBossSystem.HasBlackCubTF = true;
                    Evolve();
                }
                else if (transfurIndex == TransfurType.BlackCub && target.type == ModContent.NPCType<DarkLatexCub>())
                {
                    DownedBossSystem.HasBlackAdultTF = true;
                    Evolve();
                }
                else if (transfurIndex == TransfurType.BlackAdult && (target.type == ModContent.NPCType<MaleDarkLatex>() || target.type == ModContent.NPCType<FemaleDarkLatex>()))
                {
                    DownedBossSystem.HasBlackFastTF = true;
                    Evolve();
                }
                else if (transfurIndex == TransfurType.BlackFast && target.type == ModContent.NPCType<WingedDarkLatex>())
                {
                    DownedBossSystem.HasBlackStrongTF = true;
                    Evolve();
                }

                // White
                else if (transfurIndex == TransfurType.WhiteGoop && target.type == ModContent.NPCType<WhiteGoop>())
                {
                    DownedBossSystem.HasWhiteCubTF = true;
                    Evolve();
                }
                else if (transfurIndex == TransfurType.WhiteCub && target.type == ModContent.NPCType<WhiteLatexCub>())
                {
                    DownedBossSystem.HasWhiteAdultTF = true;
                    Evolve();
                }
                else if (transfurIndex == TransfurType.WhiteAdult && target.type == ModContent.NPCType<WhiteKnight>())
                {
                    DownedBossSystem.HasWhiteFastTF = true;
                    Evolve();
                }

                return;
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void PostUpdateEquips()
        {
            var changedPlayer = Player.ChangedPlayer();
            var transfurType = changedPlayer.transfurIndex;
            if (transfurType != TransfurType.None)
            {
                if (Player.mount.Active)
                {
                    SetTransfur(TransfurType.None);
                }
            }
            ModifyStats();
        }

        private void ModifyStats()
        {
            var changedPlayer = Player.ChangedPlayer();
            var transfurType = changedPlayer.transfurIndex;
            if (transfurType != TransfurType.None)
            {
                var lifeMultiplier = 1f;
                var speedMultiplier = 1f;
                var jumpHeightMultiplier = 1f;
                var extraDefense = 0;
                var damageBonus = 0.0f;

                var isGoop = transfurType == TransfurType.BlackGoop || transfurType == TransfurType.WhiteGoop;
                var isCub = transfurType == TransfurType.BlackCub || transfurType == TransfurType.WhiteCub;
                var isAdult = transfurType == TransfurType.BlackAdult || transfurType == TransfurType.WhiteAdult;
                var isFast = transfurType == TransfurType.BlackFast || transfurType == TransfurType.WhiteFast;

                if (isGoop)
                {
                    lifeMultiplier = 0.6f;
                    speedMultiplier = 0.75f;
                    jumpHeightMultiplier = 1.8f;

                    // Ground check needed, because this will also be true when reaching the highest point of the jump
                    if (Player.velocity.Y != 0)
                        speedMultiplier = 2;
                }
                else if (isCub)
                {
                    lifeMultiplier = 0.8f;
                    speedMultiplier = 1.2f;
                    jumpHeightMultiplier = 1.5f;
                }
                else if (isAdult)
                {
                    lifeMultiplier = 1.25f;
                    extraDefense = 5;
                    speedMultiplier = 1f;
                }
                else if (isFast)
                {
                    lifeMultiplier = 1.25f;
                    extraDefense = 5;
                    speedMultiplier = 2f;
                }
                else if (transfurType == TransfurType.BlackStrong)
                {
                    lifeMultiplier = 2f;
                    extraDefense = 10;
                    speedMultiplier = 0.75f;
                    damageBonus = 0.3f;
                }

                else if (transfurType == TransfurType.SquidAdult)
                {
                    lifeMultiplier = 1.25f;
                    extraDefense = 5;
                    speedMultiplier = 1f;
                    Player.ignoreWater = true;
                    if (Player.wet)
                        Player.AddBuff(BuffID.Gills, 1);
                }

                Player.jumpHeight = (int)(Player.jumpHeight * jumpHeightMultiplier);
                Player.statDefense += extraDefense;
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * lifeMultiplier);
                Player.accRunSpeed = (int)(Player.accRunSpeed * speedMultiplier);
                Player.maxRunSpeed = (int)(Player.maxRunSpeed * speedMultiplier);
                Player.GetDamage<MeleeDamageClass>() += damageBonus;
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

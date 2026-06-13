using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Mounts;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static ChangedSpecialMod.ChangedSpecialMod;

namespace ChangedSpecialMod
{
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

        // This is not needed for vanilla, but it is needed if you don't  
        // want the damaging effects of the Abyss from Calamity
        // Set breath to 10X the max value so the meter won't flicker rapidly
        private void CalamityAbyssBreathing(ref Player player)
        {
            player.breath = 2000;
            player.breathCD = 0;
        }

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
            {
                player.ignoreWater = true;
                player.accFlipper = true;
            }
            if (player.wet && waterBreathing)
            {
                player.AddBuff(BuffID.Gills, 1);
                CalamityAbyssBreathing(ref player);
            }
        }
    }

    public class ChangedSpecialModPlayer : ModPlayer
    {
        public Transfur TransfurTypeCurrent = null;

        public bool pictureViewerOpen = false;
        public int pictureIndex = -1;
        public bool hidePlayer = false;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
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

        /*
        public void SetTransfur(Transfur transfur)
        {
            // Only spawn dust particles on the client
            if (Main.netMode != NetmodeID.Server)
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
                AudioSystem.PlayTransfurSound(Player.Center);

            TransfurTypeCurrent = transfur;
        }
        */

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

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            var changedNPC = npc?.Changed();
            var changedPlayer = Player.ChangedPlayer();
            if (changedNPC != null && changedPlayer != null && changedNPC.GooType != GooType.Invalid && ChangedSpecialModClientConfig.Instance.NPCsCanTransfurPlayer)
            {
                var damage = hurtInfo.Damage;
                // Damage would kill player
                if (damage > Player.statLife && TransfurTypeCurrent == null)
                {
                    ChangedUtils.SetTransfurFromNPCType(Player.whoAmI, npc.type);
                    if (changedPlayer.TransfurTypeCurrent != null)
                    {
                        Player.statLife += damage;
                        Player.statLife += 50;
                    }
                }
            }

            base.OnHitByNPC(npc, hurtInfo);
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            base.Kill(damage, hitDirection, pvp, damageSource);
            ChangedUtils.UntransfurPlayer(Player.whoAmI);
        }

        public void MakeCrystalsShinier()
        {
            var indexRed = ModContent.TileType<Content.Tiles.Furniture.CrystalRed>();
            var indexGreen = ModContent.TileType<Content.Tiles.Furniture.CrystalGreen>();
            var indexWhite = ModContent.TileType<Content.Tiles.Furniture.CrystalWhite>();

            if (Player.townNPCs > 0 && BirthdayParty.PartyIsUp)
            {
                Main.tileShine[indexRed] = 800;
                Main.tileShine[indexGreen] = 800;
                Main.tileShine[indexWhite] = 800;
            }
            else
            {
                Main.tileShine[indexRed] = 2400;
                Main.tileShine[indexGreen] = 2400;
                Main.tileShine[indexWhite] = 2400;
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


        // Remove this once we made a copy of the projectile, which is hard due to how they are programmed
        // Add electrified debuff during experiment009 boss fight
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (proj.type == ProjectileID.VortexLightning && NPC.AnyNPCs(ModContent.NPCType<Experiment009>()))
            {
                Player.AddBuff(BuffID.Electrified, 60);
            }
        }

        public override void PostUpdateEquips()
        {
            var changedPlayer = Player.ChangedPlayer();
            if (TransfurTypeCurrent != null)
            {
                TransfurTypeCurrent.UpdatePlayerStats(Player);
                if (Player.mount.Active)
                    ChangedUtils.UntransfurPlayer(Player.whoAmI);
            }   
        }

        public override void Initialize()
        {
            TransfurTypeCurrent = null;
            _npcType = -1;
        }
        /*
        public override void CopyClientState(ModPlayer targetCopy)
        {
            var clone = (ChangedSpecialModPlayer)targetCopy;
            clone._npcType = _npcType;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            var oldPlayer = (ChangedSpecialModPlayer)clientPlayer;

            if (oldPlayer._npcType == _npcType)
                return;

            ModPacket packet = ModContent.GetInstance<ChangedSpecialMod>().GetPacket();

            packet.Write((byte)MessageType.SyncTransfurPlayer);
            packet.Write((byte)Player.whoAmI);
            packet.Write(_npcType);

            packet.Send();
        }
        */
        public void SetTransfur(Transfur transfur)
        {
            if (transfur == null)
            {
                NpcType = -1;
                return;
            }

            NpcType = transfur.npcType;
        }

        private void ApplyUntransfur()
        {
            TransfurVisuals();
            TransfurTypeCurrent = null;

            // Undo any persistent effects here.
            // Reset visuals, body parts, animation state, etc.
        }

        private int _npcType = -1;

        /// <summary>
        /// Network-authoritative transfur identifier.
        /// -1 means not transformed.
        /// </summary>
        public int NpcType
        {
            get => _npcType;
            set
            {
                if (_npcType == value)
                    return;

                _npcType = value;

                if (_npcType == -1)
                    ApplyUntransfur();
                else
                    ApplyTransfur(_npcType);
            }
        }

        public bool IsTransfurred => NpcType != -1;

        private void ApplyTransfur(int npcType)
        {
            if (ChangedUtils.EvolutionsLines == null ||
                ChangedUtils.EvolutionsLines.Count == 0)
            {
                ChangedUtils.InitTransfurTypes();
            }

            Transfur transfur = null;

            foreach (var evolutionLine in ChangedUtils.EvolutionsLines.Values)
            {
                transfur = evolutionLine.FirstOrDefault(x => x.npcType == npcType);

                if (transfur != null)
                    break;
            }

            TransfurTypeCurrent = transfur;
            TransfurVisuals();
        }

        private void TransfurVisuals()
        {
            // Only spawn dust particles on the client
            if (Main.netMode != NetmodeID.Server)
            {
                if (TransfurTypeCurrent != null)
                {
                    var dustType = TransfurTypeCurrent.gooType == GooType.Black ? DustID.Asphalt : DustID.SnowSpray;
                    var nParticles = 40;
                    for (int i = 0; i < nParticles; i++)
                    {
                        var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, dustType, 0, 0, 1);
                        dust.velocity.X += Main.rand.NextFloat(-0.05f, 0.05f);
                        dust.velocity.Y += Main.rand.NextFloat(-0.05f, 0.05f);
                    }
                }
            }

            if (TransfurTypeCurrent != null)
                AudioSystem.PlayTransfurSound(Player.Center);
        }

        public override void SaveData(TagCompound tag)
        {
            tag["TransfurNpcType"] = _npcType;
        }

        public override void LoadData(TagCompound tag)
        {
            NpcType = tag.GetInt("TransfurNpcType");
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

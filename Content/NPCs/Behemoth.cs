using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
    [AutoloadBossHead]
    public class Behemoth : ModNPC
	{
        private enum ActionState
        {
            Grow,
            Idle,
            Shrink,
            Death,
            ShrinkDeath,
            ShrinkDespawn
        }

        // We are gonna do it just like GameMaker, but we have to do it all ourselves
        public double imageSpeed = 5D;
        public int imageIndex = 0;

        public int[] animation = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 6, 5, 4, 3, 2, 1 };

        public int[] animGrow = new int[] { 0, 1, 2, 3, 4, 5, 6, 7};
        public int[] animIdle = new int[] { 7, 8, 9 };
        public int[] animShrink = new int[] { 6, 5, 4, 3, 2, 1, 0 };
        public int[] animShock = new int[] { 13 };

        public int ImageLength {get { return animation.Length; }}
        public bool Loop = false;

        public double imageCounter = 0D;

        public ref float AIState => ref NPC.ai[0];
        public ref float AITimer => ref NPC.ai[1];

        public int npcDirection = 1;
        public float shrinkTime = 2f;

        public int maxFollowDistance = 300 * 16;

        public double CalcAnimationDuration()
        {
            return animation.Length / imageSpeed * 60;
        }

        public override void SetStaticDefaults() 
        {
            Main.npcFrameCount[Type] = 14;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Scale = 0.5f,
                PortraitScale = 0.5f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

		public override void SetDefaults() 
        {
			NPC.width = 192;//96
			NPC.height = 192;//129
            NPC.damage = 0;
            NPC.defense = 12;//32
            NPC.lifeMax = 6000;
            //NPCHit6 = werewolf hurt sound
            NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = Item.buyPrice(0, 8);
            NPC.knockBackResist = 0.0f;
            NPC.aiStyle = -1;
			AIType = NPCID.None;
			AnimationType = NPCID.None;
            NPC.boss = true;
            SpawnModBiomes = new int[] { ModContent.GetInstance<WhiteLatexSurfaceBiome>().Type };
            NPC.scale = 0.8f;

            var changedNPC = NPC.Changed();
            changedNPC.GooType = GooType.WhiteOnly;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.HitEffectScale = 3;
        }

        public override bool CheckDead()
        {
            // Prevent the boss from dying normally
            if (NPC.life <= 1 && AIState != (float)ActionState.ShrinkDeath)
            {
                NPC.life = 1;
                return false; // Stops death
            }

            return true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.Behemoth.Description")),
            });
        }

        public override void OnSpawn(IEntitySource source)
        {
            var npcName = Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.Behemoth.DisplayName");
            Main.NewText(Language.GetTextValue("Announcement.HasAwoken", npcName), new Color(175, 75, 255));
            SpawnHand();
        }

        public override void OnKill()
        {
            DownedBossSystem.DownedBehemoth = true;
            DoDespawn();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WhiteSyringe>()));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                imageIndex = 7;
            }
            else
            {
                if (AIState == (float)ActionState.Idle)
                {
                    imageIndex = 7;

                    if (NPC.HasValidTarget)
                    {
                        var lookLeft = 7;
                        var lookMiddle = 8;
                        var lookRight = 9;
                        
                        var fiddleHand = Main.npc.FirstOrDefault(x => x.active && x.type == ModContent.NPCType<BehemothHand>() && x.ai[0] == 4);
                        
                        // Fiddling his hands, naughty face
                        if (fiddleHand != null)
                        {
                            lookLeft = 10;
                            lookMiddle = 11;
                            lookRight = 12;
                        }

                        var player = Main.player[NPC.target];
                        var dist = player.Center.X - NPC.Center.X;
                        /*if (Math.Abs(dist) < 3 * 16)
                            imageIndex = 8;
                        else*/
                        if (dist < 0 && NPC.spriteDirection < 0)
                            imageIndex = lookLeft;
                        else if (dist > 0 && NPC.spriteDirection > 0)
                            imageIndex = lookLeft;
                        else
                            imageIndex = lookRight;
                    }
                }
                else
                {
                    imageCounter += imageSpeed;
                    if (imageCounter >= ImageLength * 60)
                    {
                        if (Loop)
                        {
                            imageCounter %= ImageLength * 60;
                        }
                        else
                        {
                            imageCounter = ImageLength * 60 - 1;
                        }
                    }

                    var arrayIndex = (int)(imageCounter / 60D);
                    imageIndex = animation[arrayIndex];
                }
            }

            NPC.frame.Y = imageIndex * frameHeight;
        }

        private void SwitchAnimation(int[] newAnimation)
        {
            imageCounter = 0;
            animation = newAnimation;
        }

        private void SwitchState(ActionState newState)
        {
            AIState = (float)newState;
            AITimer = 0;
            NPC.netUpdate = true;
        }

        private void SpawnHand()
        {
            var entitySource = NPC.GetSource_FromAI();
            int xPos = (int)NPC.Center.X + Main.rand.Next(-10, 10);
            //BehemothHand
            NPC npcHand = NPC.NewNPCDirect(entitySource, xPos, (int)NPC.Center.Y, ModContent.NPCType<BehemothHand>(), NPC.whoAmI);
            NPC npcHand2 = NPC.NewNPCDirect(entitySource, xPos + 96, (int)NPC.Center.Y, ModContent.NPCType<BehemothHand>(), NPC.whoAmI);
        }

        private void StateGrow()
        {
            AITimer++;

            if (AITimer == 1)
            {
                SwitchAnimation(animGrow);
                SoundEngine.PlaySound(Sounds.SoundTransfur, NPC.Center);
            }

            if (AITimer >= 2 * 60)
            {
                SwitchState(ActionState.Idle);
            }
        }

        private void StateIdle()
        {
            AITimer++;

            if (AITimer == 1)
            {
                SwitchAnimation(animIdle);
            }

            NPC.TargetClosest(false);

            if (NPC.life == 1)
            {
                SwitchState(ActionState.Death);
            }

            if (NPC.HasValidTarget)
            {
                var player = Main.player[NPC.target];

                if (Vector2.Distance(NPC.Center, player.Center) > 16 * 60)
                {
                    SwitchState(ActionState.Shrink);
                }
            }
            else
            {
                NPC.TargetClosest();
                if (!NPC.HasValidTarget)
                    SwitchState(ActionState.Shrink);
            }
            /*
            else
            {
                SwitchState(ActionState.ShrinkDespawn);
            }*/
        }

        private void StateShrink()
        {
            AITimer++;

            if (AITimer == 1)
            {
                npcDirection *= -1;
                SwitchAnimation(animShrink);
                SoundEngine.PlaySound(Sounds.SoundTransfur, NPC.Center);
            }

            if (AITimer >= 2 * 60)
            {
                NPC.TargetClosest(false);
                if (!NPC.HasValidTarget)
                {
                    DoDespawn();
                    return;
                }
                var dist = Vector2.DistanceSquared(NPC.Center, Main.player[NPC.target].Center);
                // Despawn if target is too far away
                if (dist > maxFollowDistance * maxFollowDistance)
                {
                    DoDespawn();
                    return;
                }

                MoveToRandomPosition();
                SwitchState(ActionState.Grow);
            }
        }

        private void DoDespawn()
        {
            foreach (var npc in Main.npc)
            {
                if (npc.type == ModContent.NPCType<BehemothHand>())
                    npc.active = false;
            }
            NPC.active = false;
        }

        private void StateShrinkDeath()
        {
            AITimer++;

            if (AITimer == 1)
            {
                npcDirection *= -1;
                SwitchAnimation(animShrink);
                SoundEngine.PlaySound(Sounds.SoundTransfur, NPC.Center);
            }

            if (AITimer >= 90)
            {
                NPC.StrikeInstantKill();
            }
        }

        private void StateDeath()
        {
            AITimer++;

            if (AITimer == 1)
            {
                npcDirection *= -1;
                SwitchAnimation(animShock);
                SoundEngine.PlaySound(NPC.DeathSound, NPC.Center);
            }

            if (AITimer >= 2 * 60)
            {
                SwitchState(ActionState.ShrinkDeath);
            }
        }

        private void MoveToRandomPosition()
        {
            NPC.TargetClosest(false);

            if (NPC.HasValidTarget)
            {
                var player = Main.player[NPC.target];

                int xPos = (int)(player.Center.X / 16f) + npcDirection * Main.rand.Next(10, 20);
                int yPos = (int)(player.Center.Y / 16f) - 10;

                for (int y= yPos; y < yPos + 30; y++)
                {
                    if (WorldGen.SolidTile(xPos, y))
                    {
                        NPC.position = new Vector2(xPos * 16 - 0.5f * NPC.width, y * 16 -  NPC.height);
                        NPC.direction = player.Center.X < NPC.Center.X ? -1 : 1;
                        NPC.spriteDirection = NPC.direction;
                        break;
                    }
                }
            }
        }

        public override void AI()
        {
            NPC.dontTakeDamage = NPC.life == 1;

            switch (AIState)
            {
                case (float)ActionState.Grow:
                    StateGrow();
                    break;
                case (float)ActionState.Idle:
                    StateIdle();
                    break;
                case (float)ActionState.Shrink:
                    StateShrink();
                    break;
                case (float)ActionState.ShrinkDeath:
                    StateShrinkDeath();
                    break;
                case (float)ActionState.Death:
                    StateDeath();
                    break;
                    /*
                case (float)ActionState.ShrinkDespawn:
                    StateShrinkDespawn();
                    break;
                    */
            }
        }
    }
}

using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items.Syringes;
using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
    [AutoloadBossHead]
    public class SquidDogBoss : ModNPC
	{
        private enum ActionState
        {
            Idle,
            Attack
        }

        public double imageSpeed = 5D;
        public int imageIndex = 0;

        public int[] animation = new int[] { 4 };
        public int[] animIdle = new int[] { 4 };
        public int[] animSnap = new int[] { 5, 6, 7 };
        public int[] animShock = new int[] { 8 };
        public int[] animStand = new int[] { 9 };

        public int ImageLength => animation.Length;
        public bool Loop = false;

        public double imageCounter = 0D;

        // ai[0] and ai[1] are used in every state
        // Unles we add code to send and receive extra ai, we can only use [2] and [3]

        // All states
        public ref float AIState => ref NPC.ai[0];
        public ref float AITimer => ref NPC.ai[1];

        // Idle state
        public ref float AINSnaps => ref NPC.ai[2];

        // Stand state
        public ref float AISpikeWaveDirection => ref NPC.ai[2];
        public ref float AISpikeWaveIndex => ref NPC.ai[3];

        private Rectangle RoomBounds = Rectangle.Empty;
        public int maxFollowDistance = 120 * 16;

        public override void SetStaticDefaults() 
        {
            Main.npcFrameCount[Type] = 10;
        }

		public override void SetDefaults() 
        {
			NPC.width = 100;
			NPC.height = 100;
            NPC.damage = 0;
            NPC.defense = 10;
            NPC.lifeMax = 5000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = ChangedUtils.GetNPCValue(gold: 6);
            NPC.knockBackResist = 0.0f;
            NPC.aiStyle = -1;
			AIType = NPCID.None;
			AnimationType = NPCID.None;
            NPC.boss = true;
            SpawnModBiomes = new int[] { ModContent.GetInstance<BlackLatexSurfaceBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.GooType = GooType.Black;
        }

        public override void OnKill()
        {
            if (!DownedBossSystem.DownedWolfKing)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    var msg = Language.GetTextValue("Mods.ChangedSpecialMod.Messages.BehemothCanSpawn");
                    Main.NewText(msg, byte.MaxValue, 240, 20);
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.ChangedSpecialMod.Messages.BehemothCanSpawn", NPC.FullName), new Color(175, 75, 255));
                }
            }

            //DownedBossSystem.DownedWolfKing = true;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (!Main.dedServ)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WolfKing1").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WolfKing2").Type, 1f);
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.WolfKing.Description")),
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BlackSyringe>()));
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                imageIndex = 4;
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

            NPC.frame.Y = imageIndex * frameHeight;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(Language.GetTextValue("Announcement.HasAwoken", NPC.FullName), new Color(175, 75, 255));
            if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", NPC.FullName), new Color(175, 75, 255));
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

        private void StateIdle()
        {
            AITimer++;

            if (AITimer == 180)
            {
                SwitchState(ActionState.Attack);
            }
        }

        private void StateAttack()
        {
            AITimer++;

            if (AITimer == 1)
            {
                var player = ChangedUtils.GetClosestPlayer((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16, true);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    var xPos = (int)player.Center.X;
                    var yPos = (int)player.Center.Y + 30 * 16;

                    int npcIndex = NPC.NewNPC(NPC.GetSource_FromAI(), xPos, yPos, ModContent.NPCType<SquidDogTentacleHead>());
                }
            }

            if (AITimer == 120)
            {
                SwitchState(ActionState.Idle);
            }
        }


        public override void AI()
        {
            // No matter what state he is in, players cannot build or break blocks while he is alive.
            // The code for this is in ChangedSpecialModPlayer, because doing it here instead does not work
            
            switch (AIState)
            {
                case (float)ActionState.Idle:
                    StateIdle();
                    break;
                case (float)ActionState.Attack:
                    StateAttack();
                    break;
            }
        }
    }
}

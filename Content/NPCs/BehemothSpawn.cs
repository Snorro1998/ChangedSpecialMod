using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
    public class BehemothSpawn : ModNPC
	{
        private enum ActionState
        {
            Idle,
            StartBossFight
        }

        // We are gonna do it just like GameMaker, but we have to do it all ourselves
        public double imageSpeed = 5D;
        public int imageIndex = 0;

        public int[] animation = new int[] { 0 };
        public int[] animIdle = new int[] { 0 };

        public int ImageLength {get { return animation.Length; }}
        public bool Loop = false;

        public double imageCounter = 0D;

        public ref float AIState => ref NPC.ai[0];
        public ref float AITimer => ref NPC.ai[1];
        public ref float AINSnaps => ref NPC.ai[2];

        public int npcDirection = 1;

        public int maxFollowDistance = 300 * 16;

        public override void SetStaticDefaults() 
        {
            Main.npcFrameCount[Type] = 1;
            ChangedUtils.HideFromBestiary(this);
        }

		public override void SetDefaults() 
        {
            NPC.width = 192;//96
            NPC.height = 192;//129
            NPC.damage = 0;
            NPC.defense = 32;
            NPC.lifeMax = 5000;
            NPC.HitSound = SoundID.NPCHit6;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.0f;
            NPC.aiStyle = -1;
			AIType = NPCID.None;
			AnimationType = NPCID.None;
            NPC.dontTakeDamage = true;
            SpawnModBiomes = new int[] { ModContent.GetInstance<BlackLatexSurfaceBiome>().Type };
            NPC.scale = 0.8f;

            var changedNPC = NPC.Changed();
            changedNPC.GooType = GooType.WhiteOnly;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override void FindFrame(int frameHeight)
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
        }

        private void StateIdle()
        {
            AITimer++;

            if (AITimer == 1)
            {
                SwitchAnimation(animIdle);
            }

            if (!NPC.HasValidTarget)
            {
                NPC.TargetClosest(false);
            }

            if (NPC.HasValidTarget)
            {
                var player = Main.player[NPC.target];
                var distance = Vector2.DistanceSquared(player.Center, NPC.Center);

                if (distance > maxFollowDistance * maxFollowDistance)
                {
                    NPC.TargetClosest(false);
                    return;
                }

                if (Vector2.Distance(player.Center, NPC.Center) < 5 * 16)
                {
                    SwitchState(ActionState.StartBossFight);
                }
            }
        }

        private void StateStartBossFight()
        {
            AITimer++;

            if (AITimer == 1)
            {

                int type = ModContent.NPCType<Behemoth>();

                NPC.NewNPC(NPC.GetSource_FromAI(), 0, 0, type);

                foreach (var npc in Main.npc)
                {
                    if (npc.type == type)
                    {
                        npc.position = NPC.position;
                        break;
                    }
                }
                NPC.active = false;
            }
        }

        public override void AI()
        {
            // Players cannot build or break blocks when he is not in his idle state.
            // The code for this is in ChangedSpecialModPlayer, because doing it here instead does not work

            switch (AIState)
            {
                case (float)ActionState.Idle:
                    StateIdle();
                    break;
                case (float)ActionState.StartBossFight:
                    StateStartBossFight();
                    break;
            }
        }
    }
}

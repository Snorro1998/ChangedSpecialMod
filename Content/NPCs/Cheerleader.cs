using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
	public class Cheerleader : ModNPC
	{
        public enum ActionState
        {
            Idle,
            Cheer,
            Howl,
            Shocked
        }

        public double ImageSpeed = 10D;
        public int ImageLength => animation.Length;
        public bool Loop = true;

        public int[] animation = new int[] { 0, 1, 2, 1};
        public int[] animIdle = new int[] { 0, 1, 2, 1 };
        public int[] animCheer = new int[] { 3, 4, 5, 4 };
        public int[] animHowl = new int[] { 6 };
        public int[] animShocked = new int[] { 7 };

        public ref float AIState => ref NPC.ai[0];
        public ref float AITimer => ref NPC.ai[1];

        public override void SetStaticDefaults() 
        {
			Main.npcFrameCount[Type] = 8;
            NPCID.Sets.DoesntDespawnToInactivityAndCountsNPCSlots[NPC.type] = true;
            ChangedUtils.HideFromBestiary(this);
        }

		public override void SetDefaults() 
        {
			NPC.width = 18;
			NPC.height = 45;
            NPC.damage = 0;
            NPC.defense = 12;
            NPC.lifeMax = 80;
            NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1;
            NPC.dontTakeDamage = true;
            AIType = NPCID.None;
			AnimationType = NPCID.None;
            SpawnModBiomes = new int[] { ModContent.GetInstance<BlackLatexSurfaceBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.GooType = GooType.Black;
        }

        public override void FindFrame(int frameHeight)
        {
            int maxFrames = ImageLength * 60;
            NPC.frameCounter += ImageSpeed;

            if (Loop)
            {
                NPC.frameCounter %= maxFrames;
            }
            else if (NPC.frameCounter >= maxFrames)
            {
                NPC.frameCounter = maxFrames - 1;
            }

            int index = (int)(NPC.frameCounter / 60d);
            NPC.frame.Y = animation[index] * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        private void SwitchAnimation(int[] newAnimation)
        {
            NPC.frameCounter = 0;
            animation = newAnimation;
        }

        public void StateIdle()
        {
            AITimer++;

            if (AITimer == 1)
            {
                SwitchAnimation(animIdle);
            }
        }

        public void StateHowl()
        {
            AITimer++;

            if (AITimer == 1)
            {
                SwitchAnimation(animHowl);
                var player = ChangedUtils.GetClosestPlayer((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
                var sound = !ChangedUtils.IsDrunk(player) ? Sounds.SoundAwoo : Sounds.SoundAroo;
                SoundEngine.PlaySound(sound, NPC.Center);
            }
        }

        private void StateCheer()
        {
            AITimer++;

            if (AITimer == 1)
            {
                SwitchAnimation(animCheer);
            }
        }

        private void DespawnCheck()
        {
            var xPos = (int)(NPC.Center.X / 16f);
            var yPos = (int)(NPC.Center.Y / 16f);
            var closestPlayer = ChangedUtils.GetClosestPlayer(xPos, yPos);
            var wolfKingPresent = NPC.AnyNPCs(ModContent.NPCType<WolfKing>()) || NPC.AnyNPCs(ModContent.NPCType<WolfKingSpawn>());

            if (closestPlayer != null && !wolfKingPresent)
            {
                var dist = Vector2.DistanceSquared(closestPlayer.Center, NPC.Center);
                var minDist = 80 * 16;
                if (dist > minDist * minDist)
                {
                    NPC.active = false;
                }
            }
        }

        private void StateShocked()
        {
            AITimer++;

            if (AITimer == 1)
            {
                SwitchAnimation(animShocked);
            }
        }

        public override void AI()
        {
            DespawnCheck();

            // His state is set by the Wolf King
            switch (AIState)
            {
                case (float)ActionState.Idle:
                    StateIdle();
                    break;
                case (float)ActionState.Cheer:
                    StateCheer();
                    break;
                case (float)ActionState.Howl:
                    StateHowl();
                    break;
                case (float)ActionState.Shocked:
                    StateShocked();
                    break;
            }
        }
    }
}

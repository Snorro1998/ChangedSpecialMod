using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.Items.Placeable.Banners;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
	public class Spike : ModNPC
	{
        private enum ActionState
        {
            Idle,
            Up,
            Down
        }

        // We are gonna do it just like GameMaker, but we have to do it all ourselves
        public double imageSpeed = 10D;
        public int imageIndex = 0;
        public int ImageLength { get { return animation.Length; } }
        public bool Loop = true;
        public double imageCounter = 0D;

        public int[] animation = new int[] { 0, 1};
        public int[] animIdle = new int[] { 0 };
        public int[] animUp = new int[] { 1 };

        public ref float AIState => ref NPC.ai[0];
        public ref float AITimer => ref NPC.ai[1];
        public ref float AITimeIdle => ref NPC.ai[2];

        public override void SetStaticDefaults() 
        {
			Main.npcFrameCount[Type] = 2;
            NPCID.Sets.ShimmerTransformToNPC[NPC.type] = ModContent.NPCType<WhiteKnight>();
            ChangedUtils.HideFromBestiary(this);
        }

		public override void SetDefaults() {
			NPC.width = 18;
			NPC.height = 76;
            NPC.damage = 0;
            NPC.defense = 12;
            NPC.lifeMax = 80;
            NPC.HitSound = SoundID.NPCHit6;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1;
            NPC.dontTakeDamage = true;
            AIType = NPCID.None;
			AnimationType = NPCID.None;
            SpawnModBiomes = new int[] { ModContent.GetInstance<BlackLatexSurfaceBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.GooType = GooType.None;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.gfxOffY = 8;
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


        // Draw in front of all other NPCs except players
        public override void DrawBehind(int index)
        {
            Main.instance.DrawCacheNPCProjectiles.Add(index);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
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

        public void StateIdle()
        {
            AITimer++;

            if (AITimer == 1)
            {
                NPC.damage = 0;
                SwitchAnimation(animIdle);
            }

            if (AITimer >= AITimeIdle)
            {
                SwitchState(ActionState.Up);
            }
        }

        public void StateUp()
        {
            AITimer++;

            if (AITimer == 1)
            {
                NPC.damage = 40;
                SwitchAnimation(animUp);
                SoundEngine.PlaySound(Sounds.SoundSpike, NPC.Center);
            }

            // For now they will be up for the same time as they are idle before they are up
            //if (AITimer >= AITimeIdle)
            if (AITimer > 30)
            {
                SwitchState(ActionState.Down);
            }
        }

        public void StateDown()
        {
            NPC.active = false;
        }

        public override void AI()
        {
            switch (AIState)
            {
                case (float)ActionState.Idle:
                    StateIdle();
                    break;
                case (float)ActionState.Up:
                    StateUp();
                    break;
                case (float)ActionState.Down:
                    StateDown();
                    break;
            }
        }
    }
}

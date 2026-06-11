using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items.Food;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
	public class WhiteLatexCubSitting : ModNPC
	{
        private enum ActionState
        {
            SitDown,
            Idle,
            StandUp
        }

        // We are gonna do it just like GameMaker, but we have to do it all ourselves
        public double imageSpeed = 10D;
        public int imageIndex = 0;

        public int[] animation = new int[] { 0, 1, 2 };

        public static readonly int[] animSitDown = new int[] { 0, 1, 2 };
        public static readonly int[] animIdle = new int[] { 2 };
        public static readonly int[] animStandUp = new int[] { 2, 1, 0 };

        public int ImageLength { get { return animation.Length; } }
        public bool Loop = false;

        public double imageCounter = 0D;

        public ref float AIState => ref NPC.ai[0];
        public ref float AITimer => ref NPC.ai[1];

        public override void SetStaticDefaults() 
		{
            Main.npcFrameCount[Type] = 4;
            NPCID.Sets.ShimmerTransformToNPC[NPC.type] = ModContent.NPCType<WhiteLatexCub>();
            ChangedUtils.HideFromBestiary(this);
        }

		public override void SetDefaults() 
        {
            NPC.width = 18;
            NPC.height = 18;
            NPC.damage = 0;
            NPC.defense = 6;
            NPC.lifeMax = 50;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1;
			AIType = 0;
			AnimationType = NPCID.None;
            SpawnModBiomes = new int[] { ModContent.GetInstance<WhiteLatexSurfaceBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.HatXOffset = -2;
            changedNPC.HatYOffset = -15;
            changedNPC.GooType = GooType.White;
            changedNPC.ElementType = ElementType.None;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.SetHalloweenHatsForBlackLatex();
            changedNPC.DoOnSpawnExtra = true;
            changedNPC.CanEvolve = true;
            changedNPC.EvolveType = ModContent.NPCType<WhiteKnight>();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Orange>(), 20));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        private void UpdateHatPosition(int frameHeight)
        {
            var changedNPC = NPC.Changed();
            var frame = NPC.frame;
            var fr = frame.Top / frameHeight;
            var hatYOffset = -15;

            if (fr == 1)
            {
                hatYOffset += 4;
            }
            else if (fr == 2 || fr == 3)
            {
                hatYOffset += 8;
            }

            changedNPC.HatYOffset = hatYOffset;
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
            UpdateHatPosition(frameHeight);
        }

        private void StateSitDown()
        {
            AITimer++;

            if (AITimer == 1)
            {
                NPC.velocity.X = 0;
                SwitchAnimation(animSitDown);
                if (NPC.HasValidTarget && ChangedUtils.IsDrunk(Main.player[NPC.target]))
                    SoundEngine.PlaySound(Sounds.SoundGroanTube1, NPC.Center);
                else
                    AudioSystem.PlayTransfurSound(NPC.Center);
            }

            if (AITimer >= 30)
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

            NPC.velocity.X = 0;
            if (!NPC.HasValidTarget)
                NPC.TargetClosest(false);
            if (!NPC.HasValidTarget || NPC.Distance(Main.player[NPC.target].Center) > 12 * 16)
            {
                SwitchState(ActionState.StandUp);
            }

            if (NPC.HasValidTarget)
            {
                var player = Main.player[NPC.target];
                if (!player.HasBuff(BuffID.Slow) && NPC.Distance(Main.player[NPC.target].Center) < 2 * 16)
                {
                    player.velocity.X /= 2;
                    player.AddBuff(BuffID.Slow, 3 * 60);
                }
            }
        }

        private void StateStandUp()
        {
            AITimer++;

            if (AITimer == 1)
            {
                SwitchAnimation(animStandUp);
                if (NPC.HasValidTarget && ChangedUtils.IsDrunk(Main.player[NPC.target]))
                    SoundEngine.PlaySound(Sounds.SoundGroanTube2, NPC.Center);
                else
                    AudioSystem.PlayTransfurSound(NPC.Center);
            }

            if (AITimer >= 30)
            {
                var changedNPC = NPC.Changed();
                var currentHat = changedNPC?.CurrentHat;
                NPC.Transform(ModContent.NPCType<WhiteLatexCub>());
                if (currentHat != null)
                {
                    changedNPC = NPC.Changed();
                    if (changedNPC != null)
                        changedNPC.SetHat(currentHat);
                }
            }
        }

        public override void AI()
        {
            switch(AIState)
            {
                case (float)ActionState.SitDown:
                    StateSitDown();
                    break;
                case (float)ActionState.StandUp:
                    StateStandUp(); 
                    break;
                default:
                    StateIdle();
                    break;
            }
            /*
            NPC.velocity.X = 0;
            if (!NPC.HasValidTarget)
                NPC.TargetClosest(false);
            if (!NPC.HasValidTarget || NPC.Distance(Main.player[NPC.target].Center) > 12 * 16)
            {
                AudioSystem.PlayTransfurSound(NPC.Center);
                NPC.Transform(ModContent.NPCType<DarkLatexCub>());
                return;
            }
            */
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

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.Changed().PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }
    }
}

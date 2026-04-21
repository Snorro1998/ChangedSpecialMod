using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
	public class WhiteLatexCub : ModNPC
	{
        public override void SetStaticDefaults() 
		{
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];
            NPCID.Sets.ShimmerTransformToNPC[NPC.type] = ModContent.NPCType<WhiteKnight>();
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Scale = 1 / NPC.scale * 1.25f,
                PortraitScale = 1 / NPC.scale * 1.25f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

		public override void SetDefaults() {
			NPC.width = 18;
			NPC.height = 18;
			NPC.damage = 20;
			NPC.defense = 6;
			NPC.lifeMax = 50;
            NPC.HitSound = SoundID.NPCHit1; //SoundID.NPCHit6;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
            NPC.aiStyle = NPCAIStyleID.Fighter;// -1;
			AIType = NPCID.GoblinScout;
			AnimationType = NPCID.Zombie;
            SpawnModBiomes = new int[] { ModContent.GetInstance<WhiteLatexSurfaceBiome>().Type };

            var ChangedGlobalNPC = NPC.Changed();
            ChangedGlobalNPC.AdjustStatScaling(NPC);
            ChangedGlobalNPC.SetNPCName(NPC);
            ChangedGlobalNPC.HatXOffset = -2;
            ChangedGlobalNPC.HatYOffset = -17;//-16
            ChangedGlobalNPC.GooType = GooType.White;
            ChangedGlobalNPC.ElementType = ElementType.None;
            ChangedGlobalNPC.DefaultOnHitPlayer = true;
            ChangedGlobalNPC.DefaultHitEffect = true;

            ChangedGlobalNPC.SetHalloweenHatsForWhiteLatex();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Remove the default portrait, otherwise you get two of them
            //bestiaryEntry.Info.RemoveAt(2);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                //  Add the new portrait with the modified rarity
                //new NPCPortraitInfoElement(3),
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.DarkLatexCub.Description")),
            });
        }


        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Orange>(), 20));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            var ChangedGlobalNPC = NPC.Changed();
            return ChangedUtils.GetSurfaceSpawnChance(spawnInfo, ChangedGlobalNPC, NPC.type);
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.Changed().OnSpawnExtra(NPC);
            base.OnSpawn(source);
        }

        private void UpdateHatPosition(int frameHeight)
        {
            var changedNPC = NPC.Changed();
            var frame = NPC.frame;
            var fr = frame.Top / frameHeight;

            if (fr == 1)
            {
                changedNPC.HatYOffset = -19;
            }
            else
            {
                changedNPC.HatYOffset = -17;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
            UpdateHatPosition(frameHeight);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.Changed().PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }
    }
}

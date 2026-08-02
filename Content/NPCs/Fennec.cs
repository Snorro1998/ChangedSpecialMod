using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items.Food;
using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
	public class Fennec : ModNPC
	{
        public override void SetStaticDefaults() 
		{
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Scale = 1 / NPC.scale * 1.25f,
                PortraitScale = 1 / NPC.scale * 1.25f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

		public override void SetDefaults() 
        {
			NPC.width = 18;
			NPC.height = 18;
			NPC.damage = 20;
			NPC.defense = 6;
			NPC.lifeMax = 50;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1;
            AIType = 0;
            AnimationType = NPCID.Zombie;
            SpawnModBiomes = new int[]
            {
                ModContent.GetInstance<WhiteLatexSurfaceDesertBiome>().Type,
                ModContent.GetInstance<BlackLatexSurfaceDesertBiome>().Type
            };

            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.HatXOffset = -2;
            changedNPC.HatYOffset = -17;
            changedNPC.GooType = GooType.None;
            changedNPC.BiomeType = Common.Systems.BiomeType.Desert;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.DoOnSpawnExtra = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.DarkLatexCub.Description")),
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            var changedNPC = NPC.Changed();
            var vanillaChance = spawnInfo.Player.ZoneDesert ? 0.3f : 0;
            var changedChance = ChangedUtils.GetSurfaceSpawnChance(spawnInfo, changedNPC, NPC.type);
            return Math.Max(vanillaChance, changedChance);

            //return ChangedUtils.GetSurfaceSpawnChance(spawnInfo, changedNPC, NPC.type);
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

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FennecPlush>(), 20));
        }

        public override void AI()
        {
            AI_Fighter.AI_003_Fighter(NPC);
        }
    }
}

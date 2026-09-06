using ChangedSpecialMod.Content.Items.Placeable.Banners;
using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using ChangedSpecialMod.Content.NPCs.AIStyles;
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
	public class Lion : ModNPC
	{
        public override void SetStaticDefaults() 
        {
			Main.npcFrameCount[Type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Scale = 1 / NPC.scale,
                PortraitScale = 1 / NPC.scale
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

		public override void SetDefaults() 
        {
			NPC.width = 18;
			NPC.height = 40;
            NPC.damage = 25;
            NPC.defense = 10;
            NPC.lifeMax = 70;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = NPCAIStyleID.Unicorn;
			AIType = NPCID.Unicorn;
            AnimationType = -1;

            Banner = Type;
            BannerItem = ModContent.ItemType<LionBanner>();

            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.HatXOffset = -4;
            changedNPC.HatYOffset = -31;
            changedNPC.GooType = GooType.None;
            changedNPC.BiomeType = Common.Systems.BiomeType.Desert;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.RemoveAllHats();
            changedNPC.DoOnSpawnExtra = true;
            changedNPC.spawnRequirement = SpawnRequirement.WhiteTail;
            changedNPC.GooColor = new Color(255, 236, 160);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.Lion.Description"))
            });
        }
        
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.CatEars, 10));
            npcLoot.Add(ItemDropRule.Common(ItemID.CatMask, 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SkinLion>(), 10));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            var changedNPC = NPC.Changed();
            if (!ChangedUtils.CommonCanSpawn(spawnInfo, changedNPC))
                return 0f;

            var vanillaChance = (spawnInfo.Player.ZoneDesert && Main.hardMode && spawnInfo.Player.townNPCs < 3) ? 0.2f : 0;
            var changedChance = ChangedUtils.GetDesertSpawnChance(spawnInfo, changedNPC);
            return Math.Max(vanillaChance, changedChance);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.Changed().PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        public override void FindFrame(int frameHeight)
        {
            Animations.AnimRunner(NPC, frameHeight);
        }

        public override bool PreAI()
        {
            AIUnicorn.Update(NPC);
            return false;
        }
    }
}

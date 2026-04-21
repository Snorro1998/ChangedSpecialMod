using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items.Placeable;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ChangedSpecialMod.Content.NPCs
{
	public class WhiteGoop : ModNPC
	{
		public override void SetStaticDefaults() 
		{
            Main.npcFrameCount[Type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Scale = 1 / NPC.scale * 1.25f,
                PortraitScale = 1 / NPC.scale * 1.25f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

		public override void SetDefaults() 
		{
            NPC.width = 18;
            NPC.height = 18;
            NPC.damage = 15;
            NPC.defense = 4;
            NPC.lifeMax = 30;
            NPC.HitSound = SoundID.NPCHit1; 
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = NPCAIStyleID.Fighter;
            AIType = NPCID.GoblinScout;
            AnimationType = NPCID.Zombie;
            SpawnModBiomes = new int[] { ModContent.GetInstance<WhiteLatexSurfaceBiome>().Type };

            Banner = Type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.WhiteGoopBanner>();

            var ChangedGlobalNPC = NPC.Changed();
            ChangedGlobalNPC.GooType = GooType.White;
            ChangedGlobalNPC.ElementType = ElementType.None;
            ChangedGlobalNPC.AdjustStatScaling(NPC);
            ChangedGlobalNPC.SetNPCName(NPC);
            ChangedGlobalNPC.DefaultOnHitPlayer = true;
            ChangedGlobalNPC.DefaultHitEffect = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.WhiteGoop.Description")),
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) 
        {
            var ChangedGlobalNPC = NPC.Changed();
            return ChangedUtils.GetSurfaceSpawnChance(spawnInfo, ChangedGlobalNPC, NPC.type);
        }
	}	
}

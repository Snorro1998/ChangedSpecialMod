using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.Items.Placeable.Banners;
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
	public class WhiteKnight : ModNPC
	{
        public override void SetStaticDefaults() 
		{
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];
            NPCID.Sets.ShimmerTransformToNPC[Type] = ModContent.NPCType<MaleDarkLatex>();
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
			NPC.height = 40;
			NPC.damage = 35;
			NPC.defense = 12;
			NPC.lifeMax = 90;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = NPCAIStyleID.Fighter;
			AIType = NPCID.GoblinScout;
			AnimationType = NPCID.Zombie;
            SpawnModBiomes = new int[] { ModContent.GetInstance<WhiteLatexSurfaceBiome>().Type };

            Banner = Type;
            BannerItem = ModContent.ItemType<WhiteLatexBanner>();

            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.SetHalloweenHatsForWhiteLatex();
            changedNPC.GooType = GooType.White;
            changedNPC.ElementType = ElementType.None;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.CanHaveBeer = true;
            changedNPC.BeerXOffset = -16;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Remove the default portrait, otherwise you get two of them
            //bestiaryEntry.Info.RemoveAt(2);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                //  Add the new portrait with the modified rarity
                //new NPCPortraitInfoElement(3),
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.WhiteKnight.Description")),
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Orange>(), 20));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            var changedNPC = NPC.Changed();
            return ChangedUtils.GetSurfaceSpawnChance(spawnInfo, changedNPC, NPC.type);
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.Changed().OnSpawnExtra(NPC);
            base.OnSpawn(source);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.Changed().PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }
    }
}

using ChangedSpecialMod.Content.Biomes;
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
	public class Lion : ModNPC
	{
        public override void SetStaticDefaults() 
        {
			Main.npcFrameCount[Type] = 4;
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
            SpawnModBiomes = new int[] { ModContent.GetInstance<CityRuinsSurfaceBiome>().Type };

            ItemID.Sets.KillsToBanner[BannerItem] = 25;

            var ChangedGlobalNPC = NPC.Changed();
            ChangedGlobalNPC.AdjustStatScaling(NPC);
            ChangedGlobalNPC.SetNPCName(NPC);

            ChangedGlobalNPC.HatXOffset = -4;
            ChangedGlobalNPC.HatYOffset = -31;
            ChangedGlobalNPC.GooType = GooType.None;
            ChangedGlobalNPC.ElementType = ElementType.None;
            ChangedGlobalNPC.DefaultOnHitPlayer = true;
            ChangedGlobalNPC.DefaultHitEffect = true;

            ChangedGlobalNPC.RemoveAllHats();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.Lion.Description"))
            });
        }
        
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.CatEars, 10));
            npcLoot.Add(ItemDropRule.Common(ItemID.CatMask, 10));
            npcLoot.Add(ItemDropRule.Common(ItemID.TigerSkin, 10));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!ChangedUtils.CanSpawnFastLatex())
            {
                return 0;
            }
            var ChangedGlobalNPC = NPC.Changed();
            return 0.3f * ChangedUtils.GetSurfaceSpawnChance(spawnInfo, ChangedGlobalNPC, NPC.type);
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

        public override void FindFrame(int frameHeight)
        {
            Animations.AnimRunner(NPC, frameHeight);
        }
    }
}

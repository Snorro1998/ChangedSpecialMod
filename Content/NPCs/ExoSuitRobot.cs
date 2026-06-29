using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items.Placeable.Banners;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace ChangedSpecialMod.Content.NPCs
{
    public class ExoSuitRobot : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
            NPCID.Sets.ShimmerTransformToNPC[NPC.type] = ModContent.NPCType<WhiteKnight>();
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
            NPC.damage = 35;//40
            NPC.defense = 12;
            NPC.lifeMax = 300;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = 60f;
            NPC.knockBackResist = 0.3f;
            NPC.npcSlots = 3;
            NPC.rarity = 2;
            NPC.aiStyle = -1;
            AIType = NPCID.None;
            AnimationType = NPCID.Zombie;
            SpawnModBiomes = new int[] { ModContent.GetInstance<CityRuinsSurfaceBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
            NPC.scale = 1;
            changedNPC.HatXOffset = 0;
            changedNPC.HatYOffset = -32;
            changedNPC.RemoveAllHats();
            changedNPC.GooType = GooType.None;
            changedNPC.ElementType = ElementType.None;
            changedNPC.DoOnSpawnExtra = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.RemoveAt(2);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new NPCPortraitInfoElement(3),
                 new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.ExoSuitRobot.Description")),
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // Only sold by the travelling merchant
            npcLoot.Add(ItemDropRule.Common(ItemID.DPSMeter, 20));
            npcLoot.Add(ItemDropRule.Common(ItemID.Stopwatch, 20));
            npcLoot.Add(ItemDropRule.Common(ItemID.LifeformAnalyzer, 20));

            npcLoot.Add(ItemDropRule.Common(ItemID.Radar, 20));
            /*
            npcLoot.Add(ItemDropRule.Common(ItemID.TallyCounter, 20));
            npcLoot.Add(ItemDropRule.Common(ItemID.MetalDetector, 20));
            npcLoot.Add(ItemDropRule.Common(ItemID.Compass, 20));
            npcLoot.Add(ItemDropRule.Common(ItemID.DepthMeter, 20));
            */
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 30; k++)
                {
                    var dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.HeatRay, hit.HitDirection, -1f, 0, default, 1f);
                    if (dustID != -1)
                    {
                        var dust = Main.dust[dustID];
                        dust.scale *= 4;
                    }
                }

                if (!Main.dedServ)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ExoSuitRobot1").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ExoSuitRobot2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ExoSuitRobot3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ExoSuitRobot4").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ExoSuitRobot5").Type, 1f);
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!ChangedUtils.CanSpawnExtraStrong())
                return 0f;

            var changedNPC = NPC.Changed();
            return 0.2f * ChangedUtils.GetSurfaceSpawnChance(spawnInfo, changedNPC, NPC.type);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var changedNPC = NPC.Changed();
            var hatXOffset = 0;
            var frame = NPC.frame;

            var fr = frame.Top / 58;
            if (fr == 0)
            {
                hatXOffset = 2;//4
            }
            // Frame 1 and 3 use offset 0
            else if (fr == 2)
            {
                hatXOffset = -4;//-4
            }

            changedNPC.HatXOffset = hatXOffset;
            NPC.Changed().PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        public override void AI()
        {
            AI_Fighter.AI_003_Fighter(NPC);
        }
    }
}

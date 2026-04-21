using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items.Placeable;
using ChangedSpecialMod.Utilities;
using System.Linq;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ChangedSpecialMod.Content.NPCs
{
	public class Sweeper : ModNPC
	{
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Bunny];
            //Main.npcCatchable[Type] = true;

            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = false;
            NPCID.Sets.TownCritter[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                //Scale = 1 / NPC.scale * 1.25f,
                //PortraitScale = 1 / NPC.scale * 1.25f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Bunny);
            //NPC.catchItem = ModContent.ItemType<WhiteLatexBlock>();
            NPC.lavaImmune = false;
            AIType = NPCID.Bunny;
            AnimationType = NPCID.Bunny;
            NPC.npcSlots = 0.2f;
            NPC.width = 8;
            NPC.height = 8;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            SpawnModBiomes = new int[] 
            {
                ModContent.GetInstance<BlackLatexSurfaceBiome>().Type,
                ModContent.GetInstance<WhiteLatexSurfaceBiome>().Type,
                ModContent.GetInstance<CityRuinsSurfaceBiome>().Type 
            };
            var ChangedGlobalNPC = NPC.Changed();
            ChangedGlobalNPC.GooType = GooType.None;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.Sweeper.Description"))
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 15; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, hit.HitDirection, -1f, 0, default, 1f);
                }

                if (!Main.dedServ)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Sweeper1").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Sweeper2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Sweeper3").Type, 1f);
                    /*
                    int randomGoreCount = Main.rand.Next(1, 4);
                    for (int i = 0; i < randomGoreCount; i++)
                    {
                        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WulfrumEnemyGore" + Main.rand.Next(1, 11).ToString()).Type, 1f);
                    }
                    */
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) 
        {

		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (ChangedUtils.InChangedBiome(spawnInfo.Player))
            {
                var nSweepers = Main.npc.Where(x => x.active && x.type == Type).Count();
                if (nSweepers < 2)
                {
                    var ChangedGlobalNPC = NPC.Changed();
                    var monsterChance = ChangedUtils.GetSurfaceSpawnChance(spawnInfo, ChangedGlobalNPC, NPC.type);
                    var critterChance = new float[]
                    {
                    SpawnCondition.OverworldDayGrassCritter.Chance,
                    SpawnCondition.OverworldDaySandCritter.Chance,
                    SpawnCondition.OverworldDaySnowCritter.Chance,
                    SpawnCondition.TownJungleCritter.Chance,
                    SpawnCondition.TownCritter.Chance
                    }.Max() * 2;
                    return new float[] { monsterChance, critterChance }.Max();
                }
            }

            return 0f;
        }
	}
}

using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Utilities;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
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
            Main.npcCatchable[Type] = true;
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Bunny];
            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = false;
            NPCID.Sets.TownCritter[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Bunny);
            NPC.lavaImmune = false;
            AIType = NPCID.Bunny;
            AnimationType = NPCID.Bunny;
            NPC.npcSlots = 0.2f;
            NPC.width = 8;
            NPC.height = 8;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.catchItem = ModContent.ItemType<SweeperItem>();
            SpawnModBiomes = new int[] 
            {
                ModContent.GetInstance<BlackLatexSurfaceBiome>().Type,
                ModContent.GetInstance<WhiteLatexSurfaceBiome>().Type,
                ModContent.GetInstance<CityRuinsSurfaceBiome>().Type 
            };
            var changedNPC = NPC.Changed();
            changedNPC.GooType = GooType.None;
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
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (ChangedUtils.InChangedBiome(spawnInfo.Player))
            {
                var nSweepers = Main.npc.Where(x => x.active && x.type == Type).Count();
                if (nSweepers < 2)
                {
                    var changedNPC = NPC.Changed();
                    var monsterChance = ChangedUtils.GetSurfaceSpawnChance(spawnInfo, changedNPC, NPC.type);
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

        public override void OnSpawn(IEntitySource source)
        {
            // Not spawned by the player
            if (!(source is EntitySource_Parent { Entity: Player }) && Main.rand.NextBool(30))
            {
                NPC.Transform(ModContent.NPCType<SweeperPuro>());
            }
        }
    }
    public class SweeperItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Frog);
            Item.makeNPC = ModContent.NPCType<Sweeper>();
            Item.value += Item.buyPrice(0, 0, 30, 0);
            Item.rare = ItemRarityID.Blue;
        }
    }
}

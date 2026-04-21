using ChangedSpecialMod.Content.Biomes;
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
    public class GermanShepherd : ModNPC
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
            NPC.damage = 30;
            NPC.defense = 12;
            NPC.lifeMax = 80;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = NPCAIStyleID.Fighter;
            AIType = NPCID.GoblinScout;
            AnimationType = NPCID.Zombie;
            SpawnModBiomes = new int[] { ModContent.GetInstance<CityRuinsSurfaceBiome>().Type };

            Banner = Type;
            BannerItem = ModContent.ItemType<GermanShepherdBanner>();
            ItemID.Sets.KillsToBanner[BannerItem] = 25;

            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);

            changedNPC.HatXOffset = 0;
            changedNPC.HatYOffset = -32;
            changedNPC.RemoveHatsFromType(HatType.Halloween);
            changedNPC.RemoveHatsFromType(HatType.XMas);

            changedNPC.GooType = GooType.None;
            changedNPC.ElementType = ElementType.None;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.GermanShepherd.Description")),
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Hotdog, 5));
            npcLoot.Add(ItemDropRule.Common(ItemID.DogWhistle, 10));
            npcLoot.Add(ItemDropRule.Common(ItemID.DogEars, 20));
            npcLoot.Add(ItemDropRule.Common(ItemID.DogTail, 20));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
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
    }
}

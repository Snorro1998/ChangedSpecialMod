using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items.Weapons;
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
	public class Bloodstripe : ModNPC
	{
        public override void SetStaticDefaults() 
        {
			Main.npcFrameCount[Type] = 4;
            NPCID.Sets.ShimmerTransformToNPC[NPC.type] = ModContent.NPCType<WhiteKnight>();
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Scale = 1 / NPC.scale * 0.6f,
                PortraitScale = 1 / NPC.scale * 0.6f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

		public override void SetDefaults() 
        {
			NPC.width = 25;
			NPC.height = 84;//56
            NPC.damage = 40;
            NPC.defense = 12;
            NPC.lifeMax = 300;//400
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = ChangedUtils.GetNPCValue(gold: 1);
            NPC.knockBackResist = 0.1f;
			NPC.aiStyle = NPCAIStyleID.Fighter;
            NPC.npcSlots = 3;
            NPC.rarity = 2;
            AIType = NPCID.GoblinScout;
			AnimationType = NPCID.Zombie;
            SpawnModBiomes = new int[] { ModContent.GetInstance<CityRuinsSurfaceBiome>().Type };
            
            Banner = Type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.BloodstripeBanner>();
            ItemID.Sets.KillsToBanner[BannerItem] = 25;

            var changedNPC = NPC.Changed();
            changedNPC.BaseScaleMultiplier = 0.7f;
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.HatYOffset = -50;
            changedNPC.GooType = GooType.None;
            changedNPC.ElementType = ElementType.None;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;

            changedNPC.ChangeHatPosition(ItemID.FlowerBoyHat, new int[] { 0, 14 });
            changedNPC.ChangeHatPosition(ItemID.BuccaneerBandana, new int[] { 6, 8 });
            changedNPC.DoOnSpawnExtra = true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TigerPaw>(), 5));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return ChangedUtils.GetWorldEvilSpawnChance(spawnInfo, NPC, ModContent.NPCType<Bloodstripe>(), true);
        }

        private void UpdateHatPosition(int frameHeight)
        {
            int frameNumber = NPC.frame.Y / frameHeight;
            var changedNPC = NPC.Changed();
            var hatYOffset = -50;

            if (frameNumber == 1 || frameNumber == 3)
            {
                hatYOffset -= 1;
            }
            else if (frameNumber == 2)
            {
                hatYOffset -= 2;
            }

            changedNPC.HatYOffset = hatYOffset;
        }

        public override void FindFrame(int frameHeight)
        {
            UpdateHatPosition(frameHeight);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var changedNPC = NPC.Changed();
            changedNPC.PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.Bloodstripe.Description")),
            });
        }
    }
}

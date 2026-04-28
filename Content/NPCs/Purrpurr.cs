using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items.Weapons;
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
    // This character was normal sized, but it was the only purple one I could find and the corruption was missing a big,
    // strong character like the Buff Tiger. Also, I just like em big
	public class Purrpurr : ModNPC
	{
        public override void SetStaticDefaults() 
        {
			Main.npcFrameCount[Type] = 4;
            NPCID.Sets.ShimmerTransformToNPC[NPC.type] = ModContent.NPCType<WhiteKnight>();
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Scale = 1 / NPC.scale * 0.9f,
                PortraitScale = 1 / NPC.scale * 0.9f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

		public override void SetDefaults() 
        {
			NPC.width = 25;
			NPC.height = 56;
            NPC.damage = 40;
            NPC.defense = 12;
            NPC.lifeMax = 300;//400
            NPC.HitSound = SoundID.NPCHit1; //SoundID.NPCHit6;
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
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.PurrpurrBanner>();
            ItemID.Sets.KillsToBanner[BannerItem] = 25;

            var changedNPC = NPC.Changed();
            changedNPC.BaseScaleMultiplier = 0.9f;
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.HatYOffset = -34;
            changedNPC.GooType = GooType.None;
            changedNPC.ElementType = ElementType.None;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.ChangeHatPosition(ItemID.FlowerBoyHat, new int[] { 0, 14 });
            changedNPC.ChangeHatPosition(ItemID.TheBrideHat, new int[] { 4, 5 });
            changedNPC.ChangeHatPosition(ItemID.JackOLanternMask, new int[] { 0, 8 });
            changedNPC.ChangeHatPosition(ItemID.GhostMask, new int[] { 2, 10 });
            changedNPC.ChangeHatPosition(ItemID.WitchHat, new int[] { 4, 6 });
            changedNPC.DoOnSpawnExtra = true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            var nChest = Main.chest.Length;

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WhiskerStaff>(), 5));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return ChangedUtils.GetWorldEvilSpawnChance(spawnInfo, NPC, ModContent.NPCType<Purrpurr>(), false);
        }

        private void UpdateHatPosition(int frameHeight)
        {
            int frameNumber = NPC.frame.Y / frameHeight;
            var changedNPC = NPC.Changed();
            var hatYOffset = -34;

            if (frameNumber == 1 || frameNumber == 3)
            {
                hatYOffset++;
            }

            changedNPC.HatYOffset = hatYOffset;
        }

        public override void FindFrame(int frameHeight)
        {
            UpdateHatPosition(frameHeight);
            base.FindFrame(frameHeight);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.Changed().PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.RemoveAt(2);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new NPCPortraitInfoElement(3),
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.Purrpurr.Description")),
            });
        }
    }
}

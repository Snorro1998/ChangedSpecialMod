using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.Items.Placeable.Banners;
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
	public class SquidDog : ModNPC
	{
        public override void SetStaticDefaults() 
		{
			Main.npcFrameCount[Type] = 4;
            NPCID.Sets.ShimmerTransformToNPC[NPC.type] = ModContent.NPCType<MaleDarkLatex>();
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Scale = 1 / NPC.scale * 1.25f,
                PortraitScale = 1 / NPC.scale * 1.25f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

		public override void SetDefaults() {
			NPC.width = 18;
			NPC.height = 45;
            NPC.damage = 40;
            NPC.defense = 12;
            NPC.lifeMax = 100;
            NPC.HitSound = SoundID.NPCHit1; //SoundID.NPCHit6;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.4f;
			NPC.aiStyle = NPCAIStyleID.Fighter;
			AIType = NPCID.GoblinScout;
			AnimationType = NPCID.Zombie;
            SpawnModBiomes = new int[] { ModContent.GetInstance<CityRuinsSurfaceBiome>().Type };

            NPC.waterMovementSpeed = 1f;

            Banner = Type;
            BannerItem = ModContent.ItemType<SquidDogBanner>();

            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.HatXOffset = -2;
            changedNPC.HatYOffset = -23;
            changedNPC.RemoveHatsFromType(HatType.Rain);
            changedNPC.RemoveHat(ItemID.TreeMask);
            changedNPC.SetHalloweenHatsForWhiteLatex();
            changedNPC.ChangeHatPosition(ItemID.Fez, new int[] { 2, 2 });
            changedNPC.ChangeHatPosition(ItemID.JackOLanternMask, new int[] { 0, 3 });

            // Even though he is white, he should appear everywhere
            changedNPC.GooType = GooType.None;
            changedNPC.ElementType = ElementType.Water;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.DoOnSpawnExtra = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Remove the default portrait, otherwise you get two of them
            //bestiaryEntry.Info.RemoveAt(2);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                //  Add the new portrait with the modified rarity
                //new NPCPortraitInfoElement(3),
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.SquidDog.Description")),
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Mollash>(), 10));
            npcLoot.Add(ItemDropRule.Common(ItemID.BlackInk, 2));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            // He isn't fast, but he also requires the White Tail or any other boss to be defeated first
            if (!ChangedUtils.CanSpawnFastLatex())
                return 0;
            var changedNPC = NPC.Changed();
            return ChangedUtils.GetSurfaceSpawnChance(spawnInfo, changedNPC, NPC.type);
        }

        private void UpdateHatPosition(int frameHeight)
        {
            /*
            standing    0 2 3 4 5 9 10 11 12 16 17 24 25
            wide legs   1 13 14 15
            leg frwd    6 7 8
            sitting     18 19 20 21 22 23
            throw 26..29
            */

            var changedNPC = NPC.Changed();
            var frame = NPC.frame;
            var fr = frame.Top / frameHeight;
            var hatYOffset = -23;

            if (fr == 1 || fr == 3)
            {
                hatYOffset = -24;
            }

            changedNPC.HatYOffset = hatYOffset;
        }

        public override void FindFrame(int frameHeight)
        {
            UpdateHatPosition(frameHeight);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.Changed().PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }
    }
}

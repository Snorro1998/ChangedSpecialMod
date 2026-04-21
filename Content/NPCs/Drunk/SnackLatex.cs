using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ChangedSpecialMod.Content.NPCs
{
	public class SnackLatex : ModNPC
	{
        public override string Texture => "ChangedSpecialMod/Content/NPCs/Drunk/SnackLatex";
        public override void SetStaticDefaults() 
		{
            Main.npcFrameCount[Type] = 4;
            //NPCID.Sets.ShimmerTransformToNPC[NPC.type] = ModContent.NPCType<WhiteKnight>();
            //ChangedUtils.HideFromBestiary(this);
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
			NPC.width = 35;
			NPC.height = 60;
            NPC.damage = 30;
            NPC.defense = 12;
            NPC.lifeMax = 200; //320
            NPC.HitSound = SoundID.NPCHit1; //SoundID.NPCHit6;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
            NPC.aiStyle = NPCAIStyleID.Fighter;// -1;
			AIType = NPCID.GoblinScout;
			AnimationType = NPCID.Zombie;
            SpawnModBiomes = new int[] { ModContent.GetInstance<ZDrunkBiome>().Type };

            var ChangedNPC = NPC.Changed();
            ChangedNPC.AdjustStatScaling(NPC);
            ChangedNPC.SetNPCName(NPC);
            ChangedNPC.HatXOffset = -4;
            ChangedNPC.HatYOffset = -36;
            ChangedNPC.GooType = GooType.Black;
            ChangedNPC.ElementType = ElementType.None;
            ChangedNPC.DefaultOnHitPlayer = true;
            ChangedNPC.DefaultHitEffect = true;

           // ChangedNPC.RemoveAllHats();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.SnackLatex.Description")),
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Burger, 5));
            npcLoot.Add(ItemDropRule.Common(ItemID.Fries, 5));
            npcLoot.Add(ItemDropRule.Common(ItemID.IceCream, 5));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!ChangedUtils.IsDrunk(spawnInfo.Player))
                return 0;
            var ChangedGlobalNPC = NPC.Changed();
            return ChangedUtils.GetSurfaceSpawnChance(spawnInfo, ChangedGlobalNPC, NPC.type);
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.Changed().OnSpawnExtra(NPC);
            base.OnSpawn(source);
        }

        private void UpdateHatPosition(int frameHeight)
        {
            var changedNPC = NPC.Changed();
            var frame = NPC.frame;
            var fr = frame.Top / frameHeight;
            var hatYOffset = -36;

            if (fr == 1 || fr == 3)
            {
                hatYOffset += 2;
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

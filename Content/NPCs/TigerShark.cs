using ChangedSpecialMod.Content.Items.Placeable.Banners;
using ChangedSpecialMod.Content.Items.Placeable.Furniture;
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
    public class TigerShark : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Scale = 1 / NPC.scale,
                PortraitScale = 1 / NPC.scale
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults() 
        {
            NPC.width = 18;
            NPC.height = 45;
            NPC.damage = 40;
            NPC.defense = 12;
            NPC.lifeMax = 100;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.4f;
            NPC.aiStyle = NPCAIStyleID.Fighter;
            AIType = NPCID.GoblinScout;
            AnimationType = NPCID.Zombie;
            NPC.waterMovementSpeed = 1f;

            Banner = Type;
            BannerItem = ModContent.ItemType<TigerSharkBanner>();

            var changedNPC = NPC.Changed();
            changedNPC.AdjustStatScaling(NPC);
            changedNPC.SetNPCName(NPC);
            changedNPC.HatXOffset = 0;
            changedNPC.HatYOffset = -32;
            changedNPC.RemoveAllHats();
            changedNPC.GooType = GooType.None;
            changedNPC.ElementType = ElementType.Water;
            changedNPC.DefaultOnHitPlayer = true;
            changedNPC.DefaultHitEffect = true;
            changedNPC.DoOnSpawnExtra = true;
            changedNPC.spawnRequirement = SpawnRequirement.WhiteTail;
            changedNPC.GooColor = new Color(150, 150, 150);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.TigerShark.Description")),
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.SharkFin, 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SharkPlush>(), 20));
            npcLoot.Add(ItemDropRule.Common(ItemID.DivingHelmet, 20));
            npcLoot.Add(ItemDropRule.Common(ItemID.SharkBait, 20));
        }

        // Maybe add a config option for the old logic?
        // ChangedUtils.GetSurfaceSpawnChance(spawnInfo, changedNPC, NPC.type);
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            var changedNPC = NPC.Changed();
            var spawnTileIsWater = spawnInfo.Water;

            if (spawnInfo.Player.ZoneBeach && spawnTileIsWater && ChangedUtils.CanSpawn(changedNPC.spawnRequirement))
                return 0.2f;

            return 0f;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.Changed().PostDrawExtra(NPC, spriteBatch, screenPos, drawColor);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }
    }
}

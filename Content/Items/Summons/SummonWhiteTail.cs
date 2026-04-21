using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Summons
{
    public class SummonWhiteTail : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)
        {
            int bossType = ModContent.NPCType<WhiteTail>();

            if (NPC.AnyNPCs(bossType))
                return true;

            ChangedUtils.AnnounceBoss = false;
            NPC.SpawnOnPlayer(player.whoAmI, bossType);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Orange>(), 1)
                .AddIngredient(ItemID.Gel, 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
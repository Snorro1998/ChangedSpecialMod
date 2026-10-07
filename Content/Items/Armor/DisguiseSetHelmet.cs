using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class DisguiseSetHelmet : ModItem
    {
        public static LocalizedText SetBonusText { get; private set; }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 3;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<DisguiseSetBreastplate>() && head.type == ModContent.ItemType<DisguiseSetHelmet>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "immune to latex beasts";
        }
    }
}

using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Seasons
{
    public class SetSeasonXmas : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.value = 100;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                Main.NewText(Language.GetTextValue("Mods.ChangedSpecialMod.EventAnnouncement.Xmas"));
            }
            SeasonSystem.SetSeason(SeasonalEvent.XMas);
            return true;
        }
    }
}

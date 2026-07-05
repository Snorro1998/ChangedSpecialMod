using ChangedSpecialMod.Common.Systems;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Debug
{
    public class DebugPrintModData : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            var exData = ModSupportSystem.externalModsData;
            if (exData == null)
            {
                Main.NewText("extdata null!");
                return true;
            }

            if (exData.Count > 0)
            {
                Main.NewText($"count {exData.Count}");

                foreach (var dat in exData)
                {
                    Main.NewText("tiles");
                    var keys = dat.tileTypes.Keys.ToList();
                    foreach (var key in keys)
                    {
                        Main.NewText($"{key}, {dat.tileTypes[key]}");
                    }
                }
            }

            return true;
        }
    }
}
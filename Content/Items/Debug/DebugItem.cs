using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Debug
{
    public class DebugItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 1;
            Item.useTime = 1;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)
        {
            var infectedSnowTileIds = new List<int>();
            foreach (var mod in ModSupportSystem.externalModsData)
            {
                infectedSnowTileIds.AddRange(mod.tilesInfectedSnow);
            }

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    var tile = Main.tile[i, j];
                    if (!tile.HasTile)
                        continue;
                    if (infectedSnowTileIds.Contains(tile.TileType))
                        tile.TileType = TileID.SnowBlock;
                }
            }

            //var bookName = BookQuestSystem.GetBookName();
            //Main.NewText(bookName);
            return true;
        }
    }
}
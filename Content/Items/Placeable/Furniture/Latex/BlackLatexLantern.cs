using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Furniture.Latex
{
    public class BlackLatexLantern : ModItem//, ILocalizedModType
    {
        //public new string LocalizationCategory => "Items.Placeables";
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.Latex.BlackLatexLantern>());
            Item.value = Item.sellPrice(copper: 30);
        }

        /*
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SmoothAbyssGravel>(6).
                AddIngredient(ItemID.Torch).
                AddTile<VoidCondenser>().
                Register();
        }
        */
    }
}

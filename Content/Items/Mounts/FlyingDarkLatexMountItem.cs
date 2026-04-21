using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChangedSpecialMod.Content.Mounts;
using ChangedSpecialMod.Assets;

namespace ChangedSpecialMod.Content.Items.Mounts
{
    public class FlyingDarkLatexMountItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing; // how the player's arm moves when using the item
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Green;
            // Bunny mount fwoomp SoundID.Item79;
            Item.UseSound = Sounds.SoundTransfur; // What sound should play when using the item
            Item.noMelee = true; // this item doesn't do any melee damage
            Item.mountType = ModContent.MountType<FlyingDarkLatexMount>();
        }
    }
}

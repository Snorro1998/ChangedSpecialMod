using ChangedSpecialMod.Content.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class Mollash : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 25;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<MollashProjectile>();
            Item.shootSpeed = 4;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.UseSound = SoundID.Item152;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }
}

using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class ToyBlock : ModItem
    {
        public override void SetDefaults()
        {
            // Common Properties
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 0, 10);
            Item.maxStack = 9999;

            // Use Properties
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.consumable = true;

            // Weapon Properties			
            Item.damage = 15;
            Item.knockBack = 1f;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;

            // Projectile Properties
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<Projectiles.ToyBlockProjectile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(50)
                .AddIngredient<Blocks>()
                .Register();
        }
    }
}

using ChangedSpecialMod.Content.Items.Placeable.Furniture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class BasketballWeapon : ModItem
    {
        public override void SetDefaults()
        {
            // Common Properties
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.maxStack = 999;

            // Use Properties
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.consumable = true;

            // Weapon Properties			
            Item.damage = 0;
            Item.knockBack = 5f;
            Item.noUseGraphic = true;
            Item.noMelee = true; // The projectile will do the damage and not the item
            Item.DamageType = DamageClass.Ranged;

            // Projectile Properties
            Item.shootSpeed = 8f;
            Item.shoot = ModContent.ProjectileType<Projectiles.BasketballProjectile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<BasketballWeapon>());
            recipe.AddIngredient(ModContent.ItemType<Basketball>());
            recipe.Register();
        }
    }
}

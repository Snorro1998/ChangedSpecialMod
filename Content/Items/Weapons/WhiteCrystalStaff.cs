using ChangedSpecialMod.Content.Items.Placeable.Crystals;
using ChangedSpecialMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class WhiteCrystalStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
            Item.damage = 21;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 3.5f;
            Item.mana = 7;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.staff[Type] = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<WhiteCrystalMagicWeaponProjectile>();
            Item.shootSpeed = 11f;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 1, 50, 0);
            Item.UseSound = SoundID.Item20;
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var nProjs = 2;
            var timeVal = MathHelper.Pi * 2 / nProjs;

            for (int i = 0; i < nProjs; i++)
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, timeVal * i * (1.0f / 0.05f));
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SilverBar, 8)
                .AddIngredient(ModContent.ItemType<CrystalWhite>(), 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.TungstenBar, 8)
                .AddIngredient(ModContent.ItemType<CrystalWhite>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}

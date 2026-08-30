using ChangedSpecialMod.Content.Items.Placeable.Crystals;
using ChangedSpecialMod.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class RedCrystalStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
            Item.damage = 25;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 3.5f;
            Item.mana = 9;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.staff[Type] = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<RedCrystalMagicWeaponProjectile>();
            Item.shootSpeed = 11f;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.UseSound = SoundID.Item43;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GoldBar, 8)
                .AddIngredient(ModContent.ItemType<CrystalRed>(), 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.PlatinumBar, 8)
                .AddIngredient(ModContent.ItemType<CrystalRed>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}

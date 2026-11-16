using ChangedSpecialMod.Content.Items.Food;
using ChangedSpecialMod.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class OrangeJuiceWeapon : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
            Item.damage = 20;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 0f;
            Item.mana = 12;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<OrangeJuiceProjectile>();
            Item.shootSpeed = 4f;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 1, 50, 0);
            Item.UseSound = SoundID.Item21;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Carton>(), 1)
                .AddIngredient(ModContent.ItemType<Orange>(), 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}

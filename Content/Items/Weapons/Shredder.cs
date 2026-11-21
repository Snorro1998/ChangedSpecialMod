using ChangedSpecialMod.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class Shredder : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.LightRed;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 9; 
            Item.useAnimation = 10;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 40;
            Item.knockBack = 3;
            Item.crit = 2;

            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<Shredder>());

            recipe.AddIngredient(ModContent.ItemType<TigerPaw>());
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.Anvils);

            recipe.Register();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BloodDebuff>(), 4 * 60);
        }
    }
}

using ChangedSpecialMod.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class MegaMauler : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Pink;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 60;
            Item.knockBack = 3;
            Item.crit = 2;

            Item.value = Item.buyPrice(0, 6, 0, 0);
            Item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<MegaMauler>());

            recipe.AddIngredient(ModContent.ItemType<Shredder>());
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddTile(TileID.Anvils);

            recipe.Register();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BloodDebuff>(), 8 * 60);
        }
    }
}

using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class WhiskerStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;//20
            Item.knockBack = 2f;
            Item.mana = 10;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Meowmere;//Sounds.SoundCat;
            Item.noMelee = true;

            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<Buffs.WhiskerStaffBuff>();
            Item.shoot = ModContent.ProjectileType<WhiskerStaffProjectile>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Low chance to play the cat sound from Changed. Might remove it, because it is loud and annoying.
            if (Main.rand.NextBool(3))
                Item.UseSound = Sounds.SoundCat;
            else
                Item.UseSound = SoundID.Meowmere;

            player.AddBuff(Item.buffType, 2);
            position = Main.MouseWorld;
            return true;
        }
    }
}

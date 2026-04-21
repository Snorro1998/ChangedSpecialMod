using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class TigerPaw : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32; // The item texture's width.
            Item.height = 32; // The item texture's height.
            Item.rare = ItemRarityID.Green;

            Item.useStyle = ItemUseStyleID.Swing; // The useStyle of the Item.
            Item.useTime = 10; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
            Item.useAnimation = 10; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
            Item.autoReuse = true; // Whether the weapon can be used more than once automatically by holding the use button.

            Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
            Item.damage = 16; // The damage your item deals.
            Item.knockBack = 3; // The force of knockback of the weapon. Maximum is 20
            Item.crit = 2; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

            Item.value = Item.buyPrice(0, 2, 0, 0);
            //Item.rare = ModContent.RarityType<ExampleModRarity>(); // Give this item our custom rarity.
            Item.UseSound = SoundID.Item1; // The sound when the weapon is being used.
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            /*
            if (Main.rand.NextBool(3))
            {
                // Emit dusts when the sword is swung
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.Sparkle>());
            }
            */
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Inflict the OnFire debuff for 1 second onto any NPC/Monster that this hits.
            // 60 frames = 1 second
            //target.AddBuff(BuffID.OnFire, 60);
        }
    }
}

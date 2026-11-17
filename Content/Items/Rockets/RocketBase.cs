using ChangedSpecialMod.Content.Projectiles.Fireworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Rockets
{
    public abstract class RocketBase : ModItem
    {
        public virtual int ShootProjectile => 1;

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(silver: 15);
            Item.maxStack = 9999;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.consumable = true;
	
            Item.damage = -1;
            Item.knockBack = 0f;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Default;

            Item.shootSpeed = 12f;
            Item.shoot = ShootProjectile;
        }
    }

    public class RocketWhiteTail : RocketBase
    {
        public override int ShootProjectile => ModContent.ProjectileType<RocketWhiteTailProjectile>();
    }

    public class RocketPrototype : RocketBase
    {
        public override int ShootProjectile => ModContent.ProjectileType<RocketPrototypeProjectile>();
    }

    public class RocketEyeOfCthulu : RocketBase
    {
        public override int ShootProjectile => ModContent.ProjectileType<RocketEyeOfCthuluProjectile>();
    }

    public class RocketEaterOfWorlds : RocketBase
    {
        public override int ShootProjectile => ModContent.ProjectileType<RocketEaterOfWorldsProjectile>();
    }

    public class RocketSkeletron : RocketBase
    {
        public override int ShootProjectile => ModContent.ProjectileType<RocketSkeletronProjectile>();
    }

    public class RocketGuide : RocketBase
    {
        public override int ShootProjectile => ModContent.ProjectileType<RocketGuideProjectile>();
    }

    public class RocketPuro : RocketBase
    {
        public override int ShootProjectile => ModContent.ProjectileType<RocketPuroProjectile>();
    }
}

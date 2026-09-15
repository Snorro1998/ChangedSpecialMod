using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles
{
    // These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
    internal class SquidDogTentacleHeadProjectile : WormHeadProjectile
    {
        public override string Texture => "ChangedSpecialMod/Content/NPCs/SquidDogTentacleHead";
        public override int BodyType => ModContent.ProjectileType<SquidDogTentacleBodyProjectile>();
        public override int TailType => ModContent.ProjectileType<SquidDogTentacleTailProjectile>();

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.damage = 30;
            Projectile.scale = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.hide = true;
        }

        public override void Init()
        {
            MinSegmentLength = 20;
            MaxSegmentLength = 20;

            CommonWormInit(this);
        }

        internal static void CommonWormInit(WormProjectile worm)
        {
            worm.MoveSpeed = 8.5f; //5.5
            worm.Acceleration = 0.08f; //0.045
            worm.ReverseAfterTime = 600;
            worm.MaxEntendDistance = 512;
        }
    }

    internal class SquidDogTentacleBodyProjectile : WormBodyProjectile
    {
        public override int BodyType => ModContent.ProjectileType<SquidDogTentacleBodyProjectile>();
        public override int TailType => ModContent.ProjectileType<SquidDogTentacleTailProjectile>();

        public override string Texture => "ChangedSpecialMod/Content/NPCs/SquidDogTentacleBody";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.damage = 25;
            Projectile.scale = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.hide = true;
        }

        public override void Init()
        {
            SquidDogTentacleHeadProjectile.CommonWormInit(this);
        }
    }

    internal class SquidDogTentacleTailProjectile : WormTailProjectile
    {
        public override string Texture => "ChangedSpecialMod/Content/NPCs/SquidDogTentacleTail";

        public override int BodyType => ModContent.ProjectileType<SquidDogTentacleBodyProjectile>();
        public override int TailType => ModContent.ProjectileType<SquidDogTentacleTailProjectile>();

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.damage = 20;
            Projectile.scale = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.hide = true;
        }

        public override void Init()
        {
            SquidDogTentacleHeadProjectile.CommonWormInit(this);
        }
    }
}
namespace ChangedSpecialMod.Content.Projectiles.SharkBossFight
{
	public class ArapaimaProjectile : BaseFishProjectile
	{
        public override int ProjectileWidth => 48;
        public override int ProjectileHeight => 16;
        public override int ProjectileFrames => 1;
        public override string ProjectileTexture => "ChangedSpecialMod/Content/Projectiles/SharkBossFight/ArapaimaProjectile";
        public override bool SpinningRotation => false;
    }
}
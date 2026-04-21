namespace ChangedSpecialMod.Content.Projectiles.SharkBossFight
{
	public class JellyfishProjectile : BaseFishProjectile
    {
        public override int ProjectileWidth => 20;
        public override int ProjectileHeight => 20;
        public override int ProjectileFrames => 4;
        public override string ProjectileTexture => "ChangedSpecialMod/Content/Projectiles/SharkBossFight/JellyfishProjectile";
        public override bool SpinningRotation => true;
    }
}

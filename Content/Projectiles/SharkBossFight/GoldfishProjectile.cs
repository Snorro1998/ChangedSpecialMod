namespace ChangedSpecialMod.Content.Projectiles.SharkBossFight
{
    public class GoldfishProjectile : BaseFishProjectile
    {
        public override int ProjectileWidth => 18;
        public override int ProjectileHeight => 18;
        public override int ProjectileFrames => 2;
        public override string ProjectileTexture => "ChangedSpecialMod/Content/Projectiles/SharkBossFight/GoldfishProjectile";
        public override bool SpinningRotation => false;
    }
}
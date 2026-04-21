namespace ChangedSpecialMod.Content.Projectiles.SharkBossFight
{
    public class SharkProjectile : BaseFishProjectile
    {
        public override int ProjectileWidth => 76;
        public override int ProjectileHeight => 22;
        public override int ProjectileFrames => 4;
        public override string ProjectileTexture => "ChangedSpecialMod/Content/Projectiles/SharkBossFight/SharkProjectile";
        public override bool SpinningRotation => false;
    }
}
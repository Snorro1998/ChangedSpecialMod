namespace ChangedSpecialMod.Content.Projectiles.SharkBossFight
{
    public class OrcaProjectile : BaseFishProjectile
    {
        public override int ProjectileWidth => 112;
        public override int ProjectileHeight => 36;
        public override int ProjectileFrames => 16;
        public override string ProjectileTexture => "ChangedSpecialMod/Content/Projectiles/SharkBossFight/OrcaProjectile";
        public override bool SpinningRotation => false;
    }
}
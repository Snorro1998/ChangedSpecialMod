using Terraria.ID;

namespace ChangedSpecialMod.Content.Walls.Latex
{
    public class BlackLatexSandstoneWall : BaseBlackLatexWall
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            VanillaFallbackOnModDeletion = WallID.Sandstone;
        }
    }
}

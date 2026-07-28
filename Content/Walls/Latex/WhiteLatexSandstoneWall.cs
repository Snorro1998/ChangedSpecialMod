using Terraria.ID;

namespace ChangedSpecialMod.Content.Walls.Latex
{
    public class WhiteLatexSandstoneWall : BaseWhiteLatexWall
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            VanillaFallbackOnModDeletion = WallID.Sandstone;
        }
    }
}

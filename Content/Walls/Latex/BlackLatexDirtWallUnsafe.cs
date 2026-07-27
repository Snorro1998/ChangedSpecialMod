using Terraria.ID;

namespace ChangedSpecialMod.Content.Walls.Latex
{
    public class BlackLatexDirtWallUnsafe : BaseBlackLatexWall
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            VanillaFallbackOnModDeletion = WallID.DirtUnsafe;
        }
    }
}

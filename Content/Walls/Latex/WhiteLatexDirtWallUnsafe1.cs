using Terraria.ID;

namespace ChangedSpecialMod.Content.Walls.Latex
{
    public class WhiteLatexDirtWallUnsafe1 : BaseWhiteLatexWall
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            VanillaFallbackOnModDeletion = WallID.DirtUnsafe1;
        }
    }
}

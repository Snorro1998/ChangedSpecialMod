using Terraria.ID;

namespace ChangedSpecialMod.Content.Walls.Latex
{
    public class WhiteLatexStoneWall : BaseWhiteLatexWall
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            VanillaFallbackOnModDeletion = WallID.Stone;
        }
    }
}

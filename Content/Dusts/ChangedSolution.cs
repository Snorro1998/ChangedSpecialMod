using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Dusts
{
	public class ChangedSolution : ModDust
	{
		public override void SetStaticDefaults() {
			UpdateType = DustID.PureSpray;
		}
	}
}
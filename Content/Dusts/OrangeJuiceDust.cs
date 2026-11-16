using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Dusts
{
	public class OrangeJuiceDust : ModDust
	{
		public override void SetStaticDefaults() 
		{
			UpdateType = DustID.Water;
		}
	}
}
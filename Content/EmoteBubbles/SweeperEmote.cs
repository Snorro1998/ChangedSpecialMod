using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.EmoteBubbles
{
	public class SweeperEmote : ModEmoteBubble
	{
		public override void SetStaticDefaults() 
		{
			AddToCategory(EmoteID.Category.CrittersAndMonsters);
		}
	}
}

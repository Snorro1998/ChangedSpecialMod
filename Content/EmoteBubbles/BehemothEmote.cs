using ChangedSpecialMod.Common.Systems;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.EmoteBubbles
{
	public class WolfKingEmote : ModEmoteBubble
	{
		public override void SetStaticDefaults() 
		{
			AddToCategory(EmoteID.Category.Dangers);
		}

        public override bool IsUnlocked()
        {
            return DownedBossSystem.DownedWolfKing;
        }
    }
}

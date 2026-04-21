using ChangedSpecialMod.Utilities;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.EmoteBubbles
{
	public class BloodStripeEmote : ModEmoteBubble
	{
		public override void SetStaticDefaults() 
		{
			AddToCategory(EmoteID.Category.CrittersAndMonsters);
		}

        public override bool IsUnlocked()
        {
            return ChangedUtils.CanSpawnStrongLatex();
        }
    }
}

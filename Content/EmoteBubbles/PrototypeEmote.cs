using ChangedSpecialMod.Content.NPCs;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.EmoteBubbles
{
	public class PrototypeEmote : ModEmoteBubble
	{
		public override void SetStaticDefaults() 
		{
			AddToCategory(EmoteID.Category.Town);
		}

        public override bool IsUnlocked()
        {
            return NPC.AnyNPCs(ModContent.NPCType<Prototype>());
        }
    }
}

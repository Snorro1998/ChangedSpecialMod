
using Microsoft.Xna.Framework.Graphics;

using Terraria.UI;

namespace ChangedSpecialMod.UI
{
	// This class represents the UIState for our ExamplePerson Awesomeify chat function. It is similar to the Goblin Tinkerer's Reforge function, except it only gives Awesome and ReallyAwesome prefixes. 
	internal class PuroUI : UIState
	{
		

		// OnDeactivate is called when the UserInterface switches to a different state. In this mod, we switch between no state (null) and this state (ExamplePersonUI).
		// Using OnDeactivate is useful for clearing out Item slots and returning them to the player, as we do here.
		

		// Update is called on a UIState while it is the active state of the UserInterface.
		// We use Update to handle automatically closing our UI when the player is no longer talking to our Example Person NPC.
		

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);

			// This will hide the crafting menu similar to the reforge menu. For best results this UI is placed before "Vanilla: Inventory" to prevent 1 frame of the craft menu showing.
			

			// Here we have a lot of code. This code is mainly adapted from the vanilla code for the reforge option.
			// This code draws "Place an item here" when no item is in the slot and draws the reforge cost and a reforge button when an item is in the slot.
			// This code could possibly be better as different UIElements that are added and removed, but that's not the main point of this example.
			// If you are making a UI, add UIElements in OnInitialize that act on your ItemSlot or other inputs rather than the non-UIElement approach you see below.

		}
	}
}

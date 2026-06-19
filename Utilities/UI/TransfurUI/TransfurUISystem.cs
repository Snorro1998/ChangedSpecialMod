using ChangedSpecialMod.Common.Systems;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;


namespace ChangedSpecialMod.Utilities.UI.TransfurUI
{
    [Autoload(Side = ModSide.Client)]
    public class TransfurUISystem : ModSystem
    {
        private UserInterface transfurUserInterface;
        internal TransfurUI transfurUI;

        public void ShowMyUI(EvolutionLines evolutionLine)
        {
            transfurUI.SetVisibleCategories(evolutionLine);
            transfurUserInterface?.SetState(transfurUI);
        }

        public void HideMyUI()
        {
            transfurUserInterface?.SetState(null);
        }

        public void ToggleUI(EvolutionLines evolutionLine)
        {
            var state = transfurUserInterface?.CurrentState;
            if (state != null)
                HideMyUI();
            else
                ShowMyUI(evolutionLine);
        }

        public override void PostSetupContent()
        {
            transfurUserInterface = new UserInterface();
            transfurUI = new TransfurUI();
            transfurUI.Activate();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (transfurUserInterface?.CurrentState != null)
            {
                transfurUserInterface?.Update(gameTime);
            }
        }

        // Adding a custom layer to the vanilla layer list that will call .Draw on your interface if it has a state
        // Setting the InterfaceScaleType to UI for appropriate UI scaling
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "TransfurUI",
                    delegate {
                        if (transfurUserInterface?.CurrentState != null)
                        {
                            transfurUserInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void PreSaveAndQuit()
        {
            HideMyUI();
        }
    }
}

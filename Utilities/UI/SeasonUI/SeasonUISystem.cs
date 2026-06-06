using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;


namespace ChangedSpecialMod.Utilities.UI.SeasonUI
{
    [Autoload(Side = ModSide.Client)]
    public class SeasonUISystem : ModSystem
    {
        private UserInterface userInterface;
        internal SeasonUI uiPanel;

        public void ShowMyUI(int categories)
        {
            uiPanel.SetVisibleCategories(categories);
            userInterface?.SetState(uiPanel);
        }

        public void HideMyUI()
        {
            userInterface?.SetState(null);
        }

        public void ToggleUI(int categories)
        {
            var state = userInterface?.CurrentState;
            if (state != null)
            {
                HideMyUI();
            }
            else
            {
                ShowMyUI(categories);
            }
        }

        public override void PostSetupContent()
        {
            userInterface = new UserInterface();
            uiPanel = new SeasonUI();
            uiPanel.Activate();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (userInterface?.CurrentState != null)
            {
                userInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "SeasonUI",
                    delegate {
                        if (userInterface?.CurrentState != null)
                        {
                            userInterface.Draw(Main.spriteBatch, new GameTime());
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

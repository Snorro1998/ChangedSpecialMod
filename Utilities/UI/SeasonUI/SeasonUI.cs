using ChangedSpecialMod.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace ChangedSpecialMod.Utilities.UI.SeasonUI
{
    internal class SeasonUI : UIState
    {
        private static string TexturePath(string name) => $"ChangedSpecialMod/Content/Items/Seasons/{name}";
        private static string NamePath(string name) => Language.GetTextValue($"Mods.ChangedSpecialMod.Seasons.{name}");

        public DraggableUIPanel MainPanel;

        private List<List<UIHoverImageButton>> allCategories = new List<List<UIHoverImageButton>>();
        private List<List<UIHoverImageButton>> visibleCategories = new List<List<UIHoverImageButton>>();
        private List<UIHoverImageButton> buttonsSeasons = new List<UIHoverImageButton>();

        public override void OnInitialize()
        {
            var screenWidth = Main.screenWidth;
            var panelWidth = 300;

            var halfScreenWidth = screenWidth / 2;
            var halfPanelWidth = panelWidth / 2;

            MainPanel = new DraggableUIPanel();
            MainPanel.SetPadding(0);
            SetRectangle(MainPanel, left: halfScreenWidth - halfPanelWidth, top: 100f, width: panelWidth, height: 200f);
            MainPanel.BackgroundColor = new Color(63, 82, 151) * 0.7f;//new Color(73, 94, 171); 

            // Black
            buttonsSeasons.Add(AddButton(0, 0, SeasonalEvent.None, TexturePath("SetSeasonNone"), NamePath("None")));
            buttonsSeasons.Add(AddButton(1, 0, SeasonalEvent.Valentine, TexturePath("SetSeasonValentine"), NamePath("Valentine")));
            //buttonsSeasons.Add(AddButton(2, 0, SeasonalEvent.Fiesta, TexturePath("SetSeasonFiesta"), NamePath("Fiesta")));
            buttonsSeasons.Add(AddButton(2, 0, SeasonalEvent.Oktoberfest, TexturePath("SetSeasonOktoberfest"), NamePath("Oktoberfest")));
            buttonsSeasons.Add(AddButton(3, 0, SeasonalEvent.Halloween, TexturePath("SetSeasonHalloween"), NamePath("Halloween")));
            buttonsSeasons.Add(AddButton(4, 0, SeasonalEvent.XMas, TexturePath("SetSeasonXmas"), NamePath("Xmas")));


            var catSpirit = new List<UIHoverImageButton>();
            /*
            // Spirit
            if (ModSupportSystem.modSpirit != null)
            {
                catSpirit.Add(AddButtonSpirit(0, 0, 4, TexturePath("SetSeasonXmas"), NamePath("WingedDarkLatex")));
                catSpirit.Add(AddButtonSpirit2(1, 0, 4, TexturePath("SetSeasonXmas"), NamePath("WingedDarkLatex")));
            }
            */

            allCategories.Add(buttonsSeasons);
            allCategories.Add(catSpirit);

            Append(MainPanel);
        }

        private UIHoverImageButton AddButton(int buttonXIndex, int buttonYIndex, SeasonalEvent season, string iconPath, string description)
        {
            Asset<Texture2D> buttonTexture = ModContent.Request<Texture2D>(iconPath);
            UIHoverImageButton button = new UIHoverImageButton(buttonTexture, description);
            button.OnLeftClick += (evt, element) =>
            {
                ModContent.GetInstance<SeasonUISystem>().HideMyUI();
                SeasonSystem.SetSeason(season, true);
            };

            MainPanel.Append(button);
            SetButtonPosition(button, buttonXIndex, buttonYIndex);

            return button;
        }
        /*
        private UIHoverImageButton AddButtonSpirit(int buttonXIndex, int buttonYIndex, int transformationIndex, string iconPath, string description)
        {
            ModContent.RequestIfExists<Texture2D>("SpiritMod/Biomes/Events/MysticMoonBiome_Icon", out Asset<Texture2D> buttonTexture);
            UIHoverImageButton button = new UIHoverImageButton(buttonTexture, description);
            button.OnLeftClick += (evt, element) =>
            {
                ModContent.GetInstance<SeasonUISystem>().HideMyUI();
                ModSupportSystem.SpiritActivateMysticMoon();
            };

            MainPanel.Append(button);
            SetButtonPosition(button, buttonXIndex, buttonYIndex);

            return button;
        }

        private UIHoverImageButton AddButtonSpirit2(int buttonXIndex, int buttonYIndex, int transformationIndex, string iconPath, string description)
        {
            ModContent.RequestIfExists<Texture2D>("SpiritMod/Biomes/Events/JellyDelugeBiome_Icon", out Asset<Texture2D> buttonTexture);
            UIHoverImageButton button = new UIHoverImageButton(buttonTexture, description);
            button.OnLeftClick += (evt, element) =>
            {
                ModContent.GetInstance<SeasonUISystem>().HideMyUI();
                ModSupportSystem.SpiritActivateJellyMoon();
            };

            MainPanel.Append(button);
            SetButtonPosition(button, buttonXIndex, buttonYIndex);

            return button;
        }
        */

        public void SetVisibleCategories(int categories)
        {
            visibleCategories = allCategories;
            //visibleCategories = new List<List<UIHoverImageButton>>();
            //visibleCategories.Add(buttonsSeasons);
            MainPanel.Height.Set(visibleCategories.Count * 70, 0f);

            // This is such a stupid fix, we just move the buttons far outside the screen
            for (int i = 0; i < allCategories.Count; i++)
            {
                var category = allCategories[i];

                for (int j = 0; j < category.Count; j++)
                {
                    var button = category[j];
                    SetButtonPosition(button, j, 100);
                }
            }

            // And here we move the visible buttons back
            for (int i = 0; i < visibleCategories.Count; i++)
            {
                var category = visibleCategories[i];

                for (int j = 0; j < category.Count; j++)
                {
                    var button = category[j];
                    SetButtonPosition(button, j, i);
                }
            }
        }

        private void SetButtonPosition(UIHoverImageButton button, int xIndex, int yIndex)
        {
            var btnWith = 36;
            var btnHeight = 36;
            var spacing = 8;
            var padding = 10;

            SetRectangle(button, left: padding + xIndex * (btnWith + spacing), top: padding + yIndex * (btnHeight + spacing), width: btnWith, height: btnHeight);
        }

        private void SetRectangle(UIElement uiElement, float left, float top, float width, float height)
        {
            uiElement.Left.Set(left, 0f);
            uiElement.Top.Set(top, 0f);
            uiElement.Width.Set(width, 0f);
            uiElement.Height.Set(height, 0f);
        }
    }
}
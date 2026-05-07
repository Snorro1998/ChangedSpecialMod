using ChangedSpecialMod.Content.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace ChangedSpecialMod.Utilities.UI.TransfurUI
{
    internal class TransfurUI : UIState
    {
        private static string TexturePath(string name) => $"ChangedSpecialMod/Assets/Textures/TransfurIcons/{name}";
        private static string NamePath(string name) => Language.GetTextValue($"Mods.ChangedSpecialMod.NPCs.{name}.DisplayName");

        public DraggableUIPanel MainPanel;

        private List<List<UIHoverImageButton>> allCategories = new List<List<UIHoverImageButton>>();
        private List<List<UIHoverImageButton>> visibleCategories = new List<List<UIHoverImageButton>>();
        private List<UIHoverImageButton> buttonsBlackLatex = new List<UIHoverImageButton>();
        private List<UIHoverImageButton> buttonsWhiteLatex = new List<UIHoverImageButton>();
        private List<UIHoverImageButton> buttonsSquidDog = new List<UIHoverImageButton>();

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
            buttonsBlackLatex.Add(AddButton(0, 0, GooType.Black, 0, TexturePath("BlackGoop"), NamePath("BlackGoop")));
            buttonsBlackLatex.Add(AddButton(1, 0, GooType.Black, 1, TexturePath("DarkLatexCub"), NamePath("DarkLatexCub")));
            buttonsBlackLatex.Add(AddButton(2, 0, GooType.Black, 2, TexturePath("MaleDarkLatex"), NamePath("MaleDarkLatex")));
            buttonsBlackLatex.Add(AddButton(3, 0, GooType.Black, 3, TexturePath("WingedDarkLatex"), NamePath("WingedDarkLatex")));
            buttonsBlackLatex.Add(AddButton(4, 0, GooType.Black, 4, TexturePath("Wendigo"), NamePath("Wendigo")));

            // White
            buttonsWhiteLatex.Add(AddButton(0, 1, GooType.White, 0, TexturePath("WhiteGoop"), NamePath("WhiteGoop")));
            buttonsWhiteLatex.Add(AddButton(1, 1, GooType.White, 1, TexturePath("WhiteLatexCub"), NamePath("WhiteLatexCub")));
            buttonsWhiteLatex.Add(AddButton(2, 1, GooType.White, 2, TexturePath("WhiteKnight"), NamePath("WhiteKnight")));
            buttonsWhiteLatex.Add(AddButton(3, 1, GooType.White, 3, TexturePath("WhiteLatexTaur"), NamePath("WhiteLatexTaur")));

            // Squid Dog
            buttonsSquidDog.Add(AddButton(0, 2, GooType.None, 0, TexturePath("SquidDog"), NamePath("SquidDog")));

            allCategories.Add(buttonsBlackLatex);
            allCategories.Add(buttonsWhiteLatex);
            allCategories.Add(buttonsSquidDog);

            Append(MainPanel);
        }

        private UIHoverImageButton AddButton(int buttonXIndex, int buttonYIndex, GooType gooType, int transformationIndex, string iconPath, string description)
        {
            Asset<Texture2D> buttonTexture = ModContent.Request<Texture2D>(iconPath);
            UIHoverImageButton button = new UIHoverImageButton(buttonTexture, description);
            //SetRectangle(button, left: padding + buttonXIndex * (btnWith + spacing), top: padding + buttonYIndex * (btnHeight + spacing), width: btnWith, height: btnHeight);
            button.OnLeftClick += (evt, element) =>
            {
                ModContent.GetInstance<TransfurUISystem>().HideMyUI();
                TransformPlayer(gooType, transformationIndex);
            };

            MainPanel.Append(button);
            SetButtonPosition(button, buttonXIndex, buttonYIndex);

            return button;
        }

        public void SetVisibleCategories(int categories)
        {
            visibleCategories = new List<List<UIHoverImageButton>>();

            if (categories == 1)
            {
                visibleCategories.Add(buttonsBlackLatex);
            }
            else if (categories == 2)
            {
                visibleCategories.Add(buttonsWhiteLatex);
            }
            else if (categories == 3)
            {
                visibleCategories.Add(buttonsSquidDog);
            }
            else
            {
                visibleCategories.Add(buttonsBlackLatex);
                visibleCategories.Add(buttonsWhiteLatex);
                visibleCategories.Add(buttonsSquidDog);
            }

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
            var btnWith = 48;
            var btnHeight = 48;
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

        private void TransformPlayer(GooType gooType, int transformationIndex)
        {
            var player = Main.LocalPlayer;
            if (player == null)
                return;
            var changedPlayer = player.ChangedPlayer();
            if (changedPlayer == null)
                return;
            changedPlayer.SetTransfurFromNumber(gooType, transformationIndex);
        }
    }
}
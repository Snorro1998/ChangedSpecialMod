using ChangedSpecialMod.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
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
        public UIPanel InfoPanel;

        private Dictionary<EvolutionLines, List<UIHoverImageButton>> allCategories = new Dictionary<EvolutionLines, List<UIHoverImageButton>>();
        private Dictionary<EvolutionLines, List<UIHoverImageButton>> visibleCategories = new Dictionary<EvolutionLines, List<UIHoverImageButton>>();

        private UIText descriptionText;

        public override void OnInitialize()
        {
            var screenWidth = Main.screenWidth;
            var panelWidth = 500;

            var halfScreenWidth = screenWidth / 2;
            var halfPanelWidth = panelWidth / 2;

            MainPanel = new DraggableUIPanel();
            MainPanel.SetPadding(0);
            SetRectangle(MainPanel, left: halfScreenWidth - halfPanelWidth, top: 100f, width: panelWidth, height: 200f);
            MainPanel.BackgroundColor = new Color(63, 82, 151) * 0.7f;//new Color(73, 94, 171); 

            allCategories = new Dictionary<EvolutionLines, List<UIHoverImageButton>>();
            TransfurSystem.InitTransfurTypes();
            var keys = TransfurSystem.EvolutionsLines.Keys;

            int y = 0;
            foreach (var key in keys)
            {
                var evoline = TransfurSystem.EvolutionsLines[key];
                List<UIHoverImageButton> btns = new List<UIHoverImageButton>();
                int x = 0;
                foreach (var transfur in evoline)
                {
                    btns.Add(AddButton(x, y, transfur.npcType, TexturePath(transfur.npcName), NamePath(transfur.npcName)));
                }
                allCategories.Add(key, btns);
                y++;
            }

            InfoPanel = new UIPanel();
            SetRectangle(InfoPanel, left: 300, top: 0, width: 200, height: 200f);

            // Info part
            descriptionText = new UIText("");
            descriptionText.Left.Set(0, 0);
            descriptionText.Top.Set(0, 0);
            InfoPanel.Append(descriptionText);
            MainPanel.Append(InfoPanel);

            Append(MainPanel);
        }

        private UIHoverImageButton AddButton(int buttonXIndex, int buttonYIndex, int npcType, string iconPath, string description)
        {
            Asset<Texture2D> buttonTexture = ModContent.Request<Texture2D>(iconPath);
            UIHoverImageButton button = new UIHoverImageButton(buttonTexture, description);

            button.OnLeftClick += (evt, element) =>
            {
                ModContent.GetInstance<TransfurUISystem>().HideMyUI();
                OnButtonClicked(npcType);//TransformPlayer(npcType);
            };

            button.OnMouseOver += (evt, elemt) =>
            {
                descriptionText.SetText(TransfurSystem.GetDescription(npcType));
            };

            MainPanel.Append(button);
            SetButtonPosition(button, buttonXIndex, buttonYIndex);

            return button;
        }

        public void OnButtonClicked(int npcType)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                TransfurSystem.SetTransfurFromNPCType(Main.myPlayer, npcType);
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ModContent.GetInstance<ChangedSpecialMod>().GetPacket();
                packet.Write((byte)ChangedSpecialMod.MessageType.TransfurPlayer);
                packet.Write(npcType);
                packet.Send();
            }
        }

        public void SetVisibleCategories(EvolutionLines evolutionLine)
        {
            visibleCategories = new Dictionary<EvolutionLines, List<UIHoverImageButton>>();
            descriptionText.SetText("");

            if (evolutionLine == EvolutionLines.None)
                visibleCategories = allCategories;
            else
            {
                var keys = allCategories.Keys.ToList();
                foreach ( var key in keys)
                {
                    if (evolutionLine == key)
                        visibleCategories.Add(key, allCategories[key]);
                }
            }

            InfoPanel.Height.Set((visibleCategories.Count + 1) * 60, 0f);
            MainPanel.Height.Set((visibleCategories.Count + 1) * 60, 0f);

            // This is such a stupid fix, we just move the buttons far outside the screen
            for (int i = 0; i < allCategories.Count; i++)
            {
                var key = allCategories.Keys.ToList()[i];
                var category = allCategories[key];

                for (int j = 0; j < category.Count; j++)
                {
                    var button = category[j];
                    SetButtonPosition(button, j, 100);
                }
            }

            // And here we move the visible buttons back
            for (int i = 0; i < visibleCategories.Count; i++)
            {
                var key = visibleCategories.Keys.ToList()[i];
                var category = visibleCategories[key];

                for (int j = 0; j < category.Count; j++)
                {
                    var button = category[j];
                    SetButtonPosition(button, j, i);
                }
            }

            /*
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
                visibleCategories.Add(buttonsWorldEvil);
            }


            */
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

        private void TransformPlayer(int npcType)
        {
            var player = Main.LocalPlayer;
            if (player == null)
                return;
            TransfurSystem.SetTransfurFromNPCType(player.whoAmI, npcType);
        }
    }
}
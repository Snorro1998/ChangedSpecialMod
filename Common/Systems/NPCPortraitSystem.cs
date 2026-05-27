using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Content.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace ChangedSpecialMod.Common.Systems
{
    public class NPCPortraitSystem : ModSystem
    {
        private static Asset<Texture2D> PortraitBackground;

        private static string EventName = null;
        private static string Emotion = "Talk";
        private static List<int> npcsWithPortraits;

        static NPCPortraitSystem()
        {
            PortraitBackground = ModContent.Request<Texture2D>("Terraria/Images/TownNPCs/Portraits/Portrait_Window");
        }

        public override void Load()
        {
            On_Main.GUIChatDrawInner += (orig, self) =>
            {
                orig(self);

                var player = Main.LocalPlayer;
                if (player.talkNPC < 0 && player.sign == -1)
                    return;

                DrawNPCPortrait();
            };
        }

        public override void PostSetupContent()
        {
            npcsWithPortraits = new List<int>();
            npcsWithPortraits.Add(ModContent.NPCType<Puro>());
            npcsWithPortraits.Add(ModContent.NPCType<Prototype>());
            npcsWithPortraits.Add(ModContent.NPCType<Scientist>());
            DummyLoad();
        }

        // This is a really stupid solution. Load all textures and don't do anything with them.
        // This fixes the problem that images will flash when drawn for the first time
        private void DummyLoad()
        {
            var npcNames = new List<string>()
            {
                "Puro",
                "Prototype",
                "Scientist"
            };

            var emotions = new List<string>()
            {
                "Angry",
                "Derp",
                "Embarrassed",
                "Evil",
                "Happy",
                "Love",
                "Naughty",
                "Neutral",
                "Question",
                "Shocked",
                "Talk",
                "Tired"
            };

            var events = new List<string>()
            {
                "Normal",
                "Bloodmoon",
                "Party"
            };

            foreach (var npcName in npcNames)
            {
                var basePath = $"ChangedSpecialMod/Content/NPCs/{npcName}";
                foreach(var eventName in events)
                {
                    var path2 = basePath + "/" + eventName;
                    foreach (var emotionName in emotions)
                    {
                        var path3 = path2 + "/" + emotionName;
                        var portrait = ModContent.Request<Texture2D>(path3).Value;
                    }
                }
            }
        }

        public static void SetEmotionAndEvent(string eventName, string emotion)
        {
            EventName = eventName;
            Emotion = emotion;
        }

        private static void DrawNPCPortrait()
        {
            // Don't draw the portrait if boudler backport mod is active
            if (ModSupportSystem.modBoulderBackport != null || !ChangedSpecialModClientConfig.Instance.DialoguePortraits)
                return;

            string talkNPCName = null;
            var talkNPC = Main.npc[Main.LocalPlayer.talkNPC];

            if (!npcsWithPortraits.Contains(talkNPC.type))
                return;

            talkNPCName = talkNPC.ModNPC.GetType().Name;

            var basePath = $"ChangedSpecialMod/Content/NPCs/{talkNPCName}";
            if (EventName != null)
                basePath += $"/{EventName}";
            else
                basePath += "/Normal";

            var portrait = ModContent.Request<Texture2D>($"{basePath}/{Emotion}").Value;

            var npcChatTopLeft = new Vector2(Main.screenWidth / 2 - 250, 100);
            var drawPos = npcChatTopLeft + new Vector2(-62, 62);

            var chatBack = new Color(200, 200, 200, 200);
            var spriteBatch = Main.spriteBatch;
            var bg = PortraitBackground.Value;

            spriteBatch.Draw(bg, drawPos, bg.Frame(), chatBack,
                0f, bg.Size() / 2, Vector2.One, SpriteEffects.None, 0f);
            spriteBatch.Draw(portrait, drawPos, portrait.Frame(), Color.White,
                0f, portrait.Size() / 2, Vector2.One, SpriteEffects.None, 0f);

            var textFont = FontAssets.ItemStack.Value;
            var givenName = talkNPC.GivenName;
            var textOrigin = textFont.MeasureString(givenName) / 2f;
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, textFont, givenName,
                drawPos + new Vector2(0f, 57), Color.White, 0f, textOrigin, Vector2.One);
        }
    }
}

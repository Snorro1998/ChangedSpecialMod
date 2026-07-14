using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Debug
{
    public class DebugSpawnAllNPCs : ModItem
    {
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return true;

            /*
            if (ModSupportSystem.modMrPlagueRaces != null)
            {
                //var cont = ModSupportSystem.modMrPlagueRaces.GetContent<ModSystem>().ToList();
                ModSystem system = ModSupportSystem.modMrPlagueRaces.GetContent<ModSystem>().FirstOrDefault(s => s.GetType().Name == "RaceChangeUISystem");

                if (system != null)
                {
                    MethodInfo showMethod = system.GetType().GetMethod(
                        "ShowMyUI",
                        BindingFlags.Instance | BindingFlags.Public);

                    showMethod?.Invoke(system, null);
                }

                return true;
            }
            */

            //var changedMod = ChangedSpecialMod.Instance;
            //changedMod.Call("spawnall");
            ChangedUtils.SpawnAllNPCs(player);

            /*
            var entitySource = Item.GetSource_FromThis();

            var changedMod = ChangedSpecialMod.Instance;
            changedMod.Call("orang");

            return true;

            var npcs = new List<int>
            {
                // Sweeper
                ModContent.NPCType<Sweeper>(),
                ModContent.NPCType<SweeperPuro>(),

                // Black
                ModContent.NPCType<BlackGoop>(),
                ModContent.NPCType<DarkLatexCub>(),
                //ModContent.NPCType<Cheerleader>(),
                ModContent.NPCType<FemaleDarkLatex>(),
                ModContent.NPCType<MaleDarkLatex>(),
                ModContent.NPCType<FlyingDarkLatex>(),
                ModContent.NPCType<WingedDarkLatex>(),

                // Crystal wolves
                ModContent.NPCType<CrystalWolfBlue>(),
                ModContent.NPCType<CrystalWolfGreen>(),
                //ModContent.NPCType<CrystalWolfOrange>(),
                ModContent.NPCType<CrystalWolfPurple>(),
                ModContent.NPCType<CrystalWolfRed>(),
                //ModContent.NPCType<CrystalWolfWhite>(),

                // White
                ModContent.NPCType<WhiteGoop>(),
                ModContent.NPCType<WhiteKnight>(),
                ModContent.NPCType<WhiteLatexCub>(),
                ModContent.NPCType<WhiteLatexTaur>(),

                ModContent.NPCType<LatexMoth>(),
                ModContent.NPCType<Snek>(),

                // Others
                ModContent.NPCType<Bloodstripe>(),
                ModContent.NPCType<Purrpurr>(),
                ModContent.NPCType<GermanShepherd>(),
                ModContent.NPCType<Lion>(),
                ModContent.NPCType<Snep>(),
                ModContent.NPCType<Raccoon>(),
                ModContent.NPCType<SquidDog>(),
                ModContent.NPCType<Wendigo>(),
                ModContent.NPCType<MutatedLatex>(),

                // Drunk
                ModContent.NPCType<BackLatex>(),
                ModContent.NPCType<BrightLatex>(),
                ModContent.NPCType<DarkLatexCubOfDoom>(),
                ModContent.NPCType<FightLatex>(),
                ModContent.NPCType<FlightLatex>(),
                ModContent.NPCType<HideLatex>(),
                ModContent.NPCType<HungryLocker>(),
                ModContent.NPCType<MightLatex>(),
                ModContent.NPCType<PuroWormHead>(),
                ModContent.NPCType<QuackLatex>(),
                ModContent.NPCType<SideLatex>(),
                ModContent.NPCType<SnackLatex>(),
                ModContent.NPCType<StackLatex>(),
                ModContent.NPCType<WackLatex>(),
                ModContent.NPCType<WideLatex>(),
            };

            foreach (var npc in npcs)
            {
                int xPos = (int)player.Center.X + Main.rand.Next(-40, 40) * 16;
                int yPos = (int)player.Center.Y + Main.rand.Next(-20, 0) * 16;
                NPC.NewNPCDirect(entitySource, xPos, yPos, npc, player.whoAmI);
            }
            */

            return true;
        }
    }
}
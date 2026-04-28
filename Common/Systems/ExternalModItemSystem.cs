using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.NPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
    public enum ItemCategory
    {
        Fruit,
        Herb
    }

    public static class ExternalModItemSystem
    {
        public class ExternalMod
        {
            public Mod Mod { get; }
            private readonly Dictionary<ItemCategory, List<ExternalModItem>> _items;

            public ExternalMod(Mod mod, Dictionary<ItemCategory, List<ExternalModItem>> items)
            {
                Mod = mod;
                _items = items ?? new();
            }

            public List<int> GetAvailable(ItemCategory category)
            {
                if (Mod == null || !_items.TryGetValue(category, out var source))
                    return new();

                var result = new List<int>();

                foreach (var item in source)
                {
                    if (item is not { Available: true })
                        continue;

                    if (Mod.TryFind(item.ItemName, out ModItem modItem))
                        result.Add(modItem.Type);
                }

                return result;
            }
        }

        public class ExternalModItem
        {
            public string ItemName { get; }
            public bool Available { get; }

            public ExternalModItem(string itemName, bool available = true)
            {
                ItemName = itemName;
                Available = available;
            }
        }

        // Get items from external mods, grouped by the mod they belong to
        public static List<ExternalMod> GetExternalMods()
        {
            var anyMech = NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3;

            return new List<ExternalMod>
            {
                new(ModSupportSystem.modThorium,
                    new()
                    {
                        [ItemCategory.Fruit] = new()
                        {
                            new("Fig"),
                            new("Tamarind"),
                            new("Aril"),
                            new("Lychee"),
                            new("Kumquat", Main.hardMode),
                            new("Persimmon", Main.hardMode),
                            new("Soursop", NPC.downedPlantBoss),
                            new("Mangosteen", NPC.downedPlantBoss),
                            new("Cranberry", NPC.downedPlantBoss),
                            new("Raspberry", NPC.downedPlantBoss),
                        },
                        [ItemCategory.Herb] = new()
                        {
                            new("MarineKelp")
                        }
                    }
                ),

                new(ModSupportSystem.modCalamity,
                    new()
                    {
                        [ItemCategory.Fruit] = new()
                        {
                            new("Jackfruit"),
                            new("Salak"),
                            new("Barberry", Main.hardMode),
                            new("Cometfruit", Main.hardMode),
                            new("Lotus", Main.hardMode),
                            new("Mangosteen", Main.hardMode)
                        }
                    }
                ),

                new(ModSupportSystem.modSpirit,
                    new()
                    {
                        [ItemCategory.Fruit] = new()
                        {
                            new("EnchantedApple"),
                            new("EnchantedStarFruit"),
                            new("Durian"),
                            new("Guava"),
                            new("IceBerry"),
                            new("Glowpear", anyMech),
                            new("LuminBerry", anyMech),
                            new("BirdhouseGourd", NPC.downedGolemBoss),
                            new("CaramelApple", NPC.downedGolemBoss)
                        }
                    }
                ),

                new(ModSupportSystem.modSpiritReforged,
                    new()
                    {
                        [ItemCategory.Fruit] = new()
                        {
                            new("CrescentMelon"),
                            new("MidnightApple"),
                            new("Pearlberry"),
                            new("BaobabFruit"),
                            new("Caryocar"),
                            new("CustardApple")
                        }
                    }
                )
            };
        }

        public static List<int> GetFruitItems(List<ExternalMod> externalMods)
        {
            var list = new List<int>
            {
                ItemID.Apple,
                ItemID.Apricot,
                ItemID.Banana,
                ItemID.BlackCurrant,
                ModContent.ItemType<Orange>(),
                ItemID.BloodOrange,
                ItemID.Cherry,
                ItemID.Coconut,
                ItemID.Elderberry,
                ItemID.Grapefruit,
                ItemID.Lemon,
                ItemID.Mango,
                ItemID.Peach,
                ItemID.Pineapple,
                ItemID.Plum,
                ItemID.Pomegranate,
                ItemID.Rambutan,
                ItemID.SpicyPepper,
            };

            foreach (var mod in externalMods)
                list.AddRange(mod.GetAvailable(ItemCategory.Fruit));

            return list;
        }

        public static List<int> GetHerbItems(List<ExternalMod> externalMods)
        {
            var list = new List<int>
            {
                ItemID.Daybloom,
                ItemID.Moonglow,
                ItemID.Blinkroot,
                ItemID.Deathweed,
                ItemID.Waterleaf,
                ItemID.Fireblossom,
                ItemID.Shiverthorn
            };

            foreach (var mod in externalMods)
                list.AddRange(mod.GetAvailable(ItemCategory.Herb));

            return list;
        }
    }
}

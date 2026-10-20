using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{

    /*
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

    }
    */
}

using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using System.Linq;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public enum ItemSellCondition
    {
        None,
        HardMode,
        DownedAnyMech,
        DownedPlantera,
        DownedGolem
    }

    public enum ItemCategory
    {
        None,
        Fruit,
        Herb,
        Seeds
    }

    public class ExternalModItem
    {
        public int itemId = -1;
        public ItemCategory category = ItemCategory.None;
        public ItemSellCondition shopSellCondition = ItemSellCondition.None;

        public ExternalModItem(int itemId, ItemCategory category = ItemCategory.None, ItemSellCondition shopSellCondition = ItemSellCondition.None)
        {
            this.itemId = itemId;
            this.category = category;
            this.shopSellCondition = shopSellCondition;
        }

        public bool CanBeSold()
        {
            switch (shopSellCondition)
            {
                default:
                    return false;
                case ItemSellCondition.None:
                    return true;
                case ItemSellCondition.DownedAnyMech:
                    return NPC.downedMechBossAny;
                case ItemSellCondition.DownedPlantera:
                    return NPC.downedPlantBoss;
                case ItemSellCondition.DownedGolem:
                    return NPC.downedGolemBoss;
            }
        }
    }

    public class ModSupportData
    {
        public Mod mod = null;
        public string modName = "UnknownMod";
        
        // Tiles
        public List<int> tilesAvoidedByWorldGen = new List<int>();

        // Items
        public Dictionary<string, ExternalModItem> items = new Dictionary<string, ExternalModItem>();

        // IDs
        public Dictionary<string, int> tileTypes = new Dictionary<string, int>();
        public Dictionary<string, int> townNPCIds = new Dictionary<string, int>();

        public DownedBossObjectList bossProgressionList;

        public bool isActive => mod != null;

        public ModSupportData(Mod mod, string modName)
        {
            this.mod = mod;
            this.modName = modName;
        }

        public virtual void Init() { }

        public virtual void PostSetupContent(log4net.ILog log)
        {
            if (mod == null)
                return;

            AddTownNPCs();
            AddTiles();
            AddItemsFruit();
            AddItemsHerb();
            AddItemsSeed();
        }

        public virtual void AddTownNPCs() { }
        public virtual void AddTiles() { }
        public virtual void AddItemsFruit() { }
        public virtual void AddItemsHerb() { }
        public virtual void AddItemsSeed() { }

        public void AddTileType(string name, bool avoidedByWorldGen = false)
        {
            if (tileTypes == null)
                tileTypes = new Dictionary<string, int>();

            if (mod == null || tileTypes.ContainsKey(name))
                return;

            if (mod.TryFind(name, out ModTile modTile))
            {
                var itemType = modTile.Type;
                tileTypes.Add(name, itemType);
                if (avoidedByWorldGen)
                    tilesAvoidedByWorldGen.Add(itemType);
            }
        }

        public void AddTownNPC(string internalName, string localizationName)
        {
            if (townNPCIds == null)
                townNPCIds = new Dictionary<string, int>();

            if (mod == null || townNPCIds.ContainsKey(localizationName))
                return;

            if (mod.TryFind(internalName, out ModNPC modNPC))
            {
                var npcType = modNPC.Type;
                townNPCIds.Add(localizationName, npcType);
            }
        }

        public void AddItem(string name, ItemCategory itemCategory = ItemCategory.None, ItemSellCondition shopSellCondition = ItemSellCondition.None)
        {
            if (mod == null)
                return;

            if (items == null)
                items = new Dictionary<string, ExternalModItem>();

            if (items.ContainsKey(name))
                return;

            if (mod.TryFind(name, out ModItem modItem))
            {
                var itemEntry = new ExternalModItem(modItem.Type, itemCategory, shopSellCondition);
                items.Add(name, itemEntry);
            }
        }

        public void AddItemFruit(string name, ItemSellCondition shopSellCondition = ItemSellCondition.None)
        {
            AddItem(name, ItemCategory.Fruit, shopSellCondition);
        }

        public void AddItemHerb(string name, ItemSellCondition shopSellCondition = ItemSellCondition.None)
        {
            AddItem(name, ItemCategory.Herb, shopSellCondition);
        }

        public void AddItemSeed(string name, ItemSellCondition shopSellCondition = ItemSellCondition.None)
        {
            AddItem(name, ItemCategory.Seeds, shopSellCondition);
        }

        public bool ShouldAvoidTileType(int tileType)
        {
            return tilesAvoidedByWorldGen.Contains(tileType);
        }

        public List<int> GetItemIdsInCategory(ItemCategory itemCategory)
        {
            var result = new List<int>();

            if (items != null && items.Count > 0)
            {
                result = items.Values.Where(x => x.CanBeSold() && 
                x.category == itemCategory).Select(x => x.itemId).ToList();
            }

            return result;
        }
    }
}

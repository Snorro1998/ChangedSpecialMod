using System.Collections.Generic;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems.ModSupport
{
    public class ModSupportData
    {
        public Mod mod = null;
        public string modName = "UnknownMod";
        
        // Tiles
        public List<int> tilesAvoidedByWorldGen = new List<int>();

        // IDs
        public Dictionary<string, int> tileTypes = new Dictionary<string, int>();
        public Dictionary<string, int> townNPCIds = new Dictionary<string, int>();
        public Dictionary<string, int> itemIdsFruit = new Dictionary<string, int>();
        public Dictionary<string, int> itemIdsHerb = new Dictionary<string, int>();
        public Dictionary<string, int> itemIdsSeed = new Dictionary<string, int>();

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

        // TODO re-add shop sell conditions
        public void AddItemFruit(string name)
        {
            if (itemIdsFruit == null)
                itemIdsFruit = new Dictionary<string, int>();

            if (mod == null || itemIdsFruit.ContainsKey(name))
                return;

            if (mod.TryFind(name, out ModItem modItem))
            {
                var itemType = modItem.Type;
                itemIdsFruit.Add(name, itemType);
            }
        }

        // TODO re-add shop sell conditions
        public void AddItemHerb(string name)
        {
            if (itemIdsHerb == null)
                itemIdsHerb = new Dictionary<string, int>();

            if (mod == null || itemIdsHerb.ContainsKey(name))
                return;

            if (mod.TryFind(name, out ModItem modItem))
            {
                var itemType = modItem.Type;
                itemIdsHerb.Add(name, itemType);
            }
        }

        // TODO re-add shop sell conditions
        public void AddItemSeed(string name)
        {
            if (itemIdsSeed == null)
                itemIdsSeed = new Dictionary<string, int>();

            if (mod == null || itemIdsSeed.ContainsKey(name))
                return;

            if (mod.TryFind(name, out ModItem modItem))
            {
                var itemType = modItem.Type;
                itemIdsSeed.Add(name, itemType);
            }
        }

        public bool ShouldAvoidTileType(int tileType)
        {
            return tilesAvoidedByWorldGen.Contains(tileType);
        }
    }
}

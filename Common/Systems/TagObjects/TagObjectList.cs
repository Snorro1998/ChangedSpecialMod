using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace ChangedSpecialMod.Common.Systems.TagObjects
{
    public class TagObjectList<T> where T : TagObject
    {
        public List<T> elements = new List<T>();

        public virtual void Init()
        {
            // Child classes populate the elements list
        }

        public T PickRandom(List<string> blacklist = null)
        {
            var list = elements;
            if (blacklist != null && blacklist.Count > 0)
                list = list.Where(x => !blacklist.Contains(x.name)).ToList();

            return Utils.SelectRandom(Main.rand, list.ToArray());
        }
    }
}

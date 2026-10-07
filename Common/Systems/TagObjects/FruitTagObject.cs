using System.Collections.Generic;

namespace ChangedSpecialMod.Common.Systems.TagObjects
{
    public class FruitTagObject : TagObject
    {
        public FruitTagObject() : base() { tagCategory = "Fruit"; }
    }

    public class FruitTagObjectList : TagObjectList<FruitTagObject>
    {
        public override void Init()
        {
            // Fruit
            elements = new List<FruitTagObject>()
            {
                 new FruitTagObject() { name = "apple" },
                 new FruitTagObject() { name = "apricot" },
                 new FruitTagObject() { name = "banana" },
                 new FruitTagObject() { name = "blackberry" },
                 new FruitTagObject() { name = "blackcurrant" },
                 new FruitTagObject() { name = "blueberry" },
                 new FruitTagObject() { name = "cherry" },
                 new FruitTagObject() { name = "coconut" },
                 new FruitTagObject() { name = "grape" },
                 new FruitTagObject() { name = "grapefruit" },
                 new FruitTagObject() { name = "kiwifruit" },
                 new FruitTagObject() { name = "lemon" },
                 new FruitTagObject() { name = "mango" },
                 new FruitTagObject() { name = "peach" },
                 new FruitTagObject() { name = "pear" },
                 new FruitTagObject() { name = "pineapple" },
                 new FruitTagObject() { name = "plum" },
                 new FruitTagObject() { name = "pomegranate" },
                 new FruitTagObject() { name = "raspberry" },
                 new FruitTagObject() { name = "strawberry" },
                 new FruitTagObject() { name = "watermelon" },
            };

            foreach (var item in elements)
                item.isFoodIngredient = true;
        }
    }
}

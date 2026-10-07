using System.Collections.Generic;

namespace ChangedSpecialMod.Common.Systems.TagObjects
{
    public class VegetableTagObject : TagObject
    {
        public VegetableTagObject() : base() { tagCategory = "Vegetable"; }
    }

    public class VegetableTagObjectList : TagObjectList<VegetableTagObject>
    {
        public override void Init()
        {
            // Vegetables
            elements = new List<VegetableTagObject>()
            {
                new VegetableTagObject() { name = "bean" },
                new VegetableTagObject() { name = "beetroot" },
                new VegetableTagObject() { name = "bell pepper" },
                new VegetableTagObject() { name = "broccoli" },
                new VegetableTagObject() { name = "cabbage" },
                new VegetableTagObject() { name = "carrot" },
                new VegetableTagObject() { name = "cauliflower" },
                new VegetableTagObject() { name = "corn" },
                new VegetableTagObject() { name = "cucumber" },
                new VegetableTagObject() { name = "eggplant" },
                new VegetableTagObject() { name = "garlic" },
                new VegetableTagObject() { name = "kale" },
                new VegetableTagObject() { name = "leek" },
                new VegetableTagObject() { name = "lettuce" },
                new VegetableTagObject() { name = "mushroom" },
                new VegetableTagObject() { name = "onion" },
                new VegetableTagObject() { name = "parsnip" },
                new VegetableTagObject() { name = "pea" },
                new VegetableTagObject() { name = "peanut" },
                new VegetableTagObject() { name = "potato" },
                new VegetableTagObject() { name = "pumpkin" },
                new VegetableTagObject() { name = "radish" },
                new VegetableTagObject() { name = "rhubarb" },
                new VegetableTagObject() { name = "spinach" },
                new VegetableTagObject() { name = "tomato" },
                new VegetableTagObject() { name = "turnip" },
                new VegetableTagObject() { name = "wasabi" },
            };

            foreach (var noun in elements)
                noun.isFoodIngredient = true;
        }
    }
}

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Weapons
{
    public class TigerPaw : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10; 
            Item.useAnimation = 10;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 16;
            Item.knockBack = 3;
            Item.crit = 2;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.UseSound = SoundID.Item1;
        }
    }
}

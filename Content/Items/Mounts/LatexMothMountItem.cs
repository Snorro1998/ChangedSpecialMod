using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.Mounts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Mounts
{
    public class LatexMothMountItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = Sounds.SoundTransfur;
            Item.noMelee = true;
            Item.mountType = ModContent.MountType<LatexMothMount>();
        }
    }
}

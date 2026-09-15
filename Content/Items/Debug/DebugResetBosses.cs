using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Projectiles;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Debug
{
    public class DebugResetBosses : ModItem
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
            var nWormHead = Main.projectile.Where(x => x.active && x.type == ModContent.ProjectileType<SquidDogTentacleHeadProjectile>()).ToList().Count;
            var nWormBody = Main.projectile.Where(x => x.active && x.type == ModContent.ProjectileType<SquidDogTentacleBodyProjectile>()).ToList().Count;
            var nWormTail = Main.projectile.Where(x => x.active && x.type == ModContent.ProjectileType<SquidDogTentacleTailProjectile>()).ToList().Count;

            Main.NewText($"{nWormHead}, {nWormBody}, {nWormTail}");

            return true;

            Main.NewText(Language.GetTextValue("Mods.ChangedSpecialMod.Messages.BossProgressionReset"));
            DownedBossSystem.DownedWolfKing = false;
            DownedBossSystem.DownedWhiteTail = false;
            DownedBossSystem.DownedBehemoth = false;
            return true;
        }
    }
}
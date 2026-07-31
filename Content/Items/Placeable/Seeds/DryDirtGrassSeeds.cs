using ChangedSpecialMod.Content.Tiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Items.Placeable.Seeds
{
    public class DryDirtGrassSeeds : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.maxStack = Item.CommonMaxStack;

            Item.value = Item.buyPrice(silver: 1); // Sold by Dryad; equal to Hallowed Seeds
        }

        public override bool? UseItem(Player player) => true;

        public override bool ConsumeItem(Player player)
        {
            var tileX = Player.tileTargetX;
            var tileY = Player.tileTargetY;
            var tile = Framing.GetTileSafely(tileX, tileY);

            if (tile.HasTile && tile.TileType == ModContent.TileType<DryDirt>() && player.IsInTileInteractionRange(tileX, tileY, TileReachCheckSettings.Simple))
            {
                tile.TileType = (ushort)ModContent.TileType<DryDirtGrassTile>();
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendTileSquare(player.whoAmI, tileX, tileY);
                SoundEngine.PlaySound(SoundID.Dig, player.Center);
                WorldGen.SquareTileFrame(tileX, tileY);
                return true;
            }

            return false;
        }
    }
}

using ChangedSpecialMod.Common.Configs;
using ChangedSpecialMod.Content.NPCs.Drunk;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class Locker : ModTile
	{
        public static string retroTexturePath = "ChangedSpecialMod/Content/Tiles/Furniture/LockerRetro";

        public override void SetStaticDefaults() 
        {
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileSolidTop[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            // This line makes NPCs not try to step up this tile during their movement. Only use this for furniture with solid tops.
            TileID.Sets.IgnoredByNpcStepUp[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            DustType = DustID.Silver;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            base.KillMultiTile(i, j, frameX, frameY);

            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            var rareChance = Main.drunkWorld ? 1 : 30;
            var player = ChangedUtils.GetClosestPlayer(i, j);
            if (player != null && ChangedUtils.IsDrunk(player))
                rareChance = 1;
            if (Main.rand.Next(rareChance) == 0)
                SpawnHungryLocker(i, j);
            else
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, (j + 1) * 16, 16, 16, ModContent.ItemType<Items.Placeable.Furniture.Locker>());
        }

        public override bool CanDrop(int i, int j)
        {
            return false;
        }

        private void SpawnHungryLocker(int i, int j)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            var source = WorldGen.GetItemSource_FromTileBreak(i, j);
            NPC npc = NPC.NewNPCDirect(source, (i + 1) * 16, (j + 3) * 16, ModContent.NPCType<HungryLocker>());

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.SyncNPC, number: npc.whoAmI);
        }

        public override void RandomUpdate(int i, int j)
        {
            var player = ChangedUtils.GetClosestPlayer(i, j);
            if (ChangedUtils.IsDrunk(player))
            {
                var dist = Vector2.DistanceSquared(player.Center, new Vector2(i * 16, j * 16));
                var minDist = 16 * 30;
                if (dist < minDist * minDist)
                    // Destroys itself, which spawns a hungry locker
                    WorldGen.KillTile(i, j, false, false);
            }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (ChangedSpecialModClientConfig.Instance.RetroVisualsTiles)
            {
                RetroDraw(i, j, spriteBatch);
                return false;
            }
            return base.PreDraw(i, j, spriteBatch);
        }

        private void RetroDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];

            if (tile.IsTileInvisible && !Main.ShouldShowInvisibleWalls())
                return;

            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            int height = 16;
            int frameYOffset = 0;

            var texture = TextureAssets.Tile[Type].Value;
            if (ModContent.RequestIfExists(retroTexturePath, out Asset<Texture2D> retroTexture))
                texture = retroTexture.Value;

            spriteBatch.Draw(
                texture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY + frameYOffset, 16, height),
                Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
        }
    }
}

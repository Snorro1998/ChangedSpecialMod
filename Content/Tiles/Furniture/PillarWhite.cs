using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
	public class PillarWhite : ModTile
	{
        private int spawnChance = 5;
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.RandomStyleRange = 4;
            TileObjectData.newTile.StyleHorizontal = true;
            TileID.Sets.FramesOnKillWall[Type] = false;
            TileObjectData.addTile(Type);
            DustType = DustID.SnowBlock;
            AddMapEntry(new Color(220, 220, 220));
        }

        // Ignore style nonsense and always drop the correct item
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<Items.Placeable.Furniture.PillarWhite>());
        }

        private void SpawnLatex(int xPos, int yPos, int npcType, int playerIndex)
        {
            IEntitySource source = new EntitySource_WorldEvent();
            var npc = NPC.NewNPCDirect(source, xPos, yPos, npcType, playerIndex);

            if (npc == null)
                return;

            var nParticles = 40;
            var dustType = DustID.SnowSpray;
            var anglePerParticle = Math.PI * 2 / nParticles;
            for (int k = 0; k < nParticles; k++)
            {
                Vector2 position = npc.Center;
                position += Main.rand.NextVector2Square(-20, 21);
                var currentAngle = Main.rand.NextFloat(0, (float)(2f * Math.PI));
                Dust.NewDustPerfect(position, dustType, Vector2.Zero).noGravity = true;
            }
        }

        public override void RandomUpdate(int i, int j)
        {
            if (!Main.rand.NextBool(spawnChance))
                return;

            var topLeft = TileObjectData.TopLeft(i, j);

            i = topLeft.X;
            j = topLeft.Y;

            var player = ChangedUtils.GetClosestPlayer(i, j);
            var xPos = (int)((i + 0.5f) * 16);
            var yPos = (j + 3) * 16;

            var minDist = 8 * 16;
            var maxDist = 30 * 16;
            var pos = new Vector2(xPos, yPos);
            var npcType = ChangedUtils.Choose(ModContent.NPCType<WhiteGoop>(), ModContent.NPCType<WhiteLatexCub>(), ModContent.NPCType<WhiteKnight>());
            var maxNPCs = 6;
            var nNpcs = Main.npc.Where(x => x.active && !x.friendly).ToList().Count;

            if (player != null && nNpcs < maxNPCs && player.Distance(pos) > minDist && player.Distance(pos) < maxDist)
                SpawnLatex(xPos, yPos, npcType, player.whoAmI);

            base.RandomUpdate(i, j);
        }
    }
}

using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
    public abstract class LabDoorTile : ModTile
    {
        public abstract int DestinationTileID { get; }
        public override string Texture => "ChangedSpecialMod/Content/Tiles/Furniture/LabDoorATile";

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = false;
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.addTile(Type);
            DustType = DustID.WoodFurniture;
            AddMapEntry(new Color(160, 160, 160));
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }

        public override bool RightClick(int i, int j)
        {
            if ((DestinationTileID <= 0 && DestinationTileID > -2) || DestinationTileID < -2)
                return true;

            Player player = ChangedUtils.GetClosestPlayer(i, j);
            if (!player.IsWithinSnappngRangeToTile(i, j, 5 * 16))
                return false;

            if (NPC.AnyNPCs(ModContent.NPCType<WhiteTail>()))
            {
                if (DestinationTileID == ModContent.TileType<LabDoorETile>() ||
                    DestinationTileID == ModContent.TileType<LabDoorHTile>())
                {
                    var xPos = i;
                    var yPos = j;
                    SoundEngine.PlaySound(Assets.Sounds.SoundLockedDoor, new Vector2(xPos * 16, yPos * 16));
                    return true;
                }
            }

            if (DestinationTileID == -2)
            {
                var xPos = player.SpawnX;
                var yPos = player.SpawnY - 3;

                if (xPos == -1)
                {
                    xPos = Main.spawnTileX;
                    yPos = Main.spawnTileY - 3;
                }

                var worldPos = new Vector2(xPos * 16, yPos * 16);
                SoundEngine.PlaySound(Assets.Sounds.SoundDoor, worldPos);
                player.RemoveAllGrapplingHooks();
                ChangedUtils.TeleportPlayer(player.whoAmI, xPos, yPos);
                Main.NewText(Language.GetTextValue("Mods.ChangedSpecialMod.Messages.WalkedThroughLab"));
                var projectile = Projectile.NewProjectileDirect(player.GetSource_FromThis(), worldPos, new Vector2(0, -8), ProjectileID.RocketFireworkBlue, 0, 0, player.whoAmI);
                Main.NewText(DownedBossSystem.ExploredLabVersion.ToString());
                DownedBossSystem.SetExploredLab();
                return true;
            }

            for (int x = 0; x < Main.maxTilesX; x += 3)
            {
                for (int y = 0; y < Main.maxTilesY; y += 3)
                {
                    var tile = Framing.GetTileSafely(x, y);
                    if (tile.HasTile && tile.TileType == DestinationTileID)
                    {
                        var topLeft = TileObjectData.TopLeft(x, y);
                        if (topLeft != Point16.NegativeOne)
                        {
                            var xPos = topLeft.X;
                            var yPos = topLeft.Y;
                            SoundEngine.PlaySound(Assets.Sounds.SoundDoor, new Vector2(xPos * 16, yPos * 16));
                            player.RemoveAllGrapplingHooks();
                            ChangedUtils.TeleportPlayer(player.whoAmI, xPos + 1, yPos);
                        }
                        
                        return true;
                    }
                }
            }

            Main.NewText(Language.GetTextValue("Mods.ChangedSpecialMod.Messages.LabCouldntUseDoor"));
            
            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = ChangedUtils.GetClosestPlayer(i, j);
            if (!player.IsWithinSnappngRangeToTile(i, j, 5 * 16))
                return;

            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.ElevatorUp>();
        }
    }

	public class LabDoorATile : LabDoorTile
    {
        public override int DestinationTileID => ModContent.TileType<LabDoorBTile>();
    }

    public class LabDoorBTile : LabDoorTile
    {
        public override int DestinationTileID => ModContent.TileType<LabDoorATile>();
    }

    public class LabDoorCTile : LabDoorTile
    {
        public override int DestinationTileID => ModContent.TileType<LabDoorDTile>();
    }

    public class LabDoorDTile : LabDoorTile
    {
        public override int DestinationTileID => ModContent.TileType<LabDoorCTile>();
    }

    public class LabDoorETile : LabDoorTile
    {
        public override int DestinationTileID => ModContent.TileType<LabDoorFTile>();
    }

    public class LabDoorFTile : LabDoorTile
    {
        public override int DestinationTileID => ModContent.TileType<LabDoorETile>();
    }

    public class LabDoorGTile : LabDoorTile
    {
        public override int DestinationTileID => ModContent.TileType<LabDoorHTile>();
    }

    public class LabDoorHTile : LabDoorTile
    {
        public override int DestinationTileID => ModContent.TileType<LabDoorGTile>();
    }

    public class LabDoorITile : LabDoorTile
    {
        public override int DestinationTileID => ModContent.TileType<LabDoorJTile>();
    }

    public class LabDoorJTile : LabDoorTile
    {
        public override int DestinationTileID => ModContent.TileType<LabDoorITile>();
    }

    public class LabDoorKTile : LabDoorTile
    {
        public override int DestinationTileID => ModContent.TileType<LabDoorLTile>();
    }

    public class LabDoorLTile : LabDoorTile
    {
        public override int DestinationTileID => ModContent.TileType<LabDoorKTile>();
    }

    public class LabDoorHomeTile : LabDoorTile
    {
        public override int DestinationTileID => -2;
    }
}

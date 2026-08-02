using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChangedSpecialMod.Content.Tiles.Furniture
{
    public class DocumentPaper : ModTile
    {
        public static LocalizedText DefaultSignText { get; private set; }

        public override void SetStaticDefaults()
        {
            Main.tileSign[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileID.Sets.AvoidedByNPCs[Type] = true;
            TileID.Sets.TileInteractRead[Type] = true;
            VanillaFallbackOnModDeletion = TileID.Signs;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(200, 200, 200), name);
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            int signId = Sign.ReadSign(i, j, true);
            if (signId != -1)
            {
                //Sign.TextSign(signId, txt);
            }
        }

        public override bool RightClick(int i, int j)
        {
            int signId = Sign.ReadSign(i, j);
            if (signId != -1)
            {
                string signText = Main.sign[signId].text;
                //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(signText), Color.White);
            }
            return true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Sign.KillSign(i, j);
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }
    }
}

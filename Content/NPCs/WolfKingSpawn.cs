using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Tiles;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
    public class WolfKingSpawn : ModNPC
	{
        private enum ActionState
        {
            Idle,
            Turn,
            StartBossFight
        }

        // We are gonna do it just like GameMaker, but we have to do it all ourselves
        public double imageSpeed = 5D;
        public int imageIndex = 0;

        public int[] animation = new int[] { 0, 1, 2, 3};
        public int[] animIdle = new int[] { 0 };
        public int[] animTurn = new int[] { 1, 2 };
        public int[] animLightEyes = new int[] { 3 };

        public int ImageLength {get { return animation.Length; }}
        public bool Loop = false;

        public double imageCounter = 0D;

        public ref float AIState => ref NPC.ai[0];
        public ref float AITimer => ref NPC.ai[1];
        public ref float AINSnaps => ref NPC.ai[2];

        public int npcDirection = 1;

        public override void SetStaticDefaults() 
        {
            Main.npcFrameCount[Type] = 10;
            ChangedUtils.HideFromBestiary(this);
        }

		public override void SetDefaults() 
        {
			NPC.width = 100;
			NPC.height = 100;
            NPC.damage = 0;
            NPC.defense = 32;
            NPC.lifeMax = 5000;
            NPC.HitSound = SoundID.NPCHit6;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.0f;
            NPC.aiStyle = -1;
			AIType = NPCID.None;
			AnimationType = NPCID.None;
            NPC.dontTakeDamage = true;
            SpawnModBiomes = new int[] { ModContent.GetInstance<BlackLatexSurfaceBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.GooType = GooType.Black;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override void FindFrame(int frameHeight)
        {
            imageCounter += imageSpeed;
            if (imageCounter >= ImageLength * 60)
            {
                if (Loop)
                {
                    imageCounter %= ImageLength * 60;
                }
                else
                {
                    imageCounter = ImageLength * 60 - 1;
                }
            }

            var arrayIndex = (int)(imageCounter / 60D);
            imageIndex = animation[arrayIndex];
            NPC.frame.Y = imageIndex * frameHeight;
        }

        private void SwitchAnimation(int[] newAnimation)
        {
            imageCounter = 0;
            animation = newAnimation;
        }

        private void SwitchState(ActionState newState)
        {
            AIState = (float)newState;
            AITimer = 0;
        }

        // TODO: implement the same for tall gates
        // Replaces doors with solid blocks and repairs any gaps the player might have made on purpose
        private void ShutDoors()
        {
            var maxRange = 50;
            var xPos = (int)(NPC.position.X / 16);
            var yPos = (int)(NPC.position.Y / 16);

            var tmpX = xPos;
            var tmpY = yPos;
            var tile = Main.tile[tmpX, tmpY];

            var leftPos = -1;
            for (int iL = 0; iL < maxRange; iL++)
            {
                var tileL = Main.tile[tmpX - iL, tmpY];
                if (!ChangedUtils.IsBlackLatexWall(tileL))
                {
                    leftPos = tmpX - iL;
                    break;
                }
            }

            var rightPos = -1;
            for (int iR = 0; iR < maxRange; iR++)
            {
                var tileR = Main.tile[tmpX + iR, tmpY];
                if (!ChangedUtils.IsBlackLatexWall(tileR))
                {
                    rightPos = tmpX + iR;
                    break;
                }
            }

            var topPos = -1;
            for (int iT = 0; iT < maxRange; iT++)
            {
                var tileT = Main.tile[tmpX, tmpY - iT];
                if (!ChangedUtils.IsBlackLatexWall(tileT))
                {
                    topPos = tmpY - iT;
                    break;
                }
            }

            var bottomPos = -1;
            for (int iB = 0; iB < maxRange; iB++)
            {
                var tileR = Main.tile[tmpX, tmpY + iB];
                if (!ChangedUtils.IsBlackLatexWall(tileR))
                {
                    bottomPos = tmpY + iB;
                    break;
                }
            }

            // Fill holes in left wall
            for (var i = topPos; i <= bottomPos; i++)
            {
                var tTile = Main.tile[leftPos, i];
                if (tTile == null)
                    continue;

                if (tTile.HasActuator)
                    WorldGen.KillActuator(leftPos, i);

                if (!tTile.HasTile)
                    WorldGen.PlaceTile(leftPos, i, ModContent.TileType<BlackLatexTile>(), false, true);
                else
                {
                    if (tTile.IsActuated)
                        tTile.IsActuated = false;

                    //1X3 scenery tile
                    if (ChangedUtils.IsOpenDoorAtTile(leftPos, i) || ChangedUtils.IsDoorAtTile(leftPos, i) || ChangedUtils.IsCrystalAtTile(leftPos, i))
                    {
                        WorldGen.KillTile(leftPos, i, false, false, true);
                        WorldGen.PlaceTile(leftPos, i, ModContent.TileType<BlackLatexTile>());
                        WorldGen.PlaceTile(leftPos, i + 1, ModContent.TileType<BlackLatexTile>());
                        WorldGen.PlaceTile(leftPos, i + 2, ModContent.TileType<BlackLatexTile>());
                    }
                }
            }

            // Fill holes in right wall
            for (var i = topPos; i <= bottomPos; i++)
            {
                var tTile = Main.tile[rightPos, i];
                if (tTile == null)
                    continue;

                if (tTile.HasActuator)
                    WorldGen.KillActuator(rightPos, i);

                if (!tTile.HasTile)
                    WorldGen.PlaceTile(rightPos, i, ModContent.TileType<BlackLatexTile>(), false, true);
                else
                {
                    if (tTile.IsActuated)
                        tTile.IsActuated = false;

                    //1X3 scenery tile
                    if (ChangedUtils.IsOpenDoorAtTile(rightPos, i) || ChangedUtils.IsDoorAtTile(rightPos, i) || ChangedUtils.IsCrystalAtTile(rightPos, i))
                    {
                        WorldGen.KillTile(rightPos, i, false, false, true);
                        WorldGen.PlaceTile(rightPos, i, ModContent.TileType<BlackLatexTile>());
                        WorldGen.PlaceTile(rightPos, i + 1, ModContent.TileType<BlackLatexTile>());
                        WorldGen.PlaceTile(rightPos, i + 2, ModContent.TileType<BlackLatexTile>());
                    }
                }
            }

            // Fill holes in ceiling
            for (var i = leftPos; i <= rightPos; i++)
            {
                var tTile = Main.tile[i, topPos];
                if (!tTile.HasTile)
                {
                    WorldGen.PlaceTile(i, topPos, ModContent.TileType<BlackLatexTile>(), false, true);
                }
            }

            // Fill holes in floor
            for (var i = leftPos; i <= rightPos; i++)
            {
                var tTile = Main.tile[i, bottomPos];
                if (!tTile.HasTile)
                {
                    WorldGen.PlaceTile(i, bottomPos, ModContent.TileType<BlackLatexTile>(), false, true);
                }
            }
        }

        private void StateIdle()
        {
            AITimer++;

            if (AITimer == 1)
            {
                SwitchAnimation(animIdle);
                ChangedUtils.SpawnCheerleaders(NPC, true);
                //SpawnCheerleaders();
            }

            var player = Main.LocalPlayer;
            var pos = NPC.Center;

            if (Vector2.Distance(pos, player.Center) < 48)
            {
                SwitchState(ActionState.Turn);
            }
        }

        private void StateTurn()
        {
            AITimer++;

            if (AITimer == 1)
            {
                SoundEngine.PlaySound(Sounds.SoundPush, NPC.Center);
                SwitchAnimation(animTurn);
                ShutDoors();
            }

            // Light his eyes after n seconds
            if (AITimer == 2 * 60)
            {
                SoundEngine.PlaySound(Sounds.SoundSword1, NPC.Center);
                SwitchAnimation(animLightEyes);
            }

            // Start the bossfight after n seconds
            if (AITimer == 4 * 60)
            {
                SwitchState(ActionState.StartBossFight);
            }
        }

        private void StateStartBossFight()
        {
            AITimer++;

            if (AITimer == 1)
            {
                NPC.active = false;
                int type = ModContent.NPCType<WolfKing>();
                NPC.NewNPC(NPC.GetSource_FromAI(), 0, 0, type);

                foreach (var npc in Main.npc)
                {
                    if (npc.type == type)
                    {
                        npc.position = NPC.position;
                        break;
                    }
                }
            }
        }

        private void SpawnCheerleaders()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;
            var entitySource = NPC.GetSource_FromAI();
            int count = 4;
            int distFromBoss = 6;
            int distBetween = 4;

            var npcType = ModContent.NPCType<Cheerleader>();

            foreach (var npc in Main.ActiveNPCs)
            {
                if (npc.type == npcType)
                {
                    npc.active = false;
                }
            }

            for (int i = 0; i < count; i++)
            {
                //0,1,2,3
                int xPos = (int)NPC.Center.X;

                switch(i)
                {
                    case 0:
                        xPos = xPos - 16 * (distFromBoss + distBetween * 1);
                        break;
                    case 1:
                        xPos = xPos - 16 * (distFromBoss + distBetween * 0);
                        break;
                    case 2:
                        xPos = xPos + 16 * (distFromBoss + distBetween * 0);
                        break;
                    case 3:
                        xPos = xPos + 16 * (distFromBoss + distBetween * 1);
                        break;
                    default:
                        break;
                }

                int yPos = (int)NPC.Center.Y;
                NPC minionNPC = NPC.NewNPCDirect(entitySource, xPos, yPos, npcType, NPC.whoAmI);

                // Minions left of him
                if (i < count / 2)
                {
                    minionNPC.direction = 1;
                }
                // Minions right of him
                else
                {
                    minionNPC.direction = -1;
                }

                minionNPC.spriteDirection = minionNPC.direction;

                // Return if we reached the spawn cap
                if (minionNPC.whoAmI == Main.maxNPCs)
                    return;
            }
        }

        public override void AI()
        {
            // Players cannot build or break blocks when he is not in his idle state.
            // The code for this is in ChangedSpecialModPlayer, because doing it here instead does not work

            switch (AIState)
            {
                case (float)ActionState.Idle:
                    StateIdle();
                    break;
                case (float)ActionState.Turn:
                    StateTurn();
                    break;
                case (float)ActionState.StartBossFight:
                    StateStartBossFight();
                    break;
            }
        }
    }
}

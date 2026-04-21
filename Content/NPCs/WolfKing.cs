using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Common.Systems;
using ChangedSpecialMod.Content.Biomes;
using ChangedSpecialMod.Content.Items;
using ChangedSpecialMod.Content.Projectiles;
using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.NPCs
{
    [AutoloadBossHead]
    public class WolfKing : ModNPC
	{
        private enum ActionState
        {
            Awake,
            Idle,
            Snap,
            Stand
        }

        public double imageSpeed = 5D;
        public int imageIndex = 0;

        public int[] animation = new int[] { 0, 1, 2, 3};
        public int[] animIdle = new int[] { 4 };
        public int[] animSnap = new int[] { 5, 6, 7 };
        public int[] animShock = new int[] { 8 };
        public int[] animStand = new int[] { 9 };

        public int ImageLength => animation.Length;
        public bool Loop = false;

        public double imageCounter = 0D;

        // ai[0] and ai[1] are used in every state
        // Unles we add code to send and receive extra ai, we can only use [2] and [3]

        // All states
        public ref float AIState => ref NPC.ai[0];
        public ref float AITimer => ref NPC.ai[1];

        // Idle state
        public ref float AINSnaps => ref NPC.ai[2];

        // Stand state
        public ref float AISpikeWaveDirection => ref NPC.ai[2];
        public ref float AISpikeWaveIndex => ref NPC.ai[3];

        private Rectangle RoomBounds = Rectangle.Empty;
        public int maxFollowDistance = 120 * 16;

        public override void SetStaticDefaults() 
        {
            Main.npcFrameCount[Type] = 10;
        }

		public override void SetDefaults() 
        {
			NPC.width = 100;
			NPC.height = 100;
            NPC.damage = 0;
            NPC.defense = 10;
            NPC.lifeMax = 5000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = ChangedUtils.GetNPCValue(gold: 6);
            NPC.knockBackResist = 0.0f;
            NPC.aiStyle = -1;
			AIType = NPCID.None;
			AnimationType = NPCID.None;
            NPC.boss = true;
            SpawnModBiomes = new int[] { ModContent.GetInstance<BlackLatexSurfaceBiome>().Type };

            var changedNPC = NPC.Changed();
            changedNPC.GooType = GooType.Black;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<WolfKingSpawn>()))
            {
                NPC.active = false;
                return;
            }
            
            NPC otherWolfKing = null;
            foreach (var npc in Main.npc)
            {
                if (npc.type == ModContent.NPCType<WolfKing>() && npc.active && npc.whoAmI != NPC.whoAmI)
                {
                    otherWolfKing = npc;
                    break;
                }
            }
            if (otherWolfKing != null)
            {
                NPC.active = false;
                return;
            }

            // He was spawned directly instead of WolfKingSpawn. This breaks the fight so we remove him,
            // check if there is a black latex room in the world and restart the fight
            if (!NPC.AnyNPCs(ModContent.NPCType<Cheerleader>()))
            {
                NPC.active = false;
                ChangedUtils.WolfKingSpawnCheck(true);
                return;
            }
            ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", NPC.FullName), new Color(175, 75, 255));
        }

        #region SetCheerleaderState
        private void MakeCheerleadersDance() => ChangedUtils.SwitchAllNPCState(ModContent.NPCType<Cheerleader>(), (int)Cheerleader.ActionState.Cheer);
        private void MakeCheerleadersHowl() => ChangedUtils.SwitchAllNPCState(ModContent.NPCType<Cheerleader>(), (int)Cheerleader.ActionState.Howl);
        private void MakeCheerLeadersShocked() => ChangedUtils.SwitchAllNPCState(ModContent.NPCType<Cheerleader>(), (int)Cheerleader.ActionState.Shocked);
        #endregion

        private void OpenDoors()
        {
            var leftPos = RoomBounds.Left;
            var rightPos = RoomBounds.Right;
            var floorPos = RoomBounds.Bottom;

            // Open left wall
            WorldGen.KillTile(leftPos, floorPos - 1);
            WorldGen.KillTile(leftPos, floorPos - 2);
            WorldGen.KillTile(leftPos, floorPos - 3);

            // Open right wall
            WorldGen.KillTile(rightPos, floorPos - 1);
            WorldGen.KillTile(rightPos, floorPos - 2);
            WorldGen.KillTile(rightPos, floorPos - 3);
        }

        public override void OnKill()
        {
            if (!DownedBossSystem.DownedWolfKing)
            {
                var msg = Language.GetTextValue("Mods.ChangedSpecialMod.BossMessages.BehemothCanSpawn");
                Main.NewText(msg, byte.MaxValue, 240, 20);
            }

            DownedBossSystem.DownedWolfKing = true;
            MakeCheerLeadersShocked();
            OpenDoors();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.ChangedSpecialMod.NPCs.WolfKing.Description")),
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BlackSyringe>()));
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                imageIndex = 4;
            }

            else
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
            }

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

        private void StateAwake()
        {
            AITimer++;

            if (AITimer == 1)
            {
                RoomBounds = FindRoomBounds();
                SwitchAnimation(animIdle);
                SwitchState(ActionState.Idle);
            }
        }

        private void DoDespawn()
        {
            foreach (var npc in Main.ActiveNPCs)
            {
                if (npc.type == ModContent.NPCType<Cheerleader>())
                {
                    npc.active = false;
                }
            }
            OpenDoors();
            NPC.active = false;
        }

        private void DespawnCheck()
        {
            var dist = Vector2.DistanceSquared(NPC.Center, Main.player[NPC.target].Center);
            // Despawn if target is too far away
            if (dist > maxFollowDistance * maxFollowDistance)
            {
                NPC.TargetClosest(false);
                dist = Vector2.DistanceSquared(NPC.Center, Main.player[NPC.target].Center);
                // Don't do activeplayer check, because a player can have him on the screen while he is dead
                if (dist > maxFollowDistance * maxFollowDistance)
                {
                    DoDespawn();
                    return;
                }
            }
        }

        private void StateIdle()
        {
            AITimer++;
            DespawnCheck();

            var idleTimeMax = 180;
            var idleTimeMin = 60;
            var idleTime = idleTimeMax - (int)((1f - (float)NPC.life / (float)NPC.lifeMax) * (idleTimeMax - idleTimeMin));

            if (AITimer == 1)
            {
                SwitchAnimation(animIdle);
                MakeCheerleadersDance();
            }

            if (AITimer >= idleTime)
            {
                // Stand up after snapping fingers 3 times
                if (AINSnaps > 3)
                {
                    AINSnaps = 0;
                    SwitchState(ActionState.Stand);
                }
                // Keep snapping
                else
                {
                    SwitchState(ActionState.Snap);
                }
            }
        }

        // Should add a ceiling spike. For now spawn a black latex and remove the grappling hook
        private void KnockPlayerDown(Player player)
        {
            var type = ModContent.NPCType<DarkLatexCub>();
            NPC.NewNPC(player.GetSource_FromAI(), (int)(player.Center.X), (int)(player.Center.Y), type);
            player.RemoveAllGrapplingHooks();
        }

        private int Scan(int x, int y, int dx, int dy)
        {
            const int maxRange = 50;
            for (int i = 0; i < maxRange; i++)
            {
                int checkX = x + dx * i;
                int checkY = y + dy * i;

                var tile = Main.tile[checkX, checkY];
                if (!ChangedUtils.IsBlackLatexWall(tile))
                {
                    return dx != 0 ? checkX : checkY;
                }
            }
            return -1;
        }

        private Rectangle FindRoomBounds()
        {
            int x = (int)(NPC.position.X / 16);
            int y = (int)(NPC.position.Y / 16);
            int left = Scan(x, y, -1, 0);
            int right = Scan(x, y, 1, 0);
            int top = Scan(x, y, 0, -1);
            int bottom = Scan(x, y, 0, 1);

            // Handle invalid results (-1)
            if (left == -1 || right == -1 || top == -1 || bottom == -1)
            {
                return new Rectangle(x, y, 1, 1);
            }

            return new Rectangle(
                left,
                top,
                right - left,
                bottom - top
            );
        }

        private void StateSnap()
        {
            AITimer++;

            var idleTimeSpikesMax = 90;
            var idleTimeSpikesMin = 45;
            var idleTimeSpikes = idleTimeSpikesMax - (int)((1f - (float)NPC.life / (float)NPC.lifeMax) * (idleTimeSpikesMax - idleTimeSpikesMin));

            var nSpikesMax = 4;
            var nSpikesMin = 1;
            var nSpikes = nSpikesMax - (int)(((float)NPC.life / (float)NPC.lifeMax) * (nSpikesMax - nSpikesMin));
            var player = Main.LocalPlayer;

            // Start the snapping animation
            if (AITimer == 1)
            {
                AINSnaps++;
                SwitchAnimation(animSnap);
            }

            // Play the snap sound and spawn the spikes
            else if (AITimer == 20)
            {
                SoundEngine.PlaySound(Sounds.SoundSword1, NPC.Center);
              
                // Knock the player down if he is using a grappling hook to evade the spikes
                if (IsPlayerGrappling(player))
                {
                    KnockPlayerDown(player);   
                }

                SpikeCrissCross(player, nSpikes, idleTimeSpikes);
            }

            // Switch his animation back earlier so he wont hold his paw up until the spikes are gone
            else if (AITimer == 20 + 90)
            {
                SwitchAnimation(animIdle);
            }

            // Check again if the player is grappling and if so knock him down
            if (AITimer == 20 + idleTimeSpikes)
            {
                if (IsPlayerGrappling(player))
                {
                    KnockPlayerDown(player);
                }
            }

            // Wait for spikes to go up and down. The durations are both the same
            if (AITimer >= 20 + idleTimeSpikes * 2)
            {
                SwitchState(ActionState.Idle);
            }
        }

        private void SpawnSpike(int xPos, int yPos, int idleTime)
        {
            if (xPos < RoomBounds.Left * 16 || xPos > RoomBounds.Right * 16)
            {
                return;
            }

            var entitySource = NPC.GetSource_FromAI();
            var projectileType = ModContent.ProjectileType<SpikeProjectile>();
            Projectile.NewProjectile(entitySource, new Vector2(xPos, yPos - 24), Vector2.Zero, projectileType, 0, 0f, -1, 0f, 0f, idleTime);
        }

        private void SpikeCrissCross(Player player, int nSpikes, int idleTimeSpikes)
        {
            var nBetween = 2;
            var stepSize = nSpikes * 2 + nBetween;
            var startPosLeft = GetLeftStartPosition(player, stepSize);
            var yPos = (int)(NPC.Center.Y) + 48;
            var nShiftLeft = (int)nSpikes / 2;

            for (int i = startPosLeft; i < RoomBounds.Right; i += stepSize)
            {
                for (int j = 0; j < nSpikes; j++)
                {
                    var xPos = (i + 2 * (j - nShiftLeft)) * 16 + 32;
                    SpawnSpike(xPos, yPos, idleTimeSpikes);
                }
            }
        }

        private void SpikeWaveFromWalls(int i)
        {
            var xPosLeft = (RoomBounds.Left + 2 * i) * 16 + 32;
            var xPosRight = (RoomBounds.Right - 2 * i) * 16 + 32;
            var yPos = (int)(NPC.Center.Y) + 48;
            SpawnSpike(xPosLeft, yPos, 15);
            SpawnSpike(xPosRight, yPos, 15);
        }

        private void SpikeWaveFromCenter(int i)
        {
            var xPosCenter = RoomBounds.Left + (RoomBounds.Width / 2);
            var xPosLeft = (xPosCenter - 2 * i) * 16 + 32;
            var xPosRight = (xPosCenter + 2 * i) * 16 + 32;
            var yPos = (int)(NPC.Center.Y) + 48;
            SpawnSpike(xPosLeft, yPos, 15);
            SpawnSpike(xPosRight, yPos, 15);
        }

        private int GetLeftStartPosition(Player player, int stepSize)
        {
            var playerXPos = (int)player.Center.X;
            // Round it down to a tile position
            playerXPos = ((playerXPos - (playerXPos % 16)) / 16) - 1;

            var startPosLeft = playerXPos;

            for (int i = 0; i < 100; i++)
            {
                var tmpPos = playerXPos - i * stepSize;
                if (tmpPos <= RoomBounds.Left)
                {
                    startPosLeft = tmpPos;
                    break;
                }
            }
            return startPosLeft;
        }

        private void StateStand()
        {
            AITimer++;

            var player = Main.LocalPlayer;
            // Divide by 2 because spikes take up 2 tiles and then divide again by 2 to get half of it
            var nSpikesInRoom = RoomBounds.Width / 4;
            var framesPerSpike = 11;

            if (AITimer == 1)
            {
                SwitchAnimation(animStand);
                MakeCheerleadersHowl();

                var playerXPos = (int)player.Center.X;
                // Round it down to a tile position
                playerXPos = ((playerXPos - (playerXPos % 16)) / 16);
                var roomWidth = RoomBounds.Width;
                var roomThird = roomWidth / 3;

                // Player is standing near the center of the room
                if (playerXPos <= RoomBounds.Left + roomThird || playerXPos >= RoomBounds.Right - roomThird)
                {
                    AISpikeWaveDirection = 0;
                }
                // Player is standing near the walls
                else
                {
                    AISpikeWaveDirection = 1;
                }
            }

            if (AITimer % framesPerSpike == 0)
            {
                if (AISpikeWaveDirection == 0)
                {
                    SpikeWaveFromWalls((int)AISpikeWaveIndex);
                }
                else
                {
                    SpikeWaveFromCenter((int)AISpikeWaveIndex);
                }
                AISpikeWaveIndex++;
            }

            if (AITimer >= nSpikesInRoom * framesPerSpike)
            {
                AISpikeWaveIndex = 0;
                SwitchState(ActionState.Idle);
            }
        }

        bool IsPlayerGrappling(Player player)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj != null && proj.active && proj.owner == player.whoAmI && proj.aiStyle == ProjAIStyleID.Hook)
                {
                    return true;
                }
            }
            return false;
        }

        public override void AI()
        {
            // No matter what state he is in, players cannot build or break blocks while he is alive.
            // The code for this is in ChangedSpecialModPlayer, because doing it here instead does not work

            switch (AIState)
            {
                case (float)ActionState.Awake:
                    StateAwake();
                    break;
                case (float)ActionState.Idle:
                    StateIdle();
                    break;
                case (float)ActionState.Snap:
                    StateSnap();
                    break;
                case (float)ActionState.Stand:
                    StateStand();
                    break;
            }
        }
    }
}

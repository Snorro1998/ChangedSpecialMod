using ChangedSpecialMod.Assets;
using ChangedSpecialMod.Content.NPCs;
using ChangedSpecialMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Content.Projectiles.Latex
{
    public abstract class BaseSandBallProjectile : ModProjectile
    {
        public abstract GooType GooType { get; }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.FallingBlockDoesNotFallThroughPlatforms[Type] = true;
            ProjectileID.Sets.ForcePlateDetection[Type] = true;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            var transformIndex = GetTransfurNPCIndex(target);

            if (transformIndex != -1)
            {
                modifiers.SetMaxDamage(0);
                return;
            }

            base.ModifyHitNPC(target, ref modifiers);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var transformIndex = GetTransfurNPCIndex(target);

            if (transformIndex != -1)
            {
                SoundEngine.PlaySound(Sounds.SoundTransfur, target.Center);
                target.Transform(transformIndex);
                Projectile.Kill();
            }
        }

        public bool IsSlime(NPC npc)
        {
            return npc.type is
                (NPCID.GreenSlime or NPCID.BlueSlime or NPCID.RedSlime or
                NPCID.PurpleSlime or NPCID.YellowSlime or NPCID.BlackSlime or
                NPCID.IceSlime or NPCID.SandSlime or NPCID.JungleSlime or
                NPCID.SpikedIceSlime or NPCID.SpikedJungleSlime or NPCID.MotherSlime or
                NPCID.BabySlime or NPCID.LavaSlime or NPCID.DungeonSlime or
                // We dont add king slime
                NPCID.Pinky or NPCID.GoldenSlime or NPCID.SlimeSpiked or
                NPCID.UmbrellaSlime or NPCID.ShimmerSlime or NPCID.SlimeMasked or
                NPCID.SlimeRibbonWhite or NPCID.SlimeRibbonYellow or NPCID.SlimeRibbonGreen or
                NPCID.SlimeRibbonRed or

                NPCID.ToxicSludge or NPCID.CorruptSlime or NPCID.Slimeling or
                NPCID.Slimer or NPCID.Slimer2 or NPCID.Crimslime or
                NPCID.Gastropod or NPCID.IlluminantSlime or NPCID.RainbowSlime or
                // We dont add queen slime
                NPCID.QueenSlimeMinionBlue or NPCID.QueenSlimeMinionPink or NPCID.QueenSlimeMinionPurple
                );
        }

        public int GetTransfurNPCIndex(NPC npc)
        {
            var transformIndex = -1;

            if (NPCID.Sets.CountsAsCritter[npc.type] || IsSlime(npc))
                transformIndex = GooType == GooType.Black ? ModContent.NPCType<BlackGoop>() : ModContent.NPCType<WhiteGoop>();
            else
            {
                var changedNPC = npc.Changed();
                if (changedNPC != null)
                {
                    // Black
                    if (npc.type == ModContent.NPCType<BlackGoop>())
                        transformIndex = ModContent.NPCType<DarkLatexCub>();
                    else if (npc.type == ModContent.NPCType<DarkLatexCub>())
                        transformIndex = ModContent.NPCType<MaleDarkLatex>();
                    else if (npc.type == ModContent.NPCType<MaleDarkLatex>() || npc.type == ModContent.NPCType<FemaleDarkLatex>())
                        transformIndex = ModContent.NPCType<WingedDarkLatex>();
                    else if (npc.type == ModContent.NPCType<WingedDarkLatex>())
                        transformIndex = ModContent.NPCType<Wendigo>();

                    // White
                    else if (npc.type == ModContent.NPCType<WhiteGoop>())
                        transformIndex = ModContent.NPCType<WhiteLatexCub>();
                    else if (npc.type == ModContent.NPCType<WhiteLatexCub>())
                        transformIndex = ModContent.NPCType<WhiteKnight>();
                    else if (npc.type == ModContent.NPCType<WhiteKnight>())
                        transformIndex = ModContent.NPCType<WhiteLatexTaur>();
                }
            }

            return transformIndex;
        }
    }
}

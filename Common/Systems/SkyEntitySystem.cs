using ChangedSpecialMod.Utilities;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Skies;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace ChangedSpecialMod.Common.Systems
{
    // This system overwrites sky entities, which are moving, decorative objects you can sometimes see in the sky
    // It can happen rarely in any biome, or always when inside a biome from this mod
    public class SkyEntitySystem : ModSystem
    {
        public override void Load()
        {
            On_AmbientSky.BirdsPackSkyEntity.ctor += HookBirds;
            On_AmbientSky.SlimeBalloonGroupSkyEntity.ctor += HookSlimeBalloon;
            On_AmbientSky.AirshipSkyEntity.ctor += HookAirship;
            On_AmbientSky.WyvernSkyEntity.ctor += HookWyvern;
        }

        public override void Unload()
        {
            On_AmbientSky.BirdsPackSkyEntity.ctor -= HookBirds;
            On_AmbientSky.SlimeBalloonGroupSkyEntity.ctor -= HookSlimeBalloon;
            On_AmbientSky.AirshipSkyEntity.ctor -= HookAirship;
            On_AmbientSky.WyvernSkyEntity.ctor -= HookWyvern;
        }

        #region SkyEntityHooks
        private void HookBirds(On_AmbientSky.BirdsPackSkyEntity.orig_ctor orig, object self, Player player, FastRandom random)
        {
            orig.Invoke(self, player, random);
            RandomChanceReplaceSkyEntityTexture(self, player);
        }

        private void HookSlimeBalloon(On_AmbientSky.SlimeBalloonGroupSkyEntity.orig_ctor orig, object self, Player player, FastRandom random)
        {
            orig.Invoke(self, player, random);
            RandomChanceReplaceSkyEntityTexture(self, player, 2);
        }

        private void HookAirship(On_AmbientSky.AirshipSkyEntity.orig_ctor orig, object self, Player player, FastRandom random)
        {
            orig.Invoke(self, player, random);
            RandomChanceReplaceSkyEntityTexture(self, player);
        }

        private void HookWyvern(On_AmbientSky.WyvernSkyEntity.orig_ctor orig, object self, Player player, FastRandom random)
        {
            orig.Invoke(self, player, random);
            RandomChanceReplaceSkyEntityTexture(self, player, 1);
        }
        #endregion

        private void RandomChanceReplaceSkyEntityTexture(object self, Player player, int entityType = 0)
        {
            // entitytype:
            //0 yufeng or white dragon: 4 frames
            //1 yufeng or white dragon: 5 frames
            //2 plush balloon: 1 frame

            var inBlack = BiomeChecks.InBlackLatexBiome(player);
            var inWhite = BiomeChecks.InWhiteLatexBiome(player);
            var inBoth = BiomeChecks.InCityRuinsBiome(player) || (inBlack && inWhite);

            var chance = entityType == 2 ? 20 : 10;
            if (BiomeChecks.InChangedBiome(player))
                chance = 1;

            var entityNameBlack = "FlyingDarkLatex";
            var entityNameWhite = "WhiteDragon";
            var entityName = entityNameBlack;

            if (inBoth)
                entityName = ChangedUtils.Choose(entityNameBlack, entityNameWhite);
            else if (inWhite)
                entityName = entityNameWhite;

            if (Main.rand.NextBool(chance))
            {
                var fadingSkyEntity = typeof(AmbientSky).GetNestedType("FadingSkyEntity", BindingFlags.NonPublic);
                if (fadingSkyEntity != null)
                {
                    var baseType = fadingSkyEntity?.BaseType;
                    Asset<Texture2D> texture;
                    var basePath = "ChangedSpecialMod/Assets/Textures/AmbientEntities";

                    switch (entityType)
                    {
                        case 1:
                            texture = ModContent.Request<Texture2D>($"{basePath}/{entityName}2");
                            break;
                        case 2:
                            texture = ModContent.Request<Texture2D>($"{basePath}/PuroBalloon");
                            break;
                        default:
                            texture = ModContent.Request<Texture2D>($"{basePath}/{entityName}");
                            break;
                    }

                    if (baseType != null)
                    {
                        var textureField = baseType.GetField("Texture", BindingFlags.Instance | BindingFlags.Public);
                        if (textureField != null)
                            textureField.SetValue(self, texture);
                    }
                }
            }
        }
    }
}

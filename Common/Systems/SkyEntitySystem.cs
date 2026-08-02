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
    public class SkyEntitySystem : ModSystem
    {
        public override void Load()
        {
            On_AmbientSky.BirdsPackSkyEntity.ctor += Hook_BirdsPackSkyEntity_ctor;
            On_AmbientSky.SlimeBalloonGroupSkyEntity.ctor += Hook_SlimeBalloonGroupSkyEntity_ctor;
            On_AmbientSky.AirshipSkyEntity.ctor += Hook_AirshipSkyEntity_ctor;
            On_AmbientSky.WyvernSkyEntity.ctor += Hook_WyvernSkyEntity_ctor;
        }

        public override void Unload()
        {
            On_AmbientSky.BirdsPackSkyEntity.ctor -= Hook_BirdsPackSkyEntity_ctor;
            On_AmbientSky.SlimeBalloonGroupSkyEntity.ctor -= Hook_SlimeBalloonGroupSkyEntity_ctor;
            On_AmbientSky.AirshipSkyEntity.ctor -= Hook_AirshipSkyEntity_ctor;
            On_AmbientSky.WyvernSkyEntity.ctor -= Hook_WyvernSkyEntity_ctor;
        }

        #region SkyEntityHooks
        private void Hook_BirdsPackSkyEntity_ctor(On_AmbientSky.BirdsPackSkyEntity.orig_ctor orig, object self, Player player, FastRandom random)
        {
            orig.Invoke(self, player, random);
            RandomChanceReplaceSkyEntityTexture(self, player);
        }

        private void Hook_SlimeBalloonGroupSkyEntity_ctor(On_AmbientSky.SlimeBalloonGroupSkyEntity.orig_ctor orig, object self, Player player, FastRandom random)
        {
            orig.Invoke(self, player, random);
            RandomChanceReplaceSkyEntityTexture(self, player, 2);
        }

        private void Hook_AirshipSkyEntity_ctor(On_AmbientSky.AirshipSkyEntity.orig_ctor orig, object self, Player player, FastRandom random)
        {
            orig.Invoke(self, player, random);
            RandomChanceReplaceSkyEntityTexture(self, player);
        }

        private void Hook_WyvernSkyEntity_ctor(On_AmbientSky.WyvernSkyEntity.orig_ctor orig, object self, Player player, FastRandom random)
        {
            orig.Invoke(self, player, random);
            RandomChanceReplaceSkyEntityTexture(self, player, 1);
        }
        #endregion

        private void RandomChanceReplaceSkyEntityTexture(object self, Player player, int entityType = 0)
        {
            // entitytype:
            //0 yufeng / white dragon: 4 frames
            //1 yufeng / white dragon: 5 frames
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

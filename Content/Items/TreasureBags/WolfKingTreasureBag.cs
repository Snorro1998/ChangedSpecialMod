using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ChangedSpecialMod.Content.Items.Armor;

namespace ChangedSpecialMod.Content.Items.TreasureBags
{
    public class WolfKingTreasureBag : ModItem
    {
        public new string LocalizationCategory => "Items.TreasureBags";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.BossBag[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 34;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossBags;
        }

        /*
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return CalamityUtils.DrawTreasureBagInWorld(Item, spriteBatch, ref rotation, ref scale, whoAmI);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityMod/Items/TreasureBags/DevourerofGodsBagGlow").Value);
        }
        */

        public override bool CanRightClick() => true;

        public override Color? GetAlpha(Color lightColor) => Color.Lerp(lightColor, Color.White, 0.4f);

        /*
        public override void PostUpdate()
        {
            CalamityUtils.ForceItemIntoWorld(Item);
            Item.TreasureBagLightAndDust();
        }
        */

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DisguiseSetHelmet>()));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DisguiseSetBreastplate>()));
            /*
            // Money
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<DevourerofGodsHead>()));

            // Materials
            itemLoot.Add(ModContent.ItemType<CosmiliteBar>(), 1, 75, 90);
            itemLoot.Add(ModContent.ItemType<CosmiliteBrick>(), 1, 200, 320);

            // Weapons
            itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<MawOfInfinity>(),
                ModContent.ItemType<TheObliterator>(),
                ModContent.ItemType<ThreadOfEradication>(),
                ModContent.ItemType<HyperdeathRiftScepter>(),
                ModContent.ItemType<VoidEaterMarionette>(),
                ModContent.ItemType<DimensionTearingDisk>()
            }));
            itemLoot.Add(ModContent.ItemType<CosmicDischarge>(), 10);

            // Equipment
            itemLoot.Add(ModContent.ItemType<NebulousCore>());
            itemLoot.AddRevBagAccessories();

            // Vanity
            itemLoot.Add(ModContent.ItemType<DevourerofGodsMask>(), 7);
            itemLoot.AddIf((info) => CalamityWorld.death && info.player.difficulty == 2, ModContent.ItemType<CosmicPlushie>());
            itemLoot.Add(ModContent.ItemType<ThankYouPainting>(), ThankYouPainting.DropInt);
            */
        }
    }
}

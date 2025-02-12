﻿using ArmorAndAccessoryPrefixes.Common.Config;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace ArmorAndAccessoryPrefixes.Common.GlobalItems
{
    public class PrefixGlobalItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.IsArmor() || entity.accessory;

        public override bool InstancePerEntity => true;

        // Helper properties that get a list of our prefixes
        private static int[] removedAccessoryPrefixes = new int[] {
            PrefixID.Hard,
            PrefixID.Armored,
            PrefixID.Jagged,
            PrefixID.Angry,
            PrefixID.Brisk,
            PrefixID.Hasty2,
            PrefixID.Wild,
            PrefixID.Intrepid,
        };

        public override bool? PrefixChance(Item item, int pre, UnifiedRandom rand) {
            if (item.IsArmor() && ServerConfig.Instance.ArmorPrefixes) {
                switch (pre) {
                    // Lets armor be placed in tinker slot
                    case -3:
                        return true;
                    // Armor spawns in with a prefix 50% of the time
                    // maxStack check here prevents things like buckets getting prefixes when being crafted
                    case -1:
                        return Utils.NextBool(rand, 2) && item.maxStack == 1;
                }
            }

            if (item.accessory && ServerConfig.Instance.RemoveSteps) {
                // Prevent accessories spawning with a removed prefix
                if (removedAccessoryPrefixes.Contains(pre)) {
                    return false;
                }
            }

            return base.PrefixChance(item, pre, rand);
        }

        private int rollCount = 0;

        public override bool AllowPrefix(Item item, int pre) {
            // Removes prefixes from accessories
            if (removedAccessoryPrefixes.Contains(pre) && item.accessory && ServerConfig.Instance.RemoveSteps && rollCount < 10) {
                rollCount++;

                return false;
            }

            rollCount = 0;

            return true;
        }

        public override int ChoosePrefix(Item item, UnifiedRandom rand) {
            // Gives armor prefixes
            if (item.IsArmor()) {
                var prefixes = PrefixLoader.GetPrefixesInCategory(PrefixCategory.Custom);
                List<ModPrefix> rollable = new();
                foreach (var pre in prefixes) {
                    bool isAllowed = ServerConfig.Instance.OtherModdedArmorPrefixes ? true : pre.Mod?.Name == "ArmorAndAccessoryPrefixes";
                    if (pre.CanRoll(item) && isAllowed) {
                        rollable.Add(pre);
                    }
                }

                if (rollable.Count > 0) {
                    return rand.NextFromList(rollable.ToArray()).Type;
                }
            }

            // Don't need to add our accessory prefixes here as they can be naturally rolled

            return base.ChoosePrefix(item, rand);
        }

        public override bool PreReforge(Item item) {
            if (item.TryGetGlobalItem<AccessoryGlobalItem>(out var accessoryGlobalItem)) {
                accessoryGlobalItem.MaxMana = 0;
                accessoryGlobalItem.ReducedAmmo = 0;
                accessoryGlobalItem.MinionKnockback = 0;
                accessoryGlobalItem.TileReach = 0;
                accessoryGlobalItem.PickupRange = 0;
                accessoryGlobalItem.MiningSpeed = 0;
            }

            if (item.TryGetGlobalItem<ArmorGlobalItem>(out var armorGlobalItem)) {
                armorGlobalItem.MaxHP = 0;
                armorGlobalItem.CritDamage = 1f;
                armorGlobalItem.Aggro = 0;
                armorGlobalItem.LifeRegen = 0;
                armorGlobalItem.JumpSpeedBoost = 0f;
                armorGlobalItem.RunSpeedBoost = 1f;
                armorGlobalItem.ArmorPenetration = 0;
                armorGlobalItem.MinionSlots = 0;
                armorGlobalItem.SentrySlots = 0;
                armorGlobalItem.DamageReduction = 0;
                armorGlobalItem.FlightTime = 0;
            }

            return base.PreReforge(item);
        }

        public override void UpdateInventory(Item item, Player player) {
            if (item.IsArmor()) {
                item.accessory = false;

                if (item.canBePlacedInVanityRegardlessOfConditions && !item.vanity) {
                    item.vanity = true;
                    item.canBePlacedInVanityRegardlessOfConditions = false;
                }
            }
        }
    }
}

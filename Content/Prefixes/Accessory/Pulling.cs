﻿using ArmorAndAccessoryPrefixes.Common.GlobalItems;
using Terraria;

namespace ArmorAndAccessoryPrefixes.Content.Prefixes.Accessory {
    public class Pulling : AccessoryPrefix {
        public override bool CanRoll(Item item) => item.accessory;

        public override void ModifyValue(ref float valueMult) => valueMult = 1.1f;

        public override void Apply(Item item) {
            base.Apply(item);

            item.GetGlobalItem<AccessoryGlobalItem>().PickupRange = 2;
        }
    }
}

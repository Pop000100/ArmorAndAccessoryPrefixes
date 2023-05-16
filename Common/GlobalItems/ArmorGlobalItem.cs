using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArmorAndAccessoryPrefixes.Common.GlobalItems
{
    public class ArmorGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.IsArmor();

        public int MaxHP { get; set; } = 0;
        public float CritDamage { get; set; } = 1f;
        public int Aggro { get; set; } = 0;
        public int LifeRegen { get; set; } = 0;
        public float JumpSpeedBoost { get; set; } = 0f;
        public float RunSpeedBoost { get; set; } = 1f;
        public int ArmorPenetration { get; set; } = 0;
        public int MinionSlots { get; set; } = 0;
        public int SentrySlots { get; set; } = 0;
        public int DamageReduction { get; set; } = 0;
        public int FlightTime { get; set; } = 0;


        public override void UpdateEquip(Item item, Player player) {
            player.statLifeMax2 += MaxHP;
            player.GetModPlayer<ArmorPlayer>().CritDamage += CritDamage;
            player.aggro += Aggro;
            player.lifeRegen += LifeRegen;
            player.jumpSpeedBoost += JumpSpeedBoost;
            player.runAcceleration *= RunSpeedBoost;
            player.runSlowdown *= RunSpeedBoost;
            player.GetArmorPenetration(DamageClass.Generic) += ArmorPenetration;
            player.maxMinions += MinionSlots;
            player.maxTurrets += SentrySlots;
            player.endurance += (float)DamageReduction / 100f; // Need to do it this way as I assumed DR was an int, it was not an int...
            player.GetModPlayer<ArmorPlayer>().BonusFlightTime = FlightTime;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            // Should be after the last Tooltip but before any tooltips in our list
            int index = tooltips.FindLastIndex(tip => !AccessoryGlobalItem.tooltipNamesToInsertBefore.Contains(tip.Name)) + 1;

            if (MaxHP > 0) {
                TooltipLine line = new(Mod, "PrefixMaxHP", $"+{MaxHP} max life");
                line.IsModifier = true;
                tooltips.Insert(index, line);
            }

            if (CritDamage > 1f) {
                int critDamageBonus = CritDamage == 1.05f ? 10 : 5;
                TooltipLine line = new(Mod, "PrefixCritDamage", $"+{critDamageBonus}% critical strike damage");
                line.IsModifier = true;
                tooltips.Insert(index, line);
            }

            if (Aggro > 0) {
                TooltipLine line = new(Mod, "PrefixIncreasedAggro", $"Enemies are more likely to target you");
                line.IsModifier = true;
                tooltips.Insert(index, line);
            }

            if (Aggro < 0) {
                TooltipLine line = new(Mod, "PrefixDecreasedAggro", $"Enemies are less likely to target you");
                line.IsModifier = true;
                tooltips.Insert(index, line);
            }

            if (LifeRegen > 0) {
                TooltipLine line = new(Mod, "PrefixLifeRegen", $"+{LifeRegen} life regeneration");
                line.IsModifier = true;
                tooltips.Insert(index, line);
            }

            if (JumpSpeedBoost > 0f) {
                int jumpSpeed = (int)(JumpSpeedBoost * 20f);
                TooltipLine line = new(Mod, "PrefixJumpSpeedBoost", $"+{jumpSpeed}% jump speed");
                line.IsModifier = true;
                tooltips.Insert(index, line);
            }

            if (RunSpeedBoost > 1f) {
                int accel = (int)MathF.Abs((1f - RunSpeedBoost) * 100f);
                TooltipLine line = new(Mod, "PrefixAcceleration", $"+{accel}% acceleration");
                line.IsModifier = true;
                tooltips.Insert(index, line);
            }

            if (ArmorPenetration > 0) {
                TooltipLine line = new(Mod, "PrefixArmorPenetration", $"+{ArmorPenetration} armor penetration");
                line.IsModifier = true;
                tooltips.Insert(index, line);
            }

            if (MinionSlots > 0) {
                TooltipLine line = new(Mod, "PrefixMinionSlots", $"+{MinionSlots} minion slot");
                line.IsModifier = true;
                tooltips.Insert(index, line);
            }

            if (SentrySlots > 0) {
                TooltipLine line = new(Mod, "PrefixSentrySlots", $"+{SentrySlots} sentry slot");
                line.IsModifier = true;
                tooltips.Insert(index, line);
            }

            if (DamageReduction > 0) {
                TooltipLine line = new(Mod, "PrefixDamageReduction", $"+{DamageReduction}% reduced damage");
                line.IsModifier = true;
                tooltips.Insert(index, line);
            }

            if (FlightTime > 0) {
                float flightTimeInSeconds = (float)FlightTime / 60f;
                string flightTime = string.Format("{0:0.#}", flightTimeInSeconds);
                TooltipLine line = new(Mod, "PrefixFlightTime", $"+{flightTime}s flight time");
                line.IsModifier = true;
                tooltips.Insert(index, line);
            }
        }

        #region Not sure what to tag this, just leave it alone i guess
        public override GlobalItem Clone(Item from, Item to) {
            var clone = (ArmorGlobalItem)base.Clone(from, to);
            clone.MaxHP = MaxHP;
            clone.CritDamage = CritDamage;
            clone.Aggro = Aggro;
            clone.LifeRegen = LifeRegen;
            clone.JumpSpeedBoost = JumpSpeedBoost;
            clone.RunSpeedBoost = RunSpeedBoost;
            clone.ArmorPenetration = ArmorPenetration;
            clone.MinionSlots = MinionSlots;
            clone.SentrySlots = SentrySlots;
            clone.DamageReduction = DamageReduction;
            clone.FlightTime = FlightTime;
            return clone;
        }

        public override void SaveData(Item item, TagCompound tag) {
            tag["maxHP"] = MaxHP;
            tag["critDamage"] = CritDamage;
            tag["aggro"] = Aggro;
            tag["lifeRegen"] = LifeRegen;
            tag["jumpSpeedBoost"] = JumpSpeedBoost;
            tag["runSpeedBoost"] = RunSpeedBoost;
            tag["armorPenetration"] = ArmorPenetration;
            tag["minionSlots"] = MinionSlots;
            tag["sentrySlots"] = SentrySlots;
            tag["damageReduction"] = DamageReduction;
            tag["flightTime"] = FlightTime;
        }

        public override void LoadData(Item item, TagCompound tag) {
            MaxHP = tag.GetInt("maxHP");
            CritDamage = tag.GetFloat("critDamage");
            Aggro = tag.GetInt("aggro");
            LifeRegen = tag.GetInt("lifeRegen");
            JumpSpeedBoost = tag.GetFloat("jumpSpeedBoost");
            RunSpeedBoost = tag.GetFloat("runSpeedBoost");
            ArmorPenetration = tag.GetInt("armorPenetration");
            MinionSlots = tag.GetInt("minionSlots");
            SentrySlots = tag.GetInt("sentrySlots");
            DamageReduction = tag.GetInt("damageReduction");
            FlightTime = tag.GetInt("flightTime");
        }

        #endregion
    }

    public class ArmorPlayer : ModPlayer
    {
        public float CritDamage { get; set; }

        public int BonusFlightTime { get; set; }

        public override void ResetEffects() {
            CritDamage = 1f;
            BonusFlightTime = 0;
        }

        public override void Load() {
            On.Terraria.Player.GetWingStats += Player_GetWingStats;
        }

        public override void Unload() {
            On.Terraria.Player.GetWingStats -= Player_GetWingStats;
        }

        private WingStats Player_GetWingStats(On.Terraria.Player.orig_GetWingStats orig, Player self, int wingID) {
            var ret = orig(self, wingID);
            var modPlayer = self.GetModPlayer<ArmorPlayer>();
            if (modPlayer.BonusFlightTime > 0) {
                ret.FlyTime += modPlayer.BonusFlightTime;
            }
            return ret;
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit) {
            if (crit) {
                damage = (int)((float)damage * CritDamage);
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            if (crit) {
                damage = (int)((float)damage * CritDamage);
            }
        }
    }
}

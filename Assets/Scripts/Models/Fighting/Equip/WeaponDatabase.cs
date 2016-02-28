using System.Collections.Generic;
using Models.Fighting.Buffs;

namespace Models.Fighting.Equip {
    public class WeaponDatabase : IWeaponDatabase {
        public static readonly IWeaponDatabase Instance = new WeaponDatabase();

        private readonly Dictionary<string, Weapon> _weapons = new Dictionary<string, Weapon>();

        public WeaponDatabase() {
            Add(Weapon.Builder()
                .Name("Campaign Backblade")
                .Range(1)
                .Description("Longblade that once belonged to Audric Trassalia.")
                .Build());

            Add(Weapon.Builder()
                .Name("Slim Recurve")
                .Range(2)
                .Description("A lightweight recurve bow for defense.")
                .IsSecondary()
                .Build());

            Add(Weapon.Builder()
                .Name("Heavy Bow")
                .Description("A 50lb shortbow capable of hitting targets at longer range.")
                .Range(3)
                .InitiatorPreBuffs(new HeavyBowAccuracyDebuff())
                .IsSecondary()
                .Build());

            Add(Weapon.Builder()
                .Name("Shortsword")
                .Range(1)
                .Build());

            Add(Weapon.Builder()
                .Name("Dracian Hero Bow")
                .Description("An ornate bow with gold-leafed limbs and historical detailing.")
                .IsSecondary()
                .Range(2)
                .InitiatorPreBuffs(new ScaleDamageBuff(0.8f, "dracian_hero_bow_damage"),
                                   new LeadershipRangeBuff(1, "dracian_hero_bow_leadership"))
                .Build());
        }

        public Weapon GetByName(string name) {
            return _weapons[name];
        }

        private void Add(Weapon weapon) {
            _weapons[weapon.Name] = weapon;
        }
    }
}
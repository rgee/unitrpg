﻿using Assets.Scripts.Editor;
using Models.Fighting;
using Models.Fighting.Characters;
using Models.Fighting.Skills;
using NUnit.Framework;
using UnityEngine;

namespace Tests {
    [TestFixture]
    public class FightingTest {

        [Test]
        public void TestBasicFight() {
            var attackerCharacter = new CharacterBuilder()
                .Attributes(new AttributesBuilder()
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                .Build())
                .Weapons("Shortsword")
                .Build();

            var defenderCharacter = new CharacterBuilder()
                .Attributes(new AttributesBuilder()
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(3)
                .Build())
                .Weapons("Shortsword")
                .Build();

            var attacker = new BaseCombatant(attackerCharacter);
            var defender = new BaseCombatant(defenderCharacter);

            attacker.Position = new Vector2(0, 1);
            defender.Position = new Vector2(0, 0);

            var simulator = new FightSimulator(new ConstantRandomizer(100));

            var result = simulator.Simulate(attacker, defender, new MeleeAttack());
            var init = result.Initial;
            Assert.NotNull(init);

            var dmg = init.GetDefenderDamage();
            Assert.AreEqual(5, dmg);

            var counter = result.Counter;
            Assert.NotNull(counter);

            var counterDmg = counter.GetDefenderDamage();
            Assert.AreEqual(1, counterDmg);
        }

        [Test]
        public void TestRangedCounter() {
            var attackerCharacter = new CharacterBuilder()
                .Attributes(new AttributesBuilder()
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                .Build())
                .Stats(new StatsBuilder()
                    .ParryChance(0)
                .Build())
                .Weapons("Slim Recurve")
                .Build();

            var defenderCharacter = new CharacterBuilder()
                .Attributes(new AttributesBuilder()
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                .Build())
                .Stats(new StatsBuilder()
                    .ParryChance(0)
                .Build())
                .Weapons("Slim Recurve")
                .Build();

            var attacker = new BaseCombatant(attackerCharacter);
            var defender = new BaseCombatant(defenderCharacter);

            attacker.Position = new Vector2(0, 0);
            defender.Position = new Vector2(0, 2);

            var simulator = new FightSimulator(new ConstantRandomizer(100));

            var result = simulator.Simulate(attacker, defender, new ProjectileAttack());
            var init = result.Initial;
            Assert.NotNull(init);

            var dmg = init.GetDefenderDamage();
            Assert.AreEqual(5, dmg);

            var counter = result.Counter;
            Assert.NotNull(counter);
            Assert.AreEqual(5, counter.GetDefenderDamage());
        }

        [Test]
        public void TestNoCounter() {
            var attackerCharacter = new CharacterBuilder()
                .Attributes(new AttributesBuilder()
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                .Build())
                .Weapons("Slim Recurve")
                .Build();

            var defenderCharacter = new CharacterBuilder()
                .Attributes(new AttributesBuilder()
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                .Build())
                .Stats(new StatsBuilder()
                    .ParryChance(0)
                .Build())
                .Weapons("Shortsword")
                .Build();

            var attacker = new BaseCombatant(attackerCharacter);
            var defender = new BaseCombatant(defenderCharacter);

            attacker.Position = new Vector2(0, 0);
            defender.Position = new Vector2(0, 2);

            var simulator = new FightSimulator(new ConstantRandomizer(100));

            var result = simulator.Simulate(attacker, defender, new ProjectileAttack());
            var init = result.Initial;
            Assert.NotNull(init);

            var dmg = init.GetDefenderDamage();
            Assert.AreEqual(5, dmg);

            var counter = result.Counter;
            Assert.IsNull(counter);
        }

        [Test]
        public void TestAttackerKillsOnDouble() {
            
            var attackerCharacter = new CharacterBuilder()
                .Attributes(new AttributesBuilder()
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(33)
                    .Strength(10)
                .Build())
                .Weapons("Shortsword")
                .Build();

            var defenderCharacter = new CharacterBuilder()
                .Attributes(new AttributesBuilder()
                    .Health(15)
                    .Skill(12)
                    .Defense(0)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                .Build())
                .Stats(new StatsBuilder()
                    .ParryChance(0)
                .Build())
                .Weapons("Shortsword")
                .Build();

            var attacker = new BaseCombatant(attackerCharacter);
            var defender = new BaseCombatant(defenderCharacter);

            attacker.Position = new Vector2(0, 0);
            defender.Position = new Vector2(0, 1);

            var simulator = new FightSimulator(new ConstantRandomizer(100));
            var result = simulator.Simulate(attacker, defender, new MeleeAttack());

            Assert.IsTrue(result.DefenderDies);
            Assert.IsFalse(result.AttackerDies);
            Assert.IsNotNull(result.Counter);
            Assert.IsNotNull(result.Double);
        }

        [Test]
        public void TestAttackerKillsOnFirstHit() {
            
            var attackerCharacter = new CharacterBuilder()
                .Attributes(new AttributesBuilder()
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(70)
                .Build())
                .Weapons("Shortsword")
                .Build();

            var defenderCharacter = new CharacterBuilder()
                .Attributes(new AttributesBuilder()
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                .Build())
                .Stats(new StatsBuilder()
                    .ParryChance(0)
                .Build())
                .Weapons("Shortsword")
                .Build();

            var attacker = new BaseCombatant(attackerCharacter);
            var defender = new BaseCombatant(defenderCharacter);

            attacker.Position = new Vector2(0, 0);
            defender.Position = new Vector2(0, 1);

            var simulator = new FightSimulator(new ConstantRandomizer(100));
            var result = simulator.Simulate(attacker, defender, new MeleeAttack());

            Assert.IsTrue(result.DefenderDies);
            Assert.IsFalse(result.AttackerDies);
            Assert.IsNull(result.Counter);
        }
    }
}
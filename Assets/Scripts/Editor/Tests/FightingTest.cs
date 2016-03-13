using Assets.Scripts.Editor;
using Models.Fighting;
using Models.Fighting.Characters;
using Models.Fighting.Execution;
using Models.Fighting.Maps;
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

            var attacker = new BaseCombatant(attackerCharacter, ArmyType.Friendly);
            var defender = new BaseCombatant(defenderCharacter, ArmyType.Friendly);

            attacker.Position = new Vector2(0, 1);
            defender.Position = new Vector2(0, 0);

            var map = new Map();
            map.AddCombatant(attacker);
            map.AddCombatant(defender);

            var forecaster = new FightForecaster(map);
            var forecast = forecaster.Forecast(attacker, defender, SkillType.Melee);
            var finalizer = new FightFinalizer();

            var final = finalizer.Finalize(forecast, new ConstantRandomizer(100));
            var initialPhase = final.InitialPhase;
            Assert.NotNull(initialPhase);

            var initialDamage = initialPhase.Effects.GetDefenderDamage();
            Assert.AreEqual(5, initialDamage);
            var counterPhase = final.CounterPhase;
            Assert.NotNull(counterPhase);

            var counterDamage = counterPhase.Effects.GetDefenderDamage();
            Assert.AreEqual(1, counterDamage);
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

            var attacker = new BaseCombatant(attackerCharacter, ArmyType.Friendly);
            var defender = new BaseCombatant(defenderCharacter, ArmyType.Friendly);

            attacker.Position = new Vector2(0, 0);
            defender.Position = new Vector2(0, 2);

            var map = new Map();
            map.AddCombatant(attacker);
            map.AddCombatant(defender);

            var forecaster = new FightForecaster(map);
            var forecast = forecaster.Forecast(attacker, defender, SkillType.Ranged);

            var finalizer = new FightFinalizer();
            var final = finalizer.Finalize(forecast, new ConstantRandomizer(100));

            var initialPhase = final.InitialPhase;
            Assert.NotNull(initialPhase);

            var initialEffects = initialPhase.Effects;
            Assert.AreEqual(5, initialEffects.GetDefenderDamage());

            var counterPhase = final.CounterPhase;
            Assert.NotNull(counterPhase);

            var counterEffects = counterPhase.Effects;
            Assert.AreEqual(5, counterEffects.GetDefenderDamage());

            Assert.Null(final.DoubleAttackPhase);
            Assert.Null(final.FlankerPhase);
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

            var attacker = new BaseCombatant(attackerCharacter, ArmyType.Friendly);
            var defender = new BaseCombatant(defenderCharacter, ArmyType.Friendly);

            attacker.Position = new Vector2(0, 0);
            defender.Position = new Vector2(0, 2);

            var map = new Map();
            map.AddCombatant(attacker);
            map.AddCombatant(defender);

            var forecaster = new FightForecaster(map);
            var forecast = forecaster.Forecast(attacker, defender, SkillType.Ranged);
            var finalizer = new FightFinalizer();
            var final = finalizer.Finalize(forecast, new ConstantRandomizer(100));

            var initialPhase = final.InitialPhase;
            Assert.NotNull(initialPhase);

            var dmg = initialPhase.Effects.GetDefenderDamage();
            Assert.AreEqual(5, dmg);

            Assert.IsNull(final.CounterPhase);
            Assert.IsNull(final.FlankerPhase);
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

            var attacker = new BaseCombatant(attackerCharacter, ArmyType.Friendly);
            var defender = new BaseCombatant(defenderCharacter, ArmyType.Friendly);

            attacker.Position = new Vector2(0, 0);
            defender.Position = new Vector2(0, 1);

            var map = new Map();
            var forecaster = new FightForecaster(map);
            var forecast = forecaster.Forecast(attacker, defender, SkillType.Melee);
            var finalizer = new FightFinalizer();
            var final = finalizer.Finalize(forecast, new ConstantRandomizer(100));

            Assert.NotNull(final.InitialPhase);
            Assert.NotNull(final.CounterPhase);
            Assert.NotNull(final.DoubleAttackPhase);
            Assert.Null(final.FlankerPhase);
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

            var attacker = new BaseCombatant(attackerCharacter, ArmyType.Friendly);
            var defender = new BaseCombatant(defenderCharacter, ArmyType.Friendly);

            var map = new Map();
            var forecaster = new FightForecaster(map);
            var forecast = forecaster.Forecast(attacker, defender, SkillType.Melee);
            var finalizer = new FightFinalizer();
            var final = finalizer.Finalize(forecast, new ConstantRandomizer(100));

            Assert.NotNull(final.InitialPhase);
            Assert.Null(final.CounterPhase);
            Assert.Null(final.DoubleAttackPhase);
            Assert.Null(final.FlankerPhase);
        }
    }
}
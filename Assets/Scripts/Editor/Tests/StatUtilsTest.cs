using System;
using System.Collections.Generic;
using System.Linq;
using Models.Fighting;
using Models.Fighting.Buffs;
using NUnit.Framework;

namespace Tests {
    public class StatUtilsTest {
        class ConstantStat : Stat {
            public int Value { get; private set; }
            public StatType Type { get; private set; }

            public ConstantStat(int value, StatType type) {
                Value = value;
                Type = type;
            }
        }

        class DummyBuff : AbstractBuff {
            public DummyBuff(Dictionary<StatType, Func<int, int>> mods) : base("dummy") {
                foreach (var entry in mods) {
                    StatMods[entry.Key] = CreateMod(entry.Value);
                }
            }
        }


        [Test]
        public void CombineSingleBuffAndStat() {
            var stat1 = new ConstantStat(50, StatType.HitChance);
            var hitChanceBoost = new DummyBuff(new Dictionary<StatType, Func<int, int>> {
                { StatType.HitChance, (i) => i + 10 }
            });

            var result = StatUtils.ComputeStats(new List<Stat> {stat1}, new List<IBuff> {hitChanceBoost}).ToList();
            Assert.AreEqual(1, result.Count);

            var modded = result[0];
            Assert.AreEqual(60, modded.Value);
            Assert.AreEqual(StatType.HitChance, modded.Type);
        }

        [Test]
        public void CombineManyBuffs() {
            var stat1 = new ConstantStat(50, StatType.HitChance);
            var hitChanceBoost1 = new DummyBuff(new Dictionary<StatType, Func<int, int>> {
                { StatType.HitChance, (i) => i + 10 }
            });

            var hitChanceBoost2 = new DummyBuff(new Dictionary<StatType, Func<int, int>> {
                { StatType.HitChance, (i) => i + 1 }
            });

            var result = StatUtils.ComputeStats(new List<Stat> {stat1}, new List<IBuff> { hitChanceBoost1, hitChanceBoost2 }).ToList();
            Assert.AreEqual(1, result.Count);

            var modded = result[0];
            Assert.AreEqual(61, modded.Value);
            Assert.AreEqual(StatType.HitChance, modded.Type);
        }

        [Test]
        public void IgnoreNonBuffedStats() {
            
            var stat1 = new ConstantStat(50, StatType.HitChance);
            var crit = new ConstantStat(10, StatType.CritChance);
            var hitChanceBoost = new DummyBuff(new Dictionary<StatType, Func<int, int>> {
                { StatType.HitChance, (i) => i + 10 }
            });


            var result = StatUtils.ComputeStats(new List<Stat> {stat1, crit}, new List<IBuff> {hitChanceBoost}).ToList();
            Assert.AreEqual(2, result.Count);

            var moddedHit = result.Find(stat => stat.Type == StatType.HitChance);
            var moddedCrit = result.Find(stat => stat.Type == StatType.CritChance);

            Assert.AreEqual(60, moddedHit.Value);
            Assert.AreEqual(10, moddedCrit.Value);
        }
    }
}
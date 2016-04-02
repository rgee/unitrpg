using System.Collections;
using Models.Fighting;
using Models.Fighting.Characters;
using Models.Fighting.Execution;
using Models.Fighting.Skills;
using UI.CombatForecast;
using UnityEngine;

namespace Assets.Testing {
    public class CombatForecastTestScene : MonoBehaviour {
        void Start() {
            StartCoroutine(SetupScene());
        }

        IEnumerator SetupScene() {
            yield return new WaitForSeconds(1);

            var forecastManager = CombatForecastManager.Instance;
            var liatStats = new CharacterBuilder()
                .Id("Liat")
                .Name("Liat")
                .Attributes(new AttributesBuilder()
                    .Health(34)
                    .Move(6)
                    .Skill(99)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(70)
                .Build())
                .Stats(new StatsBuilder()
                    .ParryChance(0)
                .Build())
                .Weapons("Campaign Backblade", "Slim Recurve")
                .Build();

            var gatsuStats = new CharacterBuilder()
                .Id("Gatsu")
                .Name("Soldier")
                .Attributes(new AttributesBuilder()
                    .Health(110)
                    .Skill(12)
                    .Move(6)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                .Build())
                .Stats(new StatsBuilder()
                    .ParryChance(0)
                .Build())
                .Weapons("Chained Mace")
                .Build();

            var liat = new BaseCombatant(liatStats, ArmyType.Friendly);
            var gatsu = new BaseCombatant(gatsuStats, ArmyType.Enemy);

            var attackerForecast = new SkillForecast {
                Attacker = liat,
                Defender = gatsu,
                Hit = new SkillHit {
                    BaseDamage = 9,
                    HitCount = 2
                },
                Chances = new SkillChances {
                    HitChance = 4,
                    CritChance = 5,
                    GlanceChance = 1
                }
            };

            var defenderForecast = new SkillForecast {
                Attacker = gatsu,
                Defender = liat,
                Hit = new SkillHit {
                    BaseDamage = 54,
                    HitCount = 1
                },
                Chances = new SkillChances {
                    HitChance = 75,
                    CritChance = 11,
                    GlanceChance = 90
                }
            };

            var fightForecast = new FightForecast {
                AttackerForecast = attackerForecast,
                DefenderForecast = defenderForecast
            };

            forecastManager.ShowForcast(fightForecast);
        }
    }
}
using System.Collections.Generic;
using Models.Fighting;
using Models.Fighting.Battle;
using Models.Fighting.Characters;
using Models.Fighting.Maps;
using Models.Fighting.Skills;
using Models.SaveGames;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.Battle {
    [TestFixture]
    public class BattleTest {
        private class EmptySaveGameRepository : ISaveGameRepository {
            public ISaveGame CurrentSave { get; set; }
            public void LoadSave(ISaveGame save) {
                throw new System.NotImplementedException();
            }

            public List<ISaveGame> GetAllSaves() {
                throw new System.NotImplementedException();
            }
        }

        [Test]
        public void TestBattleSetup() {
            var map = new Map();
            var random = new BasicRandomizer();
            var turnOrder = new List<ArmyType> {ArmyType.Friendly, ArmyType.Enemy, ArmyType.Other};
            var references = new List<CombatantDatabase.CombatantReference> {
                new CombatantDatabase.CombatantReference {
                    Position = new Vector2(),
                    Name = "Liat",
                    Army = ArmyType.Friendly
                },
                new CombatantDatabase.CombatantReference {
                    Position = new Vector2(1, 0),
                    Name = "Janek",
                    Army = ArmyType.Enemy
                }
            };

            var database = new CombatantDatabase(references, new EmptySaveGameRepository());
            var battle = new Models.Fighting.Battle.Battle(map, random, database, turnOrder);

            var liat = database.GetCombatantsByArmy(ArmyType.Friendly)[0];
            var janek = database.GetCombatantsByArmy(ArmyType.Enemy)[0];

            var moveAction = new MoveAction(map, liat, new Vector2(1, 1));
            battle.SubmitAction(moveAction);
            Assert.AreEqual(new Vector2(1, 1), liat.Position);

            var forecast = battle.ForecastFight(liat, janek, SkillType.Melee);
            var finalizedFight = battle.FinalizeFight(forecast);

            var liatInitialHealth = liat.Health;
            var janekInitialHealth = janek.Health;

            var fightAction = new FightAction(liat, janek, finalizedFight);
            battle.SubmitAction(fightAction);

            var janekDamage = finalizedFight.InitialPhase.Effects.GetDefenderDamage();
            var liatDamage = finalizedFight.CounterPhase.Effects.GetDefenderDamage();

            Assert.AreEqual(janekInitialHealth - janekDamage, janek.Health);
            Assert.AreEqual(liatInitialHealth - liatDamage, liat.Health);

            Assert.IsTrue(battle.CanMove(liat));
            Assert.IsFalse(battle.CanAct(liat));
        }
    }
}
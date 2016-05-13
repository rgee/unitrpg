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

            public void CreateNewGame() {
                throw new System.NotImplementedException();
            }

            public void Choose(ISaveGame saveGame) {
                throw new System.NotImplementedException();
            }

            public void Overwrite(ISaveGame saveGame) {
                throw new System.NotImplementedException();
            }

            public List<ISaveGame> GetAllSaves() {
                throw new System.NotImplementedException();
            }
        }

        [Test]
        public void TestSingleFight() {
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

        [Test]
        public void TestKnockback() {
            var map = new Map();
            var random = new BasicRandomizer();
            var turnOrder = new List<ArmyType> { ArmyType.Friendly, ArmyType.Enemy, ArmyType.Other };
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

            var forecast = battle.ForecastFight(janek, liat, SkillType.Knockback);
            var finalizedFight = battle.FinalizeFight(forecast);

            var fightAction = new FightAction(liat, janek, finalizedFight);
            battle.SubmitAction(fightAction);

            Assert.AreEqual(liat.Position, new Vector2(1, 2));
        }

        [Test]
        public void TestObstructedKnockback() {
            
            var map = new Map();
            var random = new BasicRandomizer();
            var turnOrder = new List<ArmyType> { ArmyType.Friendly, ArmyType.Enemy, ArmyType.Other };
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
            map.AddObstruction(new Vector2(1, 2));

            var moveAction = new MoveAction(map, liat, new Vector2(1, 1));
            battle.SubmitAction(moveAction);
            Assert.AreEqual(new Vector2(1, 1), liat.Position);

            var forecast = battle.ForecastFight(janek, liat, SkillType.Knockback);
            var finalizedFight = battle.FinalizeFight(forecast);

            var fightAction = new FightAction(liat, janek, finalizedFight);
            battle.SubmitAction(fightAction);

            Assert.AreEqual(liat.Position, new Vector2(1, 1));
        }

        [Test]
        public void TestAdvanceNoKill() {
            var map = new Map();
            var random = new BasicRandomizer();
            var turnOrder = new List<ArmyType> { ArmyType.Friendly, ArmyType.Enemy, ArmyType.Other };
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

            var forecast = battle.ForecastFight(liat, janek, SkillType.Advance);
            var finalizedFight = battle.FinalizeFight(forecast);

            var numEffects = finalizedFight.InitialPhase.Effects.ReceiverEffects.Count;
            Assert.AreEqual(1, numEffects);

            var initialLiatPosition = liat.Position;
            var fightAction = new FightAction(liat, janek, finalizedFight);
            battle.SubmitAction(fightAction);

            Assert.AreEqual(liat.Position, initialLiatPosition);
        }

        [Test]
        public void TestAdvanceKill() {
            var map = new Map();
            var random = new BasicRandomizer();
            var turnOrder = new List<ArmyType> { ArmyType.Friendly, ArmyType.Enemy, ArmyType.Other };
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

            var liatStats = new CharacterBuilder()
                .Id("Liat")
                .Name("Liat")
                .Attributes(new AttributesBuilder()
                    .Health(15)
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

            var janekStats = new CharacterBuilder()
                .Id("Janek")
                .Name("Janek")
                .Attributes(new AttributesBuilder()
                    .Health(1)
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

            var saveGame = new DefaultSaveGame(new List<ICharacter> { liatStats, janekStats });
            var saveGameRepo = new EmptySaveGameRepository { CurrentSave = saveGame };

            var database = new CombatantDatabase(references, saveGameRepo);
            var battle = new Models.Fighting.Battle.Battle(map, random, database, turnOrder);

            var liat = database.GetCombatantsByArmy(ArmyType.Friendly)[0];
            var janek = database.GetCombatantsByArmy(ArmyType.Enemy)[0];

            var forecast = battle.ForecastFight(liat, janek, SkillType.Advance);
            var finalizedFight = battle.FinalizeFight(forecast);

            Assert.IsNull(finalizedFight.CounterPhase);

            var fightAction = new FightAction(liat, janek, finalizedFight);
            battle.SubmitAction(fightAction);

            Assert.IsFalse(janek.IsAlive);

            Assert.AreEqual(liat.Position, janek.Position);
        }

        [Test]
        public void TestKill() {
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

            var liatStats = new CharacterBuilder()
                .Id("Liat")
                .Name("Liat")
                .Attributes(new AttributesBuilder()
                    .Health(15)
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

            var janekStats = new CharacterBuilder()
                .Id("Janek")
                .Name("Janek")
                .Attributes(new AttributesBuilder()
                    .Health(1)
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

            var saveGame = new DefaultSaveGame(new List<ICharacter> { liatStats, janekStats });
            var saveGameRepo = new EmptySaveGameRepository { CurrentSave = saveGame };

            var database = new CombatantDatabase(references, saveGameRepo);
            var battle = new Models.Fighting.Battle.Battle(map, random, database, turnOrder);

            var liat = database.GetCombatantsByArmy(ArmyType.Friendly)[0];
            var janek = database.GetCombatantsByArmy(ArmyType.Enemy)[0];

            var forecast = battle.ForecastFight(liat, janek, SkillType.Melee);
            var finalizedFight = battle.FinalizeFight(forecast);

            Assert.IsNull(finalizedFight.CounterPhase);

            var fightAction = new FightAction(liat, janek, finalizedFight);
            battle.SubmitAction(fightAction);

            Assert.IsFalse(janek.IsAlive);
        }
    }
}
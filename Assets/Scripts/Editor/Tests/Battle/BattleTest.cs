using System.Collections.Generic;
using Models.Fighting;
using Models.Fighting.Battle;
using Models.Fighting.Characters;
using Models.Fighting.Maps;
using Models.SaveGames;
using NUnit.Framework;
using UnityEngine;

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
                }
            };

            var database = new CombatantDatabase(references, new EmptySaveGameRepository());
            var battle = new Models.Fighting.Battle.Battle(map, random, database, turnOrder);
        }
    }
}
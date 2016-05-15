using System;
using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Characters;
using Models.SaveGames;
using UnityEngine;

namespace Models.Fighting.Battle {
    public class CombatantDatabase : ICombatantDatabase {
        private readonly ILookup<ArmyType, ICombatant> _combatants;

        public class CombatantReference {
            public string Id { get; set; }
            public string Name { get; set; }
            public Vector2 Position { get; set; }
            public ArmyType Army { get; set; }
        }

        public List<ICombatant> GetAllCombatants() {
            return _combatants.SelectMany(group => group).ToList();
        }

        public ICombatant GetCombatantById(string id) {
            return _combatants
                .Select(group => group.FirstOrDefault(combatant => combatant.Id == id))
                .FirstOrDefault(result => result != null);
        }

        public CombatantDatabase(IEnumerable<CombatantReference> combatantReferences, ISaveGame saveGame) {
            _combatants = combatantReferences.Select(reference => {
                return LoadCombatantFromReference(saveGame, reference);
            })
            .Cast<ICombatant>()
            .ToLookup(c => c.Army, c => c);
        }


        public CombatantDatabase(IEnumerable<CombatantReference> combatantReferences, ISaveGameRepository saveRepository) 
            : this(combatantReferences, saveRepository.CurrentSave) {

        }

        private static BaseCombatant LoadCombatantFromReference(ISaveGame saveGame, CombatantReference reference) {
            var character = BaseCharacterDatabase.Instance.GetCharacter(reference.Name);
            if (saveGame != null) {
                var savedCharacter = saveGame.GetCharacterByName(reference.Name);
                if (savedCharacter != null) {
                    character = savedCharacter;
                }
            }

            var id = reference.Id;
            if (id == null) {
                id = Guid.NewGuid().ToString();
            }

            var result = new BaseCombatant(character, reference.Army) {
                Position = reference.Position,
                Id = id
            };
            return result;
        }

        public List<ICombatant> GetCombatantsByArmy(ArmyType army) {
            return _combatants[army].ToList();
        }
    }
}
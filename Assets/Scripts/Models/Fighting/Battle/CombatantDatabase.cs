using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Characters;
using Models.SaveGames;

namespace Models.Fighting.Battle {
    public class CombatantDatabase : ICombatantDatabase {
        private readonly ILookup<ArmyType, ICombatant> _combatants;

        public class CombatantReference {
            public string Name { get; set; }
            public ArmyType Army { get; set; }
        }

        public CombatantDatabase(IEnumerable<CombatantReference> combatantReferences, ISaveGameRepository saveRepository) {
            var saveGame = saveRepository.CurrentSave;

            _combatants = combatantReferences.Select(reference => {
                return LoadCombatantFromReference(saveGame, reference);
            })
            .Cast<ICombatant>()
            .ToLookup(c => c.Army, c => c);
        }

        private static BaseCombatant LoadCombatantFromReference(ISaveGame saveGame, CombatantReference reference) {
            var character = saveGame.GetCharacterByName(reference.Name);
            if (character == null) {
                character = BaseCharacterDatabase.Instance.GetCharacter(reference.Name);
            }

            return new BaseCombatant(character, reference.Army);
        }

        public List<ICombatant> GetCombatantsByArmy(ArmyType army) {
            return _combatants[army].ToList();
        }
    }
}
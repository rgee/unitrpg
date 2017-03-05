using Contexts.Battle.Models;
using Contexts.Battle.Views;
using Contexts.Global.Services;
using Models.Fighting;
using Models.Fighting.Characters;
using Models.SaveGames;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Battle.Commands {
    public class SpawnCombatantCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public GameObject SpawningUnit { get; set; }

        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        public override void Execute() {
            var view = SpawningUnit.GetComponent<CombatantView>();
            var dimensions = Model.Dimensions;
            var position = dimensions.GetGridPositionForWorldPosition(SpawningUnit.transform.position);
            var save = SaveGameService.CurrentSave;

            ICharacter character;
            if (save != null) {
                character = save.GetCharacterByName(view.CharacterName);
            } else {
                character = BaseCharacterDatabase.Instance.GetCharacter(view.CharacterId);
            }

            var combatant = new BaseCombatant(character, view.Army) {
                Position = position,
                Id = view.CombatantId
            };

            Model.Battle.SpawnCombatant(combatant);
        }
    }
}
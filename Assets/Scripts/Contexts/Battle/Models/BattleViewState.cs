using Models.Fighting;
using Models.Fighting.Battle;
using Models.Fighting.Maps;

namespace Contexts.Battle.Models {
    public class BattleViewState {
        public BattleUIState State { get; set; }

        public IBattle Battle { get; set; }

        public IMap Map { get; set; }

        public ICombatant SelectedCombatant { get; set; }
    }
}
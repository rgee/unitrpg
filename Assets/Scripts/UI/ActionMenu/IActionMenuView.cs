using System.Collections;
using System.Collections.Generic;
using Models.Combat;
using UnityEngine;

namespace UI.ActionMenu {
    public interface IActionMenuView {
        void Show(IEnumerable<CombatActionType> actions, IEnumerable<CombatActionType> fightActions);
        bool Canceled { get; set; }
        CombatActionType? SelectedActionType { get; set; }
        IEnumerator Hide();
    }
}

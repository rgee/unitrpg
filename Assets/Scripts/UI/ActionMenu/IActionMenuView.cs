using System.Collections;
using System.Collections.Generic;
using Models.Combat;
using UnityEngine;

namespace UI.ActionMenu {
    public interface IActionMenuView {
        void Show(IEnumerable<CombatAction> actions, IEnumerable<CombatAction> fightActions);
        bool Canceled { get; set; }
        CombatAction? SelectedAction { get; set; }
        IEnumerator Hide();
    }
}

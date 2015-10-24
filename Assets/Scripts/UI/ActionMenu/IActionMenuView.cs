using System.Collections.Generic;
using Models.Combat;
using UnityEngine;

namespace UI.ActionMenu {
    public interface IActionMenuView {
        void Show(IEnumerable<CombatAction> actions);
        CombatAction? SelectedAction { get; set; }
        void Hide();
    }
}

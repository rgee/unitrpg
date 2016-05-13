using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Models.Combat;
using UnityEngine;

namespace UI.ActionMenu {
    public class NamedActionSelector : MonoBehaviour {
        public CombatActionType? SelectedActionType { get; private set; }

        public void SelectActionByName(string name) {
            var titleCased = CultureInfo.GetCultureInfo("en-US").TextInfo.ToTitleCase(name.ToLower());
            var action = (CombatActionType) Enum.Parse(typeof (CombatActionType), titleCased);
            SelectedActionType = action;
        }
    }
}

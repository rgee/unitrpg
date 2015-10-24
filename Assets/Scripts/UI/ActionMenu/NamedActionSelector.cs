using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Models.Combat;
using UnityEngine;

namespace UI.ActionMenu {
    public class NamedActionSelector : MonoBehaviour {
        public CombatAction? SelectedAction { get; private set; }

        public void SelectActionByName(string name) {
            var titleCased = CultureInfo.GetCultureInfo("en-US").TextInfo.ToTitleCase(name.ToLower());
            var action = (CombatAction) Enum.Parse(typeof (CombatAction), titleCased);
            SelectedAction = action;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EmptySaveSlot : MonoBehaviour {

    public event Action OnSelect;

    public void SelectEmptySlot() {
        if (OnSelect != null) {
            OnSelect();
        }
    }
}

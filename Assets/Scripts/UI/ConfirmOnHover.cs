using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConfirmOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public void OnPointerEnter(PointerEventData data) {
        CombatCursor.Instance.Confirming = true;
    }

    public void OnPointerExit(PointerEventData data) {
        CombatCursor.Instance.Confirming = false;
    }

    public void OnDestroy() {
        CombatCursor.Instance.Confirming = false;
    }
}

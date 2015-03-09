using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class FightClickTrigger : MonoBehaviour, IPointerClickHandler {

    public BattleManager BattleManager;

    public void OnPointerClick(PointerEventData eventData) {
        BattleManager.SelectBattleAction("fight");
    }
}
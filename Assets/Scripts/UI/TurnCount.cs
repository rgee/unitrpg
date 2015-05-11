using UnityEngine.UI;
using UnityEngine;

public class TurnCount : MonoBehaviour {
    private Text _text;

    public void Awake() {
        _text = GetComponent<Text>();
    }

    public void Update() {
        var battle = CombatObjects.GetBattleState().Model;
        if (battle != null) {
            var turnCount = battle.TurnCount;

            _text.text = "TURN " + (turnCount + 1);
        }
    }
}
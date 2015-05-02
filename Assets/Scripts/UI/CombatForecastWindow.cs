using UnityEngine;
using UnityEngine.UI;

public class CombatForecastWindow : MonoBehaviour {
    public delegate void ForecastResponseHandler(ForecastResponse resp);

    public enum ForecastResponse {
        CONFIRM,
        REJECT
    }

    private GameObject attackerCritPctText;
    private GameObject attackerDamageText;
    private GameObject attackerHealthText;
    private GameObject attackerHitPctText;
    private GameObject defenderCritPctText;
    private GameObject defenderDamageText;
    private GameObject defenderHealthText;
    private GameObject defenderHitPctText;
    private FightResult ForecastData;
    public event ForecastResponseHandler OnForecastResponse;

    public void SetForecastData(FightResult forecast) {
        ForecastData = forecast;

        var attacker = ForecastData.Participants.Attacker;
        var defender = ForecastData.Participants.Defender;
        var initialPhase = ForecastData.InitialAttack;

        // TODO: Externalize
        attackerHealthText.GetComponent<Text>().text = attacker.Health.ToString();
        attackerDamageText.GetComponent<Text>().text = initialPhase.AttackerDamage.ToString();
        attackerHitPctText.GetComponent<Text>().text = initialPhase.AttackerParams.HitChance.ToString();
        attackerCritPctText.GetComponent<Text>().text = initialPhase.AttackerParams.CritChance.ToString();

        ChangeText(defenderHealthText, defender.Health.ToString());
        ChangeText(defenderDamageText, initialPhase.CounterDamage.ToString());
        ChangeText(defenderCritPctText, initialPhase.CounterParams.CritChance.ToString());
        ChangeText(defenderHitPctText, initialPhase.CounterParams.HitChance.ToString());
    }

    private void Awake() {
        attackerHealthText = FindChild("Window/Attacker/atk_healthValue");
        attackerDamageText = FindChild("Window/Attacker/atk_damageValue");
        attackerHitPctText = FindChild("Window/Attacker/atk_hitPctValue");
        attackerCritPctText = FindChild("Window/Attacker/atk_critPctValue");

        defenderHealthText = FindChild("Window/Defender/def_healthValue");
        defenderDamageText = FindChild("Window/Defender/def_damageValue");
        defenderHitPctText = FindChild("Window/Defender/def_hitPctValue");
        defenderCritPctText = FindChild("Window/Defender/def_critPctValue");
    }

    private GameObject FindChild(string name) {
        return transform.Find(name).gameObject;
    }

    private static void ChangeText(GameObject obj, string text) {
        obj.GetComponent<Text>().text = text;
    }

    public void Confirm() {
        if (OnForecastResponse != null) {
            OnForecastResponse(ForecastResponse.CONFIRM);
        }
    }

    public void Reject() {
        if (OnForecastResponse != null) {
            OnForecastResponse(ForecastResponse.REJECT);
        }
    }
}
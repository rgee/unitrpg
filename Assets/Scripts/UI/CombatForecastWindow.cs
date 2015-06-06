using UnityEngine;
using UnityEngine.UI;

public class CombatForecastWindow : MonoBehaviour {
    public delegate void ForecastResponseHandler(ForecastResponse resp);

    public enum ForecastResponse {
        CONFIRM,
        REJECT
    }

    public event ForecastResponseHandler OnForecastResponse;

    public class FightForecast {
        public int Health { get; set; }
        public int Damage { get; set; }
        public int HitChance { get; set; }
        public int CritChance { get; set; }
        public int GlanceChance { get; set; }
    }

    public void SetForecastData(FightForecast phase) {
        ChangeValue("health", phase.Health);
        ChangeValue("damage", phase.Damage);
        ChangeValue("hitPct", phase.HitChance);
        ChangeValue("critPct", phase.CritChance);
        ChangeValue("glancePct", phase.GlanceChance);
    }

    private void ChangeValue(string name, int value) {
        var panel = transform.FindChild("Panel");
        panel.FindChild(name).GetComponent<CombatForecastText>().Value = value;
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
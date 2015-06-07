using UnityEngine;

public class CombatForecaster : MonoBehaviour {
    public delegate void ForecastResponseHandler();

    public GameObject DefenderForecast;
    public GameObject AttackerForecast; 

    public event ForecastResponseHandler OnConfirm;
    public event ForecastResponseHandler OnReject;

    private GameObject _attackerForecast;
    private GameObject _defenderForecast;

    public void ShowAttackForecast(FightResult result, GameObject attacker, GameObject defender) {
        HideCurrentForecast();

        _attackerForecast = Instantiate(AttackerForecast);
        _attackerForecast.transform.SetParent(attacker.transform);
        _attackerForecast.transform.localPosition = new Vector3(-47, 0, 0);
        var forecast = _attackerForecast.GetComponent<CombatForecastWindow>();
        forecast.OnForecastResponse += HandleForecastResponse;
        forecast.SetForecastData(new CombatForecastWindow.FightForecast {
            Health = result.Participants.Attacker.Health,
            Damage = result.InitialAttack.AttackerDamage,
            HitChance = result.InitialAttack.AttackerParams.HitChance,
            CritChance = result.InitialAttack.AttackerParams.CritChance,
            GlanceChance = result.InitialAttack.AttackerParams.GlanceChance
        });


        _defenderForecast = Instantiate(DefenderForecast);
        _defenderForecast.transform.SetParent(defender.transform);
        _defenderForecast.transform.localPosition = new Vector3(57, 0, 0);
        forecast = _defenderForecast.GetComponent<CombatForecastWindow>();
        forecast.OnForecastResponse += HandleForecastResponse;
        forecast.SetForecastData(new CombatForecastWindow.FightForecast {
            Health = result.Participants.Defender.Health,
            Damage = result.CounterAttack.AttackerDamage,
            HitChance = result.CounterAttack.AttackerParams.HitChance,
            CritChance = result.CounterAttack.AttackerParams.CritChance,
            GlanceChance = result.CounterAttack.AttackerParams.GlanceChance
        });

    }

    public void HideCurrentForecast() {
        HideForecast(_attackerForecast);
        _attackerForecast = null;

        HideForecast(_defenderForecast);
        _defenderForecast = null;
    }

    private void HideForecast(GameObject forecast) {
        if (forecast == null) {
            return;
        }

        forecast.GetComponent<CombatForecastWindow>().OnForecastResponse -= HandleForecastResponse;
        Destroy(forecast);
    }

    private void HandleForecastResponse(CombatForecastWindow.ForecastResponse resp) {
        if (OnConfirm != null && resp == CombatForecastWindow.ForecastResponse.CONFIRM) {
            OnConfirm();
        } else if (OnReject != null && resp == CombatForecastWindow.ForecastResponse.REJECT) {
            OnReject();
        }
    }
}
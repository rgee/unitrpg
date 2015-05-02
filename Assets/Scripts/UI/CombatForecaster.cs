using UnityEngine;

public class CombatForecaster : MonoBehaviour {
    public delegate void ForecastResponseHandler();

    private GameObject currentForecast;
    public GameObject ForecastPrefab;
    public event ForecastResponseHandler OnConfirm;
    public event ForecastResponseHandler OnReject;

    public void ShowAttackForecast(FightResult result, GameObject attacker, GameObject defender) {
        HideCurrentForecast();

        currentForecast = Instantiate(ForecastPrefab);
        currentForecast.transform.SetParent(gameObject.transform);

        var forecast = currentForecast.GetComponent<CombatForecastWindow>();
        forecast.OnForecastResponse += HandleForecastResponse;
        forecast.SetForecastData(result);
    }

    public void HideCurrentForecast() {
        if (currentForecast != null) {
            var window = currentForecast.GetComponent<CombatForecastWindow>();
            window.OnForecastResponse -= HandleForecastResponse;

            Destroy(currentForecast);
        }
    }

    private void HandleForecastResponse(CombatForecastWindow.ForecastResponse resp) {
        if (OnConfirm != null && resp == CombatForecastWindow.ForecastResponse.CONFIRM) {
            OnConfirm();
        } else if (OnReject != null && resp == CombatForecastWindow.ForecastResponse.REJECT) {
            OnReject();
        }
    }
}
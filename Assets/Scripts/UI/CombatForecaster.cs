using UnityEngine;
using System.Collections;

public class CombatForecaster : MonoBehaviour {

	public GameObject ForecastPrefab;

	private GameObject currentForecast;

    public delegate void ForecastResponseHandler();

    public event ForecastResponseHandler OnConfirm;
    public event ForecastResponseHandler OnReject;

	public void ShowAttackForecast(FightResult result, GameObject attacker, GameObject defender) {
        HideCurrentForecast();

		currentForecast = Instantiate(ForecastPrefab) as GameObject;
		currentForecast.transform.SetParent(gameObject.transform);

		CombatForecastWindow forecast = currentForecast.GetComponent<CombatForecastWindow>();
        forecast.OnForecastResponse += new CombatForecastWindow.ForecastResponseHandler(HandleForecastResponse);
		forecast.SetForecastData(result);
	}

    public void HideCurrentForecast() {
		if (currentForecast != null) {
            CombatForecastWindow window = currentForecast.GetComponent<CombatForecastWindow>();
            window.OnForecastResponse -= new CombatForecastWindow.ForecastResponseHandler(HandleForecastResponse);

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

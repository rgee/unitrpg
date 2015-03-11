using UnityEngine;
using System.Collections;

public class CombatForecaster : MonoBehaviour {

	public GameObject ForecastPrefab;

	private GameObject currentForecast;

	public void ShowAttackForecast(GameObject attacker, GameObject defender) {
        HideCurrentForecast();

		currentForecast = Instantiate(ForecastPrefab) as GameObject;
		currentForecast.transform.SetParent(gameObject.transform);

		CombatForecastWindow forecast = currentForecast.GetComponent<CombatForecastWindow>();
		forecast.SetUnits(attacker.GetComponent<Grid.Unit>().model, defender.GetComponent<Grid.Unit>().model);
	}

    public void HideCurrentForecast() {
		if (currentForecast != null) {
			Destroy(currentForecast);
		}
    }
}

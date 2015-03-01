using UnityEngine;
using System.Collections;

public class CombatForecaster : MonoBehaviour {

	public GameObject ForecastPrefab;

	private GameObject currentForecast;

	public void ShowAttackForecast(GameObject attacker, GameObject defender) {
		if (currentForecast != null) {
			Destroy(currentForecast);
		}

		currentForecast = Instantiate(ForecastPrefab) as GameObject;
		currentForecast.transform.SetParent(gameObject.transform);

		CombatForecastWindow forecast = currentForecast.GetComponent<CombatForecastWindow>();
		forecast.SetUnits(attacker.GetComponent<Grid.Unit>().model, defender.GetComponent<Grid.Unit>().model);
	}
}

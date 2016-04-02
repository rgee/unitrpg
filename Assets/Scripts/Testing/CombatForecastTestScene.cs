using System.Collections;
using Models.Fighting.Execution;
using UI.CombatForecast;
using UnityEngine;

namespace Assets.Testing {
    public class CombatForecastTestScene : MonoBehaviour {
        void Start() {
            StartCoroutine(SetupScene());
        }

        IEnumerator SetupScene() {
            yield return new WaitForSeconds(1);

            var forecastManager = CombatForecastManager.Instance;
            var fightForecast = null as FightForecast;

            forecastManager.ShowForcast(fightForecast);
        }
    }
}
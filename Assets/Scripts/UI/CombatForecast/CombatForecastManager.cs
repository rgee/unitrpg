
using System;
using DG.Tweening;
using Models.Fighting.Execution;
using UnityEngine;

namespace UI.CombatForecast {
    public class CombatForecastManager : Singleton<CombatForecastManager> {
        private const float TransitionTime = 0.7f;
        public event Action OnConfirm;
        public event Action OnReject;

        public GameObject ForecastPrefab;

        private GameObject _currentWindow;

        void Awake() {
            CombatEventBus.Backs.AddListener(HideForecast);
        }

        public void ShowForcast(FightForecast forecast) {
            _currentWindow = Instantiate(ForecastPrefab);
            _currentWindow.transform.SetParent(transform);

            var forecastWindow = _currentWindow.transform.FindChild("Forecast");
            var canvasGroup = forecastWindow.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            forecastWindow.transform.localPosition = new Vector3(0, -40, 0);

            _currentWindow.GetComponent<CombatForecast>().SetForecast(forecast);

            var animSequence = DOTween.Sequence().SetEase(Ease.OutCubic);

            animSequence.Insert(0, canvasGroup.DOFade(1, TransitionTime));
            animSequence.Insert(0, forecastWindow.transform.DOLocalMove(Vector3.zero, TransitionTime));

            animSequence.Play();
        }

        public void HideForecast() {
            if (_currentWindow == null) {
                return;
            } 

            var forecastWindow = _currentWindow.transform.FindChild("Forecast");
            var canvasGroup = forecastWindow.GetComponent<CanvasGroup>();


            var animSequence = DOTween.Sequence().SetEase(Ease.OutCubic);

            animSequence.Insert(0, canvasGroup.DOFade(0, TransitionTime));
            animSequence.Insert(0, forecastWindow.transform.DOLocalMove(new Vector3(0, -40, 0), TransitionTime));

            animSequence.Play();
        }
    }
}
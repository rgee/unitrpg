using System.Collections;
using DG.Tweening;
using Models.Fighting.Execution;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class CombatForecastView : View {
        private const float TransitionTime = 0.7f;
        public Signal ConfirmSignal = new Signal();
        public Signal RejectSignal = new Signal();

        public GameObject Prefab;

        private GameObject _currentWindow;

        public void ShowForecast(FightForecast forecast) {
            _currentWindow = Instantiate(Prefab);
            _currentWindow.transform.SetParent(transform);

            var forecastWindow = _currentWindow.transform.FindChild("Forecast");
            var canvasGroup = forecastWindow.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            forecastWindow.transform.localPosition = new Vector3(0, -40, 0);

            var forecastComponent = _currentWindow.GetComponent<UI.CombatForecast.CombatForecast>();
            forecastComponent.SetForecast(forecast);

            var animSequence = DOTween.Sequence().SetEase(Ease.OutCubic);

            animSequence.Insert(0, canvasGroup.DOFade(1, TransitionTime));
            animSequence.Insert(0, forecastWindow.transform.DOLocalMove(Vector3.zero, TransitionTime));

            StartCoroutine(PlayAndAttachEvents(animSequence));
        }

        public void Hide() {
            if (_currentWindow == null) {
                return;
            }

            var forecastWindow = _currentWindow.transform.FindChild("Forecast");
            var canvasGroup = forecastWindow.GetComponent<CanvasGroup>();


            var animSequence = DOTween.Sequence().SetEase(Ease.OutCubic);

            animSequence.Insert(0, canvasGroup.DOFade(0, TransitionTime));
            animSequence.Insert(0, forecastWindow.transform.DOLocalMove(new Vector3(0, -40, 0), TransitionTime));

            StartCoroutine(PlayAndDetachEvents(animSequence));
        }

        IEnumerator PlayAndAttachEvents(Tween seq) {
            yield return seq.WaitForCompletion();

            var forecastComponent = _currentWindow.GetComponent<UI.CombatForecast.CombatForecast>();
            forecastComponent.OnConfirm += DispatchConfirm;
            forecastComponent.OnReject += DispatchReject;
        }

        private void DispatchConfirm() {
            ConfirmSignal.Dispatch();
        }

        private void DispatchReject() {
            RejectSignal.Dispatch();
        }

        IEnumerator PlayAndDetachEvents(Tween seq) {
            yield return seq.WaitForCompletion();

            var forecastComponent = _currentWindow.GetComponent<UI.CombatForecast.CombatForecast>();
            forecastComponent.OnConfirm -= DispatchConfirm;
            forecastComponent.OnReject -= DispatchReject;
        }
    }
}
using Contexts.Battle.Signals;
using Models.Fighting.Execution;
using strange.extensions.mediation.impl;

namespace Contexts.Battle.Views {
    public class CombatForecastViewMediator : Mediator {
        [Inject]
        public CombatForecastView View { get; set; }

        [Inject]
        public NewFightForecastSignal FightForecastSignal { get; set; }

        [Inject]
        public FightForecastDisableSignal FightForecastDisableSignal { get; set; }

        public override void OnRegister() {
            base.OnRegister();

            FightForecastDisableSignal.AddListener(OnForecastDisable);
            FightForecastSignal.AddListener(OnNewForecast);
        }

        private void OnNewForecast(FightForecast forecast) {
            View.ShowForecast(forecast);
        }

        private void OnForecastDisable() {
            View.Hide();
        }
    }
}
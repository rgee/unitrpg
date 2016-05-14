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

        [Inject]
        public FightConfirmedSignal FightConfirmedSignal { get; set; }

        [Inject]
        public BackSignal BackSignal { get; set; }

        public override void OnRegister() {
            base.OnRegister();

            FightForecastDisableSignal.AddListener(OnForecastDisable);
            FightForecastSignal.AddListener(OnNewForecast);
            View.ConfirmSignal.AddListener(RelayConfirm);
            View.RejectSignal.AddListener(RelayReject);
        }

        private void RelayReject() {
           BackSignal.Dispatch();
        }

        private void RelayConfirm() {
           FightConfirmedSignal.Dispatch(); 
        }

        private void OnNewForecast(FightForecast forecast) {
            View.ShowForecast(forecast);
        }

        private void OnForecastDisable() {
            View.Hide();
        }
    }
}
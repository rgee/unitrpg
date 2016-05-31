using Models.SaveGames;
using strange.extensions.mediation.impl;

namespace Assets.Contexts.SaveManagement.Common {
    public class SaveGameFileMediator : Mediator {
        [Inject]
        public SaveGameFileView View { get; set; }

        [Inject]
        public SaveGameSelectedSignal SaveGameSelectedSignal { get; set; }

        [Inject]
        public EmptySaveSlotSelectedSignal EmptySaveSlotSelectedSignal { get; set; }

        public override void OnRegister() {
            base.OnRegister();

            View.SelectedSignal.AddListener(OnSelected);
        }

        void OnSelected(ISaveGame save) {
            if (save == null) {
                EmptySaveSlotSelectedSignal.Dispatch();
            } else {
                SaveGameSelectedSignal.Dispatch(save);
            }
        }
    }
}
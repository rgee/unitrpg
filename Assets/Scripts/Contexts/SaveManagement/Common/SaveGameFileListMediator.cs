using strange.extensions.mediation.impl;

namespace Assets.Contexts.SaveManagement.Common {
    public class SaveGameFileListMediator : Mediator {
        [Inject]
        public SaveGameList SaveGames { get; set; }

        [Inject]
        public SaveGameFileListView View { get; set; }
        
        public override void OnRegister() {
            base.OnRegister();
            
            View.SetFiles(SaveGames.AllSaves);
        }
    }
}
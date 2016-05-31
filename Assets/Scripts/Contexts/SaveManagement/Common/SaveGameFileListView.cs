using System.Collections.Generic;
using System.Linq;
using Models.SaveGames;
using strange.extensions.mediation.impl;

namespace Assets.Contexts.SaveManagement.Common {
    public class SaveGameFileListView : View {
        private List<SaveGameFileView> _files = new List<SaveGameFileView>();

        void Awake() {
            _files = GetComponentsInChildren<SaveGameFileView>().ToList();
        }

        public void SetFiles(IEnumerable<ISaveGame> saves) {
            var idx = 0;
            foreach (var save in saves) {
                _files[idx].SetSaveGame(save);
                idx++;
            }
        }
    }
}
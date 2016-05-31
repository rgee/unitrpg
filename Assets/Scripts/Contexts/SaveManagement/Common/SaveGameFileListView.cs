using System.Collections.Generic;
using System.Linq;
using Models.SaveGames;
using strange.extensions.mediation.impl;

namespace Assets.Contexts.SaveManagement.Common {
    public class SaveGameFileListView : View {
        public void SetFiles(IEnumerable<ISaveGame> saves) {
            var files = GetComponentsInChildren<SaveGameFileView>().ToList();
            var idx = 0;
            foreach (var save in saves) {
                files[idx].SetSaveGame(save);
                idx++;
            }
        }
    }
}
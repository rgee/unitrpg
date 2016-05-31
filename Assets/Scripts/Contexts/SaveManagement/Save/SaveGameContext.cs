

using Assets.Contexts.Base;
using Assets.Contexts.SaveManagement.Common;
using Contexts.Global.Services;
using strange.extensions.context.impl;
using UnityEngine;

namespace Assets.Contexts.SaveManagement.Save {
    public class SaveGameContext : BaseContext {
        public SaveGameContext(MonoBehaviour view) : base(view) {

        }

        protected override void mapBindings() {
            base.mapBindings();

            var saveService = injectionBinder.GetInstance<ISaveGameService>();
            var saveList = new SaveGameList {AllSaves = saveService.GetAll()};

            injectionBinder.Bind<SaveGameList>().ToValue(saveList);
            mediationBinder.Bind<SaveGameFileListView>().To<SaveGameFileListMediator>();
        }
    }
}
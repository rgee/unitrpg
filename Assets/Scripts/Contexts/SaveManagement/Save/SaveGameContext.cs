

using Assets.Contexts.Base;
using Assets.Contexts.SaveManagement.Common;
using Contexts.Global.Services;
using strange.extensions.context.impl;
using UnityEngine;

namespace Assets.Contexts.SaveManagement.Save {
    public class SaveGameContext : BaseSaveManagementContext {
        public SaveGameContext(MonoBehaviour view) : base(view) {

        }

        protected override void mapBindings() {
            base.mapBindings();

            commandBinder.Bind<EmptySaveSlotSelectedSignal>().To<WriteCurrentSaveCommand>();
        }
    }
}
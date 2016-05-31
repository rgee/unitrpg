

using Assets.Contexts.SaveManagement.Common;
using strange.extensions.context.impl;
using UnityEngine;

namespace Assets.Contexts.SaveManagement.Load {
    public class LoadGameContext : BaseSaveManagementContext {
        public LoadGameContext(MonoBehaviour view) : base(view) {

        }

        protected override void mapBindings() {
            base.mapBindings();

            commandBinder.Bind<SaveGameSelectedSignal>().To<SetCurrentSaveCommand>();
        }
    }
}
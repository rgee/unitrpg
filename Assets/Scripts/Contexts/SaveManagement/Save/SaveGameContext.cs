

using Assets.Contexts.Base;
using strange.extensions.context.impl;
using UnityEngine;

namespace Assets.Contexts.SaveManagement.Save {
    public class SaveGameContext : BaseContext {
        public SaveGameContext(MonoBehaviour view) : base(view) {

        }

        protected override void mapBindings() {
           base.mapBindings();
        }
    }
}
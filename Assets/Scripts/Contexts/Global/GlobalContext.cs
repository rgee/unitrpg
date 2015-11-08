

using strange.extensions.context.impl;
using UnityEngine;

namespace Contexts.Global {
    public class GlobalContext : MVCSContext {
        public GlobalContext(MonoBehaviour view) : base(view) {

        }

        protected override void mapBindings() {
        }
    }
}
using Contexts.Global.Models;
using Newtonsoft.Json.Linq;
using strange.extensions.context.impl;
using UnityEngine;

namespace Contexts.Global {
    public class GlobalRoot : ContextView {

        void Awake() {
            context = new GlobalContext(this);
        } 
    }
}
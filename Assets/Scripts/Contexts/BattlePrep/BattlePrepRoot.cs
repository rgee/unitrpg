using System;
using strange.extensions.context.impl;
using UnityEngine;

namespace Contexts.BattlePrep {
    public class BattlePrepRoot : ContextView {
        void Awake() {
            if (context == Context.firstContext) {
                Instantiate(Resources.Load("Prefabs/Scenes/Chapter 1 Root"));
            }
            context = new BattlePrepContext(this);
        } 
    }
}
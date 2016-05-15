using System.Collections;
using Contexts.Battle.Models;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class PhaseChangeView : View {
        public IEnumerator ShowPhaseChangeText(BattlePhase phase) {
            Debug.Log("Phase changing to: " + phase);
            yield return null;
        } 
    }
}
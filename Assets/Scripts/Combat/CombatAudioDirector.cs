using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CombatAudioDirector : Singleton<CombatAudioDirector> {
    void Start() {
        CombatEventBus.Hits.AddListener(PlayHit);
    }

    private void PlayHit(Hit hit) {
        SoundFX fx = SoundFX.Instance;
        if (hit.Crit) {
            fx.PlayCrit();
        } else if (hit.Glanced) {
            fx.PlayGlance();
        } else if (hit.Missed) {
            fx.PlayMiss();
        } else {
            fx.PlayHit();
        }
    }
}

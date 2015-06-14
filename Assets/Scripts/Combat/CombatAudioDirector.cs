using UnityEngine;

public class CombatAudioDirector : Singleton<CombatAudioDirector> {
    public GameObject SoundbankPrefab;
    private SoundFX _fx;

    private void Awake() {
        var soundbank = Instantiate(SoundbankPrefab);
        _fx = soundbank.GetComponent<SoundFX>();
        CombatEventBus.Hits.AddListener(PlayHit);
    }

    private void OnDestroy() {
        CombatEventBus.Hits.RemoveListener(PlayHit);
    }

    private void PlayHit(Hit hit) {
        if (hit.Crit) {
            _fx.PlayCrit();
        } else if (hit.Glanced) {
            _fx.PlayGlance();
        } else if (hit.Missed) {
            _fx.PlayMiss();
        } else {
            _fx.PlayHit();
        }
    }
}
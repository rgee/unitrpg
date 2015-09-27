using Combat;
using UnityEngine;

public class CombatAudioDirector : Singleton<CombatAudioDirector> {
    public GameObject SoundbankPrefab;
    private SoundFX _fx;

    private void Awake() {
        var soundbank = Instantiate(SoundbankPrefab);
        _fx = soundbank.GetComponent<SoundFX>();
        CombatEventBus.HitEvents.AddListener(PlayHit);
    }

    public new void OnDestroy() {
        CombatEventBus.HitEvents.RemoveListener(PlayHit);
    }

    private void PlayHit(HitEvent hitEvent) {
        var hit = hitEvent.Data;
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
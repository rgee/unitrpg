public class CombatAudioDirector : Singleton<CombatAudioDirector> {
    private void Start() {
        CombatEventBus.Hits.AddListener(PlayHit);
    }

    private void OnDestroy() {
        CombatEventBus.Hits.RemoveListener(PlayHit);
    }

    private void PlayHit(Hit hit) {
        var fx = SoundFX.Instance;
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
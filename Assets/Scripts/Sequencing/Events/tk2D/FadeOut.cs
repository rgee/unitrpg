using DG.Tweening;
using WellFired;

namespace Assets.Sequencing.Events.tk2D {
    [USequencerFriendlyName("Fade Out")]
    [USequencerEvent("tk2D/FadeOut")]
    public class FadeOut : USEventBase {
        public override void FireEvent() {
            var sprite = AffectedObject.GetComponent<tk2dSprite>();
            sprite.DOFade(0, Duration);
        }

        public override void ProcessEvent(float runningTime) {
        }
    }
}
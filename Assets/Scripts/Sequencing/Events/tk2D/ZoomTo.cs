using DG.Tweening;
using WellFired;

namespace Sequencing.Events.tk2D {
    [USequencerFriendlyName("Zoom To")]
    [USequencerEvent("tk2D/Camera/ZoomTo")]
    public class ZoomTo : USEventBase {
        public float NewZoom;

        public override void FireEvent() {
            var camera = AffectedObject.GetComponent<tk2dCamera>();
            DOTween.To(() => camera.ZoomFactor, zf => camera.ZoomFactor = zf, NewZoom, Duration)
                .SetEase(Ease.OutCubic);
        }

        public override void ProcessEvent(float runningTime) {
        }
    }
}
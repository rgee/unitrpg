using DG.Tweening;
using WellFired;

namespace Sequencing.Events.tk2D {
    [USequencerFriendlyName("Reset Zoom")]
    [USequencerEvent("tk2D/Camera/ResetZoom")]
    public class ResetToStandardZoom : USEventBase {
        public override void FireEvent() {
            var camera = AffectedObject.GetComponent<tk2dCamera>();
            DOTween.To(() => camera.ZoomFactor, zf => camera.ZoomFactor = zf, 1.89f, Duration)
                .SetEase(Ease.OutCubic);
        }

        public override void ProcessEvent(float runningTime) {
        }
    }
}
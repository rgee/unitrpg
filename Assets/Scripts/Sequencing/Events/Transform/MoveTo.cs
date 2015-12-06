using DG.Tweening;
using UnityEngine;
using WellFired;

namespace Sequencing.Events.Transform {
    [USequencerFriendlyName("Move To")]
    [USequencerEvent("Transform/Move To")]        
    [ExecuteInEditMode]
    public class MoveTo : USEventBase {
        public Vector3 Destination;
        public Ease Easing = Ease.OutCubic;

        public override void FireEvent() {
            AffectedObject.transform.DOLocalMove(Destination, Duration).SetEase(Easing);
        }

        public override void ProcessEvent(float runningTime) {

        }
    }
}
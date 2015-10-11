using UnityEngine;

public class CenterPortraitAligner : MonoBehaviour, IPortraitAligner {
    public Facing Facing { get; set; }

    void Update() {
        var facingCoefficient = Facing == Facing.Left ? -1 : 1;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class NeckLevelPortraitAligner : CenterPortraitAligner {
    public override void Align(GameObject portrait, Vector3 scale) {
        base.Align(portrait, scale);

        var bounds = GameObjectUtils.CalculateBounds(portrait);
        var portraitHeight = (bounds.max - bounds.min).y;
        var adjustment = new Vector3(0, portraitHeight*.45f, 0);
        portrait.transform.localPosition = portrait.transform.localPosition - adjustment;
    }
}

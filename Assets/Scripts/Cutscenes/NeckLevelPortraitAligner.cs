using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class NeckLevelPortraitAligner : CenterPortraitAligner {
    public override void Align(GameObject portrait, Facing facing, Vector3 scale) {
        base.Align(portrait, facing, scale);

        var bounds = GameObjectUtils.CalculateBounds(portrait);
        var portraitHeight = (bounds.max - bounds.min).y;
        var adjustment = new Vector3(0, portraitHeight*.45f, 0);
        portrait.transform.localPosition = portrait.transform.localPosition - adjustment;
    }
}

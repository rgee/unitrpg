using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StaticSortedObject : DynamicSortedObject {
    protected void AlignToMap() {
        if (!Application.isEditor) {
            return;
        }
        base.AlignToMap();
    }
}

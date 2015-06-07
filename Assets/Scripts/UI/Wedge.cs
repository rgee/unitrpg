using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Wedge : MonoBehaviour {
    public void SetPosition(Vector2 newPos) {
        ((RectTransform) transform).anchoredPosition = newPos;
    }
}

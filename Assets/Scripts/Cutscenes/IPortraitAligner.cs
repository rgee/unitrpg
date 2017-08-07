using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Responsible for aligning dialogue portrait children
/// </summary>
interface IPortraitAligner {
    void Align(GameObject portrait, Vector3 scale);
}

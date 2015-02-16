using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * This AI brain on every turn just tries to seek out a target.
 * Can hold off until the target is within a certain range, as well.
 */
[RequireComponent(typeof(Grid.Unit))]
public class SingleMindedFury : MonoBehaviour {
    public GameObject target;
    public int AggroRange = int.MaxValue;
    public Grid.UnitManager UnitManager;
}